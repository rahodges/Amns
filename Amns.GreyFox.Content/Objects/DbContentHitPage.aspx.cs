using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.ContentWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class DbContentHitPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbContentHitGrid1.ToolbarClicked += new ToolbarEventHandler(DbContentHitGrid1_ToolbarClicked);
			DbContentHitEditor1.Cancelled += new EventHandler(showGrid);
			DbContentHitEditor1.Updated += new EventHandler(showGrid);
			DbContentHitView1.OkClicked += new EventHandler(showGrid);
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
			DbContentHitGrid1.Visible = false;
			DbContentHitEditor1.Visible = false;
			DbContentHitView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			DbContentHitGrid1.Visible = true;
			DbContentHitEditor1.Visible = false;
			DbContentHitView1.Visible = false;
		}

		private void DbContentHitGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					DbContentHitEditor1.DbContentHitID = 0;
					DbContentHitEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					DbContentHitView1.DbContentHitID = DbContentHitGrid1.SelectedID;
					DbContentHitView1.Visible = true;
					break;
				case "edit":
					resetControls();
					DbContentHitEditor1.DbContentHitID = DbContentHitGrid1.SelectedID;
					DbContentHitEditor1.Visible = true;
					break;
			}
		}
	}
}
