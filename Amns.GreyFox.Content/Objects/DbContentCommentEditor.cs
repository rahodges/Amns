using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentComment.
	/// </summary>
	[ToolboxData("<{0}:DbContentCommentEditor runat=server></{0}:DbContentCommentEditor>")]
	public class DbContentCommentEditor : TableWindow, INamingContainer
	{
		private int dbContentCommentID;
		private DbContentComment obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for _system Folder

		private Literal ltCreateDate = new Literal();
		private Literal ltModifyDate = new Literal();

		#endregion

		#region Private Control Fields for New_Folder Folder

		private TextBox tbName = new TextBox();
		private TextBox tbEmail = new TextBox();
		private TextBox tbUrl = new TextBox();
		private TextBox tbIP = new TextBox();
		private TextBox tbBody = new TextBox();

		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int DbContentCommentID
		{
			get
			{
				return dbContentCommentID;
			}
			set
			{
				loadFlag = true;
				dbContentCommentID = value;
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

			#region Child Controls for _system Folder

			ltCreateDate.EnableViewState = false;
			Controls.Add(ltCreateDate);

			ltModifyDate.EnableViewState = false;
			Controls.Add(ltModifyDate);

			#endregion

			#region Child Controls for New Folder Folder

			tbName.EnableViewState = false;
			Controls.Add(tbName);

			tbEmail.EnableViewState = false;
			Controls.Add(tbEmail);

			tbUrl.EnableViewState = false;
			Controls.Add(tbUrl);

			tbIP.EnableViewState = false;
			Controls.Add(tbIP);

			tbBody.EnableViewState = false;
			Controls.Add(tbBody);

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
			if(dbContentCommentID == 0)
				obj = new DbContentComment();
			else
				obj = new DbContentComment(dbContentCommentID);

			obj.Name = tbName.Text;
			obj.Email = tbEmail.Text;
			obj.Url = tbUrl.Text;
			obj.IP = tbIP.Text;
			obj.Body = tbBody.Text;

			if(editOnAdd)
				dbContentCommentID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbName.Text = string.Empty;
				tbEmail.Text = string.Empty;
				tbUrl.Text = string.Empty;
				tbIP.Text = string.Empty;
				tbBody.Text = string.Empty;
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

			Tab New_FolderTab = new Tab("New Folder");
			New_FolderTab.Visible = true;
			New_FolderTab.RenderDiv += new TabRenderHandler(renderNew_FolderFolder);
			tabStrip.Tabs.Add(New_FolderTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(dbContentCommentID > 0)
				{
					obj = new DbContentComment(dbContentCommentID);
					text = "Edit  - " + obj.ToString();
				}
				else if(dbContentCommentID <= 0)
				{
					obj = new DbContentComment();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				ltCreateDate.Text = obj.CreateDate.ToString();
				ltModifyDate.Text = obj.ModifyDate.ToString();
				tbName.Text = obj.Name;
				tbEmail.Text = obj.Email;
				tbUrl.Text = obj.Url;
				tbIP.Text = obj.IP;
				tbBody.Text = obj.Body;

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
			//
			// Render CreateDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("CreateDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			ltCreateDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ModifyDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ModifyDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			ltModifyDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderNew_FolderFolder(HtmlTextWriter output)
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
			// Render Email
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEmail.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Url
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Url");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUrl.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IP
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IP");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbIP.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Body
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Body");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbBody.RenderControl(output);
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
					dbContentCommentID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = dbContentCommentID;
			return myState;
		}
	}
}

