using System;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for PageAttemptProtector.
	/// </summary>
	public sealed class AttemptProtector
	{
		public AttemptProtector()
		{
		}

		public static int GetAttemptCount(string key)
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string encodedCountKey = "gfx_attemptprotectorc_" + key;
			string encodedTimestampKey = "gfx_attemptprotectt_" + key;
			if(context.Session[encodedCountKey] != null)
				return (int) context.Session[encodedCountKey];
			return -1;
		}

		public static AttemptProtectorResult ExecuteAttempt(string key, int maxAttempt, TimeSpan timeout)
		{
			if(System.Web.HttpContext.Current == null)
				throw(new AttemptProtectorException("HttpContext is null."));

			int attemptCount = 1;
			DateTime attemptTimestamp = DateTime.Now;
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string encodedCountKey = "gfx_attemptprotectorc_" + key;
			string encodedTimestampKey = "gfx_attemptprotectt_" + key;

			if(context.Session[encodedCountKey] != null)
			{
				attemptCount = (int) context.Session[encodedCountKey];
				attemptTimestamp = (DateTime) context.Session[encodedTimestampKey];

				attemptCount++;

				if(attemptCount > maxAttempt)
				{
					if(DateTime.Now.Subtract(attemptTimestamp) > timeout)
					{
						attemptCount = 1;
						attemptTimestamp = DateTime.Now;
					}
					else
					{
						return AttemptProtectorResult.BadAttempt;
					}
				}
			}

			context.Session[encodedCountKey] = attemptCount;
			context.Session[encodedTimestampKey] = attemptTimestamp;

			if(attemptCount == maxAttempt)
				return AttemptProtectorResult.LastAttempt;
			else
				return AttemptProtectorResult.GoodAttempt;
		}

		public static void Reset(System.Web.HttpContext context, string key)
		{
			context.Session.Remove("gfx_attemptprotect_" + key);
			context.Session.Remove("gfx_attemptprotect_" + key);
		}
	}
}