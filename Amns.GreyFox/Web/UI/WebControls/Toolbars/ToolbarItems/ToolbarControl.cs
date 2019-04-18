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
	public class ToolbarControl : ToolbarItem
	{
		private Control _childControl;

		public ToolbarControl(Control childControl)
		{
			_childControl = childControl;
		}

		public override void Render(ToolbarRenderer r)
		{
			if(!Visible)
				return;

			HtmlTextWriter output = r.Output;

			output.WriteLine("<td>");
			output.Indent++;
			_childControl.RenderControl(output);
			output.Indent--;
			output.WriteLine("</td>");
		}
	}
}
