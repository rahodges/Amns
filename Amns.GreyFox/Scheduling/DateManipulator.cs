using System;

namespace Amns.GreyFox.Scheduling
{
	/// <summary>
	/// Summary description for DateManipulator.
	/// </summary>
	public sealed class DateManipulator
	{
        public static DateTime FirstOfNextDay(DateTime date)
        {
            DateTime temp = new DateTime(date.Year, date.Month, date.Day);
            return temp.AddDays(1);
        }

        public static DateTime FirstOfNextYear(DateTime date)
        {
            DateTime temp = new DateTime(date.Year, 1, 1);
            return temp.AddYears(1);
        }

        public static DateTime FirstOfYear(DateTime date)
        {
            return new DateTime(date.Year, 1, 1,
                0, 0, 0, 0);
        }

		public static DateTime FirstOfMonth(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1, 
				0, 0, 0, 0);
		}

		public static DateTime EndOfMonth(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 
				DateTime.DaysInMonth(date.Year, date.Month),
                23, 59, 59, 999);
		}

        public static DateTime EndOfFollowingMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month + 1,
                DateTime.DaysInMonth(date.Year, date.Month + 1),
                23, 59, 59, 999);
        }

        public static DateTime EndOfYear(DateTime date)
        {
            return new DateTime(date.Year, 12,
                DateTime.DaysInMonth(date.Year, 12),
                23, 59, 59, 999);
        }

        public static DateTime EndOfFollowingYear(DateTime date)
        {
            return new DateTime(date.Year + 1, 12,
                DateTime.DaysInMonth(date.Year, 12),
                23, 59, 59, 999);
        }

		/// <summary>
		/// Returns the first day of the week. This method does not reset the time to 00:00.
		/// </summary>
		/// <param name="date">The date to calculate from.</param>
		/// <returns></returns>
		public static DateTime FirstOfWeek(DateTime date)
		{
			try 
			{
				switch(date.DayOfWeek)
				{
					case(DayOfWeek.Sunday):
						return date.Date;
					case(DayOfWeek.Monday):
						return date.Subtract(TimeSpan.FromDays(1)).Date;
					case(DayOfWeek.Tuesday):
						return date.Subtract(TimeSpan.FromDays(2)).Date;
					case(DayOfWeek.Wednesday):
						return date.Subtract(TimeSpan.FromDays(3)).Date;
					case(DayOfWeek.Thursday):
						return date.Subtract(TimeSpan.FromDays(4)).Date;
					case(DayOfWeek.Friday):
						return date.Subtract(TimeSpan.FromDays(5)).Date;
					case(DayOfWeek.Saturday):
						return date.Subtract(TimeSpan.FromDays(6)).Date;
					default:
						throw(new ArgumentException("Unknown day of week."));
				}
			}
			catch
			{
				throw(new Exception("Illegal date " + date.ToString() + "."));
			}
		}

		/// <summary>
		/// Returns the last day of the week. This method does not reset the time to 00:00.
		/// </summary>
		/// <param name="date">The date to calculate from.</param>
		/// <returns></returns>
		public static DateTime LastOfWeek(DateTime date)
		{
			switch(date.DayOfWeek)
			{
				case(DayOfWeek.Sunday):
					return date.AddDays(6);
				case(DayOfWeek.Monday):
					return date.AddDays(5);
				case(DayOfWeek.Tuesday):
					return date.AddDays(4);
				case(DayOfWeek.Wednesday):
					return date.AddDays(3);
				case(DayOfWeek.Thursday):
					return date.AddDays(2);
				case(DayOfWeek.Friday):
					return date.AddDays(1);
				case(DayOfWeek.Saturday):
					return date;
			}

			throw(new Exception("Illegal DayOfWeek."));
		}

		public static DateTime SubtractMonths(DateTime date, int months)
		{
			int backMonths = months % 12;
			int backYears = months / 12;
			return new DateTime(date.Year - backYears, date.Month - backMonths,
				date.Day, 
				date.Hour, date.Minute, date.Second, date.Millisecond);
		}

		public static DateTime SubtractYears(DateTime date, int years)
		{
			return new DateTime(date.Year - years, date.Month, date.Day,
				date.Hour, date.Minute, date.Second, date.Millisecond);
		}

//		public string DurationFormatter(DateTime startDate, DateTime endDate)
//		{
//			System.Text.StringBuilder s = new System.Text.StringBuilder();
//
//			// The class is on the same day, no need to print end date's day name
//			bool sameYears = startDate.Year == endDate.Year;
//			bool sameMonths = !sameYears & startDate.Month == endDate.Month;
//			bool sameDays = !sameMonths & startDate.Day == endDate.Day;
//			bool sameTime = !sameDays & startDate.TimeOfDay == endDate.TimeOfDay;
//
//			s.Append(startDate.ToLongDateString());
//			s.Append(" - ");
//
//			if(sameYears)
//			{
//				if(sameMonths)
//				{
//					if(sameDays)
//					{
//						if(sameTime)
//						{
//						}
//						else
//						{
//						}
//					}
//					else
//					{						
//					}
//				}
//				else
//				{
//				}
//			}
//		}
	}
}
