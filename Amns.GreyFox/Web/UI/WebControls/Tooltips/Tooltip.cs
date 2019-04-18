using System;

namespace Amns.GreyFox.Web.UI.WebControls.Tooltips
{
	/// <summary>
	/// Summary description for Tooltip.
	/// </summary>
	public class Tooltip
	{
		string __id;
		string __title;
		string __body;
		int __delay;

		public string ID
		{
			get { return __id; }
			set { __id = value; }
		}

		public string Title
		{
			get { return __title; }
			set { __title = value; }
		}

		public string Body
		{
			get { return __body; }
			set { __body = value; }
		}

		public int Delay
		{
			get { return __delay; }
			set { __delay = value; }
		}

		public Tooltip(string id, string title, string body, int delay)
		{
			__id = id;
			__title = title;
			__body = body;
			__delay = delay;
		}

		public string GetClientFunction(TooltipFunction function)
		{
			switch(function)
			{
				case TooltipFunction.Show:
					return "gfx_showtip(" + __id + ")";
				case TooltipFunction.Hide:
					return "gfx_hidetip()";
			}

			throw(new Exception("Illegal TooltipFunction."));
		}
	}
}
