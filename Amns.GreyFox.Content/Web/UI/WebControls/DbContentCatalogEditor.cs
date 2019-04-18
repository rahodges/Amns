using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using ComponentArt.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
    /// <summary>
    /// Default web editor for DbContentCatalog.
    /// </summary>
    [ToolboxData("<{0}:DbContentCatalogEditor runat=server></{0}:DbContentCatalogEditor>")]
    public class DbContentCatalogEditor : System.Web.UI.WebControls.WebControl, INamingContainer
    {
        private int dbContentCatalogID;
        private DbContentCatalog obj;
        private bool loadFlag = false;
        private bool resetOnAdd;
        private bool editOnAdd;

        protected ComponentArt.Web.UI.TabStrip tabstrip;
        protected ComponentArt.Web.UI.MultiPage multipage;
        protected Literal headerText;

        #region Private Control Fields for General Folder

        protected ComponentArt.Web.UI.PageView GeneralView;
        private TextBox tbTitle;
        private TextBox tbDescription;
        private TextBox tbKeywords;
        private TextBox tbStatus;
        private TextBox tbWorkflowMode;
        private CheckBox cbCommentsEnabled;
        private CheckBox cbNotifyOnComments;
        private CheckBox cbEnabled;
        private TextBox tbSortOrder;
        private RegularExpressionValidator revSortOrder;
        private TextBox tbIcon;
        private MultiSelectBox msParentCatalog;
        private MultiSelectBox msChildCatalogs;
        private MultiSelectBox msDefaultClip;

        #endregion

        #region Private Control Fields for _system Folder

        protected ComponentArt.Web.UI.PageView _systemView;
        private Literal ltCreateDate;
        private Literal ltModifyDate;

        #endregion

        #region Private Control Fields for Defaults Folder

        protected ComponentArt.Web.UI.PageView DefaultsView;
        private TextBox tbDefaultTimeToPublish;
        private TextBox tbDefaultTimeToExpire;
        private TextBox tbDefaultTimeToArchive;
        private TextBox tbDefaultKeywords;
        private TextBox tbDefaultMenuLeftIcon;
        private TextBox tbDefaultMenuRightIcon;
        private MultiSelectBox msDefaultStatus;
        private MultiSelectBox msDefaultRating;
        private MultiSelectBox msDefaultArchive;
        private MultiSelectBox msTemplates;

        #endregion

        #region Private Control Fields for Menu Folder

        protected ComponentArt.Web.UI.PageView MenuView;
        private TextBox tbMenuLabel;
        private TextBox tbMenuTooltip;
        private CheckBox cbMenuEnabled;
        private TextBox tbMenuOrder;
        private RegularExpressionValidator revMenuOrder;
        private TextBox tbMenuLeftIcon;
        private TextBox tbMenuRightIcon;
        private TextBox tbMenuBreakImage;
        private TextBox tbMenuBreakCssClass;
        private TextBox tbMenuCssClass;
        private TextBox tbMenuCatalogCssClass;
        private TextBox tbMenuCatalogSelectedCssClass;
        private TextBox tbMenuCatalogChildSelectedCssClass;
        private TextBox tbMenuClipCssClass;
        private TextBox tbMenuClipSelectedCssClass;
        private TextBox tbMenuClipChildSelectedCssClass;
        private TextBox tbMenuClipChildExpandedCssClass;
        private TextBox tbMenuOverrideFlags;
        private TextBox tbMenuIconFlags;

        #endregion

        #region Private Control Fields for Security Folder

        protected ComponentArt.Web.UI.PageView SecurityView;
        private MultiSelectBox msAuthorRole;
        private MultiSelectBox msEditorRole;
        private MultiSelectBox msReviewerRole;

        #endregion

        private Button btOk;
        private Button btCancel;
        private Button btDelete;

        #region Public Control Properties

        [Bindable(true), Category("Data"), DefaultValue(0)]
        public int DbContentCatalogID
        {
            get
            {
                return dbContentCatalogID;
            }
            set
            {
                loadFlag = true;
                dbContentCatalogID = value;
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
            content.Controls.Add(tabstrip);

            #endregion

            #region MultiPage

            multipage = new ComponentArt.Web.UI.MultiPage();
            multipage.ID = this.ID + "_MultiPage";
            multipage.CssClass = "MultiPage";
            content.Controls.Add(multipage);

            #endregion

            #region Child Controls for General Folder

            GeneralView = new ComponentArt.Web.UI.PageView();
            GeneralView.CssClass = "PageContent";
            multipage.PageViews.Add(GeneralView);

            TabStripTab GeneralTab = new TabStripTab();
            GeneralTab.Text = "General";
            GeneralTab.PageViewId = GeneralView.ID;
            tabstrip.Tabs.Add(GeneralTab);

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Title</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbTitle = new TextBox();
            tbTitle.EnableViewState = false;
            GeneralView.Controls.Add(tbTitle);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Description</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDescription = new TextBox();
            tbDescription.EnableViewState = false;
            GeneralView.Controls.Add(tbDescription);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Keywords</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbKeywords = new TextBox();
            tbKeywords.EnableViewState = false;
            GeneralView.Controls.Add(tbKeywords);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Status</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbStatus = new TextBox();
            tbStatus.EnableViewState = false;
            GeneralView.Controls.Add(tbStatus);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Workflow Mode</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbWorkflowMode = new TextBox();
            tbWorkflowMode.EnableViewState = false;
            GeneralView.Controls.Add(tbWorkflowMode);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">CommentsEnabled</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbCommentsEnabled = new CheckBox();
            cbCommentsEnabled.EnableViewState = false;
            GeneralView.Controls.Add(cbCommentsEnabled);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">NotifyOnComments</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbNotifyOnComments = new CheckBox();
            cbNotifyOnComments.EnableViewState = false;
            GeneralView.Controls.Add(cbNotifyOnComments);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Enabled</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbEnabled = new CheckBox();
            cbEnabled.EnableViewState = false;
            GeneralView.Controls.Add(cbEnabled);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">SortOrder</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbSortOrder = new TextBox();
            tbSortOrder.ID = this.ID + "_SortOrder";
            tbSortOrder.EnableViewState = false;
            GeneralView.Controls.Add(tbSortOrder);
            revSortOrder = new RegularExpressionValidator();
            revSortOrder.ControlToValidate = tbSortOrder.ID;
            revSortOrder.ValidationExpression = "^(\\+|-)?\\d+$";
            revSortOrder.ErrorMessage = "*";
            revSortOrder.Display = ValidatorDisplay.Dynamic;
            revSortOrder.EnableViewState = false;
            GeneralView.Controls.Add(revSortOrder);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">ParentCatalog</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msParentCatalog = new MultiSelectBox();
            msParentCatalog.Mode = MultiSelectBoxMode.DropDownList;
            GeneralView.Controls.Add(msParentCatalog);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">ChildCatalogs</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msChildCatalogs = new MultiSelectBox();
            msChildCatalogs.Mode = MultiSelectBoxMode.DualSelect;
            GeneralView.Controls.Add(msChildCatalogs);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">DefaultClip</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msDefaultClip = new MultiSelectBox();
            msDefaultClip.Mode = MultiSelectBoxMode.DropDownList;
            GeneralView.Controls.Add(msDefaultClip);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            GeneralView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Icon</span>"));
            GeneralView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbIcon = new TextBox();
            tbIcon.EnableViewState = false;
            GeneralView.Controls.Add(tbIcon);
            GeneralView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Child Controls for _system Folder

            _systemView = new ComponentArt.Web.UI.PageView();
            _systemView.CssClass = "PageContent";
            multipage.PageViews.Add(_systemView);

            TabStripTab _systemTab = new TabStripTab();
            _systemTab.Text = "_system";
            _systemTab.PageViewId = _systemView.ID;
            tabstrip.Tabs.Add(_systemTab);

            _systemView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            _systemView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">CreateDate</span>"));
            _systemView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            ltCreateDate = new Literal();
            ltCreateDate.EnableViewState = false;
            _systemView.Controls.Add(ltCreateDate);
            _systemView.Controls.Add(new LiteralControl("</span></div>"));

            _systemView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            _systemView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">ModifyDate</span>"));
            _systemView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            ltModifyDate = new Literal();
            ltModifyDate.EnableViewState = false;
            _systemView.Controls.Add(ltModifyDate);
            _systemView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Child Controls for Defaults Folder

            DefaultsView = new ComponentArt.Web.UI.PageView();
            DefaultsView.CssClass = "PageContent";
            multipage.PageViews.Add(DefaultsView);

            TabStripTab DefaultsTab = new TabStripTab();
            DefaultsTab.Text = "Defaults";
            DefaultsTab.PageViewId = DefaultsView.ID;
            tabstrip.Tabs.Add(DefaultsTab);

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Time To Publish</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDefaultTimeToPublish = new TextBox();
            tbDefaultTimeToPublish.EnableViewState = false;
            DefaultsView.Controls.Add(tbDefaultTimeToPublish);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Time To Expiration</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDefaultTimeToExpire = new TextBox();
            tbDefaultTimeToExpire.EnableViewState = false;
            DefaultsView.Controls.Add(tbDefaultTimeToExpire);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Time To Archive</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDefaultTimeToArchive = new TextBox();
            tbDefaultTimeToArchive.EnableViewState = false;
            DefaultsView.Controls.Add(tbDefaultTimeToArchive);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">DefaultKeywords</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDefaultKeywords = new TextBox();
            tbDefaultKeywords.EnableViewState = false;
            DefaultsView.Controls.Add(tbDefaultKeywords);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">DefaultStatus</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msDefaultStatus = new MultiSelectBox();
            msDefaultStatus.Mode = MultiSelectBoxMode.DropDownList;
            DefaultsView.Controls.Add(msDefaultStatus);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">DefaultRating</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msDefaultRating = new MultiSelectBox();
            msDefaultRating.Mode = MultiSelectBoxMode.DropDownList;
            DefaultsView.Controls.Add(msDefaultRating);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Default Archive</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msDefaultArchive = new MultiSelectBox();
            msDefaultArchive.Mode = MultiSelectBoxMode.DropDownList;
            DefaultsView.Controls.Add(msDefaultArchive);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Templates</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msTemplates = new MultiSelectBox();
            msTemplates.Mode = MultiSelectBoxMode.DualSelect;
            DefaultsView.Controls.Add(msTemplates);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">DefaultMenuLeftIcon</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDefaultMenuLeftIcon = new TextBox();
            tbDefaultMenuLeftIcon.EnableViewState = false;
            DefaultsView.Controls.Add(tbDefaultMenuLeftIcon);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            DefaultsView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">DefaultMenuRightIcon</span>"));
            DefaultsView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDefaultMenuRightIcon = new TextBox();
            tbDefaultMenuRightIcon.EnableViewState = false;
            DefaultsView.Controls.Add(tbDefaultMenuRightIcon);
            DefaultsView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Child Controls for Menu Folder

            MenuView = new ComponentArt.Web.UI.PageView();
            MenuView.CssClass = "PageContent";
            multipage.PageViews.Add(MenuView);

            TabStripTab MenuTab = new TabStripTab();
            MenuTab.Text = "Menu";
            MenuTab.PageViewId = MenuView.ID;
            tabstrip.Tabs.Add(MenuTab);

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Label</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuLabel = new TextBox();
            tbMenuLabel.EnableViewState = false;
            MenuView.Controls.Add(tbMenuLabel);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Tooltip</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuTooltip = new TextBox();
            tbMenuTooltip.EnableViewState = false;
            MenuView.Controls.Add(tbMenuTooltip);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Enabled</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbMenuEnabled = new CheckBox();
            cbMenuEnabled.EnableViewState = false;
            MenuView.Controls.Add(cbMenuEnabled);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Menu Order</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuOrder = new TextBox();
            tbMenuOrder.ID = this.ID + "_MenuOrder";
            tbMenuOrder.EnableViewState = false;
            MenuView.Controls.Add(tbMenuOrder);
            revMenuOrder = new RegularExpressionValidator();
            revMenuOrder.ControlToValidate = tbMenuOrder.ID;
            revMenuOrder.ValidationExpression = "^(\\+|-)?\\d+$";
            revMenuOrder.ErrorMessage = "*";
            revMenuOrder.Display = ValidatorDisplay.Dynamic;
            revMenuOrder.EnableViewState = false;
            MenuView.Controls.Add(revMenuOrder);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Left Icon URL</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuLeftIcon = new TextBox();
            tbMenuLeftIcon.EnableViewState = false;
            MenuView.Controls.Add(tbMenuLeftIcon);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Right Icon URL</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuRightIcon = new TextBox();
            tbMenuRightIcon.EnableViewState = false;
            MenuView.Controls.Add(tbMenuRightIcon);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Break Image URL</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuBreakImage = new TextBox();
            tbMenuBreakImage.EnableViewState = false;
            MenuView.Controls.Add(tbMenuBreakImage);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Break CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuBreakCssClass = new TextBox();
            tbMenuBreakCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuBreakCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuCssClass = new TextBox();
            tbMenuCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Catalog CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuCatalogCssClass = new TextBox();
            tbMenuCatalogCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuCatalogCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Catalog Selected CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuCatalogSelectedCssClass = new TextBox();
            tbMenuCatalogSelectedCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuCatalogSelectedCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Catalog Child Selected CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuCatalogChildSelectedCssClass = new TextBox();
            tbMenuCatalogChildSelectedCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuCatalogChildSelectedCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Clip CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuClipCssClass = new TextBox();
            tbMenuClipCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuClipCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Clip Selected CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuClipSelectedCssClass = new TextBox();
            tbMenuClipSelectedCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuClipSelectedCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Clip Child Selected CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuClipChildSelectedCssClass = new TextBox();
            tbMenuClipChildSelectedCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuClipChildSelectedCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Clip Child Expanded CSS Class</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuClipChildExpandedCssClass = new TextBox();
            tbMenuClipChildExpandedCssClass.EnableViewState = false;
            MenuView.Controls.Add(tbMenuClipChildExpandedCssClass);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Style Overrides</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuOverrideFlags = new TextBox();
            tbMenuOverrideFlags.EnableViewState = false;
            MenuView.Controls.Add(tbMenuOverrideFlags);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            MenuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">MenuIconFlags</span>"));
            MenuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuIconFlags = new TextBox();
            tbMenuIconFlags.EnableViewState = false;
            MenuView.Controls.Add(tbMenuIconFlags);
            MenuView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Child Controls for Security Folder

            SecurityView = new ComponentArt.Web.UI.PageView();
            SecurityView.CssClass = "PageContent";
            multipage.PageViews.Add(SecurityView);

            TabStripTab SecurityTab = new TabStripTab();
            SecurityTab.Text = "Security";
            SecurityTab.PageViewId = SecurityView.ID;
            tabstrip.Tabs.Add(SecurityTab);

            SecurityView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            SecurityView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Author Roles</span>"));
            SecurityView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msAuthorRole = new MultiSelectBox();
            msAuthorRole.Mode = MultiSelectBoxMode.DropDownList;
            SecurityView.Controls.Add(msAuthorRole);
            SecurityView.Controls.Add(new LiteralControl("</span></div>"));

            SecurityView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            SecurityView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Editor Roles</span>"));
            SecurityView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msEditorRole = new MultiSelectBox();
            msEditorRole.Mode = MultiSelectBoxMode.DropDownList;
            SecurityView.Controls.Add(msEditorRole);
            SecurityView.Controls.Add(new LiteralControl("</span></div>"));

            SecurityView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            SecurityView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Reviewer Role</span>"));
            SecurityView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msReviewerRole = new MultiSelectBox();
            msReviewerRole.Mode = MultiSelectBoxMode.DropDownList;
            SecurityView.Controls.Add(msReviewerRole);
            SecurityView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            Panel buttons = new Panel();
            buttons.CssClass = "pButtons";
            content.Controls.Add(buttons);

            btOk = new Button();
            btOk.Text = "OK";
            btOk.Width = Unit.Pixel(72);
            btOk.EnableViewState = false;
            btOk.Click += new EventHandler(ok_Click);
            buttons.Controls.Add(btOk);

            btCancel = new Button();
            btCancel.Text = "Cancel";
            btCancel.Width = Unit.Pixel(72);
            btCancel.EnableViewState = false;
            btCancel.CausesValidation = false;
            btCancel.Click += new EventHandler(cancel_Click);
            buttons.Controls.Add(btCancel);

            btDelete = new Button();
            btDelete.Text = "Delete";
            btDelete.Width = Unit.Pixel(72);
            btDelete.EnableViewState = false;
            btDelete.Click += new EventHandler(delete_Click);
            buttons.Controls.Add(btDelete);

            bind();
            ChildControlsCreated = true;
        }

        private void bind()
        {
            #region Bind General Child Data

            msParentCatalog.Items.Add(new ListItem("Null", "Null"));
            DbContentCatalogManager parentCatalogManager = new DbContentCatalogManager();
            DbContentCatalogCollection parentCatalogCollection = parentCatalogManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentCatalog parentCatalog in parentCatalogCollection)
            {
                ListItem i = new ListItem(parentCatalog.ToString(), parentCatalog.ID.ToString());
                msParentCatalog.Items.Add(i);
            }

            msChildCatalogs.Items.Add(new ListItem("Null", "Null"));
            DbContentCatalogManager childCatalogsManager = new DbContentCatalogManager();
            DbContentCatalogCollection childCatalogsCollection = childCatalogsManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentCatalog childCatalogs in childCatalogsCollection)
            {
                ListItem i = new ListItem(childCatalogs.ToString(), childCatalogs.ID.ToString());
                msChildCatalogs.Items.Add(i);
            }

            msDefaultClip.Items.Add(new ListItem("Null", "Null"));
            DbContentClipManager defaultClipManager = new DbContentClipManager();
            DbContentClipCollection defaultClipCollection = defaultClipManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentClip defaultClip in defaultClipCollection)
            {
                ListItem i = new ListItem(defaultClip.ToString(), defaultClip.ID.ToString());
                msDefaultClip.Items.Add(i);
            }

            #endregion

            #region Bind Defaults Child Data

            msDefaultStatus.Items.Add(new ListItem("Null", "Null"));
            DbContentStatusManager defaultStatusManager = new DbContentStatusManager();
            DbContentStatusCollection defaultStatusCollection = defaultStatusManager.GetCollection(string.Empty, string.Empty);
            foreach (DbContentStatus defaultStatus in defaultStatusCollection)
            {
                ListItem i = new ListItem(defaultStatus.ToString(), defaultStatus.ID.ToString());
                msDefaultStatus.Items.Add(i);
            }

            msDefaultRating.Items.Add(new ListItem("Null", "Null"));
            DbContentRatingManager defaultRatingManager = new DbContentRatingManager();
            DbContentRatingCollection defaultRatingCollection = defaultRatingManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentRating defaultRating in defaultRatingCollection)
            {
                ListItem i = new ListItem(defaultRating.ToString(), defaultRating.ID.ToString());
                msDefaultRating.Items.Add(i);
            }

            msDefaultArchive.Items.Add(new ListItem("Null", "Null"));
            DbContentCatalogManager defaultArchiveManager = new DbContentCatalogManager();
            DbContentCatalogCollection defaultArchiveCollection = defaultArchiveManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentCatalog defaultArchive in defaultArchiveCollection)
            {
                ListItem i = new ListItem(defaultArchive.ToString(), defaultArchive.ID.ToString());
                msDefaultArchive.Items.Add(i);
            }

            msTemplates.Items.Add(new ListItem("Null", "Null"));
            DbContentClipManager templatesManager = new DbContentClipManager();
            DbContentClipCollection templatesCollection = templatesManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentClip templates in templatesCollection)
            {
                ListItem i = new ListItem(templates.ToString(), templates.ID.ToString());
                msTemplates.Items.Add(i);
            }

            #endregion

            #region Bind Security Child Data

            msAuthorRole.Items.Add(new ListItem("Null", "Null"));
            GreyFoxRoleManager authorRoleManager = new GreyFoxRoleManager();
            GreyFoxRoleCollection authorRoleCollection = authorRoleManager.GetCollection(string.Empty, string.Empty);
            foreach (GreyFoxRole authorRole in authorRoleCollection)
            {
                ListItem i = new ListItem(authorRole.ToString(), authorRole.ID.ToString());
                msAuthorRole.Items.Add(i);
            }

            msEditorRole.Items.Add(new ListItem("Null", "Null"));
            GreyFoxRoleManager editorRoleManager = new GreyFoxRoleManager();
            GreyFoxRoleCollection editorRoleCollection = editorRoleManager.GetCollection(string.Empty, string.Empty);
            foreach (GreyFoxRole editorRole in editorRoleCollection)
            {
                ListItem i = new ListItem(editorRole.ToString(), editorRole.ID.ToString());
                msEditorRole.Items.Add(i);
            }

            msReviewerRole.Items.Add(new ListItem("Null", "Null"));
            GreyFoxRoleManager reviewerRoleManager = new GreyFoxRoleManager();
            GreyFoxRoleCollection reviewerRoleCollection = reviewerRoleManager.GetCollection(string.Empty, string.Empty);
            foreach (GreyFoxRole reviewerRole in reviewerRoleCollection)
            {
                ListItem i = new ListItem(reviewerRole.ToString(), reviewerRole.ID.ToString());
                msReviewerRole.Items.Add(i);
            }

            #endregion

        }

        #region Events

        protected void ok_Click(object sender, EventArgs e)
        {
            if (dbContentCatalogID == 0)
                obj = new DbContentCatalog();
            else
                obj = new DbContentCatalog(dbContentCatalogID);

            obj.Title = tbTitle.Text;
            obj.Description = tbDescription.Text;
            obj.Keywords = tbKeywords.Text;
            obj.Status = byte.Parse(tbStatus.Text);
            obj.WorkflowMode = byte.Parse(tbWorkflowMode.Text);
            obj.CommentsEnabled = cbCommentsEnabled.Checked;
            obj.NotifyOnComments = cbNotifyOnComments.Checked;
            obj.Enabled = cbEnabled.Checked;
            obj.SortOrder = int.Parse(tbSortOrder.Text);
            obj.Icon = tbIcon.Text;
            obj.DefaultTimeToPublish = TimeSpan.Parse(tbDefaultTimeToPublish.Text);
            obj.DefaultTimeToExpire = TimeSpan.Parse(tbDefaultTimeToExpire.Text);
            obj.DefaultTimeToArchive = TimeSpan.Parse(tbDefaultTimeToArchive.Text);
            obj.DefaultKeywords = tbDefaultKeywords.Text;
            obj.DefaultMenuLeftIcon = tbDefaultMenuLeftIcon.Text;
            obj.DefaultMenuRightIcon = tbDefaultMenuRightIcon.Text;
            obj.MenuLabel = tbMenuLabel.Text;
            obj.MenuTooltip = tbMenuTooltip.Text;
            obj.MenuEnabled = cbMenuEnabled.Checked;
            obj.MenuOrder = int.Parse(tbMenuOrder.Text);
            obj.MenuLeftIcon = tbMenuLeftIcon.Text;
            obj.MenuRightIcon = tbMenuRightIcon.Text;
            obj.MenuBreakImage = tbMenuBreakImage.Text;
            obj.MenuBreakCssClass = tbMenuBreakCssClass.Text;
            obj.MenuCssClass = tbMenuCssClass.Text;
            obj.MenuCatalogCssClass = tbMenuCatalogCssClass.Text;
            obj.MenuCatalogSelectedCssClass = tbMenuCatalogSelectedCssClass.Text;
            obj.MenuCatalogChildSelectedCssClass = tbMenuCatalogChildSelectedCssClass.Text;
            obj.MenuClipCssClass = tbMenuClipCssClass.Text;
            obj.MenuClipSelectedCssClass = tbMenuClipSelectedCssClass.Text;
            obj.MenuClipChildSelectedCssClass = tbMenuClipChildSelectedCssClass.Text;
            obj.MenuClipChildExpandedCssClass = tbMenuClipChildExpandedCssClass.Text;
            obj.MenuOverrideFlags = byte.Parse(tbMenuOverrideFlags.Text);
            obj.MenuIconFlags = byte.Parse(tbMenuIconFlags.Text);

            if (msParentCatalog.SelectedItem != null && msParentCatalog.SelectedItem.Value != "Null")
                obj.ParentCatalog = DbContentCatalog.NewPlaceHolder(
                    int.Parse(msParentCatalog.SelectedItem.Value));
            else
                obj.ParentCatalog = null;

            if (msChildCatalogs.IsChanged)
            {
                obj.ChildCatalogs = new DbContentCatalogCollection();
                foreach (ListItem i in msChildCatalogs.Items)
                    if (i.Selected)
                        obj.ChildCatalogs.Add(DbContentCatalog.NewPlaceHolder(int.Parse(i.Value)));
            }

            if (msDefaultClip.SelectedItem != null && msDefaultClip.SelectedItem.Value != "Null")
                obj.DefaultClip = DbContentClip.NewPlaceHolder(
                    int.Parse(msDefaultClip.SelectedItem.Value));
            else
                obj.DefaultClip = null;

            if (msDefaultStatus.SelectedItem != null && msDefaultStatus.SelectedItem.Value != "Null")
                obj.DefaultStatus = DbContentStatus.NewPlaceHolder(
                    int.Parse(msDefaultStatus.SelectedItem.Value));
            else
                obj.DefaultStatus = null;

            if (msDefaultRating.SelectedItem != null && msDefaultRating.SelectedItem.Value != "Null")
                obj.DefaultRating = DbContentRating.NewPlaceHolder(
                    int.Parse(msDefaultRating.SelectedItem.Value));
            else
                obj.DefaultRating = null;

            if (msDefaultArchive.SelectedItem != null && msDefaultArchive.SelectedItem.Value != "Null")
                obj.DefaultArchive = DbContentCatalog.NewPlaceHolder(
                    int.Parse(msDefaultArchive.SelectedItem.Value));
            else
                obj.DefaultArchive = null;

            if (msTemplates.IsChanged)
            {
                obj.Templates = new DbContentClipCollection();
                foreach (ListItem i in msTemplates.Items)
                    if (i.Selected)
                        obj.Templates.Add(DbContentClip.NewPlaceHolder(int.Parse(i.Value)));
            }

            if (msAuthorRole.SelectedItem != null && msAuthorRole.SelectedItem.Value != "Null")
                obj.AuthorRole = GreyFoxRole.NewPlaceHolder(
                    int.Parse(msAuthorRole.SelectedItem.Value));
            else
                obj.AuthorRole = null;

            if (msEditorRole.SelectedItem != null && msEditorRole.SelectedItem.Value != "Null")
                obj.EditorRole = GreyFoxRole.NewPlaceHolder(
                    int.Parse(msEditorRole.SelectedItem.Value));
            else
                obj.EditorRole = null;

            if (msReviewerRole.SelectedItem != null && msReviewerRole.SelectedItem.Value != "Null")
                obj.ReviewerRole = GreyFoxRole.NewPlaceHolder(
                    int.Parse(msReviewerRole.SelectedItem.Value));
            else
                obj.ReviewerRole = null;

            if (editOnAdd)
                dbContentCatalogID = obj.Save();
            else
                obj.Save();

            if (resetOnAdd)
            {
                tbTitle.Text = string.Empty;
                tbDescription.Text = string.Empty;
                tbKeywords.Text = string.Empty;
                tbStatus.Text = string.Empty;
                tbWorkflowMode.Text = string.Empty;
                cbCommentsEnabled.Checked = false;
                cbNotifyOnComments.Checked = false;
                cbEnabled.Checked = false;
                tbSortOrder.Text = string.Empty;
                tbIcon.Text = string.Empty;
                tbDefaultTimeToPublish.Text = string.Empty;
                tbDefaultTimeToExpire.Text = string.Empty;
                tbDefaultTimeToArchive.Text = string.Empty;
                tbDefaultKeywords.Text = string.Empty;
                tbDefaultMenuLeftIcon.Text = string.Empty;
                tbDefaultMenuRightIcon.Text = string.Empty;
                tbMenuLabel.Text = string.Empty;
                tbMenuTooltip.Text = string.Empty;
                cbMenuEnabled.Checked = false;
                tbMenuOrder.Text = string.Empty;
                tbMenuLeftIcon.Text = string.Empty;
                tbMenuRightIcon.Text = string.Empty;
                tbMenuBreakImage.Text = string.Empty;
                tbMenuBreakCssClass.Text = string.Empty;
                tbMenuCssClass.Text = string.Empty;
                tbMenuCatalogCssClass.Text = string.Empty;
                tbMenuCatalogSelectedCssClass.Text = string.Empty;
                tbMenuCatalogChildSelectedCssClass.Text = string.Empty;
                tbMenuClipCssClass.Text = string.Empty;
                tbMenuClipSelectedCssClass.Text = string.Empty;
                tbMenuClipChildSelectedCssClass.Text = string.Empty;
                tbMenuClipChildExpandedCssClass.Text = string.Empty;
                tbMenuOverrideFlags.Text = string.Empty;
                tbMenuIconFlags.Text = string.Empty;
                msParentCatalog.SelectedIndex = 0;
                msDefaultClip.SelectedIndex = 0;
                msDefaultStatus.SelectedIndex = 0;
                msDefaultRating.SelectedIndex = 0;
                msDefaultArchive.SelectedIndex = 0;
                msAuthorRole.SelectedIndex = 0;
                msEditorRole.SelectedIndex = 0;
                msReviewerRole.SelectedIndex = 0;
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
            if (Cancelled != null)
                Cancelled(this, e);
        }

        public event EventHandler Updated;
        protected virtual void OnUpdated(EventArgs e)
        {
            if (Updated != null)
                Updated(this, e);
        }

        public event EventHandler DeleteClicked;
        protected virtual void OnDeleteClicked(EventArgs e)
        {
            if (DeleteClicked != null)
                DeleteClicked(this, e);
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            if (loadFlag)
            {
                if (dbContentCatalogID > 0)
                {
                    obj = new DbContentCatalog(dbContentCatalogID);
                    headerText.Text = "Edit  - " + obj.ToString();
                }
                else if (dbContentCatalogID <= 0)
                {
                    obj = new DbContentCatalog();
                    headerText.Text = "Add ";
                }

                //
                // Set Field Entries
                //
                tbTitle.Text = obj.Title;
                tbDescription.Text = obj.Description;
                tbKeywords.Text = obj.Keywords;
                tbStatus.Text = obj.Status.ToString();
                tbWorkflowMode.Text = obj.WorkflowMode.ToString();
                cbCommentsEnabled.Checked = obj.CommentsEnabled;
                cbNotifyOnComments.Checked = obj.NotifyOnComments;
                cbEnabled.Checked = obj.Enabled;
                tbSortOrder.Text = obj.SortOrder.ToString();
                tbIcon.Text = obj.Icon;
                ltCreateDate.Text = obj.CreateDate.ToString();
                ltModifyDate.Text = obj.ModifyDate.ToString();
                tbDefaultTimeToPublish.Text = obj.DefaultTimeToPublish.ToString();
                tbDefaultTimeToExpire.Text = obj.DefaultTimeToExpire.ToString();
                tbDefaultTimeToArchive.Text = obj.DefaultTimeToArchive.ToString();
                tbDefaultKeywords.Text = obj.DefaultKeywords;
                tbDefaultMenuLeftIcon.Text = obj.DefaultMenuLeftIcon;
                tbDefaultMenuRightIcon.Text = obj.DefaultMenuRightIcon;
                tbMenuLabel.Text = obj.MenuLabel;
                tbMenuTooltip.Text = obj.MenuTooltip;
                cbMenuEnabled.Checked = obj.MenuEnabled;
                tbMenuOrder.Text = obj.MenuOrder.ToString();
                tbMenuLeftIcon.Text = obj.MenuLeftIcon;
                tbMenuRightIcon.Text = obj.MenuRightIcon;
                tbMenuBreakImage.Text = obj.MenuBreakImage;
                tbMenuBreakCssClass.Text = obj.MenuBreakCssClass;
                tbMenuCssClass.Text = obj.MenuCssClass;
                tbMenuCatalogCssClass.Text = obj.MenuCatalogCssClass;
                tbMenuCatalogSelectedCssClass.Text = obj.MenuCatalogSelectedCssClass;
                tbMenuCatalogChildSelectedCssClass.Text = obj.MenuCatalogChildSelectedCssClass;
                tbMenuClipCssClass.Text = obj.MenuClipCssClass;
                tbMenuClipSelectedCssClass.Text = obj.MenuClipSelectedCssClass;
                tbMenuClipChildSelectedCssClass.Text = obj.MenuClipChildSelectedCssClass;
                tbMenuClipChildExpandedCssClass.Text = obj.MenuClipChildExpandedCssClass;
                tbMenuOverrideFlags.Text = obj.MenuOverrideFlags.ToString();
                tbMenuIconFlags.Text = obj.MenuIconFlags.ToString();

                //
                // Set Children Selections
                //
                if (obj.ParentCatalog != null)
                    foreach (ListItem item in msParentCatalog.Items)
                        item.Selected = obj.ParentCatalog.ID.ToString() == item.Value;
                else
                    msParentCatalog.SelectedIndex = 0;

                foreach (ListItem i in msChildCatalogs.Items)
                    foreach (DbContentCatalog dbContentCatalog in obj.ChildCatalogs)
                        if (i.Value == dbContentCatalog.ID.ToString())
                        {
                            i.Selected = true;
                            break;
                        }

                if (obj.DefaultClip != null)
                    foreach (ListItem item in msDefaultClip.Items)
                        item.Selected = obj.DefaultClip.ID.ToString() == item.Value;
                else
                    msDefaultClip.SelectedIndex = 0;

                if (obj.DefaultStatus != null)
                    foreach (ListItem item in msDefaultStatus.Items)
                        item.Selected = obj.DefaultStatus.ID.ToString() == item.Value;
                else
                    msDefaultStatus.SelectedIndex = 0;

                if (obj.DefaultRating != null)
                    foreach (ListItem item in msDefaultRating.Items)
                        item.Selected = obj.DefaultRating.ID.ToString() == item.Value;
                else
                    msDefaultRating.SelectedIndex = 0;

                if (obj.DefaultArchive != null)
                    foreach (ListItem item in msDefaultArchive.Items)
                        item.Selected = obj.DefaultArchive.ID.ToString() == item.Value;
                else
                    msDefaultArchive.SelectedIndex = 0;

                foreach (ListItem i in msTemplates.Items)
                    foreach (DbContentClip dbContentClip in obj.Templates)
                        if (i.Value == dbContentClip.ID.ToString())
                        {
                            i.Selected = true;
                            break;
                        }

                if (obj.AuthorRole != null)
                    foreach (ListItem item in msAuthorRole.Items)
                        item.Selected = obj.AuthorRole.ID.ToString() == item.Value;
                else
                    msAuthorRole.SelectedIndex = 0;

                if (obj.EditorRole != null)
                    foreach (ListItem item in msEditorRole.Items)
                        item.Selected = obj.EditorRole.ID.ToString() == item.Value;
                else
                    msEditorRole.SelectedIndex = 0;

                if (obj.ReviewerRole != null)
                    foreach (ListItem item in msReviewerRole.Items)
                        item.Selected = obj.ReviewerRole.ID.ToString() == item.Value;
                else
                    msReviewerRole.SelectedIndex = 0;

                tabstrip.SelectedTab = tabstrip.Tabs[0];
            }
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] myState = (object[])savedState;
                if (myState[0] != null)
                    base.LoadViewState(myState[0]);
                if (myState[1] != null)
                    dbContentCatalogID = (int)myState[1];
            }
        }

        protected override object SaveViewState()
        {
            object baseState = base.SaveViewState();
            object[] myState = new object[2];
            myState[0] = baseState;
            myState[1] = dbContentCatalogID;
            return myState;
        }
    }
}

