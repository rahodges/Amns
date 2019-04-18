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
	[ToolboxData("<{0}:DbContentStatusEditor runat=server></{0}:DbContentStatusEditor>")]
	public class DbContentStatusEditor : TableWindow, INamingContainer
	{
		private int dbContentStatusID;
		private DbContentStatus obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for _system Folder


		#endregion

		#region Private Control Fields for System Folder

		private TextBox tbName = new TextBox();
		private CheckBox cbIsDraft = new CheckBox();
		private CheckBox cbIsPublished = new CheckBox();
		private CheckBox cbFeeEnabled = new CheckBox();
		private CheckBox cbEditEnabled = new CheckBox();
		private CheckBox cbArchiveEnabled = new CheckBox();
		private CheckBox cbReviewEnabled = new CheckBox();
		private TextBox tbIcon = new TextBox();

		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
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
				loadFlag = true;
				dbContentStatusID = value;
			}
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool ResetOnAdd
		{
			get
			{
				return resetOnAdd;
			}
			set
			{
				resetOnAdd = value;
			}
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool EditOnAdd
		{
			get
			{
				return editOnAdd;
			}
			set
			{
				editOnAdd = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();
			bindDropDownLists();

			#region Child Controls for System Folder

			tbName.EnableViewState = false;
			Controls.Add(tbName);

			cbIsDraft.EnableViewState = false;
			Controls.Add(cbIsDraft);

			cbIsPublished.EnableViewState = false;
			Controls.Add(cbIsPublished);

			cbFeeEnabled.EnableViewState = false;
			Controls.Add(cbFeeEnabled);

			cbEditEnabled.EnableViewState = false;
			Controls.Add(cbEditEnabled);

			cbArchiveEnabled.EnableViewState = false;
			Controls.Add(cbArchiveEnabled);

			cbReviewEnabled.EnableViewState = false;
			Controls.Add(cbReviewEnabled);

			tbIcon.EnableViewState = false;
			Controls.Add(tbIcon);

			#endregion

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btCancel.Text = "Cancel";
			btCancel.Width = Unit.Pixel(72);
			btCancel.EnableViewState = false;
			btCancel.CausesValidation = false;
			btCancel.Click += new EventHandler(cancel_Click);
			Controls.Add(btCancel);

			btDelete.Text = "Delete";
			btDelete.Width = Unit.Pixel(72);
			btDelete.EnableViewState = false;
			btDelete.Click += new EventHandler(delete_Click);
			Controls.Add(btDelete);

			ChildControlsCreated = true;
		}

		private void bindDropDownLists()
		{
		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(dbContentStatusID == 0)
				obj = new DbContentStatus();
			else
				obj = new DbContentStatus(dbContentStatusID);

			obj.Name = tbName.Text;
			obj.IsDraft = cbIsDraft.Checked;
			obj.IsPublished = cbIsPublished.Checked;
			obj.FeeEnabled = cbFeeEnabled.Checked;
			obj.EditEnabled = cbEditEnabled.Checked;
			obj.ArchiveEnabled = cbArchiveEnabled.Checked;
			obj.ReviewEnabled = cbReviewEnabled.Checked;
			obj.Icon = tbIcon.Text;

			if(editOnAdd)
				dbContentStatusID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbName.Text = string.Empty;
				cbIsDraft.Checked = false;
				cbIsPublished.Checked = false;
				cbFeeEnabled.Checked = false;
				cbEditEnabled.Checked = false;
				cbArchiveEnabled.Checked = false;
				cbReviewEnabled.Checked = false;
				tbIcon.Text = string.Empty;
			}

			OnUpdated(EventArgs.Empty);
		}

		#endregion

		protected void cancel_Click(object sender, EventArgs e)
		{
			this.OnCancelled(EventArgs.Empty);
		}

		protected void delete_Click(object sender, EventArgs e)
		{
			this.OnDeleteClicked(EventArgs.Empty);
		}

		public event EventHandler Cancelled;
		protected virtual void OnCancelled(EventArgs e)
		{
			if(Cancelled != null)
				Cancelled(this, e);
		}

		public event EventHandler Updated;
		protected virtual void OnUpdated(EventArgs e)
		{
			if(Updated != null)
				Updated(this, e);
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
			features = TableWindowFeatures.DisableContentSeparation;
			components = TableWindowComponents.Tabs;
			tabStrip = new TabStrip();
			tabStrip.Tabs = new TabList();

			Tab SystemTab = new Tab("System");
			SystemTab.Visible = true;
			SystemTab.RenderDiv += new TabRenderHandler(renderSystemFolder);
			tabStrip.Tabs.Add(SystemTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(dbContentStatusID > 0)
				{
					obj = new DbContentStatus(dbContentStatusID);
					text = "Edit  - " + obj.ToString();
				}
				else if(dbContentStatusID <= 0)
				{
					obj = new DbContentStatus();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbName.Text = obj.Name;
				cbIsDraft.Checked = obj.IsDraft;
				cbIsPublished.Checked = obj.IsPublished;
				cbFeeEnabled.Checked = obj.FeeEnabled;
				cbEditEnabled.Checked = obj.EditEnabled;
				cbArchiveEnabled.Checked = obj.ArchiveEnabled;
				cbReviewEnabled.Checked = obj.ReviewEnabled;
				tbIcon.Text = obj.Icon;

			}
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			RenderTabPanels(output);
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
			output.Write("&nbsp;");
			btCancel.RenderControl(output);
			if(DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		private void render_systemFolder(HtmlTextWriter output)
		{
		}

		private void renderSystemFolder(HtmlTextWriter output)
		{
			//
			// Render Name
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Name");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsDraft
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IsDraft");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbIsDraft.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsPublished
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IsPublished");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbIsPublished.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render FeeEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("FeeEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbFeeEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EditEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EditEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbEditEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ArchiveEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ArchiveEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbArchiveEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ReviewEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ReviewEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbReviewEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Icon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Icon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

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

