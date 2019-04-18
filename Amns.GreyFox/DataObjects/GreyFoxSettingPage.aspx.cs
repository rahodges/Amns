using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace GreyFoxWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public class GreyFoxSettingPage : System.Web.UI.Page
	{
		protected Amns.GreyFox.Web.UI.WebControls.GreyFoxSettingGrid GreyFoxSettingGrid1;
		protected Amns.GreyFox.Web.UI.WebControls.GreyFoxSettingEditor GreyFoxSettingEditor1;
		protected Amns.GreyFox.Web.UI.WebControls.GreyFoxSettingView GreyFoxSettingView1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			GreyFoxSettingGrid1.ToolbarClicked += new ToolbarEventHandler(GreyFoxSettingGrid1_ToolbarClicked);
			GreyFoxSettingEditor1.Cancelled += new EventHandler(showGrid);
			GreyFoxSettingEditor1.Updated += new EventHandler(showGrid);
			GreyFoxSettingView1.OkClicked += new EventHandler(showGrid);
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
			GreyFoxSettingGrid1.Visible = false;
			GreyFoxSettingEditor1.Visible = false;
			GreyFoxSettingView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			GreyFoxSettingGrid1.Visible = true;
			GreyFoxSettingEditor1.Visible = false;
			GreyFoxSettingView1.Visible = false;
		}

		private void GreyFoxSettingGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					GreyFoxSettingEditor1.GreyFoxSettingID = 0;
					GreyFoxSettingEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					GreyFoxSettingView1.GreyFoxSettingID = GreyFoxSettingGrid1.SelectedID;
					GreyFoxSettingView1.Visible = true;
					break;
				case "edit":
					resetControls();
					GreyFoxSettingEditor1.GreyFoxSettingID = GreyFoxSettingGrid1.SelectedID;
					GreyFoxSettingEditor1.Visible = true;
					break;
			}
		}
	}
}
