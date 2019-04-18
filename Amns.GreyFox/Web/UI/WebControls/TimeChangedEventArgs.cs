using System;

namespace Amns.GreyFox.WebControls
{
	/// <summary>
	/// Provides data for the DateChanged events of the Active DateTime control.
	/// </summary>
	public class TimeChangedEventArgs : EventArgs
	{
		private TimeSpan _oldTime, _newTime;
            
		/// <summary>
		/// Initializes a new instance of DateChangedEventArgs class.
		/// </summary>
		/// <param name="oldDate"></param>
		/// <param name="newDate"></param>
		public TimeChangedEventArgs (TimeSpan oldTime, DateTime newTime)
		{
			_oldTime = oldTime;
			_newTime = newTime;
		}

		/// <summary>
		/// The date value before the change.
		/// </summary>
		public TimeSpan OldTime
		{
			get
			{
				return _oldTime;
			}
		}

		/// <summary>
		/// The date value after the change.
		/// </summary>
		public DateTime NewTime
		{
			get
			{
				return _newTime;
			}
		}
	}      
}
