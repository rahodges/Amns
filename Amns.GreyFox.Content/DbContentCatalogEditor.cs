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
	/// Default web editor for DbContentCatalog.
	/// </summary>
	[ToolboxData("<{0}:DbContentCatalogEditor runat=server></{0}:DbContentCatalogEditor>")]
	public class DbContentCatalogEditor : TableWindow, INamingContainer
	{
		private int dbContentCatalogID;
		private DbContentCatalog obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for General Folder

		private TextBox tbTitle = new TextBox();
		private TextBox tbDescription = new TextBox();
		private TextBox tbKeywords = new TextBox();
		private TextBox tbStatus = new TextBox();
		private TextBox tbWorkflowMode = new TextBox();
		private CheckBox cbCommentsEnabled = new CheckBox();
		private CheckBox cbNotifyOnComments = new CheckBox();
		private CheckBox cbEnabled = new CheckBox();
		private TextBox tbSortOrder = new TextBox();
		private RegularExpressionValidator revSortOrder = new RegularExpressionValidator();
		private TextBox tbIcon = new TextBox();
		private MultiSelectBox msParentCatalog = new MultiSelectBox();
		private MultiSelectBox msChildCatalogs = new MultiSelectBox();
		private MultiSelectBox msDefaultClip = new MultiSelectBox();

		#endregion

		#region Private Control Fields for _system Folder

		private Literal ltCreateDate = new Literal();
		private Literal ltModifyDate = new Literal();

		#endregion

		#region Private Control Fields for Defaults Folder

		private TextBox tbDefaultTimeToPublish = new TextBox();
		private TextBox tbDefaultTimeToExpire = new TextBox();
		private TextBox tbDefaultTimeToArchive = new TextBox();
		private TextBox tbDefaultKeywords = new TextBox();
		private TextBox tbDefaultMenuLeftIcon = new TextBox();
		private TextBox tbDefaultMenuRightIcon = new TextBox();
		private MultiSelectBox msDefaultStatus = new MultiSelectBox();
		private MultiSelectBox msDefaultRating = new MultiSelectBox();
		private MultiSelectBox msDefaultArchive = new MultiSelectBox();
		private MultiSelectBox msTemplates = new MultiSelectBox();

		#endregion

		#region Private Control Fields for Menu Folder

		private TextBox tbMenuLabel = new TextBox();
		private TextBox tbMenuTooltip = new TextBox();
		private CheckBox cbMenuEnabled = new CheckBox();
		private TextBox tbMenuOrder = new TextBox();
		private RegularExpressionValidator revMenuOrder = new RegularExpressionValidator();
		private TextBox tbMenuLeftIcon = new TextBox();
		private TextBox tbMenuRightIcon = new TextBox();
		private TextBox tbMenuBreakImage = new TextBox();
		private TextBox tbMenuBreakCssClass = new TextBox();
		private TextBox tbMenuCssClass = new TextBox();
		private TextBox tbMenuCatalogCssClass = new TextBox();
		private TextBox tbMenuCatalogSelectedCssClass = new TextBox();
		private TextBox tbMenuCatalogChildSelectedCssClass = new TextBox();
		private TextBox tbMenuClipCssClass = new TextBox();
		private TextBox tbMenuClipSelectedCssClass = new TextBox();
		private TextBox tbMenuClipChildSelectedCssClass = new TextBox();
		private TextBox tbMenuClipChildExpandedCssClass = new TextBox();
		private TextBox tbMenuOverrideFlags = new TextBox();
		private TextBox tbMenuIconFlags = new TextBox();

		#endregion

		#region Private Control Fields for Security Folder

		private MultiSelectBox msAuthorRole = new MultiSelectBox();
		private MultiSelectBox msEditorRole = new MultiSelectBox();
		private MultiSelectBox msReviewerRole = new MultiSelectBox();

		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

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
			Controls.Clear();
			bindDropDownLists();

			#region Child Controls for General Folder

			tbTitle.EnableViewState = false;
			Controls.Add(tbTitle);

			tbDescription.EnableViewState = false;
			Controls.Add(tbDescription);

			tbKeywords.EnableViewState = false;
			Controls.Add(tbKeywords);

			tbStatus.EnableViewState = false;
			Controls.Add(tbStatus);

			tbWorkflowMode.EnableViewState = false;
			Controls.Add(tbWorkflowMode);

			cbCommentsEnabled.EnableViewState = false;
			Controls.Add(cbCommentsEnabled);

			cbNotifyOnComments.EnableViewState = false;
			Controls.Add(cbNotifyOnComments);

			cbEnabled.EnableViewState = false;
			Controls.Add(cbEnabled);

			tbSortOrder.ID = this.ID + "_SortOrder";
			tbSortOrder.EnableViewState = false;
			Controls.Add(tbSortOrder);
			revSortOrder.ControlToValidate = tbSortOrder.ID;
			revSortOrder.ValidationExpression = "^(\\+|-)?\\d+$";
			revSortOrder.ErrorMessage = "*";
			revSortOrder.Display = ValidatorDisplay.Dynamic;
			revSortOrder.EnableViewState = false;
			Controls.Add(revSortOrder);

			msParentCatalog.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msParentCatalog);

			msChildCatalogs.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msChildCatalogs);

			msDefaultClip.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msDefaultClip);

			tbIcon.EnableViewState = false;
			Controls.Add(tbIcon);

			#endregion

			#region Child Controls for _system Folder

			ltCreateDate.EnableViewState = false;
			Controls.Add(ltCreateDate);

			ltModifyDate.EnableViewState = false;
			Controls.Add(ltModifyDate);

			#endregion

			#region Child Controls for Defaults Folder

			tbDefaultTimeToPublish.EnableViewState = false;
			Controls.Add(tbDefaultTimeToPublish);

			tbDefaultTimeToExpire.EnableViewState = false;
			Controls.Add(tbDefaultTimeToExpire);

			tbDefaultTimeToArchive.EnableViewState = false;
			Controls.Add(tbDefaultTimeToArchive);

			tbDefaultKeywords.EnableViewState = false;
			Controls.Add(tbDefaultKeywords);

			msDefaultStatus.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msDefaultStatus);

			msDefaultRating.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msDefaultRating);

			msDefaultArchive.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msDefaultArchive);

			msTemplates.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msTemplates);

			tbDefaultMenuLeftIcon.EnableViewState = false;
			Controls.Add(tbDefaultMenuLeftIcon);

			tbDefaultMenuRightIcon.EnableViewState = false;
			Controls.Add(tbDefaultMenuRightIcon);

			#endregion

			#region Child Controls for Menu Folder

			tbMenuLabel.EnableViewState = false;
			Controls.Add(tbMenuLabel);

			tbMenuTooltip.EnableViewState = false;
			Controls.Add(tbMenuTooltip);

			cbMenuEnabled.EnableViewState = false;
			Controls.Add(cbMenuEnabled);

			tbMenuOrder.ID = this.ID + "_MenuOrder";
			tbMenuOrder.EnableViewState = false;
			Controls.Add(tbMenuOrder);
			revMenuOrder.ControlToValidate = tbMenuOrder.ID;
			revMenuOrder.ValidationExpression = "^(\\+|-)?\\d+$";
			revMenuOrder.ErrorMessage = "*";
			revMenuOrder.Display = ValidatorDisplay.Dynamic;
			revMenuOrder.EnableViewState = false;
			Controls.Add(revMenuOrder);

			tbMenuLeftIcon.EnableViewState = false;
			Controls.Add(tbMenuLeftIcon);

			tbMenuRightIcon.EnableViewState = false;
			Controls.Add(tbMenuRightIcon);

			tbMenuBreakImage.EnableViewState = false;
			Controls.Add(tbMenuBreakImage);

			tbMenuBreakCssClass.EnableViewState = false;
			Controls.Add(tbMenuBreakCssClass);

			tbMenuCssClass.EnableViewState = false;
			Controls.Add(tbMenuCssClass);

			tbMenuCatalogCssClass.EnableViewState = false;
			Controls.Add(tbMenuCatalogCssClass);

			tbMenuCatalogSelectedCssClass.EnableViewState = false;
			Controls.Add(tbMenuCatalogSelectedCssClass);

			tbMenuCatalogChildSelectedCssClass.EnableViewState = false;
			Controls.Add(tbMenuCatalogChildSelectedCssClass);

			tbMenuClipCssClass.EnableViewState = false;
			Controls.Add(tbMenuClipCssClass);

			tbMenuClipSelectedCssClass.EnableViewState = false;
			Controls.Add(tbMenuClipSelectedCssClass);

			tbMenuClipChildSelectedCssClass.EnableViewState = false;
			Controls.Add(tbMenuClipChildSelectedCssClass);

			tbMenuClipChildExpandedCssClass.EnableViewState = false;
			Controls.Add(tbMenuClipChildExpandedCssClass);

			tbMenuOverrideFlags.EnableViewState = false;
			Controls.Add(tbMenuOverrideFlags);

			tbMenuIconFlags.EnableViewState = false;
			Controls.Add(tbMenuIconFlags);

			#endregion

			#region Child Controls for Security Folder

			msAuthorRole.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msAuthorRole);

			msEditorRole.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msEditorRole);

			msReviewerRole.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msReviewerRole);

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

			msParentCatalog.Items.Add(new ListItem("Null", "Null"));
			DbContentCatalogManager parentCatalogManager = new DbContentCatalogManager();
			DbContentCatalogCollection parentCatalogCollection = parentCatalogManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentCatalog parentCatalog in parentCatalogCollection)
			{
				ListItem i = new ListItem(parentCatalog.ToString(), parentCatalog.ID.ToString());
				msParentCatalog.Items.Add(i);
			}

			msChildCatalogs.Items.Add(new ListItem("Null", "Null"));
			DbContentCatalogManager childCatalogsManager = new DbContentCatalogManager();
			DbContentCatalogCollection childCatalogsCollection = childCatalogsManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentCatalog childCatalogs in childCatalogsCollection)
			{
				ListItem i = new ListItem(childCatalogs.ToString(), childCatalogs.ID.ToString());
				msChildCatalogs.Items.Add(i);
			}

			msDefaultClip.Items.Add(new ListItem("Null", "Null"));
			DbContentClipManager defaultClipManager = new DbContentClipManager();
			DbContentClipCollection defaultClipCollection = defaultClipManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentClip defaultClip in defaultClipCollection)
			{
				ListItem i = new ListItem(defaultClip.ToString(), defaultClip.ID.ToString());
				msDefaultClip.Items.Add(i);
			}

			#endregion

			#region Bind Defaults Child Data

			msDefaultStatus.Items.Add(new ListItem("Null", "Null"));
			DbContentStatusManager defaultStatusManager = new DbContentStatusManager();
			DbContentStatusCollection defaultStatusCollection = defaultStatusManager.GetCollection(string.Empty, string.Empty);
			foreach(DbContentStatus defaultStatus in defaultStatusCollection)
			{
				ListItem i = new ListItem(defaultStatus.ToString(), defaultStatus.ID.ToString());
				msDefaultStatus.Items.Add(i);
			}

			msDefaultRating.Items.Add(new ListItem("Null", "Null"));
			DbContentRatingManager defaultRatingManager = new DbContentRatingManager();
			DbContentRatingCollection defaultRatingCollection = defaultRatingManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentRating defaultRating in defaultRatingCollection)
			{
				ListItem i = new ListItem(defaultRating.ToString(), defaultRating.ID.ToString());
				msDefaultRating.Items.Add(i);
			}

			msDefaultArchive.Items.Add(new ListItem("Null", "Null"));
			DbContentCatalogManager defaultArchiveManager = new DbContentCatalogManager();
			DbContentCatalogCollection defaultArchiveCollection = defaultArchiveManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentCatalog defaultArchive in defaultArchiveCollection)
			{
				ListItem i = new ListItem(defaultArchive.ToString(), defaultArchive.ID.ToString());
				msDefaultArchive.Items.Add(i);
			}

			msTemplates.Items.Add(new ListItem("Null", "Null"));
			DbContentClipManager templatesManager = new DbContentClipManager();
			DbContentClipCollection templatesCollection = templatesManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentClip templates in templatesCollection)
			{
				ListItem i = new ListItem(templates.ToString(), templates.ID.ToString());
				msTemplates.Items.Add(i);
			}

			#endregion

			#region Bind Security Child Data

			msAuthorRole.Items.Add(new ListItem("Null", "Null"));
			GreyFoxRoleManager authorRoleManager = new GreyFoxRoleManager();
			GreyFoxRoleCollection authorRoleCollection = authorRoleManager.GetCollection(string.Empty, string.Empty);
			foreach(GreyFoxRole authorRole in authorRoleCollection)
			{
				ListItem i = new ListItem(authorRole.ToString(), authorRole.ID.ToString());
				msAuthorRole.Items.Add(i);
			}

			msEditorRole.Items.Add(new ListItem("Null", "Null"));
			GreyFoxRoleManager editorRoleManager = new GreyFoxRoleManager();
			GreyFoxRoleCollection editorRoleCollection = editorRoleManager.GetCollection(string.Empty, string.Empty);
			foreach(GreyFoxRole editorRole in editorRoleCollection)
			{
				ListItem i = new ListItem(editorRole.ToString(), editorRole.ID.ToString());
				msEditorRole.Items.Add(i);
			}

			msReviewerRole.Items.Add(new ListItem("Null", "Null"));
			GreyFoxRoleManager reviewerRoleManager = new GreyFoxRoleManager();
			GreyFoxRoleCollection reviewerRoleCollection = reviewerRoleManager.GetCollection(string.Empty, string.Empty);
			foreach(GreyFoxRole reviewerRole in reviewerRoleCollection)
			{
				ListItem i = new ListItem(reviewerRole.ToString(), reviewerRole.ID.ToString());
				msReviewerRole.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(dbContentCatalogID == 0)
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

			if(msParentCatalog.SelectedItem != null && msParentCatalog.SelectedItem.Value != "Null")
				obj.ParentCatalog = DbContentCatalog.NewPlaceHolder(
					int.Parse(msParentCatalog.SelectedItem.Value));
			else
				obj.ParentCatalog = null;

			if(msChildCatalogs.IsChanged)
			{
				obj.ChildCatalogs = new DbContentCatalogCollection();
				foreach(ListItem i in msChildCatalogs.Items)
					if(i.Selected)
						obj.ChildCatalogs.Add(DbContentCatalog.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(msDefaultClip.SelectedItem != null && msDefaultClip.SelectedItem.Value != "Null")
				obj.DefaultClip = DbContentClip.NewPlaceHolder(
					int.Parse(msDefaultClip.SelectedItem.Value));
			else
				obj.DefaultClip = null;

			if(msDefaultStatus.SelectedItem != null && msDefaultStatus.SelectedItem.Value != "Null")
				obj.DefaultStatus = DbContentStatus.NewPlaceHolder(
					int.Parse(msDefaultStatus.SelectedItem.Value));
			else
				obj.DefaultStatus = null;

			if(msDefaultRating.SelectedItem != null && msDefaultRating.SelectedItem.Value != "Null")
				obj.DefaultRating = DbContentRating.NewPlaceHolder(
					int.Parse(msDefaultRating.SelectedItem.Value));
			else
				obj.DefaultRating = null;

			if(msDefaultArchive.SelectedItem != null && msDefaultArchive.SelectedItem.Value != "Null")
				obj.DefaultArchive = DbContentCatalog.NewPlaceHolder(
					int.Parse(msDefaultArchive.SelectedItem.Value));
			else
				obj.DefaultArchive = null;

			if(msTemplates.IsChanged)
			{
				obj.Templates = new DbContentClipCollection();
				foreach(ListItem i in msTemplates.Items)
					if(i.Selected)
						obj.Templates.Add(DbContentClip.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(msAuthorRole.SelectedItem != null && msAuthorRole.SelectedItem.Value != "Null")
				obj.AuthorRole = GreyFoxRole.NewPlaceHolder(
					int.Parse(msAuthorRole.SelectedItem.Value));
			else
				obj.AuthorRole = null;

			if(msEditorRole.SelectedItem != null && msEditorRole.SelectedItem.Value != "Null")
				obj.EditorRole = GreyFoxRole.NewPlaceHolder(
					int.Parse(msEditorRole.SelectedItem.Value));
			else
				obj.EditorRole = null;

			if(msReviewerRole.SelectedItem != null && msReviewerRole.SelectedItem.Value != "Null")
				obj.ReviewerRole = GreyFoxRole.NewPlaceHolder(
					int.Parse(msReviewerRole.SelectedItem.Value));
			else
				obj.ReviewerRole = null;

			if(editOnAdd)
				dbContentCatalogID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
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
			GeneralTab.Visible = true;
			tabStrip.Tabs.Add(GeneralTab);

			Tab DefaultsTab = new Tab("Defaults");
			DefaultsTab.RenderDiv += new TabRenderHandler(renderDefaultsFolder);
			tabStrip.Tabs.Add(DefaultsTab);

			Tab MenuTab = new Tab("Menu");
			MenuTab.RenderDiv += new TabRenderHandler(renderMenuFolder);
			tabStrip.Tabs.Add(MenuTab);

			Tab SecurityTab = new Tab("Security");
			SecurityTab.RenderDiv += new TabRenderHandler(renderSecurityFolder);
			tabStrip.Tabs.Add(SecurityTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(dbContentCatalogID > 0)
				{
					obj = new DbContentCatalog(dbContentCatalogID);
					text = "Edit  - " + obj.ToString();
				}
				else if(dbContentCatalogID <= 0)
				{
					obj = new DbContentCatalog();
					text = "Add ";
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
				if(obj.ParentCatalog != null)
					foreach(ListItem item in msParentCatalog.Items)
						item.Selected = obj.ParentCatalog.ID.ToString() == item.Value;
					else
						msParentCatalog.SelectedIndex = 0;

				foreach(ListItem i in msChildCatalogs.Items)
					foreach(DbContentCatalog dbContentCatalog in obj.ChildCatalogs)
						if(i.Value == dbContentCatalog.ID.ToString())
						{
							i.Selected = true;
							break;
						}
				if(obj.DefaultClip != null)
					foreach(ListItem item in msDefaultClip.Items)
						item.Selected = obj.DefaultClip.ID.ToString() == item.Value;
					else
						msDefaultClip.SelectedIndex = 0;

				if(obj.DefaultStatus != null)
					foreach(ListItem item in msDefaultStatus.Items)
						item.Selected = obj.DefaultStatus.ID.ToString() == item.Value;
					else
						msDefaultStatus.SelectedIndex = 0;

				if(obj.DefaultRating != null)
					foreach(ListItem item in msDefaultRating.Items)
						item.Selected = obj.DefaultRating.ID.ToString() == item.Value;
					else
						msDefaultRating.SelectedIndex = 0;

				if(obj.DefaultArchive != null)
					foreach(ListItem item in msDefaultArchive.Items)
						item.Selected = obj.DefaultArchive.ID.ToString() == item.Value;
					else
						msDefaultArchive.SelectedIndex = 0;

				foreach(ListItem i in msTemplates.Items)
					foreach(DbContentClip dbContentClip in obj.Templates)
						if(i.Value == dbContentClip.ID.ToString())
						{
							i.Selected = true;
							break;
						}
				if(obj.AuthorRole != null)
					foreach(ListItem item in msAuthorRole.Items)
						item.Selected = obj.AuthorRole.ID.ToString() == item.Value;
					else
						msAuthorRole.SelectedIndex = 0;

				if(obj.EditorRole != null)
					foreach(ListItem item in msEditorRole.Items)
						item.Selected = obj.EditorRole.ID.ToString() == item.Value;
					else
						msEditorRole.SelectedIndex = 0;

				if(obj.ReviewerRole != null)
					foreach(ListItem item in msReviewerRole.Items)
						item.Selected = obj.ReviewerRole.ID.ToString() == item.Value;
					else
						msReviewerRole.SelectedIndex = 0;

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

		private void renderGeneralFolder(HtmlTextWriter output)
		{
			//
			// Render Title
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Title");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbTitle.RenderControl(output);
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
			// Render Keywords
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Keywords");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbKeywords.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Status
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Status");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbStatus.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render WorkflowMode
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Workflow Mode");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbWorkflowMode.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render CommentsEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("CommentsEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbCommentsEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotifyOnComments
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("NotifyOnComments");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbNotifyOnComments.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Enabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SortOrder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("SortOrder");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSortOrder.RenderControl(output);
			revSortOrder.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ParentCatalog
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ParentCatalog");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msParentCatalog.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ChildCatalogs
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ChildCatalogs");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msChildCatalogs.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultClip
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultClip");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msDefaultClip.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Icon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Icon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbIcon.RenderControl(output);
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

		private void renderDefaultsFolder(HtmlTextWriter output)
		{
			//
			// Render DefaultTimeToPublish
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Time To Publish");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefaultTimeToPublish.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultTimeToExpire
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Time To Expiration");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefaultTimeToExpire.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultTimeToArchive
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Time To Archive");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefaultTimeToArchive.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultKeywords
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultKeywords");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefaultKeywords.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultStatus
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultStatus");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msDefaultStatus.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultRating
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultRating");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msDefaultRating.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultArchive
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Default Archive");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msDefaultArchive.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Templates
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Templates");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msTemplates.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultMenuLeftIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultMenuLeftIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefaultMenuLeftIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultMenuRightIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultMenuRightIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefaultMenuRightIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderMenuFolder(HtmlTextWriter output)
		{
			//
			// Render MenuLabel
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Label");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuLabel.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuTooltip
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Tooltip");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuTooltip.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbMenuEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuOrder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Menu Order");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuOrder.RenderControl(output);
			revMenuOrder.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuLeftIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Left Icon URL");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuLeftIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuRightIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Right Icon URL");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuRightIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuBreakImage
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Break Image URL");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuBreakImage.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuBreakCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Break CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuBreakCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCatalogCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuCatalogCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCatalogSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuCatalogSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCatalogChildSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog Child Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuCatalogChildSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuClipCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuClipSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipChildSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip Child Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuClipChildSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipChildExpandedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip Child Expanded CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuClipChildExpandedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuOverrideFlags
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Style Overrides");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuOverrideFlags.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuIconFlags
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuIconFlags");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuIconFlags.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderSecurityFolder(HtmlTextWriter output)
		{
			//
			// Render AuthorRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Author Roles");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msAuthorRole.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EditorRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Editor Roles");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msEditorRole.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ReviewerRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Reviewer Role");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msReviewerRole.RenderControl(output);
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
					dbContentCatalogID = (int) myState[1];
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

