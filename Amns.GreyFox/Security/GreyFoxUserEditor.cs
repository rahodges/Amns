using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.People;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Security.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxUser.
	/// </summary>
	[ToolboxData("<{0}:GreyFoxUserEditor runat=server></{0}:GreyFoxUserEditor>")]
	public class GreyFoxUserEditor : TableWindow, INamingContainer
	{
		private int greyFoxUserID;
		private GreyFoxUser obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for Default Folder

		private TextBox tbUserName = new TextBox();
		private CheckBox cbIsDisabled = new CheckBox();
		private TextBox tbLoginDate = new TextBox();
		private TextBox tbLoginCount = new TextBox();
		private RegularExpressionValidator revLoginCount = new RegularExpressionValidator();
		private TextBox tbLoginPassword = new TextBox();
		private TextBox tbActivationID = new TextBox();
		private MultiSelectBox msContact = new MultiSelectBox();
		private MultiSelectBox msRoles = new MultiSelectBox();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxUserID
		{
			get
			{
				return greyFoxUserID;
			}
			set
			{
				loadFlag = true;
				greyFoxUserID = value;
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

			#region Child Controls for Default Folder

			tbUserName.EnableViewState = false;
			Controls.Add(tbUserName);

			cbIsDisabled.EnableViewState = false;
			Controls.Add(cbIsDisabled);

			tbLoginDate.EnableViewState = false;
			Controls.Add(tbLoginDate);

			tbLoginCount.ID = this.ID + "_LoginCount";
			tbLoginCount.EnableViewState = false;
			Controls.Add(tbLoginCount);
			revLoginCount.ControlToValidate = tbLoginCount.ID;
			revLoginCount.ValidationExpression = "^(\\+|-)?\\d+$";
			revLoginCount.ErrorMessage = "*";
			revLoginCount.Display = ValidatorDisplay.Dynamic;
			revLoginCount.EnableViewState = false;
			Controls.Add(revLoginCount);

			tbLoginPassword.EnableViewState = false;
			Controls.Add(tbLoginPassword);

			msContact.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msContact);

			msRoles.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msRoles);

			tbActivationID.EnableViewState = false;
			Controls.Add(tbActivationID);

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
			#region Bind Default Child Data

			msContact.Items.Add(new ListItem("Null", "Null"));
			GreyFoxContactManager contactManager = new GreyFoxContactManager("sysGlobal_Contacts");
			GreyFoxContactCollection contactCollection = contactManager.GetCollection(string.Empty, string.Empty);
			foreach(GreyFoxContact contact in contactCollection)
			{
				ListItem i = new ListItem(contact.ToString(), contact.ID.ToString());
				msContact.Items.Add(i);
			}

			msRoles.Items.Add(new ListItem("Null", "Null"));
			GreyFoxRoleManager rolesManager = new GreyFoxRoleManager();
			GreyFoxRoleCollection rolesCollection = rolesManager.GetCollection(string.Empty, string.Empty);
			foreach(GreyFoxRole roles in rolesCollection)
			{
				ListItem i = new ListItem(roles.ToString(), roles.ID.ToString());
				msRoles.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(greyFoxUserID == 0)
				obj = new GreyFoxUser();
			else
				obj = new GreyFoxUser(greyFoxUserID);

			obj.UserName = tbUserName.Text;
			obj.IsDisabled = cbIsDisabled.Checked;
			obj.LoginDate = DateTime.Parse(tbLoginDate.Text);
			obj.LoginCount = int.Parse(tbLoginCount.Text);
			obj.LoginPassword = tbLoginPassword.Text;
			obj.ActivationID = tbActivationID.Text;

			if(msContact.SelectedItem != null && msContact.SelectedItem.Value != "Null")
				obj.Contact = GreyFoxContact.NewPlaceHolder("sysGlobal_Contacts", 
					int.Parse(msContact.SelectedItem.Value));
			else
				obj.Contact = null;

			if(msRoles.IsChanged)
			{
				obj.Roles = new GreyFoxRoleCollection();
				foreach(ListItem i in msRoles.Items)
					if(i.Selected)
						obj.Roles.Add(GreyFoxRole.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(editOnAdd)
				greyFoxUserID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbUserName.Text = string.Empty;
				cbIsDisabled.Checked = false;
				tbLoginDate.Text = DateTime.Now.ToString();
				tbLoginCount.Text = string.Empty;
				tbLoginPassword.Text = string.Empty;
				tbActivationID.Text = string.Empty;
				msContact.SelectedIndex = 0;
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

			Tab DefaultTab = new Tab("Default");
			DefaultTab.Visible = true;
			DefaultTab.RenderDiv += new TabRenderHandler(renderDefaultFolder);
			DefaultTab.Visible = true;
			tabStrip.Tabs.Add(DefaultTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(greyFoxUserID > 0)
				{
					obj = new GreyFoxUser(greyFoxUserID);
					text = "Edit  - " + obj.ToString();
				}
				else if(greyFoxUserID <= 0)
				{
					obj = new GreyFoxUser();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbUserName.Text = obj.UserName;
				cbIsDisabled.Checked = obj.IsDisabled;
				tbLoginDate.Text = obj.LoginDate.ToString();
				tbLoginCount.Text = obj.LoginCount.ToString();
				tbLoginPassword.Text = obj.LoginPassword;
				tbActivationID.Text = obj.ActivationID;

				//
				// Set Children Selections
				//
				if(obj.Contact != null)
					foreach(ListItem item in msContact.Items)
						item.Selected = obj.Contact.ID.ToString() == item.Value;
					else
						msContact.SelectedIndex = 0;

				foreach(ListItem i in msRoles.Items)
					foreach(GreyFoxRole greyFoxRole in obj.Roles)
						if(i.Value == greyFoxRole.ID.ToString())
						{
							i.Selected = true;
							break;
						}
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

		private void renderDefaultFolder(HtmlTextWriter output)
		{
			//
			// Render UserName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Username");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUserName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsDisabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Disable user account.");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbIsDisabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LoginDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("LoginDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLoginDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LoginCount
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("LoginCount");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLoginCount.RenderControl(output);
			revLoginCount.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LoginPassword
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("LoginPassword");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLoginPassword.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Contact
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Contact");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msContact.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Roles
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Roles");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msRoles.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ActivationID
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ActivationID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbActivationID.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void render_systemFolder(HtmlTextWriter output)
		{
		}

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null)
					base.LoadViewState(myState[0]);
				if(myState[1] != null)
					greyFoxUserID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxUserID;
			return myState;
		}
	}
}

