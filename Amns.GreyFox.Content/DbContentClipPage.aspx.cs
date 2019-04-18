using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.ContentWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class DbContentClipPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			DbContentClipGrid1.ToolbarClicked += new ToolbarEventHandler(DbContentClipGrid1_ToolbarClicked);
			DbContentClipEditor1.Cancelled += new EventHandler(showGrid);
			DbContentClipEditor1.Updated += new EventHandler(showGrid);
			DbContentClipView1.OkClicked += new EventHandler(showGrid);
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
			DbContentClipGrid1.Visible = false;
			DbContentClipEditor1.Visible = false;
			DbContentClipView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			DbContentClipGrid1.Visible = true;
			DbContentClipEditor1.Visible = false;
			DbContentClipView1.Visible = false;
		}

		private void DbContentClipGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					DbContentClipEditor1.DbContentClipID = 0;
					DbContentClipEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					DbContentClipView1.DbContentClipID = DbContentClipGrid1.SelectedID;
					DbContentClipView1.Visible = true;
					break;
				case "edit":
					resetControls();
					DbContentClipEditor1.DbContentClipID = DbContentClipGrid1.SelectedID;
					DbContentClipEditor1.Visible = true;
					break;
			}
		}
	}
}
