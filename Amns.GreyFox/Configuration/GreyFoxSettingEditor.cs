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
	/// Default web editor for GreyFoxSetting.
	/// </summary>
	[ToolboxData("<{0}:GreyFoxSettingEditor runat=server></{0}:GreyFoxSettingEditor>")]
	public class GreyFoxSettingEditor : TableWindow, INamingContainer
	{
		private int greyFoxSettingID;
		private GreyFoxSetting obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for _system Folder


		#endregion

		#region Private Control Fields for General Folder

		private TextBox tbName = new TextBox();
		private TextBox tbSettingValue = new TextBox();
		private CheckBox cbIsSystemSetting = new CheckBox();
		private MultiSelectBox msParent = new MultiSelectBox();
		private MultiSelectBox msModifyRole = new MultiSelectBox();

		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxSettingID
		{
			get
			{
				return greyFoxSettingID;
			}
			set
			{
				loadFlag = true;
				greyFoxSettingID = value;
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

			tbName.EnableViewState = false;
			Controls.Add(tbName);

			tbSettingValue.EnableViewState = false;
			Controls.Add(tbSettingValue);

			msParent.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msParent);

			msModifyRole.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msModifyRole);

			cbIsSystemSetting.EnableViewState = false;
			Controls.Add(cbIsSystemSetting);

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

			msParent.Items.Add(new ListItem("Null", "Null"));
			GreyFoxSettingManager parentManager = new GreyFoxSettingManager();
			GreyFoxSettingCollection parentCollection = parentManager.GetCollection(string.Empty, string.Empty, null);
			foreach(GreyFoxSetting parent in parentCollection)
			{
				ListItem i = new ListItem(parent.ToString(), parent.ID.ToString());
				msParent.Items.Add(i);
			}

			msModifyRole.Items.Add(new ListItem("Null", "Null"));
			GreyFoxRoleManager modifyRoleManager = new GreyFoxRoleManager();
			GreyFoxRoleCollection modifyRoleCollection = modifyRoleManager.GetCollection(string.Empty, string.Empty);
			foreach(GreyFoxRole modifyRole in modifyRoleCollection)
			{
				ListItem i = new ListItem(modifyRole.ToString(), modifyRole.ID.ToString());
				msModifyRole.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(greyFoxSettingID == 0)
				obj = new GreyFoxSetting();
			else
				obj = new GreyFoxSetting(greyFoxSettingID);

			obj.Name = tbName.Text;
			obj.SettingValue = tbSettingValue.Text;
			obj.IsSystemSetting = cbIsSystemSetting.Checked;

			if(msParent.SelectedItem != null && msParent.SelectedItem.Value != "Null")
				obj.Parent = GreyFoxSetting.NewPlaceHolder(
					int.Parse(msParent.SelectedItem.Value));
			else
				obj.Parent = null;

			if(msModifyRole.SelectedItem != null && msModifyRole.SelectedItem.Value != "Null")
				obj.ModifyRole = GreyFoxRole.NewPlaceHolder(
					int.Parse(msModifyRole.SelectedItem.Value));
			else
				obj.ModifyRole = null;

			if(editOnAdd)
				greyFoxSettingID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbName.Text = string.Empty;
				tbSettingValue.Text = string.Empty;
				cbIsSystemSetting.Checked = false;
				msParent.SelectedIndex = 0;
				msModifyRole.SelectedIndex = 0;
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
				if(greyFoxSettingID > 0)
				{
					obj = new GreyFoxSetting(greyFoxSettingID);
					text = "Edit  - " + obj.ToString();
				}
				else if(greyFoxSettingID <= 0)
				{
					obj = new GreyFoxSetting();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbName.Text = obj.Name;
				tbSettingValue.Text = obj.SettingValue;
				cbIsSystemSetting.Checked = obj.IsSystemSetting;

				//
				// Set Children Selections
				//
				if(obj.Parent != null)
					foreach(ListItem item in msParent.Items)
						item.Selected = obj.Parent.ID.ToString() == item.Value;
					else
						msParent.SelectedIndex = 0;

				if(obj.ModifyRole != null)
					foreach(ListItem item in msModifyRole.Items)
						item.Selected = obj.ModifyRole.ID.ToString() == item.Value;
					else
						msModifyRole.SelectedIndex = 0;

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
			// Render SettingValue
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("SettingValue");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSettingValue.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Parent
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Parent");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msParent.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ModifyRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ModifyRole");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msModifyRole.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsSystemSetting
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("IsSystemSetting");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbIsSystemSetting.RenderControl(output);
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
					greyFoxSettingID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxSettingID;
			return myState;
		}
	}
}

