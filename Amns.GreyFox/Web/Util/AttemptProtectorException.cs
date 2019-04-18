using System;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for PageAttemptProtectorException.
	/// </summary>
	public class AttemptProtectorException : System.Exception
	{
		public AttemptProtectorException(string message) : base(message)
		{
		}
	}
}
