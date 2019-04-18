using System;
using System.Text;
using System.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls.Tooltips
{
	/// <summary>
	/// A tooltip register for use in ASP.net pages. Tooltips are registered with
	/// the register directly.
	/// </summary>
	public class TooltipRegister
	{
		TooltipCollection __tooltips;
	
		public TooltipCollection Tooltips
		{
			get { return __tooltips; }
		}

		public TooltipRegister()
		{
			__tooltips = new TooltipCollection();
		}

		public void RegisterTips(Page page)
		{
			StringBuilder s = new StringBuilder();
#if DEBUG
			// Start Window Output Notice
			s.Append("\r\n<!-- ******************************************************* -->\r\n");
			s.Append("<!-- * GreyFox Tooltips v1.0                               * -->\r\n");
			s.Append("<!-- ******************************************************* -->\r\n");
#endif
			s.Append("<div style=\"visibility:hidden;height:0px;width:0px;border:0px;padding:0px;\">\r\n");
			
			foreach(Tooltip tooltip in __tooltips)
			{
				s.Append("<div id=\"gfx_tt_" + tooltip.ID + "\" style=\"visibility:hidden;\">");
				s.Append(tooltip.Body);
				s.Append("</div>\r\n");
			}

			s.Append("</div>");

			page.ClientScript.RegisterStartupScript(this.GetType(), "gfx_tooltips", s.ToString());
		}

		public void RenderTips(HtmlTextWriter output)
		{	

#if DEBUG
			// Start Window Output Notice
			output.WriteLine();
			output.WriteLine("<!-- ******************************************************* -->");
			output.WriteLine("<!-- * GreyFox Tooltips v1.0                               * -->");
			output.WriteLine("<!-- ******************************************************* -->");
#endif

			output.WriteBeginTag("div");
			output.WriteAttribute("style", "visibility:hidden;height:0px;width:0px;border:0px;padding:0px;");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();

			foreach(Tooltip tooltip in __tooltips)
			{
				output.WriteBeginTag("div");
				output.WriteAttribute("id", "gfx_tt_" + tooltip.ID);
				output.WriteAttribute("style", "visibility:hidden;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(tooltip.Body);
				output.WriteEndTag("</div>");
				output.WriteLine();
			}

			output.WriteEndTag("</div>");
		}
	}
}
