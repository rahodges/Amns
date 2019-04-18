using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Amns.GreyFox.Web.Util;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentClip.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
		ToolboxData("<{0}:DbContentClipEditor runat=server></{0}:DbContentClipEditor>")]
	public class DbContentClipEditor : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		int dbContentClipID;
		int catalogID;

		DbContentClip editDbContentClip;
		string connectionString;
		bool loadFlag = false;
		bool resetOnAdd;
		bool editOnAdd;
        string _designModeCss = string.Empty;
        string _editorUserControlUrl = "~/_skins/editor/editor.ascx";

        private ComponentArt.Web.UI.TabStrip tabstrip;
        private ComponentArt.Web.UI.MultiPage multipage;
        private Literal headerText;

		#region Private Control Fields for General Folder

        private ComponentArt.Web.UI.PageView generalView;
		private TextBox tbTitle;
		private TextBox tbDescription;
		private TextBox tbKeywords;
		private TextBox tbIcon;
		private TextBox tbOverrideUrl;
		private ComponentArt.Web.UI.ComboBox ratingComboBox;
		private MultiSelectBox msStatus;

		#endregion

		#region Private Control Fields for Body Folder

        private ComponentArt.Web.UI.PageView bodyView;
        //private Control userEditor;
        //private ComponentArt.Web.UI.Editor caEditor;
        private TextBox tbBody;

		#endregion

        #region Private Control Fields for Index Folder

        private ComponentArt.Web.UI.PageView indexView;
        private TextBox tbReferences;
        private ComponentArt.Web.UI.ComboBox referenceComboBox;

        #endregion

		#region Private Control Fields for Publishing Folder

        private ComponentArt.Web.UI.PageView publishingView;

        private Literal ltCreateDate;
        private Literal ltModifyDate;    
        
		private MultiSelectBox msParentCatalog = new MultiSelectBox();        

		private ComponentArt.Web.UI.Calendar calPublishDateP;
		private System.Web.UI.HtmlControls.HtmlImage calPublishDateB;
		private ComponentArt.Web.UI.Calendar calPublishDateC;

		private ComponentArt.Web.UI.Calendar calExpirationDateP;
		private System.Web.UI.HtmlControls.HtmlImage calExpirationDateB;
		private ComponentArt.Web.UI.Calendar calExpirationDateC;

		private ComponentArt.Web.UI.Calendar calArchiveDateP;
		private System.Web.UI.HtmlControls.HtmlImage calArchiveDateB;
		private ComponentArt.Web.UI.Calendar calArchiveDateC;

		private TextBox tbPriority;
		private TextBox tbSortOrder;
		private CheckBox cbCommentsEnabled;
		private CheckBox cbNotifyOnComments;

		#endregion

		#region Private Control Fields for Contributors Folder

        private ComponentArt.Web.UI.PageView contributorView;
		private TextBox tbAuthors;
		private TextBox tbEditors;

		#endregion

		#region Private Control Fields for Menu Folder

        private ComponentArt.Web.UI.PageView menuView;
		private TextBox tbMenuLabel;
		private TextBox tbMenuTooltip;
		private CheckBox cbMenuEnabled;
		private TextBox tbMenuOrder;
		private TextBox tbMenuLeftIcon;
		private TextBox tbMenuLeftIconOver;
		private TextBox tbMenuRightIcon;
		private TextBox tbMenuRightIconOver;
		private CheckBox cbMenuBreak;

		#endregion

		private Button btOk;
        private Button btCancel;
        private Button btDelete;

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int DbContentClipID
		{
			get
			{
				return dbContentClipID;
			}
			set
			{
				loadFlag = true;
				dbContentClipID = value;
			}
		}

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int CatalogID
		{
			get
			{
				return catalogID;
			}
			set
			{
				catalogID = value;
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

        [Bindable(true), Category("Behavior"), DefaultValue("")]
        public string DesignModeCss
        {
            get { return _designModeCss; }
            set { _designModeCss = value; }
        }

        [Bindable(true), Category("Behavior"), DefaultValue("~/_skins/editor/editor.ascx")]
        public string EditorUserControlUrl
        {
            get { return _editorUserControlUrl; }
            set { _editorUserControlUrl = value; }
        }

		[Bindable(false),
			Category("Data"),
			DefaultValue("")]
		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				// Parse Connection String
				if(value.StartsWith("<jet40virtual>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(value.Substring(14, value.Length - 14));
				else if(value.StartsWith("<jet40config>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get(value.Substring(13, value.Length - 13)));
				else
					connectionString = value;
			}
		}

		#endregion

        private void bind()
        {
            #region Bind General Child Data

            DbContentCatalogManager parentCatalogManager = new DbContentCatalogManager();
            DbContentCatalogCollection parentCatalogCollection = parentCatalogManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentCatalog parentCatalog in parentCatalogCollection)
            {
                ListItem i = new ListItem(parentCatalog.ToString(), parentCatalog.ID.ToString());
                msParentCatalog.Items.Add(i);
            }

            DbContentRatingManager ratingManager = new DbContentRatingManager();
            DbContentRatingCollection ratingCollection = ratingManager.GetCollection(string.Empty, string.Empty, null);
            foreach (DbContentRating rating in ratingCollection)
            {
                ComboBoxItem i = new ComboBoxItem();
                i.Text = rating.Name;
                i.Value = rating.ID.ToString();
                ratingComboBox.Items.Add(i);
            }

            DbContentStatusManager statusManager = new DbContentStatusManager();
            DbContentStatusCollection statusCollection = statusManager.GetCollection(string.Empty, string.Empty);
            foreach (DbContentStatus status in statusCollection)
            {
                ListItem i = new ListItem(status.ToString(), status.ID.ToString());
                msStatus.Items.Add(i);
            }

            DbContentClipManager clipManager = new DbContentClipManager();
            DbContentClipCollection clips = clipManager.GetCollection(string.Empty, "Title", null);
            foreach (DbContentClip clip in clips)
            {
                referenceComboBox.Items.Add(
                    new ComboBoxItem(
                    Regex.Replace(clip.Title, "<[^>]*>", "")));
            }

            #endregion
        }

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

            multipage = new ComponentArt.Web.UI.MultiPage();
            multipage.ID = this.ID + "_MultiPage";
            multipage.CssClass = "MultiPage";
            content.Controls.Add(multipage);

            #region General Folder

            generalView = new ComponentArt.Web.UI.PageView();

            generalView.CssClass = "PageContent";
            multipage.PageViews.Add(generalView);

            TabStripTab generalTab = new TabStripTab();
            generalTab.Text = "General";
            generalTab.PageViewId = generalView.ID;
            tabstrip.Tabs.Add(generalTab);

            generalView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Title</span>"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbTitle = new TextBox();
            tbTitle.EnableViewState = false;
            tbTitle.Rows = 2;
            tbTitle.MaxLength = 255;
            tbTitle.TextMode = TextBoxMode.MultiLine;
            tbTitle.Width = Unit.Pixel(350);
            tbTitle.ToolTip = "Title of content.";
            generalView.Controls.Add(tbTitle);
            generalView.Controls.Add(new LiteralControl("</span></div>"));

            generalView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Description</span>"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbDescription = new TextBox();
            tbDescription.EnableViewState = false;
            tbDescription.Rows = 5;
            tbDescription.MaxLength = 255;
            tbDescription.TextMode = TextBoxMode.MultiLine;
            tbDescription.Width = Unit.Pixel(350);
            tbDescription.ToolTip = "Description for search engines, directories and summaries.";
            generalView.Controls.Add(tbDescription);
            generalView.Controls.Add(new LiteralControl("</span></div>"));

            generalView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Override URL</span>"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbOverrideUrl = new TextBox();
            tbOverrideUrl.EnableViewState = false;
            tbOverrideUrl.Width = Unit.Pixel(350);
            tbOverrideUrl.ToolTip = "Overrides the body with a URL.";
            generalView.Controls.Add(tbOverrideUrl);
            generalView.Controls.Add(new LiteralControl("</span></div>"));

            generalView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Icon</span>"));
            generalView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbIcon = new TextBox();
            tbIcon.EnableViewState = false;
            tbIcon.Width = Unit.Pixel(300);
            generalView.Controls.Add(tbIcon);
            generalView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Body Folder

            bodyView = new ComponentArt.Web.UI.PageView();
            bodyView.CssClass = "PageContent";
            multipage.PageViews.Add(bodyView);

            TabStripTab bodyTab = new TabStripTab();
            bodyTab.Text = "Body";
            bodyTab.PageViewId = bodyView.ID;
            tabstrip.Tabs.Add(bodyTab);

            //userEditor = Page.LoadControl(_editorUserControlUrl);
            ////bodyView.Controls.Add(userEditor);
            //caEditor = ((IEditor)userEditor).Editor;
            //((IEditor)userEditor).SkinPath = userEditor.TemplateSourceDirectory;
            //((IEditor)userEditor).Editor.DesignCssClass = DesignModeCss;

            bodyView.Controls.Add(new LiteralControl("<div>WSYWIG editing is available through a new interface, " +
                "please use the link provided to access the HTML editor.</div>"));

            tbBody = new TextBox();
            tbBody.Rows = 40;
            tbBody.TextMode = TextBoxMode.MultiLine;
            tbBody.Width = Unit.Pixel(450);
            tbBody.EnableViewState = false;
            tbBody.Style.Add("font-family", "courier");
            bodyView.Controls.Add(tbBody);

            #endregion

            #region Index Folder

            indexView = new ComponentArt.Web.UI.PageView();

            indexView.CssClass = "PageContent";
            multipage.PageViews.Add(indexView);

            TabStripTab indexTab = new TabStripTab();
            indexTab.Text = "Indexing";
            indexTab.PageViewId = generalView.ID;
            tabstrip.Tabs.Add(indexTab);

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Keywords</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbKeywords = new TextBox();
            tbKeywords.EnableViewState = false;
            tbKeywords.Width = Unit.Pixel(350);
            tbKeywords.ToolTip = "Metadata keywords for search engines.";
            indexView.Controls.Add(tbKeywords);
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">References</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbReferences = new TextBox();
            tbReferences.ID = this.ID + "_ref";
            tbReferences.EnableViewState = false;
            tbReferences.Rows = 5;
            tbReferences.MaxLength = 1500;
            tbReferences.TextMode = TextBoxMode.MultiLine;
            tbReferences.Width = Unit.Pixel(350);
            tbReferences.ToolTip = "Referenced clips. Separate references with a carriage return.";
            indexView.Controls.Add(tbReferences);
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">&nbsp;</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\"><div style=\"float:left\">"));
            referenceComboBox = new ComponentArt.Web.UI.ComboBox();
            referenceComboBox.ID = this.ID + "_rcb";
            referenceComboBox.CssClass = "comboBox";
            referenceComboBox.HoverCssClass = "comboBoxHover";
            referenceComboBox.FocusedCssClass = "comboBoxHover";
            referenceComboBox.TextBoxCssClass = "comboTextBox";
            referenceComboBox.DropDownCssClass = "comboDropDown";
            referenceComboBox.ItemCssClass = "comboItem";
            referenceComboBox.ItemHoverCssClass = "comboItemHover";
            referenceComboBox.SelectedItemCssClass = "comboItemHover";
            referenceComboBox.DropHoverImageUrl = "combobox_images/drop_hover.gif";
            referenceComboBox.DropImageUrl = "combobox_images/drop.gif";
            referenceComboBox.Width = Unit.Pixel(250);
            referenceComboBox.EnableViewState = false;
            indexView.Controls.Add(referenceComboBox);
            indexView.Controls.Add(new LiteralControl("</div><div><input type=\"button\" value=\"Add\" " +
                "align=\"right\" onClick=\"" + 
                tbReferences.ClientID + ".value += (" + tbReferences.ClientID + ".value != '' ? '\\r\\n' : '') + " +
                referenceComboBox.ClientObjectId + ".getSelectedItem().Text;\"></div>"));
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Rating</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            ratingComboBox = new ComponentArt.Web.UI.ComboBox();
            ratingComboBox.ID = this.ID + "_rating";
            ratingComboBox.CssClass = "comboBox";
            ratingComboBox.HoverCssClass = "comboBoxHover";
            ratingComboBox.FocusedCssClass = "comboBoxHover";
            ratingComboBox.TextBoxCssClass = "comboTextBox";
            ratingComboBox.DropDownCssClass = "comboDropDown";
            ratingComboBox.ItemCssClass = "comboItem";
            ratingComboBox.ItemHoverCssClass = "comboItemHover";
            ratingComboBox.SelectedItemCssClass = "comboItemHover";
            ratingComboBox.DropHoverImageUrl = "combobox_images/drop_hover.gif";
            ratingComboBox.DropImageUrl = "combobox_images/drop.gif";
            ratingComboBox.Width = Unit.Pixel(250);
            indexView.Controls.Add(ratingComboBox);
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Status</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msStatus = new MultiSelectBox();
            msStatus.Mode = MultiSelectBoxMode.DropDownList;
            indexView.Controls.Add(msStatus);
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Priority</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbPriority = new TextBox();
            tbPriority.EnableViewState = false;
            indexView.Controls.Add(tbPriority);
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            indexView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Sort Order</span>"));
            indexView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbSortOrder = new TextBox();
            tbSortOrder.EnableViewState = false;
            indexView.Controls.Add(tbSortOrder);
            indexView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion
            
            #region Publishing Folder

            publishingView = new ComponentArt.Web.UI.PageView();
            publishingView.CssClass = "PageContent";
            multipage.PageViews.Add(publishingView);

            TabStripTab publishingTab = new TabStripTab();
            publishingTab.Text = "Publishing";
            publishingTab.PageViewId = publishingView.ID;
            tabstrip.Tabs.Add(publishingTab);

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Clip ID</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            publishingView.Controls.Add(new LiteralControl(this.dbContentClipID.ToString()));
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Create Date</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            ltCreateDate = new Literal();
            ltCreateDate.EnableViewState = false;
            publishingView.Controls.Add(ltCreateDate);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Modify Date</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            ltModifyDate = new Literal();
            ltModifyDate.EnableViewState = false;
            publishingView.Controls.Add(ltModifyDate);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Parent Catalog</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            msParentCatalog = new MultiSelectBox();
            msParentCatalog.Mode = MultiSelectBoxMode.DropDownList;
            msParentCatalog.EnableViewState = false;
            publishingView.Controls.Add(msParentCatalog);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Publish Date</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            PlaceHolder phPublishDate = new PlaceHolder();
            publishingView.Controls.Add(phPublishDate);
            CalendarHelper.RegisterCalendarPair(phPublishDate, "calPublishDate",
                out calPublishDateP, out calPublishDateB, out calPublishDateC, true);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Expiration Date</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            PlaceHolder phExpirationDate = new PlaceHolder();
            publishingView.Controls.Add(phExpirationDate);
            CalendarHelper.RegisterCalendarPair(phExpirationDate, "calExpirationDate",
                out calExpirationDateP, out calExpirationDateB, out calExpirationDateC, true);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Archive Date</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            PlaceHolder phArchiveDate = new PlaceHolder();
            publishingView.Controls.Add(phArchiveDate);
            CalendarHelper.RegisterCalendarPair(phArchiveDate, "calArchiveDate",
                out calArchiveDateP, out calArchiveDateB, out calArchiveDateC, true);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Comments Enabled</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbCommentsEnabled = new CheckBox();
            cbCommentsEnabled.EnableViewState = false;
            publishingView.Controls.Add(cbCommentsEnabled);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            publishingView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Comments Notification</span>"));
            publishingView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbNotifyOnComments = new CheckBox();
            cbNotifyOnComments.EnableViewState = false;
            publishingView.Controls.Add(cbNotifyOnComments);
            publishingView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Contributors Folder

            contributorView = new ComponentArt.Web.UI.PageView();
            contributorView.CssClass = "PageContent";
            multipage.PageViews.Add(contributorView);

            TabStripTab contributorTab = new TabStripTab();
            contributorTab.Text = "Contributors";
            contributorTab.PageViewId = contributorView.ID;
            tabstrip.Tabs.Add(contributorTab);

            contributorView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            contributorView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Authors</span>"));
            contributorView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbAuthors = new TextBox();
            contributorView.Controls.Add(tbAuthors);
            contributorView.Controls.Add(new LiteralControl("</span></div>"));

            contributorView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            contributorView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Editors</span>"));
            contributorView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbEditors = new TextBox();
            contributorView.Controls.Add(tbEditors);
            contributorView.Controls.Add(new LiteralControl("</span></div>"));

            #endregion

            #region Menu Folder

            menuView = new ComponentArt.Web.UI.PageView();
            menuView.CssClass = "PageContent";
            multipage.PageViews.Add(menuView);

            TabStripTab menuTab = new TabStripTab();
            menuTab.Text = "Menu";
            menuTab.PageViewId = menuView.ID;
            tabstrip.Tabs.Add(menuTab);

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Label</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuLabel = new TextBox();
            tbMenuLabel.EnableViewState = false;
            tbMenuLabel.Width = Unit.Pixel(250);
            tbMenuLabel.ToolTip = "Label displayed on menu.";
            menuView.Controls.Add(tbMenuLabel);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Tooltip</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuTooltip = new TextBox();
            tbMenuTooltip.EnableViewState = false;
            tbMenuTooltip.Width = Unit.Pixel(250);
            tbMenuTooltip.ToolTip = "Tooltip displayed when hovering over item.";
            menuView.Controls.Add(tbMenuTooltip);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Enabled</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbMenuEnabled = new CheckBox();
            cbMenuEnabled.EnableViewState = false;
            menuView.Controls.Add(cbMenuEnabled);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Order</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuOrder = new TextBox();
            tbMenuOrder.EnableViewState = false;
            menuView.Controls.Add(tbMenuOrder);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Left Icon</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuLeftIcon = new TextBox();
            tbMenuLeftIcon.EnableViewState = false;
            menuView.Controls.Add(tbMenuLeftIcon);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Left Icon Over</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuLeftIconOver = new TextBox();
            tbMenuLeftIconOver.EnableViewState = false;
            menuView.Controls.Add(tbMenuLeftIconOver);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Right Icon</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuRightIcon = new TextBox();
            tbMenuRightIcon.EnableViewState = false;
            menuView.Controls.Add(tbMenuRightIcon);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Right Icon Over</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            tbMenuRightIconOver = new TextBox();
            tbMenuRightIconOver.EnableViewState = false;
            menuView.Controls.Add(tbMenuRightIconOver);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

            menuView.Controls.Add(new LiteralControl("<div class=\"inputrow\">"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputlabel\">Menu Break</span>"));
            menuView.Controls.Add(new LiteralControl("<span class=\"inputfield\">"));
            cbMenuBreak = new CheckBox();
            cbMenuBreak.EnableViewState = false;
            menuView.Controls.Add(cbMenuBreak);
            menuView.Controls.Add(new LiteralControl("</span></div>"));

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

		#region ok_Click Save and Update Apply

		protected void ok_Click(object sender, EventArgs e)
		{
			if(dbContentClipID == 0)
				editDbContentClip = new DbContentClip();
			else
				editDbContentClip = new DbContentClip(dbContentClipID);

			editDbContentClip.Title = tbTitle.Text;
			editDbContentClip.Description = tbDescription.Text;
			editDbContentClip.Keywords = tbKeywords.Text;
			editDbContentClip.OverrideUrl = tbOverrideUrl.Text;
			editDbContentClip.Icon = tbIcon.Text;
            editDbContentClip.Body = tbBody.Text;
			editDbContentClip.PublishDate = calPublishDateP.SelectedDate;
			editDbContentClip.ExpirationDate = calExpirationDateP.SelectedDate;
			editDbContentClip.ArchiveDate = calArchiveDateP.SelectedDate;
			editDbContentClip.Priority = int.Parse(tbPriority.Text);
			editDbContentClip.SortOrder = int.Parse(tbSortOrder.Text);
			editDbContentClip.CommentsEnabled = cbCommentsEnabled.Checked;
			editDbContentClip.NotifyOnComments = cbNotifyOnComments.Checked;
			editDbContentClip.MenuLabel = tbMenuLabel.Text;
			editDbContentClip.MenuTooltip = tbMenuTooltip.Text;
			editDbContentClip.MenuEnabled = cbMenuEnabled.Checked;
			editDbContentClip.MenuOrder = int.Parse(tbMenuOrder.Text);
			editDbContentClip.MenuLeftIcon = tbMenuLeftIcon.Text;
			editDbContentClip.MenuLeftIconOver = tbMenuLeftIconOver.Text;
			editDbContentClip.MenuRightIcon = tbMenuRightIcon.Text;
			editDbContentClip.MenuRightIconOver = tbMenuRightIconOver.Text;
			editDbContentClip.MenuBreak = cbMenuBreak.Checked;

            DbContentClipManager clipManager = new DbContentClipManager();
            editDbContentClip.References = clipManager.EncodeClips(tbReferences.Text);

			if(msParentCatalog.SelectedItem != null)
				editDbContentClip.ParentCatalog = DbContentCatalog.NewPlaceHolder( 
					int.Parse(msParentCatalog.SelectedItem.Value));
			else
				editDbContentClip.ParentCatalog = null;

			if(ratingComboBox.SelectedItem != null)
				editDbContentClip.Rating = DbContentRating.NewPlaceHolder(
                    int.Parse(ratingComboBox.SelectedItem.Value));
			else
				editDbContentClip.Rating = null;

			if(msStatus.SelectedItem != null)
				editDbContentClip.Status = DbContentStatus.NewPlaceHolder( 
					int.Parse(msStatus.SelectedItem.Value));
			else
				editDbContentClip.Status = null;


			GreyFoxUserManager userManager = new GreyFoxUserManager();
			editDbContentClip.Authors = userManager.DecodeString(tbAuthors.Text, ",");
            editDbContentClip.Editors = userManager.DecodeString(tbEditors.Text, ",");

			if(editOnAdd)
				dbContentClipID = editDbContentClip.Save();
			else
				editDbContentClip.Save();

			if(resetOnAdd)
			{
				tbTitle.Text = string.Empty;
				tbDescription.Text = string.Empty;
				tbKeywords.Text = string.Empty;
				tbOverrideUrl.Text = string.Empty;
				tbIcon.Text = string.Empty;
				tbBody.Text = string.Empty;
				calPublishDateP.SelectedDate = DateTime.Now;
				calExpirationDateP.SelectedDate = DateTime.Now;
				calArchiveDateP.SelectedDate = DateTime.Now;
				tbPriority.Text = string.Empty;
				tbSortOrder.Text = string.Empty;
				cbCommentsEnabled.Checked = false;
				cbNotifyOnComments.Checked = false;
				tbMenuLabel.Text = string.Empty;
				tbMenuTooltip.Text = string.Empty;
				cbMenuEnabled.Checked = false;
				tbMenuOrder.Text = string.Empty;
				tbMenuLeftIcon.Text = string.Empty;
				tbMenuLeftIconOver.Text = string.Empty;
				tbMenuRightIcon.Text = string.Empty;
				tbMenuRightIconOver.Text = string.Empty;
				cbMenuBreak.Checked = false;
                tbReferences.Text = string.Empty;
				msParentCatalog.SelectedIndex = 0;
				ratingComboBox.SelectedIndex = 0;
				msStatus.SelectedIndex = 0;
			}

			// Clear Caches
			Amns.GreyFox.Content.Caching.SiteMapCacheControl.ClearSiteMaps();
			Amns.GreyFox.Content.Caching.MenuCacheControl.ClearMenus();

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

		protected override void OnPreRender(EventArgs e)
		{
            //ratingComboBox.Enabled = Page.User.IsInRole("Tessen/Rater");

			if(dbContentClipID != 0 & loadFlag)
			{
				editDbContentClip = new DbContentClip(dbContentClipID);

				//
				// Set Field Entries
				//
				ltCreateDate.Text = editDbContentClip.CreateDate.ToString();
				ltModifyDate.Text = editDbContentClip.ModifyDate.ToString();
				tbTitle.Text = editDbContentClip.Title;
				tbDescription.Text = editDbContentClip.Description;
				tbKeywords.Text = editDbContentClip.Keywords;
				tbOverrideUrl.Text = editDbContentClip.OverrideUrl;
				tbIcon.Text = editDbContentClip.Icon;
                tbBody.Text = editDbContentClip.Body;
				calPublishDateP.SelectedDate = editDbContentClip.PublishDate;
				calExpirationDateP.SelectedDate = editDbContentClip.ExpirationDate;
				calArchiveDateP.SelectedDate = editDbContentClip.ArchiveDate;
				tbPriority.Text = editDbContentClip.Priority.ToString();
				tbSortOrder.Text = editDbContentClip.SortOrder.ToString();
				cbCommentsEnabled.Checked = editDbContentClip.CommentsEnabled;
				cbNotifyOnComments.Checked = editDbContentClip.NotifyOnComments;
				tbMenuLabel.Text = editDbContentClip.MenuLabel;
				tbMenuTooltip.Text = editDbContentClip.MenuTooltip;
				cbMenuEnabled.Checked = editDbContentClip.MenuEnabled;
				tbMenuOrder.Text = editDbContentClip.MenuOrder.ToString();
				tbMenuLeftIcon.Text = editDbContentClip.MenuLeftIcon;
				tbMenuLeftIconOver.Text = editDbContentClip.MenuLeftIconOver;
				tbMenuRightIcon.Text = editDbContentClip.MenuRightIcon;
				tbMenuRightIconOver.Text = editDbContentClip.MenuRightIconOver;
				cbMenuBreak.Checked = editDbContentClip.MenuBreak;

				//
				// Set Children Selections
				//
                tbReferences.Text = editDbContentClip.References.ToTitleString();

				if(editDbContentClip.ParentCatalog != null)
					foreach(ListItem item in msParentCatalog.Items)
						item.Selected = editDbContentClip.ParentCatalog.ID.ToString() == item.Value;

				if(editDbContentClip.Rating != null)
					foreach(ComboBoxItem item in ratingComboBox.Items)
						item.Selected = editDbContentClip.Rating.ID.ToString() == item.Value;

				if(editDbContentClip.Status != null)
					foreach(ListItem item in msStatus.Items)
						item.Selected = editDbContentClip.Status.ID.ToString() == item.Value;

				tbAuthors.Text = editDbContentClip.Authors.ToEncodedString(",", ",");
                tbEditors.Text = editDbContentClip.Editors.ToEncodedString(",", ",");

                tabstrip.SelectedTab = tabstrip.Tabs[0];

                headerText.Text = "Edit - " +
                    Regex.Replace(editDbContentClip.Title, "<[^>]*>", "");
			}
			else
			{
				this.initializeDefaults();
                headerText.Text = "Edit - New Clip";
			}
		}

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null) base.LoadViewState(myState[0]);
				if(myState[1] != null) dbContentClipID = (int) myState[1];
				if(myState[2] != null) catalogID = (int) myState[2];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[3];
			myState[0] = baseState;
			myState[1] = dbContentClipID;
			myState[2] = catalogID;
			return myState;
		}

		// --- Begin Custom Code ---

		private void initializeDefaults()
		{
			int newMenuOrder = 1;
			int newSortOrder = 1;

			DbContentCatalog catalog = new DbContentCatalog(catalogID);
			DbContentClipCollection clips = catalog.GetClips();

			// Find Max Orders
			foreach(DbContentClip clip in clips)
			{
				if(clip.MenuOrder >= newMenuOrder)
					newMenuOrder = clip.MenuOrder + 1;
				if(clip.SortOrder >= newSortOrder)
					newSortOrder = clip.SortOrder + 1;
			}
			
			this.tbKeywords.Text = catalog.DefaultKeywords;
			this.calPublishDateP.SelectedDate = 
				DateTime.Now.Add(catalog.DefaultTimeToPublish);
			this.calExpirationDateP.SelectedDate = 
				this.calPublishDateP.SelectedDate.Add(catalog.DefaultTimeToExpire);
			this.calArchiveDateP.SelectedDate = 
				this.calPublishDateP.SelectedDate.Add(catalog.DefaultTimeToArchive);
			this.cbCommentsEnabled.Checked = catalog.CommentsEnabled;
			this.cbMenuEnabled.Checked = catalog.MenuEnabled;
			this.tbAuthors.Text = string.Empty;
			this.tbEditors.Text = string.Empty;
			this.tbMenuOrder.Text = newMenuOrder.ToString();
			this.tbPriority.Text = "-1";
			this.tbSortOrder.Text = newSortOrder.ToString();

			foreach(ListItem item in msParentCatalog.Items)
				item.Selected = catalogID.ToString() == item.Value;

			if(catalog.DefaultStatus != null)
				foreach(ListItem item in msStatus.Items)
					item.Selected = catalog.DefaultStatus.ID.ToString() == item.Value;
			else
				msStatus.SelectedIndex = 0;

			if(catalog.DefaultRating != null)
				foreach(ComboBoxItem item in ratingComboBox.Items)
					item.Selected = catalog.DefaultRating.ID.ToString() == item.Value;
			else
				ratingComboBox.SelectedIndex = 0;
		}

		// --- End Custom Code ---
	}
}

