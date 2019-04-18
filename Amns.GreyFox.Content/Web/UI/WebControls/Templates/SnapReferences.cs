using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	public class SnapReferences : ITemplate
	{
		string linkFormat;
		DbContentClip clip; 

		public SnapReferences(string linkFormat, DbContentClip clip)
		{
			this.linkFormat = linkFormat;
			this.clip = clip;
		}

		public void InstantiateIn(Control container)
		{
			Literal L1 = new Literal();

			L1.Text = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";

			foreach(DbContentClip reference in clip.References)
			{
				L1.Text += "<tr><td><a href=\"";
				if(reference.OverrideUrl != string.Empty)
					L1.Text += reference.OverrideUrl;
				else
					L1.Text += container.ResolveUrl(string.Format(linkFormat, reference.ID));
				if(reference.MenuTooltip != string.Empty)
					L1.Text += "\" title=\"" + reference.MenuTooltip + "\"";
				L1.Text += ">" + reference.Title + "</a></td></tr>";
			}

			L1.Text += "</table>";

			container.Controls.Add(L1); 
		}
	}

}
