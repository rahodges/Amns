using System;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for DatabaseLockedException.
	/// </summary>
	public class MSJetUtilityLockedException : System.Exception
	{
		public MSJetUtilityLockedException() : base() { }

		public MSJetUtilityLockedException(string message, System.Exception innerException) :
			base(message, innerException) { }
	}
}
