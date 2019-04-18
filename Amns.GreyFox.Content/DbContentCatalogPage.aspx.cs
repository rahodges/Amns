using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.ContentWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class DbContentCatalogPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbContentCatalogGrid1.ToolbarClicked += new ToolbarEventHandler(DbContentCatalogGrid1_ToolbarClicked);
			DbContentCatalogEditor1.Cancelled += new EventHandler(showGrid);
			DbContentCatalogEditor1.Updated += new EventHandler(showGrid);
			DbContentCatalogView1.OkClicked += new EventHandler(showGrid);
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
			DbContentCatalogGrid1.Visible = false;
			DbContentCatalogEditor1.Visible = false;
			DbContentCatalogView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			DbContentCatalogGrid1.Visible = true;
			DbContentCatalogEditor1.Visible = false;
			DbContentCatalogView1.Visible = false;
		}

		private void DbContentCatalogGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					DbContentCatalogEditor1.DbContentCatalogID = 0;
					DbContentCatalogEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					DbContentCatalogView1.DbContentCatalogID = DbContentCatalogGrid1.SelectedID;
					DbContentCatalogView1.Visible = true;
					break;
				case "edit":
					resetControls();
					DbContentCatalogEditor1.DbContentCatalogID = DbContentCatalogGrid1.SelectedID;
					DbContentCatalogEditor1.Visible = true;
					break;
			}
		}
	}
}
