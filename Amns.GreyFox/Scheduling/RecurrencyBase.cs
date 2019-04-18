using System;

namespace Amns.GreyFox.Scheduling
{
	

	/// <summary>
	/// Summary description for RecurrencyPlan.
	/// </summary>
	public class RecurrencyBase
	{
		#region Type Fields

		private SchedulingRecurrencyType type;
		public SchedulingRecurrencyType Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		private DateTime date;
		
		private int intervalDays;
		private int intervalWeeks;
		private int intervalMonths;

		private byte selectedDay;

		private bool weekdayMonday;
		private bool weekdayTuesday;
		private bool weekdayWednesday;
		private bool weekdayThursday;
		private bool weekdayFriday;
		private bool weekdaySaturday;
		private bool weekdaySunday;

		private byte weekCode;				// Every 'first' 'second', 'third', 'fourth', 'last'

		private SchedulingOccurenceType occurenceType;
		private int occurenceCount;
		private DateTime occurenceEndDate;
        
		#endregion

		public DateTime Date
		{
			get
			{
				return date;
			}
			set
			{
				date = value;
			}
		}

		public int DaysInterval
		{
			get
			{
				return intervalDays;
			}
			set
			{
				intervalDays = value;
			}
		}

		public RecurrencyBase()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool IsNextAvailable
		{
			get
			{
				switch(occurenceType)
				{
					case SchedulingOccurenceType.Infinite:
						return true;
					case SchedulingOccurenceType.OccurenceCount:
						return occurenceCount != 0;
					case SchedulingOccurenceType.EndDate:
						return DateTime.Now < occurenceEndDate;
					default:
						throw(new InvalidOperationException("SchedulingOccurenceType is invalid."));
				}
			}
		}

		public DateTime Next()
		{			
			if(!IsNextAvailable)
				throw(new InvalidOperationException("Cannot generate next date, recurrency is expired."));

			switch(type)
			{
				case SchedulingRecurrencyType.Daily:
					return date.AddDays((double) intervalDays);					
				default:
					throw(new NotImplementedException("RecurrencyPlan.Next only supports Daily recurrencies presently."));
			}
		}
	}
}