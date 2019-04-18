using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.EventLog.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxEvent.
	/// </summary>
	[ToolboxData("<{0}:GreyFoxEventEditor runat=server></{0}:GreyFoxEventEditor>")]
	public class GreyFoxEventEditor : TableWindow, INamingContainer
	{
		private int greyFoxEventID;
		private string greyFoxEventTable = "sysGlobal_EventLog";
		private GreyFoxEvent obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for Default Folder

		private TextBox tbType = new TextBox();
		private TextBox tbEventDate = new TextBox();
		private TextBox tbSource = new TextBox();
		private TextBox tbCategory = new TextBox();
		private TextBox tbDescription = new TextBox();
		private TextBox tbEventID = new TextBox();
		private RegularExpressionValidator revEventID = new RegularExpressionValidator();
		private MultiSelectBox msUser = new MultiSelectBox();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxEventID
		{
			get
			{
				return greyFoxEventID;
			}
			set
			{
				loadFlag = true;
				greyFoxEventID = value;
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

		[Bindable(true), Category("Data"), DefaultValue("sysGlobal_EventLog")]
		public string GreyFoxEventTable
		{
			get
			{
				return greyFoxEventTable;
			}
			set
			{
				greyFoxEventTable = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();
			bindDropDownLists();

			#region Child Controls for Default Folder

			tbType.EnableViewState = false;
			Controls.Add(tbType);

			tbEventDate.EnableViewState = false;
			Controls.Add(tbEventDate);

			tbSource.EnableViewState = false;
			Controls.Add(tbSource);

			tbCategory.EnableViewState = false;
			Controls.Add(tbCategory);

			tbDescription.EnableViewState = false;
			Controls.Add(tbDescription);

			tbEventID.ID = this.ID + "_EventID";
			tbEventID.EnableViewState = false;
			Controls.Add(tbEventID);
			revEventID.ControlToValidate = tbEventID.ID;
			revEventID.ValidationExpression = "^(\\+|-)?\\d+$";
			revEventID.ErrorMessage = "*";
			revEventID.Display = ValidatorDisplay.Dynamic;
			revEventID.EnableViewState = false;
			Controls.Add(revEventID);

			msUser.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msUser);

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
			if(greyFoxEventID == 0)
				obj = new GreyFoxEvent(greyFoxEventTable);
			else
				obj = new GreyFoxEvent(greyFoxEventTable, greyFoxEventID);

			obj.Type = byte.Parse(tbType.Text);
			obj.EventDate = DateTime.Parse(tbEventDate.Text);
			obj.Source = tbSource.Text;
			obj.Category = tbCategory.Text;
			obj.Description = tbDescription.Text;
			obj.EventID = int.Parse(tbEventID.Text);

			if(msUser.SelectedItem != null && msUser.SelectedItem.Value != "Null")
				obj.User = GreyFoxUser.NewPlaceHolder(
					int.Parse(msUser.SelectedItem.Value));
			else
				obj.User = null;

			if(editOnAdd)
				greyFoxEventID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbType.Text = string.Empty;
				tbEventDate.Text = DateTime.Now.ToString();
				tbSource.Text = string.Empty;
				tbCategory.Text = string.Empty;
				tbDescription.Text = string.Empty;
				tbEventID.Text = string.Empty;
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
				if(greyFoxEventID > 0)
				{
					obj = new GreyFoxEvent(greyFoxEventTable, greyFoxEventID);
					text = "Edit  - " + obj.ToString();
				}
				else if(greyFoxEventID <= 0)
				{
					obj = new GreyFoxEvent(greyFoxEventTable);
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbType.Text = obj.Type.ToString();
				tbEventDate.Text = obj.EventDate.ToString();
				tbSource.Text = obj.Source;
				tbCategory.Text = obj.Category;
				tbDescription.Text = obj.Description;
				tbEventID.Text = obj.EventID.ToString();

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

		private void renderDefaultFolder(HtmlTextWriter output)
		{
			//
			// Render Type
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Type of event.");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbType.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EventDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EventDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEventDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Source
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Source");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSource.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Category
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Category");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbCategory.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Description
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Description");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDescription.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EventID
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EventID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEventID.RenderControl(output);
			revEventID.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

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
					greyFoxEventID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxEventID;
			return myState;
		}
	}
}

