using System;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for ContentManagerException.
	/// </summary>
	public class ContentException : ApplicationException
	{
		public int ErrorCode
		{
			get
			{
				return HResult;
			}
		}

		public ContentException(string message, int errorCode) : base(message)
		{
			this.HResult = errorCode;
		}
	}
}
