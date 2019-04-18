using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace GreyFoxWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class GreyFoxUserPreferencePage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			GreyFoxUserPreferenceGrid1.ToolbarClicked += new ToolbarEventHandler(GreyFoxUserPreferenceGrid1_ToolbarClicked);
			GreyFoxUserPreferenceEditor1.Cancelled += new EventHandler(showGrid);
			GreyFoxUserPreferenceEditor1.Updated += new EventHandler(showGrid);
			GreyFoxUserPreferenceView1.OkClicked += new EventHandler(showGrid);
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
			GreyFoxUserPreferenceGrid1.Visible = false;
			GreyFoxUserPreferenceEditor1.Visible = false;
			GreyFoxUserPreferenceView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			GreyFoxUserPreferenceGrid1.Visible = true;
			GreyFoxUserPreferenceEditor1.Visible = false;
			GreyFoxUserPreferenceView1.Visible = false;
		}

		private void GreyFoxUserPreferenceGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					GreyFoxUserPreferenceEditor1.GreyFoxUserPreferenceID = 0;
					GreyFoxUserPreferenceEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					GreyFoxUserPreferenceView1.GreyFoxUserPreferenceID = GreyFoxUserPreferenceGrid1.SelectedID;
					GreyFoxUserPreferenceView1.Visible = true;
					break;
				case "edit":
					resetControls();
					GreyFoxUserPreferenceEditor1.GreyFoxUserPreferenceID = GreyFoxUserPreferenceGrid1.SelectedID;
					GreyFoxUserPreferenceEditor1.Visible = true;
					break;
			}
		}
	}
}
