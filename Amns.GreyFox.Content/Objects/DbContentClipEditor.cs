using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using FreeTextBoxControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentClip.
	/// </summary>
	[ToolboxData("<{0}:DbContentClipEditor runat=server></{0}:DbContentClipEditor>")]
	public class DbContentClipEditor : TableWindow, INamingContainer
	{
		private int dbContentClipID;
		private DbContentClip obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for _system Folder

		private Literal ltCreateDate = new Literal();
		private Literal ltModifyDate = new Literal();

		#endregion

		#region Private Control Fields for General Folder

		private TextBox tbTitle = new TextBox();
		private TextBox tbDescription = new TextBox();
		private TextBox tbKeywords = new TextBox();
		private TextBox tbIcon = new TextBox();
		private MultiSelectBox msStatus = new MultiSelectBox();

		#endregion

		#region Private Control Fields for Body Folder

		private FreeTextBox ftbBody = new FreeTextBox();

		#endregion

		#region Private Control Fields for Publishing Folder

		private DateEditor dePublishDate = new DateEditor();
		private DateEditor deExpirationDate = new DateEditor();
		private DateEditor deArchiveDate = new DateEditor();
		private TextBox tbPriority = new TextBox();
		private RegularExpressionValidator revPriority = new RegularExpressionValidator();
		private TextBox tbSortOrder = new TextBox();
		private RegularExpressionValidator revSortOrder = new RegularExpressionValidator();
		private CheckBox cbCommentsEnabled = new CheckBox();
		private CheckBox cbNotifyOnComments = new CheckBox();
		private TextBox tbOverrideUrl = new TextBox();
		private MultiSelectBox msParentCatalog = new MultiSelectBox();
		private MultiSelectBox msRating = new MultiSelectBox();
		private MultiSelectBox msReferences = new MultiSelectBox();

		#endregion

		#region Private Control Fields for Contributors Folder

		private MultiSelectBox msAuthors = new MultiSelectBox();
		private MultiSelectBox msEditors = new MultiSelectBox();

		#endregion

		#region Private Control Fields for Menu Folder

		private TextBox tbMenuLabel = new TextBox();
		private TextBox tbMenuTooltip = new TextBox();
		private CheckBox cbMenuEnabled = new CheckBox();
		private TextBox tbMenuOrder = new TextBox();
		private RegularExpressionValidator revMenuOrder = new RegularExpressionValidator();
		private TextBox tbMenuLeftIcon = new TextBox();
		private TextBox tbMenuLeftIconOver = new TextBox();
		private TextBox tbMenuRightIcon = new TextBox();
		private TextBox tbMenuRightIconOver = new TextBox();
		private CheckBox cbMenuBreak = new CheckBox();

		#endregion

		#region Private Control Fields for Security Folder


		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

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

			#region Child Controls for _system Folder

			ltCreateDate.EnableViewState = false;
			Controls.Add(ltCreateDate);

			ltModifyDate.EnableViewState = false;
			Controls.Add(ltModifyDate);

			#endregion

			#region Child Controls for General Folder

			tbTitle.EnableViewState = false;
			Controls.Add(tbTitle);

			tbDescription.EnableViewState = false;
			Controls.Add(tbDescription);

			tbKeywords.EnableViewState = false;
			Controls.Add(tbKeywords);

			tbIcon.EnableViewState = false;
			Controls.Add(tbIcon);

			msStatus.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msStatus);

			#endregion

			#region Child Controls for Body Folder

			ftbBody.ID = this.ID + "_Body";
			ftbBody.Width = Unit.Percentage(100);
			ftbBody.EnableViewState = false;
			Controls.Add(ftbBody);

			#endregion

			#region Child Controls for Publishing Folder

			msParentCatalog.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msParentCatalog);

			dePublishDate.ID = this.ID + "_PublishDate";
			dePublishDate.AutoAdjust = true;
			dePublishDate.EnableViewState = false;
			Controls.Add(dePublishDate);

			deExpirationDate.ID = this.ID + "_ExpirationDate";
			deExpirationDate.AutoAdjust = true;
			deExpirationDate.EnableViewState = false;
			Controls.Add(deExpirationDate);

			deArchiveDate.ID = this.ID + "_ArchiveDate";
			deArchiveDate.AutoAdjust = true;
			deArchiveDate.EnableViewState = false;
			Controls.Add(deArchiveDate);

			tbPriority.ID = this.ID + "_Priority";
			tbPriority.EnableViewState = false;
			Controls.Add(tbPriority);
			revPriority.ControlToValidate = tbPriority.ID;
			revPriority.ValidationExpression = "^(\\+|-)?\\d+$";
			revPriority.ErrorMessage = "*";
			revPriority.Display = ValidatorDisplay.Dynamic;
			revPriority.EnableViewState = false;
			Controls.Add(revPriority);

			tbSortOrder.ID = this.ID + "_SortOrder";
			tbSortOrder.EnableViewState = false;
			Controls.Add(tbSortOrder);
			revSortOrder.ControlToValidate = tbSortOrder.ID;
			revSortOrder.ValidationExpression = "^(\\+|-)?\\d+$";
			revSortOrder.ErrorMessage = "*";
			revSortOrder.Display = ValidatorDisplay.Dynamic;
			revSortOrder.EnableViewState = false;
			Controls.Add(revSortOrder);

			msRating.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msRating);

			msReferences.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msReferences);

			cbCommentsEnabled.EnableViewState = false;
			Controls.Add(cbCommentsEnabled);

			cbNotifyOnComments.EnableViewState = false;
			Controls.Add(cbNotifyOnComments);


			tbOverrideUrl.EnableViewState = false;
			Controls.Add(tbOverrideUrl);

			#endregion

			#region Child Controls for Contributors Folder

			msAuthors.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msAuthors);

			msEditors.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msEditors);

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

			tbMenuLeftIconOver.EnableViewState = false;
			Controls.Add(tbMenuLeftIconOver);

			tbMenuRightIcon.EnableViewState = false;
			Controls.Add(tbMenuRightIcon);

			tbMenuRightIconOver.EnableViewState = false;
			Controls.Add(tbMenuRightIconOver);

			cbMenuBreak.EnableViewState = false;
			Controls.Add(cbMenuBreak);

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

			msStatus.Items.Add(new ListItem("Null", "Null"));
			DbContentStatusManager statusManager = new DbContentStatusManager();
			DbContentStatusCollection statusCollection = statusManager.GetCollection(string.Empty, string.Empty);
			foreach(DbContentStatus status in statusCollection)
			{
				ListItem i = new ListItem(status.ToString(), status.ID.ToString());
				msStatus.Items.Add(i);
			}

			#endregion

			#region Bind Publishing Child Data

			msParentCatalog.Items.Add(new ListItem("Null", "Null"));
			DbContentCatalogManager parentCatalogManager = new DbContentCatalogManager();
			DbContentCatalogCollection parentCatalogCollection = parentCatalogManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentCatalog parentCatalog in parentCatalogCollection)
			{
				ListItem i = new ListItem(parentCatalog.ToString(), parentCatalog.ID.ToString());
				msParentCatalog.Items.Add(i);
			}

			msRating.Items.Add(new ListItem("Null", "Null"));
			DbContentRatingManager ratingManager = new DbContentRatingManager();
			DbContentRatingCollection ratingCollection = ratingManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentRating rating in ratingCollection)
			{
				ListItem i = new ListItem(rating.ToString(), rating.ID.ToString());
				msRating.Items.Add(i);
			}

			msReferences.Items.Add(new ListItem("Null", "Null"));
			DbContentClipManager referencesManager = new DbContentClipManager();
			DbContentClipCollection referencesCollection = referencesManager.GetCollection(string.Empty, string.Empty, null);
			foreach(DbContentClip references in referencesCollection)
			{
				ListItem i = new ListItem(references.ToString(), references.ID.ToString());
				msReferences.Items.Add(i);
			}

			#endregion

			#region Bind Contributors Child Data

			msAuthors.Items.Add(new ListItem("Null", "Null"));
			GreyFoxUserManager authorsManager = new GreyFoxUserManager();
			GreyFoxUserCollection authorsCollection = authorsManager.GetCollection(string.Empty, string.Empty, null);
			foreach(GreyFoxUser authors in authorsCollection)
			{
				ListItem i = new ListItem(authors.ToString(), authors.ID.ToString());
				msAuthors.Items.Add(i);
			}

			msEditors.Items.Add(new ListItem("Null", "Null"));
			GreyFoxUserManager editorsManager = new GreyFoxUserManager();
			GreyFoxUserCollection editorsCollection = editorsManager.GetCollection(string.Empty, string.Empty, null);
			foreach(GreyFoxUser editors in editorsCollection)
			{
				ListItem i = new ListItem(editors.ToString(), editors.ID.ToString());
				msEditors.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(dbContentClipID == 0)
				obj = new DbContentClip();
			else
				obj = new DbContentClip(dbContentClipID);

			obj.Title = tbTitle.Text;
			obj.Description = tbDescription.Text;
			obj.Keywords = tbKeywords.Text;
			obj.Icon = tbIcon.Text;
			obj.Body = ftbBody.Text;
			obj.PublishDate = dePublishDate.Date;
			obj.ExpirationDate = deExpirationDate.Date;
			obj.ArchiveDate = deArchiveDate.Date;
			obj.Priority = int.Parse(tbPriority.Text);
			obj.SortOrder = int.Parse(tbSortOrder.Text);
			obj.CommentsEnabled = cbCommentsEnabled.Checked;
			obj.NotifyOnComments = cbNotifyOnComments.Checked;
			obj.OverrideUrl = tbOverrideUrl.Text;
			obj.MenuLabel = tbMenuLabel.Text;
			obj.MenuTooltip = tbMenuTooltip.Text;
			obj.MenuEnabled = cbMenuEnabled.Checked;
			obj.MenuOrder = int.Parse(tbMenuOrder.Text);
			obj.MenuLeftIcon = tbMenuLeftIcon.Text;
			obj.MenuLeftIconOver = tbMenuLeftIconOver.Text;
			obj.MenuRightIcon = tbMenuRightIcon.Text;
			obj.MenuRightIconOver = tbMenuRightIconOver.Text;
			obj.MenuBreak = cbMenuBreak.Checked;

			if(msStatus.SelectedItem != null && msStatus.SelectedItem.Value != "Null")
				obj.Status = DbContentStatus.NewPlaceHolder(
					int.Parse(msStatus.SelectedItem.Value));
			else
				obj.Status = null;

			if(msParentCatalog.SelectedItem != null && msParentCatalog.SelectedItem.Value != "Null")
				obj.ParentCatalog = DbContentCatalog.NewPlaceHolder(
					int.Parse(msParentCatalog.SelectedItem.Value));
			else
				obj.ParentCatalog = null;

			if(msRating.SelectedItem != null && msRating.SelectedItem.Value != "Null")
				obj.Rating = DbContentRating.NewPlaceHolder(
					int.Parse(msRating.SelectedItem.Value));
			else
				obj.Rating = null;

			if(msReferences.IsChanged)
			{
				obj.References = new DbContentClipCollection();
				foreach(ListItem i in msReferences.Items)
					if(i.Selected)
						obj.References.Add(DbContentClip.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(msAuthors.IsChanged)
			{
				obj.Authors = new GreyFoxUserCollection();
				foreach(ListItem i in msAuthors.Items)
					if(i.Selected)
						obj.Authors.Add(GreyFoxUser.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(msEditors.IsChanged)
			{
				obj.Editors = new GreyFoxUserCollection();
				foreach(ListItem i in msEditors.Items)
					if(i.Selected)
						obj.Editors.Add(GreyFoxUser.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(editOnAdd)
				dbContentClipID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbTitle.Text = string.Empty;
				tbDescription.Text = string.Empty;
				tbKeywords.Text = string.Empty;
				tbIcon.Text = string.Empty;
				ftbBody.Text = string.Empty;
				dePublishDate.Date = DateTime.Now;
				deExpirationDate.Date = DateTime.Now;
				deArchiveDate.Date = DateTime.Now;
				tbPriority.Text = string.Empty;
				tbSortOrder.Text = string.Empty;
				cbCommentsEnabled.Checked = false;
				cbNotifyOnComments.Checked = false;
				tbOverrideUrl.Text = string.Empty;
				tbMenuLabel.Text = string.Empty;
				tbMenuTooltip.Text = string.Empty;
				cbMenuEnabled.Checked = false;
				tbMenuOrder.Text = string.Empty;
				tbMenuLeftIcon.Text = string.Empty;
				tbMenuLeftIconOver.Text = string.Empty;
				tbMenuRightIcon.Text = string.Empty;
				tbMenuRightIconOver.Text = string.Empty;
				cbMenuBreak.Checked = false;
				msStatus.SelectedIndex = 0;
				msParentCatalog.SelectedIndex = 0;
				msRating.SelectedIndex = 0;
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

			Tab BodyTab = new Tab("Body");
			BodyTab.RenderDiv += new TabRenderHandler(renderBodyFolder);
			tabStrip.Tabs.Add(BodyTab);

			Tab PublishingTab = new Tab("Publishing");
			PublishingTab.RenderDiv += new TabRenderHandler(renderPublishingFolder);
			tabStrip.Tabs.Add(PublishingTab);

			Tab ContributorsTab = new Tab("Contributors");
			ContributorsTab.RenderDiv += new TabRenderHandler(renderContributorsFolder);
			tabStrip.Tabs.Add(ContributorsTab);

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
				if(dbContentClipID > 0)
				{
					obj = new DbContentClip(dbContentClipID);
					text = "Edit  - " + obj.ToString();
				}
				else if(dbContentClipID <= 0)
				{
					obj = new DbContentClip();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				ltCreateDate.Text = obj.CreateDate.ToString();
				ltModifyDate.Text = obj.ModifyDate.ToString();
				tbTitle.Text = obj.Title;
				tbDescription.Text = obj.Description;
				tbKeywords.Text = obj.Keywords;
				tbIcon.Text = obj.Icon;
				ftbBody.Text = obj.Body;
				dePublishDate.Date = obj.PublishDate;
				deExpirationDate.Date = obj.ExpirationDate;
				deArchiveDate.Date = obj.ArchiveDate;
				tbPriority.Text = obj.Priority.ToString();
				tbSortOrder.Text = obj.SortOrder.ToString();
				cbCommentsEnabled.Checked = obj.CommentsEnabled;
				cbNotifyOnComments.Checked = obj.NotifyOnComments;
				tbOverrideUrl.Text = obj.OverrideUrl;
				tbMenuLabel.Text = obj.MenuLabel;
				tbMenuTooltip.Text = obj.MenuTooltip;
				cbMenuEnabled.Checked = obj.MenuEnabled;
				tbMenuOrder.Text = obj.MenuOrder.ToString();
				tbMenuLeftIcon.Text = obj.MenuLeftIcon;
				tbMenuLeftIconOver.Text = obj.MenuLeftIconOver;
				tbMenuRightIcon.Text = obj.MenuRightIcon;
				tbMenuRightIconOver.Text = obj.MenuRightIconOver;
				cbMenuBreak.Checked = obj.MenuBreak;

				//
				// Set Children Selections
				//
				if(obj.Status != null)
					foreach(ListItem item in msStatus.Items)
						item.Selected = obj.Status.ID.ToString() == item.Value;
					else
						msStatus.SelectedIndex = 0;

				if(obj.ParentCatalog != null)
					foreach(ListItem item in msParentCatalog.Items)
						item.Selected = obj.ParentCatalog.ID.ToString() == item.Value;
					else
						msParentCatalog.SelectedIndex = 0;

				if(obj.Rating != null)
					foreach(ListItem item in msRating.Items)
						item.Selected = obj.Rating.ID.ToString() == item.Value;
					else
						msRating.SelectedIndex = 0;

				foreach(ListItem i in msReferences.Items)
					foreach(DbContentClip dbContentClip in obj.References)
						if(i.Value == dbContentClip.ID.ToString())
						{
							i.Selected = true;
							break;
						}
				foreach(ListItem i in msAuthors.Items)
					foreach(GreyFoxUser greyFoxUser in obj.Authors)
						if(i.Value == greyFoxUser.ID.ToString())
						{
							i.Selected = true;
							break;
						}
				foreach(ListItem i in msEditors.Items)
					foreach(GreyFoxUser greyFoxUser in obj.Editors)
						if(i.Value == greyFoxUser.ID.ToString())
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
			msStatus.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderBodyFolder(HtmlTextWriter output)
		{
			//
			// Render Body
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Content");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			ftbBody.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderPublishingFolder(HtmlTextWriter output)
		{
			//
			// Render ParentCatalog
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Parent Catalog");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msParentCatalog.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PublishDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Publish Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			dePublishDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ExpirationDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Expiration Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			deExpirationDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ArchiveDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Archive Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			deArchiveDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Priority
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Priority");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPriority.RenderControl(output);
			revPriority.RenderControl(output);
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
			// Render Rating
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Rating");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msRating.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render References
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Cross References");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msReferences.RenderControl(output);
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
			// Render OverrideUrl
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("OverrideUrl");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbOverrideUrl.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderContributorsFolder(HtmlTextWriter output)
		{
			//
			// Render Authors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Authors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Editors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Editors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msEditors.RenderControl(output);
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
			output.Write("MenuLabel");
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
			output.Write("MenuTooltip");
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
			output.Write("MenuEnabled");
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
			output.Write("MenuOrder");
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
			output.Write("MenuLeftIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuLeftIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuLeftIconOver
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuLeftIconOver");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuLeftIconOver.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuRightIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuRightIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuRightIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuRightIconOver
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuRightIconOver");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMenuRightIconOver.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuBreak
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuBreak");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbMenuBreak.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderSecurityFolder(HtmlTextWriter output)
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
					dbContentClipID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = dbContentClipID;
			return myState;
		}
	}
}

