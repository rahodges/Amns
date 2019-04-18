using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ToolbarItem.
	/// </summary>
	public class ToolbarLiteral : ToolbarItem
	{
		public ToolbarLiteral(string text)
		{
			this.Text = text;
		}

		public override void Render(ToolbarRenderer r)
		{
			if(!Visible)
				return;

			HtmlTextWriter output = r.Output;

			output.Write("<td nowrap=\"true\" class=\"");
			output.Write(r.Style.Name);
			output.Write("_ButtonNormal\"");

			if(Function != string.Empty)
			{
				output.Write(" onClick=\"");
				output.Write(Function);
				output.Write("\"");
			}
			else if(Command != string.Empty)
			{
				output.Write(" onClick=\"javascript:");
				output.Write(r.ParentControl.Page.ClientScript.GetPostBackEventReference(r.ParentControl, Command));
				output.Write("\"");
			}

			output.Write(" unselectable=\"on\">");
			output.Write(Text);
			output.Write("</td>");
		}
	}
}
