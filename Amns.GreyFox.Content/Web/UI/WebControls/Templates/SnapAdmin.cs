using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	public class SnapAdmin : ITemplate
	{
		LinkButton editButton;

		public SnapAdmin(LinkButton editButton)
		{
			this.editButton = editButton;
		}

		public void InstantiateIn(Control container)
		{
			container.Controls.Add(editButton);
		}
	}
}
