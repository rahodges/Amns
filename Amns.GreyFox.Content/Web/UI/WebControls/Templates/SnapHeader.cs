using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	public class SnapHeader : ITemplate
	{
		string snapID; 
		string headerCssClass;
		string title; 

		public SnapHeader(string snapID, string headerCssClass, string title)
		{
			this.snapID = snapID;
			this.headerCssClass = headerCssClass;
			this.title = title;
		}

		public void InstantiateIn(Control container)
		{
			Literal L1 = new Literal(); 
			
			L1.Text = "<div style=\"CURSOR: move; width: 100%;\"><table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">";
			L1.Text += "<tr><td class=\"" + headerCssClass + "\" onmousedown=\"" + snapID + ".StartDragging(event);\">" + title + "</td>";
			L1.Text += "<td width=\"10\" style=\"cursor: hand\" align=\"right\"><img onclick=\"" + snapID + ".ToggleExpand();\" src=\"images/i_open.gif\" width=\"22\" height=\"19\" border=\"0\"></td>";
			L1.Text += "</tr></table></div>";

			container.Controls.Add(L1); 
		}
	}

}
