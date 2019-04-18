using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace GreyFoxWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class GreyFoxUserPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			GreyFoxUserGrid1.ToolbarClicked += new ToolbarEventHandler(GreyFoxUserGrid1_ToolbarClicked);
			GreyFoxUserEditor1.Cancelled += new EventHandler(showGrid);
			GreyFoxUserEditor1.Updated += new EventHandler(showGrid);
			GreyFoxUserView1.OkClicked += new EventHandler(showGrid);
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
			GreyFoxUserGrid1.Visible = false;
			GreyFoxUserEditor1.Visible = false;
			GreyFoxUserView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			GreyFoxUserGrid1.Visible = true;
			GreyFoxUserEditor1.Visible = false;
			GreyFoxUserView1.Visible = false;
		}

		private void GreyFoxUserGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					GreyFoxUserEditor1.GreyFoxUserID = 0;
					GreyFoxUserEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					GreyFoxUserView1.GreyFoxUserID = GreyFoxUserGrid1.SelectedID;
					GreyFoxUserView1.Visible = true;
					break;
				case "edit":
					resetControls();
					GreyFoxUserEditor1.GreyFoxUserID = GreyFoxUserGrid1.SelectedID;
					GreyFoxUserEditor1.Visible = true;
					break;
			}
		}
	}
}
