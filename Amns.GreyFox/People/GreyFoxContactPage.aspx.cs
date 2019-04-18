using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace GreyFoxWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class GreyFoxContactPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			GreyFoxContactGrid1.ToolbarClicked += new ToolbarEventHandler(GreyFoxContactGrid1_ToolbarClicked);
			GreyFoxContactEditor1.Cancelled += new EventHandler(showGrid);
			GreyFoxContactEditor1.Updated += new EventHandler(showGrid);
			GreyFoxContactView1.OkClicked += new EventHandler(showGrid);
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
			GreyFoxContactGrid1.Visible = false;
			GreyFoxContactEditor1.Visible = false;
			GreyFoxContactView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			GreyFoxContactGrid1.Visible = true;
			GreyFoxContactEditor1.Visible = false;
			GreyFoxContactView1.Visible = false;
		}

		private void GreyFoxContactGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					GreyFoxContactEditor1.GreyFoxContactID = 0;
					GreyFoxContactEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					GreyFoxContactView1.GreyFoxContactID = GreyFoxContactGrid1.SelectedID;
					GreyFoxContactView1.Visible = true;
					break;
				case "edit":
					resetControls();
					GreyFoxContactEditor1.GreyFoxContactID = GreyFoxContactGrid1.SelectedID;
					GreyFoxContactEditor1.Visible = true;
					break;
			}
		}
	}
}
