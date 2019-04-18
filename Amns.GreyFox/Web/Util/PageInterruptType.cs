using System;
using System.Web;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for PageInterruptor.
	/// </summary>
	public class PageInterrupt
	{
		public static void ExecuteInterrupt(PageInterruptType interruptType)
		{
			HttpResponse r = HttpContext.Current.Response;

			switch(interruptType)
			{
				case PageInterruptType.OK:
					r.Clear();
					r.Write("<html><body>This page is for internal use only!<br><strong>G</strong></body><html>");
					r.StatusCode = (int) interruptType;
					r.Flush();
					r.Close();
					break;
				default:
					r.Clear();
					r.Write("<html><body>This page is for internal use only!<br><strong>" + interruptType.ToString() + "</strong></body><html>");
					r.StatusCode = (int) interruptType;
					r.Flush();
					r.Close();
					break;
			}
		}
	}
}
