using System;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;

namespace YariWeb.Administration
{
	/// <summary>
	/// Default DbModel Class Page Codebehind
	/// </summary>
	public partial class YariMediaTypePage : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			YariMediaTypeGrid1.ToolbarClicked += new ToolbarEventHandler(YariMediaTypeGrid1_ToolbarClicked);
			YariMediaTypeEditor1.Cancelled += new EventHandler(showGrid);
			YariMediaTypeEditor1.Updated += new EventHandler(showGrid);
			YariMediaTypeView1.OkClicked += new EventHandler(showGrid);
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
			YariMediaTypeGrid1.Visible = false;
			YariMediaTypeEditor1.Visible = false;
			YariMediaTypeView1.Visible = false;
		}

		private void showGrid(object sender, EventArgs e)
		{
			YariMediaTypeGrid1.Visible = true;
			YariMediaTypeEditor1.Visible = false;
			YariMediaTypeView1.Visible = false;
		}

		private void YariMediaTypeGrid1_ToolbarClicked(object sender, ToolbarEventArgs e)
		{
			switch(e.SelectedToolbarItem.Command)
			{
				case "new":
					resetControls();
					YariMediaTypeEditor1.YariMediaTypeID = 0;
					YariMediaTypeEditor1.Visible = true;
					break;
				case "view":
					resetControls();
					YariMediaTypeView1.YariMediaTypeID = YariMediaTypeGrid1.SelectedID;
					YariMediaTypeView1.Visible = true;
					break;
				case "edit":
					resetControls();
					YariMediaTypeEditor1.YariMediaTypeID = YariMediaTypeGrid1.SelectedID;
					YariMediaTypeEditor1.Visible = true;
					break;
			}
		}
	}
}
