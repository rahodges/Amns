using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Default web editor for GreyFoxRole.
	/// </summary>
	[ToolboxData("<{0}:GreyFoxRoleEditor runat=server></{0}:GreyFoxRoleEditor>")]
	public class GreyFoxRoleEditor : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private int greyFoxRoleID;
		private GreyFoxRole obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		protected ComponentArt.Web.UI.TabStrip tabstrip;
		protected ComponentArt.Web.UI.MultiPage multipage;
        protected Literal headerText;

		#region Private Control Fields for General Folder

		protected ComponentArt.Web.UI.PageView GeneralView;
		private TextBox tbName;
		private TextBox tbDescription;
		private CheckBox cbIsDisabled;

		#endregion

		#region Private Control Fields for _system Folder

        protected ComponentArt.Web.UI.PageView _systemView;

		#endregion

		private Button btOk;
		private Button btCancel;
		private Button btDelete;

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxRoleID
		{
			get
			{
				return greyFoxRoleID;
			}
			set
			{
				loadFlag = true;
				greyFoxRoleID = value;
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
            Panel container = new Panel();
            container.CssClass = this.CssClass;
            Controls.Add(container);

            Panel header = new Panel();
            header.CssClass = "pHead";
            container.Controls.Add(header);

            headerText = new Literal();
            header.Controls.Add(headerText);

            Panel content = new Panel();
            content.CssClass = "pContent";
            container.Controls.Add(content);

            #region Tab Strip

            tabstrip = new ComponentArt.Web.UI.TabStrip();

			// Create the DefaultTabLook instance and add it to the ItemLooks collection
			ComponentArt.Web.UI.ItemLook defaultTabLook = new ComponentArt.Web.UI.ItemLook();
			defaultTabLook.LookId = "DefaultTabLook";
			defaultTabLook.CssClass = "DefaultTab";
			defaultTabLook.HoverCssClass = "DefaultTabHover";
			defaultTabLook.LabelPaddingLeft = Unit.Parse("10");
			defaultTabLook.LabelPaddingRight = Unit.Parse("10");
			defaultTabLook.LabelPaddingTop = Unit.Parse("5");
			defaultTabLook.LabelPaddingBottom = Unit.Parse("4");
			defaultTabLook.LeftIconUrl = "tab_left_icon.gif";
			defaultTabLook.RightIconUrl = "tab_right_icon.gif";
			defaultTabLook.HoverLeftIconUrl = "hover_tab_left_icon.gif";
			defaultTabLook.HoverRightIconUrl = "hover_tab_right_icon.gif";
			defaultTabLook.LeftIconWidth = Unit.Parse("3");
			defaultTabLook.LeftIconHeight = Unit.Parse("21");
			defaultTabLook.RightIconWidth = Unit.Parse("3");
			defaultTabLook.RightIconHeight = Unit.Parse("21");
			tabstrip.ItemLooks.Add(defaultTabLook);

			// Create the SelectedTabLook instance and add it to the ItemLooks collection
			ComponentArt.Web.UI.ItemLook selectedTabLook = new ComponentArt.Web.UI.ItemLook();
			selectedTabLook.LookId = "SelectedTabLook";
			selectedTabLook.CssClass = "SelectedTab";
			selectedTabLook.LabelPaddingLeft = Unit.Parse("10");
			selectedTabLook.LabelPaddingRight = Unit.Parse("10");
			selectedTabLook.LabelPaddingTop = Unit.Parse("5");
			selectedTabLook.LabelPaddingBottom = Unit.Parse("4");
			selectedTabLook.LeftIconUrl = "selected_tab_left_icon.gif";
			selectedTabLook.RightIconUrl = "selected_tab_right_icon.gif";
			selectedTabLook.LeftIconWidth = Unit.Parse("3");
			selectedTabLook.LeftIconHeight = Unit.Parse("21");
			selectedTabLook.RightIconWidth = Unit.Parse("3");
			selectedTabLook.RightIconHeight = Unit.Parse("21");
			tabstrip.ItemLooks.Add(selectedTabLook);

			tabstrip.ID = this.ID + "_TabStrip";
			tabstrip.CssClass = "TopGroup";
			tabstrip.DefaultItemLookId = "DefaultTabLook";
			tabstrip.DefaultSelectedItemLookId = "SelectedTabLook";
			tabstrip.DefaultGroupTabSpacing = 1;
			tabstrip.ImagesBaseUrl = "tabstrip_images/";
			tabstrip.MultiPageId = this.ID + "_MultiPage";
            tabstrip.EnableViewState = false;
            content.Controls.Add(tabstrip);

			#endregion

			#region MultiPage

			multipage = new ComponentArt.Web.UI.MultiPage();
			multipage.ID = this.ID + "_MultiPage";
			multipage.CssClass = "MultiPage";
            content.Controls.Add(multipage);

			#endregion

			#region General Folder

			GeneralView = new ComponentArt.Web.UI.PageView();
            GeneralView.CssClass = "PageContent";
			multipage.PageViews.Add(GeneralView);

			TabStripTab GeneralTab = new TabStripTab();
			GeneralTab.Text = Localization.PeopleStrings.GeneralTab;
            GeneralTab.PageViewId = GeneralView.ID;
			tabstrip.Tabs.Add(GeneralTab);
            			
			tbName = new TextBox();
            tbName.ID = this.ID + "_Name";
            tbName.MaxLength = 255;
            tbName.Width = Unit.Pixel(250);
            tbName.ToolTip = Localization.SecurityStrings.Role_NameTooltip;
			tbName.EnableViewState = false;
            cbIsDisabled = new CheckBox();
            cbIsDisabled.ID = this.ID + "_IsDisabled";
            cbIsDisabled.ToolTip = Localization.SecurityStrings.Role_DisabledTooltip;
            cbIsDisabled.EnableViewState = false;
            cbIsDisabled.Text = Localization.Strings.Disabled;
            registerControl(GeneralView, Localization.PeopleStrings.Name, tbName, cbIsDisabled);

			tbDescription = new TextBox();
            tbDescription.ID = this.ID + "_Description";
            tbDescription.Rows = 3;
            tbDescription.MaxLength = 255;
            tbDescription.TextMode = TextBoxMode.MultiLine;
            tbDescription.Width = Unit.Pixel(350);
            tbDescription.ToolTip = Localization.SecurityStrings.Role_DescriptionTooltip;
			tbDescription.EnableViewState = false;
            registerControl(GeneralView, Localization.Strings.Description, tbDescription);

			#endregion

            #region Buttons

            Panel buttons = new Panel();
            buttons.CssClass = "pButtons";
            content.Controls.Add(buttons);
                        
			btOk = new Button();
            btOk.ID = this.ID + "_Ok";
            btOk.Text = Localization.Strings.OK;
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
            buttons.Controls.Add(btOk);

			btCancel = new Button();
            btCancel.ID = this.ID + "_Cancel";
			btCancel.Text = Localization.Strings.Cancel;
			btCancel.Width = Unit.Pixel(72);
			btCancel.EnableViewState = false;
			btCancel.CausesValidation = false;
			btCancel.Click += new EventHandler(cancel_Click);
            buttons.Controls.Add(btCancel);

			btDelete = new Button();
            btDelete.ID = this.ID + "_Delete";
			btDelete.Text = Localization.Strings.Delete;            
			btDelete.Width = Unit.Pixel(72);
			btDelete.EnableViewState = false;
			btDelete.Click += new EventHandler(delete_Click);
            buttons.Controls.Add(btDelete);

            #endregion

			ChildControlsCreated = true;
		}

        private void registerControl(ComponentArt.Web.UI.PageView pageView,
            string caption,
            params Control[] controls)
        {
            ControlCollection pageViewControls = pageView.Controls;

            pageViewControls.Add(new LiteralControl("<div class=\"inputrow\">"));
            pageViewControls.Add(new LiteralControl("<span class=\"inputlabel\">"));
            pageViewControls.Add(new LiteralControl(caption));
            pageViewControls.Add(new LiteralControl("</span><span class=\"inputfield\">"));
            foreach (Control control in controls)
                pageViewControls.Add(control);
            pageViewControls.Add(new LiteralControl("</span></div>"));
        }

		#region Events

		protected void ok_Click(object sender, EventArgs e)
		{
			if(greyFoxRoleID == 0)
				obj = new GreyFoxRole();
			else
				obj = new GreyFoxRole(greyFoxRoleID);

			obj.Name = tbName.Text;
			obj.Description = tbDescription.Text;
			obj.IsDisabled = cbIsDisabled.Checked;

			if(editOnAdd)
				greyFoxRoleID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbName.Text = string.Empty;
				tbDescription.Text = string.Empty;
				cbIsDisabled.Checked = false;
			}

			OnUpdated(EventArgs.Empty);
		}

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

		#endregion

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
                if (greyFoxRoleID != 0)
                {
                    obj = new GreyFoxRole(greyFoxRoleID);

                    tbName.Text = obj.Name;
                    tbDescription.Text = obj.Description;
                    cbIsDisabled.Checked = obj.IsDisabled;

                    tabstrip.SelectedTab = tabstrip.Tabs[0];

                    headerText.Text = Localization.Strings.Edit +
                        Localization.PeopleStrings.Space +
                        obj.Name;
                }
                else
                {
                    headerText.Text = Localization.Strings.Edit;
                }
			}
		}

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null)
					base.LoadViewState(myState[0]);
				if(myState[1] != null)
					greyFoxRoleID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxRoleID;
			return myState;
		}
	}
}