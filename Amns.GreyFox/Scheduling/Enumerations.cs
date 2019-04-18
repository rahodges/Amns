using System;

namespace Amns.GreyFox.Scheduling
{
	public enum SchedulingRecurrencyType: byte 
	{
		Daily = 1,
		Weekly = 2,
		Monthly = 3,
		Yearly = 4
	};

	public enum SchedulingOccurenceType: byte {Infinite, OccurenceCount, EndDate};
}
