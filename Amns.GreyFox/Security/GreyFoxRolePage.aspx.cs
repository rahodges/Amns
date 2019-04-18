using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace GreyFoxWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class GreyFoxRolePage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			GreyFoxRoleGrid1.ToolbarClicked += new ToolbarEventHandler(GreyFoxRoleGrid1_ToolbarClicked);
			GreyFoxRoleEditor1.Cancelled += new EventHandler(showGrid);
			GreyFoxRoleEditor1.Updated += new EventHandler(showGrid);
			GreyFoxRoleView1.OkClicked += new EventHandler(showGrid);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void resetControls()
		{
			GreyFoxRoleGrid1.Visible = false;
			GreyFoxRoleEditor1.Visible = false;
			GreyFoxRoleView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			GreyFoxRoleGrid1.Visible = true;
			GreyFoxRoleEditor1.Visible = false;
			GreyFoxRoleView1.Visible = false;
		}

		private void GreyFoxRoleGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					GreyFoxRoleEditor1.GreyFoxRoleID = 0;
					GreyFoxRoleEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					GreyFoxRoleView1.GreyFoxRoleID = GreyFoxRoleGrid1.SelectedID;
					GreyFoxRoleView1.Visible = true;
					break;
				case "edit":
					resetControls();
					GreyFoxRoleEditor1.GreyFoxRoleID = GreyFoxRoleGrid1.SelectedID;
					GreyFoxRoleEditor1.Visible = true;
					break;
			}
		}
	}
}
