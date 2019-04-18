using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using ComponentArt.Web.UI;
using Amns.GreyFox.People;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Default web editor for GreyFoxUser.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
		ToolboxData("<{0}:GreyFoxUserEditor runat=server></{0}:GreyFoxUserEditor>")]
	public class GreyFoxUserEditor : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private int greyFoxUserID;
		private GreyFoxUser editGreyFoxUser;
        private bool loadFlag;
		private bool resetOnAdd;
		private bool editOnAdd;		

        protected ComponentArt.Web.UI.TabStrip tabstrip;
        protected ComponentArt.Web.UI.MultiPage multipage;
        protected Literal headerText;

        protected ComponentArt.Web.UI.PageView generalView;
        private TextBox tbUserName;
        private CheckBox cbIsDisabled;
        private TextBox tbLoginPassword;
        private TextBox tbRoles;
        private ComponentArt.Web.UI.ComboBox comboRoles;

        protected ComponentArt.Web.UI.PageView contactView;
        private TextBox tbName;
        private TextBox tbAddress1;
        private TextBox tbAddress2;
        private TextBox tbCity;
        private TextBox tbStateProvince;
        private TextBox tbPostalCode;
        private TextBox tbCountry;
        private TextBox tbHomePhone;
        private TextBox tbWorkPhone;
        private TextBox tbMobilePhone;
        private TextBox tbPager;
        private TextBox tbEmail1;
        private TextBox tbEmail2;
        private TextBox tbUrl;
        private TextBox tbBirthDate;
        private CheckBox cbBadAddress;        
        private CheckBox cbBadHomePhone;
        private CheckBox cbBadWorkPhone;
        private CheckBox cbBadMobilePhone;
        private CheckBox cbBadEmail;
        private CheckBox cbBadUrl;

        protected ComponentArt.Web.UI.PageView memoView;
		private TextBox tbMemoText;

		private Button btOk;
		private Button btCancel;

        #region Properties

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

            #region TabStrip

            tabstrip = new ComponentArt.Web.UI.TabStrip();

            // Create the DefaultTabLook instance and add it to the ItemLooks collection
            ComponentArt.Web.UI.ItemLook defaultTabLook = new ComponentArt.Web.UI.ItemLook();
            defaultTabLook.LookId = "DefaultTabLook";
            defaultTabLook.CssClass = "DefaultTab";
            defaultTabLook.HoverCssClass = "DefaultTabHover";
            defaultTabLook.LabelPaddingLeft = Unit.Pixel(10);
            defaultTabLook.LabelPaddingRight = Unit.Pixel(10);
            defaultTabLook.LabelPaddingTop = Unit.Pixel(5);
            defaultTabLook.LabelPaddingBottom = Unit.Pixel(4);
            defaultTabLook.LeftIconUrl = "tab_left_icon.gif";
            defaultTabLook.RightIconUrl = "tab_right_icon.gif";
            defaultTabLook.HoverLeftIconUrl = "hover_tab_left_icon.gif";
            defaultTabLook.HoverRightIconUrl = "hover_tab_right_icon.gif";
            defaultTabLook.LeftIconWidth = Unit.Pixel(3);
            defaultTabLook.LeftIconHeight = Unit.Pixel(21);
            defaultTabLook.RightIconWidth = Unit.Pixel(3);
            defaultTabLook.RightIconHeight = Unit.Pixel(21);
            tabstrip.ItemLooks.Add(defaultTabLook);

            // Create the SelectedTabLook instance and add it to the ItemLooks collection
            ComponentArt.Web.UI.ItemLook selectedTabLook = new ComponentArt.Web.UI.ItemLook();
            selectedTabLook.LookId = "SelectedTabLook";
            selectedTabLook.CssClass = "SelectedTab";
            selectedTabLook.LabelPaddingLeft = Unit.Pixel(10);
            selectedTabLook.LabelPaddingRight = Unit.Pixel(10);
            selectedTabLook.LabelPaddingTop = Unit.Pixel(5);
            selectedTabLook.LabelPaddingBottom = Unit.Pixel(4);
            selectedTabLook.LeftIconUrl = "selected_tab_left_icon.gif";
            selectedTabLook.RightIconUrl = "selected_tab_right_icon.gif";
            selectedTabLook.LeftIconWidth = Unit.Pixel(3);
            selectedTabLook.LeftIconHeight = Unit.Pixel(21);
            selectedTabLook.RightIconWidth = Unit.Pixel(3);
            selectedTabLook.RightIconHeight = Unit.Pixel(21);
            tabstrip.ItemLooks.Add(selectedTabLook);

            ComponentArt.Web.UI.ItemLook scrollItemLook = new ItemLook();
            scrollItemLook.LookId = "ScrollItem";
            scrollItemLook.CssClass = "ScrollItem";
            scrollItemLook.HoverCssClass = "ScrollItemHover";
            scrollItemLook.LabelPaddingLeft = Unit.Pixel(5);
            scrollItemLook.LabelPaddingRight = Unit.Pixel(5);
            scrollItemLook.LabelPaddingTop = Unit.Pixel(0);
            scrollItemLook.LabelPaddingBottom = Unit.Pixel(0);
            tabstrip.ItemLooks.Add(scrollItemLook);

            tabstrip.ID = this.ID + "_TabStrip";
            tabstrip.CssClass = "TopGroup";
            tabstrip.DefaultItemLookId = "DefaultTabLook";
            tabstrip.DefaultSelectedItemLookId = "SelectedTabLook";
            tabstrip.DefaultGroupTabSpacing = 1;
            tabstrip.ImagesBaseUrl = "tabstrip_images/";
            tabstrip.MultiPageId = this.ID + "_MultiPage";
            tabstrip.ScrollingEnabled = true;
            tabstrip.ScrollLeftLookId = "ScrollItem";
            tabstrip.ScrollRightLookId = "ScrollItem";
            content.Controls.Add(tabstrip);

            #endregion

            #region MultiPage

            multipage = new ComponentArt.Web.UI.MultiPage();
            multipage.ID = this.ID + "_MultiPage";
            multipage.CssClass = "MultiPage";
            content.Controls.Add(multipage);

            #endregion

            #region General View

            generalView = new ComponentArt.Web.UI.PageView();
            generalView.CssClass = "PageContent";
            multipage.PageViews.Add(generalView);

            TabStripTab generalTab = new TabStripTab();
            generalTab.Text = Localization.PeopleStrings.GeneralTab;
            generalTab.PageViewId = generalView.ID;
            tabstrip.Tabs.Add(generalTab);

            tbUserName = new TextBox();
			tbUserName.Width = Unit.Pixel(175);
			tbUserName.EnableViewState = false;
            cbIsDisabled = new CheckBox();
            cbIsDisabled.Width = Unit.Pixel(175);
            cbIsDisabled.EnableViewState = false;
            cbIsDisabled.Text = Localization.Strings.Disabled;
			registerControl(generalView, 
                Localization.SecurityStrings.Username, tbUserName, cbIsDisabled);

            tbLoginPassword = new TextBox();
			tbLoginPassword.Width = Unit.Pixel(175);
			tbLoginPassword.TextMode = TextBoxMode.Password;
			tbLoginPassword.EnableViewState = false;
            registerControl(generalView, 
                Localization.SecurityStrings.Password, tbLoginPassword);

            registerControl(generalView,
                string.Empty, new LiteralControl("<em>" +
                Localization.SecurityStrings.PasswordTextBoxNote + "</em>"));

            tbRoles = new TextBox();
            tbRoles.ID = this.ID + "_ref";
            tbRoles.EnableViewState = false;
            tbRoles.Rows = 10;
            tbRoles.MaxLength = 1500;
            tbRoles.TextMode = TextBoxMode.MultiLine;
            tbRoles.Width = Unit.Pixel(350);
            tbRoles.ToolTip = Localization.SecurityStrings.RolesTextBoxNote;
            registerControl(generalView,
                Localization.SecurityStrings.Roles, tbRoles);

            Panel rolesPanel = new Panel();            
            rolesPanel.Controls.Add(new LiteralControl("<div style=\"float:left\">"));
            comboRoles = new ComponentArt.Web.UI.ComboBox();
            comboRoles.ID = this.ID + "_rcb";
            comboRoles.CssClass = "comboBox";
            comboRoles.HoverCssClass = "comboBoxHover";
            comboRoles.FocusedCssClass = "comboBoxHover";
            comboRoles.TextBoxCssClass = "comboTextBox";
            comboRoles.DropDownCssClass = "comboDropDown";
            comboRoles.ItemCssClass = "comboItem";
            comboRoles.ItemHoverCssClass = "comboItemHover";
            comboRoles.SelectedItemCssClass = "comboItemHover";
            comboRoles.DropHoverImageUrl = "combobox_images/drop_hover.gif";
            comboRoles.DropImageUrl = "combobox_images/drop.gif";
            comboRoles.Width = Unit.Pixel(250);
            comboRoles.EnableViewState = false;
            rolesPanel.Controls.Add(comboRoles);
            rolesPanel.Controls.Add(new LiteralControl("</div><div><input type=\"button\" value=\"" +
                Localization.Strings.Add + "\" " +
                "align=\"right\" onClick=\"" +
                tbRoles.ClientID + ".value += (" + tbRoles.ClientID + ".value != '' ? '\\r\\n' : '') + " +
                comboRoles.ClientObjectId + ".getSelectedItem().Text;\"></div>"));
            registerControl(generalView, string.Empty, rolesPanel);

            registerControl(generalView,
                string.Empty, new LiteralControl("<em>" +
                    Localization.SecurityStrings.RolesTextBoxNote + "</em>"));

            #endregion

            #region Contact View

            contactView = new ComponentArt.Web.UI.PageView();
            contactView.CssClass = "PageContent";
            multipage.PageViews.Add(contactView);

            TabStripTab contactTab = new TabStripTab();
            contactTab.Text = Localization.PeopleStrings.ContactTab;
            contactTab.PageViewId = generalView.ID;
            tabstrip.Tabs.Add(contactTab);

            tbName = new TextBox();
			tbName.Width = Unit.Pixel(200);
			tbName.EnableViewState = false;
            registerControl(contactView, 
                Localization.PeopleStrings.Name, tbName);

            tbAddress1 = new TextBox();
			tbAddress1.Width = Unit.Pixel(175);
			tbAddress1.EnableViewState = false;
            cbBadAddress = new CheckBox();
            cbBadAddress.Text = Localization.PeopleStrings.Invalid;
            registerControl(contactView,
                Localization.PeopleStrings.Address, tbAddress1, cbBadAddress);

            tbAddress2 = new TextBox();
			tbAddress2.Width = Unit.Pixel(175);
			tbAddress2.EnableViewState = false;
            registerControl(contactView, string.Empty, tbAddress2);

            tbCity = new TextBox();
			tbCity.Width = Unit.Pixel(175);
			tbCity.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.City, tbCity);

            tbStateProvince = new TextBox();
			tbStateProvince.Width = Unit.Pixel(175);
			tbStateProvince.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.StateProvince, tbStateProvince);

            tbPostalCode = new TextBox();
			tbPostalCode.Width = Unit.Pixel(175);
			tbPostalCode.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.PostalCode, tbPostalCode);

            tbCountry = new TextBox();
			tbCountry.Width = Unit.Pixel(175);
			tbCountry.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.Country, tbCountry);

            tbHomePhone = new TextBox();
			tbHomePhone.Width = Unit.Pixel(175);
			tbHomePhone.EnableViewState = false;
            cbBadHomePhone = new CheckBox();
            cbBadHomePhone.Text = Localization.PeopleStrings.Invalid;
            registerControl(contactView,
                Localization.PeopleStrings.HomePhone, tbHomePhone, cbBadHomePhone);

            tbWorkPhone = new TextBox();
			tbWorkPhone.Width = Unit.Pixel(175);
			tbWorkPhone.EnableViewState = false;
            cbBadWorkPhone = new CheckBox();
            cbBadWorkPhone.Text = Localization.PeopleStrings.Invalid;
            registerControl(contactView,
                Localization.PeopleStrings.WorkPhone, tbWorkPhone, cbBadWorkPhone);

            tbMobilePhone = new TextBox();
			tbMobilePhone.Width = Unit.Pixel(175);
			tbMobilePhone.EnableViewState = false;
            cbBadMobilePhone = new CheckBox();
            cbBadMobilePhone.Text = Localization.PeopleStrings.Invalid;
            registerControl(contactView,
                Localization.PeopleStrings.MobilePhone, tbMobilePhone, cbBadMobilePhone);

            tbPager = new TextBox();
			tbPager.Width = Unit.Pixel(175);
			tbPager.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.Pager, tbPager);

            tbEmail1 = new TextBox();
			tbEmail1.Width = Unit.Pixel(175);
			tbEmail1.EnableViewState = false;
            cbBadEmail = new CheckBox();
            cbBadEmail.Text = Localization.PeopleStrings.Invalid;
            registerControl(contactView,
                Localization.PeopleStrings.Email, tbEmail1, cbBadEmail);

            tbEmail2 = new TextBox();
			tbEmail2.Width = Unit.Pixel(175);
			tbEmail2.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.Email2, tbEmail2);

            tbUrl = new TextBox();
			tbUrl.Width = Unit.Pixel(175);
			tbUrl.EnableViewState = false;
            cbBadUrl = new CheckBox();
            cbBadUrl.Text = Localization.PeopleStrings.Invalid;
            registerControl(contactView,
                Localization.PeopleStrings.WebsiteUrl, tbUrl, cbBadUrl);

            tbBirthDate = new TextBox();
			tbBirthDate.Width = Unit.Pixel(175);
			tbBirthDate.EnableViewState = false;
            registerControl(contactView,
                Localization.PeopleStrings.BirthDate, tbBirthDate);

            #endregion

            #region Memo View

            memoView = new ComponentArt.Web.UI.PageView();
            memoView.CssClass = "PageContent";
            multipage.PageViews.Add(memoView);

            TabStripTab memoTab = new TabStripTab();
            memoTab.Text = Localization.PeopleStrings.MemoTab;
            memoTab.PageViewId = generalView.ID;
            tabstrip.Tabs.Add(memoTab);

            tbMemoText = new TextBox();
            tbMemoText.TextMode = TextBoxMode.MultiLine;
			tbMemoText.Rows = 25;
			tbMemoText.Width = Unit.Percentage(100);
			tbMemoText.EnableViewState = false;
            registerControl(memoView, 
                Localization.PeopleStrings.Memo, tbMemoText);

            #endregion

            #region Buttons

            Panel buttons = new Panel();
            buttons.CssClass = "pButtons";
            content.Controls.Add(buttons);

            btOk = new Button();
            btOk.Text = Localization.Strings.OK;
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
            buttons.Controls.Add(btOk);

            btCancel = new Button();
			btCancel.Text = Localization.Strings.Cancel;
			btCancel.Width = Unit.Pixel(72);
			btCancel.EnableViewState = false;
			btCancel.Click += new EventHandler(cancel_Click);
            buttons.Controls.Add(btCancel);

            #endregion

            bind();

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
            foreach(Control control in controls)
                pageViewControls.Add(control);
            pageViewControls.Add(new LiteralControl("</span></div>"));
        }

		private void bind()
		{
			GreyFoxRoleManager m = new GreyFoxRoleManager();
			GreyFoxRoleCollection roles = m.GetCollection(string.Empty, "Name");
            foreach (GreyFoxRole role in roles)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = role.Name;
                item.Value = role.ID.ToString();
                comboRoles.Items.Add(item);
            }
		}

		protected void ok_Click(object sender, EventArgs e)
		{
			bool reset = false; 

			if(greyFoxUserID == 0)
			{
				editGreyFoxUser = new GreyFoxUser();
				reset = resetOnAdd;
			}
			else
			{
				editGreyFoxUser = new GreyFoxUser(greyFoxUserID);
			}

			if(editGreyFoxUser.Contact == null)
				editGreyFoxUser.Contact = new GreyFoxContact("sysGlobal_Contacts");

			editGreyFoxUser.Contact.ParseName(tbName.Text);
			editGreyFoxUser.Contact.Address1 = tbAddress1.Text;
            editGreyFoxUser.Contact.IsBadAddress = cbBadAddress.Checked;
			editGreyFoxUser.Contact.Address2 = tbAddress2.Text;
			editGreyFoxUser.Contact.City = tbCity.Text;
			editGreyFoxUser.Contact.StateProvince = tbStateProvince.Text;
			editGreyFoxUser.Contact.PostalCode = tbPostalCode.Text;
			editGreyFoxUser.Contact.Country = tbCountry.Text;
			editGreyFoxUser.Contact.HomePhone = tbHomePhone.Text;
            editGreyFoxUser.Contact.IsBadHomePhone = cbBadHomePhone.Checked;
			editGreyFoxUser.Contact.WorkPhone = tbWorkPhone.Text;
            editGreyFoxUser.Contact.IsBadWorkPhone = cbBadWorkPhone.Checked;
			editGreyFoxUser.Contact.MobilePhone = tbMobilePhone.Text;
			editGreyFoxUser.Contact.Pager = tbPager.Text;
            editGreyFoxUser.Contact.IsBadMobilePhone = cbBadMobilePhone.Checked;
			editGreyFoxUser.Contact.Email1 = tbEmail1.Text;
            editGreyFoxUser.Contact.IsBadEmail = cbBadEmail.Checked;
			editGreyFoxUser.Contact.Email2 = tbEmail2.Text;
			editGreyFoxUser.Contact.Url = tbUrl.Text;
            editGreyFoxUser.Contact.IsBadUrl = cbBadUrl.Checked;
			editGreyFoxUser.Contact.BirthDate = DateTime.Parse(tbBirthDate.Text);
			editGreyFoxUser.Contact.MemoText = tbMemoText.Text;

			editGreyFoxUser.UserName = tbUserName.Text;
			editGreyFoxUser.IsDisabled = cbIsDisabled.Checked;
			
			if(tbLoginPassword.Text != "")
				editGreyFoxUser.LoginPassword = GreyFoxPassword.EncodePassword(tbLoginPassword.Text);

            GreyFoxRoleManager roleManager = new GreyFoxRoleManager();
            editGreyFoxUser.Roles = roleManager.DecodeString(tbRoles.Text, "\r\n");

			if(editOnAdd)
				greyFoxUserID = editGreyFoxUser.Save();
			else
				editGreyFoxUser.Save();

			if(reset)
			{
				tbUserName.Text = string.Empty;
				cbIsDisabled.Checked = false;
				tbLoginPassword.Text = string.Empty;

				tbName.Text = string.Empty;
				tbAddress1.Text = string.Empty;
				tbAddress2.Text = string.Empty;
				tbCity.Text = string.Empty;
				tbStateProvince.Text = string.Empty;
				tbPostalCode.Text = string.Empty;
				tbCountry.Text = string.Empty;
				tbHomePhone.Text = string.Empty;
				tbWorkPhone.Text = string.Empty;
				tbMobilePhone.Text = string.Empty;
				tbPager.Text = string.Empty;
				tbEmail1.Text = string.Empty;
				tbEmail2.Text = string.Empty;
				tbUrl.Text = string.Empty;
				tbBirthDate.Text = string.Empty;
				tbMemoText.Text = string.Empty;
			}

			OnUpdated(EventArgs.Empty);
		}

		protected void cancel_Click(object sender, EventArgs e)
		{
			this.OnCancelled(EventArgs.Empty);
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

		protected override void OnPreRender(EventArgs e)
		{
            if (loadFlag)
            {
                tabstrip.SelectedTab = tabstrip.Tabs[0];

                if (greyFoxUserID != 0)
                {
                    editGreyFoxUser = new GreyFoxUser(greyFoxUserID);

                    headerText.Text = Localization.Strings.Edit +
                        Localization.PeopleStrings.Space + editGreyFoxUser.Contact.FullName;

                    //
                    // Set Field Entries
                    //
                    tbUserName.Text = editGreyFoxUser.UserName.ToString();
                    cbIsDisabled.Checked = editGreyFoxUser.IsDisabled;

                    tbName.Text = editGreyFoxUser.Contact.FullName;
                    tbAddress1.Text = editGreyFoxUser.Contact.Address1;
                    cbBadAddress.Checked = editGreyFoxUser.Contact.IsBadAddress;
                    tbAddress2.Text = editGreyFoxUser.Contact.Address2;
                    tbCity.Text = editGreyFoxUser.Contact.City;
                    tbStateProvince.Text = editGreyFoxUser.Contact.StateProvince;
                    tbPostalCode.Text = editGreyFoxUser.Contact.PostalCode;
                    tbCountry.Text = editGreyFoxUser.Contact.Country;
                    tbHomePhone.Text = editGreyFoxUser.Contact.HomePhone;
                    cbBadHomePhone.Checked = editGreyFoxUser.Contact.IsBadHomePhone;
                    tbWorkPhone.Text = editGreyFoxUser.Contact.WorkPhone;
                    cbBadWorkPhone.Checked = editGreyFoxUser.Contact.IsBadWorkPhone;
                    tbMobilePhone.Text = editGreyFoxUser.Contact.MobilePhone;
                    cbBadMobilePhone.Checked = editGreyFoxUser.Contact.IsBadMobilePhone;
                    tbPager.Text = editGreyFoxUser.Contact.Pager;
                    tbEmail1.Text = editGreyFoxUser.Contact.Email1;
                    cbBadEmail.Checked = editGreyFoxUser.Contact.IsBadEmail;
                    tbEmail2.Text = editGreyFoxUser.Contact.Email2;
                    tbUrl.Text = editGreyFoxUser.Contact.Url;
                    cbBadUrl.Checked = editGreyFoxUser.Contact.IsBadUrl;
                    tbBirthDate.Text = editGreyFoxUser.Contact.BirthDate.ToShortDateString();
                    tbMemoText.Text = editGreyFoxUser.Contact.MemoText;

                    tbRoles.Text = editGreyFoxUser.Roles.ToEncodedString("\r\n", "\r\n");
                }
                else
                {
                    headerText.Text = Localization.Strings.New;
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
