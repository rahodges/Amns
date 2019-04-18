using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace YariWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class YariMediaRecordPage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			YariMediaRecordGrid1.ToolbarClicked += new ToolbarEventHandler(YariMediaRecordGrid1_ToolbarClicked);
			YariMediaRecordEditor1.Cancelled += new EventHandler(showGrid);
			YariMediaRecordEditor1.Updated += new EventHandler(showGrid);
			YariMediaRecordView1.OkClicked += new EventHandler(showGrid);
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
			YariMediaRecordGrid1.Visible = false;
			YariMediaRecordEditor1.Visible = false;
			YariMediaRecordView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			YariMediaRecordGrid1.Visible = true;
			YariMediaRecordEditor1.Visible = false;
			YariMediaRecordView1.Visible = false;
		}

		private void YariMediaRecordGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					YariMediaRecordEditor1.YariMediaRecordID = 0;
					YariMediaRecordEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					YariMediaRecordView1.YariMediaRecordID = YariMediaRecordGrid1.SelectedID;
					YariMediaRecordView1.Visible = true;
					break;
				case "edit":
					resetControls();
					YariMediaRecordEditor1.YariMediaRecordID = YariMediaRecordGrid1.SelectedID;
					YariMediaRecordEditor1.Visible = true;
					break;
			}
		}
	}
}
