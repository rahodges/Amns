using System;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Provides data for the DateChanged events of the Active DateTime control.
	/// </summary>
	public class DateChangedEventArgs : EventArgs
	{
		private DateTime _oldDate, _newDate;
            
		/// <summary>
		/// Initializes a new instance of DateChangedEventArgs class.
		/// </summary>
		/// <param name="oldDate"></param>
		/// <param name="newDate"></param>
		public DateChangedEventArgs (DateTime oldDate, DateTime newDate)
		{
			_oldDate = oldDate;
			_newDate = newDate;
		}

		/// <summary>
		/// The date value before the change.
		/// </summary>
		public DateTime OldDate
		{
			get
			{
				return _oldDate;
			}
		}

		/// <summary>
		/// The date value after the change.
		/// </summary>
		public DateTime NewDate
		{
			get
			{
				return _newDate;
			}
		}
	}      
}
