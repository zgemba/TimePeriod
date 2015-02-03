# TimePeriod
.NET (c#) Simple class to handle time intervals

A simple class to handle basic time interval operations like overlaps and inclusions.

## Usage

``` c#
int thisYear = DateTime.UtcNow.Year;
DateTime firstOfJanuary = new DateTime(thisYear, 1, 1);
DateTime tenthOfMarch = new DateTime(thisYear, 3, 1);

// create TimePeriod from two DateTime Objects
TimePeriod yearSoFar = new TimePeriod(firstOfJanuary, DateTime.UtcNow);

// create TimePeriod from DateTime object (start) and TimeSpan object (duration)
TimePeriod january = new TimePeriod(firstOfJanuary, new TimeSpan(31, 0, 0, 0));

TimePeriod startOfMarch = new TimePeriod(new DateTime(thisYear, 3, 1), tenthOfMarch);
TimePeriod restOfMarch = new TimePeriod(tenthOfMarch, new DateTime(thisYear, 3, 28));
TimePeriod startOfApril = new TimePeriod(new DateTime(thisYear, 4, 1), new TimeSpan(10,0,0,0)); // first 10 days of April

bool a = yearSoFar.Overlaps(january);           // true
bool d = startOfMarch.Overlaps(startOfApril);   // false
bool b = january.Contains(firstOfJanuary);      // true
bool c = january.Contains(tenthOfMarch);        // false           

TimePeriod march = startOfMarch + restOfMarch;  // adding two periods
TimePeriod impossible = startOfMarch + startOfApril; // Does not make sense -> Exception
```
