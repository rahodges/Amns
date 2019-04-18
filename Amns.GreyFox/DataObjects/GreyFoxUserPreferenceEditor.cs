using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxUserPreference.
	/// </summary>
	[ToolboxData("<{0}:GreyFoxUserPreferenceEditor runat=server></{0}:GreyFoxUserPreferenceEditor>")]
	public class GreyFoxUserPreferenceEditor : TableWindow, INamingContainer
	{
		private int greyFoxUserPreferenceID;
		private GreyFoxUserPreference obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for _system Folder


		#endregion

		#region Private Control Fields for New_Folder Folder

		private TextBox tbName = new TextBox();
		private TextBox tbValue = new TextBox();
		private MultiSelectBox msUser = new MultiSelectBox();

		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxUserPreferenceID
		{
			get
			{
				return greyFoxUserPreferenceID;
			}
			set
			{
				loadFlag = true;
				greyFoxUserPreferenceID = value;
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

			#region Child Controls for New Folder Folder

			msUser.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msUser);

			tbName.EnableViewState = false;
			Controls.Add(tbName);

			tbValue.EnableViewState = false;
			Controls.Add(tbValue);

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
			#region Bind New Folder Child Data

			msUser.Items.Add(new ListItem("Null", "Null"));
			GreyFoxUserManager userManager = new GreyFoxUserManager();
			GreyFoxUserCollection userCollection = userManager.GetCollection(string.Empty, string.Empty, null);
			foreach(GreyFoxUser user in userCollection)
			{
				ListItem i = new ListItem(user.ToString(), user.ID.ToString());
				msUser.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(greyFoxUserPreferenceID == 0)
				obj = new GreyFoxUserPreference();
			else
				obj = new GreyFoxUserPreference(greyFoxUserPreferenceID);

			obj.Name = tbName.Text;
			obj.Value = tbValue.Text;

			if(msUser.SelectedItem != null && msUser.SelectedItem.Value != "Null")
				obj.User = GreyFoxUser.NewPlaceHolder(
					int.Parse(msUser.SelectedItem.Value));
			else
				obj.User = null;

			if(editOnAdd)
				greyFoxUserPreferenceID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbName.Text = string.Empty;
				tbValue.Text = string.Empty;
				msUser.SelectedIndex = 0;
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
				if(greyFoxUserPreferenceID > 0)
				{
					obj = new GreyFoxUserPreference(greyFoxUserPreferenceID);
					text = "Edit  - " + obj.ToString();
				}
				else if(greyFoxUserPreferenceID <= 0)
				{
					obj = new GreyFoxUserPreference();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbName.Text = obj.Name;
				tbValue.Text = obj.Value;

				//
				// Set Children Selections
				//
				if(obj.User != null)
					foreach(ListItem item in msUser.Items)
						item.Selected = obj.User.ID.ToString() == item.Value;
					else
						msUser.SelectedIndex = 0;

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

		private void renderNew_FolderFolder(HtmlTextWriter output)
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
			// Render Value
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Value");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbValue.RenderControl(output);
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
					greyFoxUserPreferenceID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxUserPreferenceID;
			return myState;
		}
	}
}

