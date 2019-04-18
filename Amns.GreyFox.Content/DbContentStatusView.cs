using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentStatus.
	/// </summary>
	[ToolboxData("<DbContentStatus:DbContentStatusView runat=server></{0}:DbContentStatusView>")]
	public class DbContentStatusView : TableWindow, INamingContainer
	{
		private int dbContentStatusID;
		private DbContentStatus dbContentStatus;

		#region Private Control Fields for _system Folder


		#endregion

		#region Private Control Fields for System Folder

		private Literal ltName = new Literal();
		private Literal ltIsDraft = new Literal();
		private Literal ltIsPublished = new Literal();
		private Literal ltFeeEnabled = new Literal();
		private Literal ltEditEnabled = new Literal();
		private Literal ltArchiveEnabled = new Literal();
		private Literal ltReviewEnabled = new Literal();
		private Literal ltIcon = new Literal();

		#endregion

		private Button btOk = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int DbContentStatusID
		{
			get
			{
				return dbContentStatusID;
			}
			set
			{
				dbContentStatusID = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for System Folder

			ltName.EnableViewState = false;
			Controls.Add(ltName);

			ltIsDraft.EnableViewState = false;
			Controls.Add(ltIsDraft);

			ltIsPublished.EnableViewState = false;
			Controls.Add(ltIsPublished);

			ltFeeEnabled.EnableViewState = false;
			Controls.Add(ltFeeEnabled);

			ltEditEnabled.EnableViewState = false;
			Controls.Add(ltEditEnabled);

			ltArchiveEnabled.EnableViewState = false;
			Controls.Add(ltArchiveEnabled);

			ltReviewEnabled.EnableViewState = false;
			Controls.Add(ltReviewEnabled);

			ltIcon.EnableViewState = false;
			Controls.Add(ltIcon);

			#endregion

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btDelete.Text = "Delete";
			btDelete.Width = Unit.Pixel(72);
			btDelete.EnableViewState = false;
			btDelete.Click += new EventHandler(delete_Click);
			Controls.Add(btDelete);

			ChildControlsCreated = true;
		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			OnOkClicked(EventArgs.Empty);
		}

		#endregion

		protected void delete_Click(object sender, EventArgs e)
		{
			this.OnDeleteClicked(EventArgs.Empty);
		}

		public event EventHandler OkClicked;
		protected virtual void OnOkClicked(EventArgs e)
		{
			if(OkClicked != null)
				OkClicked(this, e);
		}

		public event EventHandler DeleteClicked;
		protected virtual void OnDeleteClicked(EventArgs e)
		{
			if(DeleteClicked != null)
			DeleteClicked(this, e);
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation | 
				TableWindowFeatures.WindowPrinter;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if(dbContentStatusID != 0)
			{
				dbContentStatus = new DbContentStatus(dbContentStatusID);

				#region Bind _system Folder

				//
				// Set Field Entries
				//



				#endregion

				#region Bind System Folder

				//
				// Set Field Entries
				//

				ltName.Text = dbContentStatus.Name.ToString();
				ltIsDraft.Text = dbContentStatus.IsDraft.ToString();
				ltIsPublished.Text = dbContentStatus.IsPublished.ToString();
				ltFeeEnabled.Text = dbContentStatus.FeeEnabled.ToString();
				ltEditEnabled.Text = dbContentStatus.EditEnabled.ToString();
				ltArchiveEnabled.Text = dbContentStatus.ArchiveEnabled.ToString();
				ltReviewEnabled.Text = dbContentStatus.ReviewEnabled.ToString();
				ltIcon.Text = dbContentStatus.Icon.ToString();


				#endregion

				text = "View  - " + dbContentStatus.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "DbContentStatus ID", dbContentStatusID.ToString());
			output.WriteEndTag("tr");

			render_systemFolder(output);

			renderSystemFolder(output);

			//
			// Render OK/Cancel Buttons
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan", "2");
			output.WriteAttribute("align", "right");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			btOk.RenderControl(output);
			if(DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		#region Render _system Folder

		private void render_systemFolder(HtmlTextWriter output)
		{
			//
			// Render _system Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("System Folder");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render System Folder

		private void renderSystemFolder(HtmlTextWriter output)
		{
			//
			// Render System Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("System");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Name
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Name");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsDraft
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IsDraft");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltIsDraft.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsPublished
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IsPublished");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltIsPublished.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render FeeEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("FeeEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltFeeEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EditEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EditEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEditEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ArchiveEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ArchiveEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltArchiveEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ReviewEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ReviewEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltReviewEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Icon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Icon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null)
					base.LoadViewState(myState[0]);
				if(myState[1] != null)
					dbContentStatusID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = dbContentStatusID;
			return myState;
		}
	}
}
