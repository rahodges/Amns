using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	public class SnapFooter : ITemplate
	{
		int height;

		public SnapFooter(int height)
		{
			this.height = height;			
		}

		public void InstantiateIn(Control container)
		{
			Literal L1 = new Literal(); 
			
			L1.Text = "<br>"; // <div style=\"height=" + height.ToString() + "px;width=2px;\" />";

			container.Controls.Add(L1); 
		}
	}

}
