using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.ContentWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class DbContentRatingPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbContentRatingGrid1.ToolbarClicked += new ToolbarEventHandler(DbContentRatingGrid1_ToolbarClicked);
			DbContentRatingEditor1.Cancelled += new EventHandler(showGrid);
			DbContentRatingEditor1.Updated += new EventHandler(showGrid);
			DbContentRatingView1.OkClicked += new EventHandler(showGrid);
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
			DbContentRatingGrid1.Visible = false;
			DbContentRatingEditor1.Visible = false;
			DbContentRatingView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			DbContentRatingGrid1.Visible = true;
			DbContentRatingEditor1.Visible = false;
			DbContentRatingView1.Visible = false;
		}

		private void DbContentRatingGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					DbContentRatingEditor1.DbContentRatingID = 0;
					DbContentRatingEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					DbContentRatingView1.DbContentRatingID = DbContentRatingGrid1.SelectedID;
					DbContentRatingView1.Visible = true;
					break;
				case "edit":
					resetControls();
					DbContentRatingEditor1.DbContentRatingID = DbContentRatingGrid1.SelectedID;
					DbContentRatingEditor1.Visible = true;
					break;
			}
		}
	}
}
