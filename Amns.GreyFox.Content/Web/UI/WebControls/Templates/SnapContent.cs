using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	public class SnapContent : ITemplate
	{
		string snapID; 
		string content; 

		public SnapContent(string snapId, string content)
		{
			this.snapID = snapId;
			this.content = content;
		}

		public void InstantiateIn(Control container)
		{
			Literal L1 = new Literal(); 

			L1.Text = content;

			container.Controls.Add(L1); 
		}
	}

}
