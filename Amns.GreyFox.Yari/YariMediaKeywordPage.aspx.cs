using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace YariWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class YariMediaKeywordPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			YariMediaKeywordGrid1.ToolbarClicked += new ToolbarEventHandler(YariMediaKeywordGrid1_ToolbarClicked);
			YariMediaKeywordEditor1.Cancelled += new EventHandler(showGrid);
			YariMediaKeywordEditor1.Updated += new EventHandler(showGrid);
			YariMediaKeywordView1.OkClicked += new EventHandler(showGrid);
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
			YariMediaKeywordGrid1.Visible = false;
			YariMediaKeywordEditor1.Visible = false;
			YariMediaKeywordView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			YariMediaKeywordGrid1.Visible = true;
			YariMediaKeywordEditor1.Visible = false;
			YariMediaKeywordView1.Visible = false;
		}

		private void YariMediaKeywordGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					YariMediaKeywordEditor1.YariMediaKeywordID = 0;
					YariMediaKeywordEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					YariMediaKeywordView1.YariMediaKeywordID = YariMediaKeywordGrid1.SelectedID;
					YariMediaKeywordView1.Visible = true;
					break;
				case "edit":
					resetControls();
					YariMediaKeywordEditor1.YariMediaKeywordID = YariMediaKeywordGrid1.SelectedID;
					YariMediaKeywordEditor1.Visible = true;
					break;
			}
		}
	}
}
