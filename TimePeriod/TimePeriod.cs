namespace Zgemba.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Simple TimePeriod class, defines starting and ending time
    /// used mainly to simplify checking for overlaps
    /// </summary>
    [Serializable]
    public class TimePeriod : IComparable
    {
        private bool reversed = false;

        public DateTime Start
        {
            get;
            set;
        }

        public DateTime End
        {
            get;
            set;
        }

        public bool Reversed
        {
            get { return this.reversed; }
        }


        /// <summary>
        /// Creates new instance of the TimePeriod class
        /// </summary>
        /// <param name="start">Starting time</param>
        /// <param name="end">Ending time</param>
        public TimePeriod(DateTime start, DateTime end)
        {
            if (start < end)
            {
                this.Start = start;
                this.End = end;
            }
            else
            {
                this.Start = end;       // obrnemo, da so primerjave enostavnejše
                this.End = start;
                this.reversed = true;   // zabeležim, da sem ga obrnil!
            }
        }

        /// <summary>
        /// Creates new instance of the TimePeriod class
        /// </summary>
        /// <param name="start">Starting time</param>
        /// <param name="duration">Duration</param>
        public TimePeriod(DateTime start, TimeSpan duration)
        {
            this.Start = start;
            this.End = start + duration;
        }

        public TimeSpan Duration
        {
            get { return this.End - this.Start; }
        }

        public bool Overlaps(TimePeriod other)
        {
            if (this.End >= other.Start && this.Start <= other.Start) return true;
            if (other.End >= this.Start && other.Start <= this.Start) return true;
            if (other.Start >= this.Start && other.End <= this.End) return true;
            if (this.Start >= other.Start && this.End <= other.End) return true;
            return false;
        }

        public bool Contains(DateTime time)
        {
            return (this.Start <= time && time <= this.End);
        }

        public bool Contains(TimePeriod other)
        {
            return (this.Start <= other.Start && this.End >= other.End);
        }

        static public TimePeriod operator +(TimePeriod a, TimePeriod b)
        {
            if (a.Overlaps(b))
            {
                DateTime start = a.Start <= b.Start ? a.Start : b.Start;
                DateTime end = b.End >= a.End ? b.End : a.End;
                return new TimePeriod(start, end);
            }
            return null;
            //throw new ArithmeticException("Periods not overlapping");
        }


        public override string ToString()
        {
            return String.Format("{0} -> {1} : {2}", this.Start, this.End, this.Duration);
        }

        public string ToString(string format)
        {
            string s, e;
            if (format == "s")
            {
                s = (this.Start == DateTime.MinValue) ? "-∞" : this.Start.ToShortDateString();
                e = (this.End == DateTime.MaxValue) ? "∞" : this.End.ToShortDateString();
                return string.Format("{0} -> {1}", s, e);
            }
            else
                return this.ToString();
        }


        int IComparable.CompareTo(object obj)
        {
            TimePeriod that = (TimePeriod)obj;
            return DateTime.Compare(this.Start, that.Start);
        }


        // Equals boilerplate
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            TimePeriod that = obj as TimePeriod;
            if ((object)that == null)
            {
                return false;
            }

            return this.Start == that.Start &&
                   this.End == that.End;
        }

        public bool Equals(TimePeriod other)
        {
            if ((object)other == null)
            {
                return false;
            }

            return this.Start == other.Start &&
                   this.End == other.End;
        }

        public override int GetHashCode()
        {
            return (int)(this.Start.Ticks ^ this.End.Ticks);
        }


        public static bool PeriodsOverlap(IList<TimePeriod> periods)
        {
            if (periods.Count <= 1)
                return false;

            for (int i = 0; i < periods.Count; i++)
            {
                for (int j = i + 1; j < periods.Count; j++)
                {
                    if (periods[i].Overlaps(periods[j]))
                        return true;
                }
            }
            return false;
        }
    }
}
