using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.ContentWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class DbContentStatusPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbContentStatusGrid1.ToolbarClicked += new ToolbarEventHandler(DbContentStatusGrid1_ToolbarClicked);
			DbContentStatusEditor1.Cancelled += new EventHandler(showGrid);
			DbContentStatusEditor1.Updated += new EventHandler(showGrid);
			DbContentStatusView1.OkClicked += new EventHandler(showGrid);
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
			DbContentStatusGrid1.Visible = false;
			DbContentStatusEditor1.Visible = false;
			DbContentStatusView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			DbContentStatusGrid1.Visible = true;
			DbContentStatusEditor1.Visible = false;
			DbContentStatusView1.Visible = false;
		}

		private void DbContentStatusGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					DbContentStatusEditor1.DbContentStatusID = 0;
					DbContentStatusEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					DbContentStatusView1.DbContentStatusID = DbContentStatusGrid1.SelectedID;
					DbContentStatusView1.Visible = true;
					break;
				case "edit":
					resetControls();
					DbContentStatusEditor1.DbContentStatusID = DbContentStatusGrid1.SelectedID;
					DbContentStatusEditor1.Visible = true;
					break;
			}
		}
	}
}
