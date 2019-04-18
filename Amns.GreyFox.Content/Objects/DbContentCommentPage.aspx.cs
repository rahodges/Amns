using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.ContentWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class DbContentCommentPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbContentCommentGrid1.ToolbarClicked += new ToolbarEventHandler(DbContentCommentGrid1_ToolbarClicked);
			DbContentCommentEditor1.Cancelled += new EventHandler(showGrid);
			DbContentCommentEditor1.Updated += new EventHandler(showGrid);
			DbContentCommentView1.OkClicked += new EventHandler(showGrid);
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
			DbContentCommentGrid1.Visible = false;
			DbContentCommentEditor1.Visible = false;
			DbContentCommentView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			DbContentCommentGrid1.Visible = true;
			DbContentCommentEditor1.Visible = false;
			DbContentCommentView1.Visible = false;
		}

		private void DbContentCommentGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					DbContentCommentEditor1.DbContentCommentID = 0;
					DbContentCommentEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					DbContentCommentView1.DbContentCommentID = DbContentCommentGrid1.SelectedID;
					DbContentCommentView1.Visible = true;
					break;
				case "edit":
					resetControls();
					DbContentCommentEditor1.DbContentCommentID = DbContentCommentGrid1.SelectedID;
					DbContentCommentEditor1.Visible = true;
					break;
			}
		}
	}
}
