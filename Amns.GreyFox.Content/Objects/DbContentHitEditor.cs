using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentHit.
	/// </summary>
	[ToolboxData("<{0}:DbContentHitEditor runat=server></{0}:DbContentHitEditor>")]
	public class DbContentHitEditor : TableWindow, INamingContainer
	{
		private int dbContentHitID;
		private DbContentHit obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for _system Folder


		#endregion

		#region Private Control Fields for General Folder

		private TextBox tbUserAgent = new TextBox();
		private TextBox tbUserHostAddress = new TextBox();
		private TextBox tbUserHostName = new TextBox();
		private TextBox tbRequestDate = new TextBox();
		private TextBox tbRequestReferrer = new TextBox();
		private CheckBox cbIsUnique = new CheckBox();
		private MultiSelectBox msUser = new MultiSelectBox();
		private MultiSelectBox msRequestContent = new MultiSelectBox();

		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int DbContentHitID
		{
			get
			{
				return dbContentHitID;
			}
			set
			{
				loadFlag = true;
				dbContentHitID = value;
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

			#region Child Controls for General Folder

			msUser.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msUser);

			tbUserAgent.EnableViewState = false;
			Controls.Add(tbUserAgent);

			tbUserHostAddress.EnableViewState = false;
			Controls.Add(tbUserHostAddress);

			tbUserHostName.EnableViewState = false;
			Controls.Add(tbUserHostName);

			tbRequestDate.EnableViewState = false;
			Controls.Add(tbRequestDate);

			tbRequestReferrer.EnableViewState = false;
			Controls.Add(tbRequestReferrer);

			msRequestContent.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msRequestContent);

			cbIsUnique.EnableViewState = false;
			Controls.Add(cbIsUnique);

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
			#region Bind General Child Data

			msUser.Items.Add(new ListItem("Null", "Null"));
			GreyFoxUserManager userManager = new GreyFoxUserManager();
			GreyFoxUserCollection userCollection = userManager.GetCollection(string.Empty, string.Empty, null);
			foreach(GreyFoxUser user in userCollection)
			{
				ListItem i = new ListItem(user.ToString(), user.ID.ToString());
				msUser.Items.Add(i);
			}

			msRequestContent.Items.Add(new ListItem("Null", "Null"));
			DbContentClipManager requestContentManager = new DbContentClipManager();
			DbContentClipCollection requestContentCollection = requestContentManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentClip requestContent in requestContentCollection)
			{
				ListItem i = new ListItem(requestContent.ToString(), requestContent.ID.ToString());
				msRequestContent.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(dbContentHitID == 0)
				obj = new DbContentHit();
			else
				obj = new DbContentHit(dbContentHitID);

			obj.UserAgent = tbUserAgent.Text;
			obj.UserHostAddress = tbUserHostAddress.Text;
			obj.UserHostName = tbUserHostName.Text;
			obj.RequestDate = DateTime.Parse(tbRequestDate.Text);
			obj.RequestReferrer = tbRequestReferrer.Text;
			obj.IsUnique = cbIsUnique.Checked;

			if(msUser.SelectedItem != null && msUser.SelectedItem.Value != "Null")
				obj.User = GreyFoxUser.NewPlaceHolder(
					int.Parse(msUser.SelectedItem.Value));
			else
				obj.User = null;

			if(msRequestContent.SelectedItem != null && msRequestContent.SelectedItem.Value != "Null")
				obj.RequestContent = DbContentClip.NewPlaceHolder(
					int.Parse(msRequestContent.SelectedItem.Value));
			else
				obj.RequestContent = null;

			if(editOnAdd)
				dbContentHitID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbUserAgent.Text = string.Empty;
				tbUserHostAddress.Text = string.Empty;
				tbUserHostName.Text = string.Empty;
				tbRequestDate.Text = DateTime.Now.ToString();
				tbRequestReferrer.Text = string.Empty;
				cbIsUnique.Checked = false;
				msUser.SelectedIndex = 0;
				msRequestContent.SelectedIndex = 0;
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

			Tab GeneralTab = new Tab("General");
			GeneralTab.Visible = true;
			GeneralTab.RenderDiv += new TabRenderHandler(renderGeneralFolder);
			tabStrip.Tabs.Add(GeneralTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(dbContentHitID > 0)
				{
					obj = new DbContentHit(dbContentHitID);
					text = "Edit  - " + obj.ToString();
				}
				else if(dbContentHitID <= 0)
				{
					obj = new DbContentHit();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbUserAgent.Text = obj.UserAgent;
				tbUserHostAddress.Text = obj.UserHostAddress;
				tbUserHostName.Text = obj.UserHostName;
				tbRequestDate.Text = obj.RequestDate.ToString();
				tbRequestReferrer.Text = obj.RequestReferrer;
				cbIsUnique.Checked = obj.IsUnique;

				//
				// Set Children Selections
				//
				if(obj.User != null)
					foreach(ListItem item in msUser.Items)
						item.Selected = obj.User.ID.ToString() == item.Value;
					else
						msUser.SelectedIndex = 0;

				if(obj.RequestContent != null)
					foreach(ListItem item in msRequestContent.Items)
						item.Selected = obj.RequestContent.ID.ToString() == item.Value;
					else
						msRequestContent.SelectedIndex = 0;

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

		private void renderGeneralFolder(HtmlTextWriter output)
		{
			//
			// Render User
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("User");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msUser.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render UserAgent
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("UserAgent");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUserAgent.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render UserHostAddress
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("UserHostAddress");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUserHostAddress.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render UserHostName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("UserHostName");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUserHostName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render RequestDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("RequestDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbRequestDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render RequestReferrer
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("RequestReferrer");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbRequestReferrer.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render RequestContent
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("RequestContent");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msRequestContent.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsUnique
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IsUnique");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbIsUnique.RenderControl(output);
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
					dbContentHitID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = dbContentHitID;
			return myState;
		}
	}
}

