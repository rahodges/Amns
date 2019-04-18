using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace GreyFoxWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class GreyFoxEventPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			GreyFoxEventGrid1.ToolbarClicked += new ToolbarEventHandler(GreyFoxEventGrid1_ToolbarClicked);
			GreyFoxEventEditor1.Cancelled += new EventHandler(showGrid);
			GreyFoxEventEditor1.Updated += new EventHandler(showGrid);
			GreyFoxEventView1.OkClicked += new EventHandler(showGrid);
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
			GreyFoxEventGrid1.Visible = false;
			GreyFoxEventEditor1.Visible = false;
			GreyFoxEventView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			GreyFoxEventGrid1.Visible = true;
			GreyFoxEventEditor1.Visible = false;
			GreyFoxEventView1.Visible = false;
		}

		private void GreyFoxEventGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					GreyFoxEventEditor1.GreyFoxEventID = 0;
					GreyFoxEventEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					GreyFoxEventView1.GreyFoxEventID = GreyFoxEventGrid1.SelectedID;
					GreyFoxEventView1.Visible = true;
					break;
				case "edit":
					resetControls();
					GreyFoxEventEditor1.GreyFoxEventID = GreyFoxEventGrid1.SelectedID;
					GreyFoxEventEditor1.Visible = true;
					break;
			}
		}
	}
}
