using System;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentCatalog.
	/// </summary>
	[DefaultProperty("ConnectionString"), ToolboxData("<{0}:DbContentCatalogView runat=server></{0}:DbContentCatalogView>")]
	public class DbContentCatalogView : TableWindow, INamingContainer
	{
		private int dbContentCatalogID;
		private DbContentCatalog dbContentCatalog;
		private string connectionString;

		#region Private Control Fields for General Folder

		private Literal ltTitle = new Literal();
		private Literal ltDescription = new Literal();
		private Literal ltKeywords = new Literal();
		private Literal ltStatus = new Literal();
		private Literal ltWorkflowMode = new Literal();
		private Literal ltCommentsEnabled = new Literal();
		private Literal ltNotifyOnComments = new Literal();
		private Literal ltEnabled = new Literal();
		private Literal ltSortOrder = new Literal();
		private Literal ltIcon = new Literal();
		private Literal ltParentCatalog = new Literal();
		private Literal ltChildCatalogs = new Literal();
		private Literal ltDefaultClip = new Literal();

		#endregion

		#region Private Control Fields for _system Folder

		private Literal ltCreateDate = new Literal();
		private Literal ltModifyDate = new Literal();

		#endregion

		#region Private Control Fields for Defaults Folder

		private Literal ltDefaultTimeToPublish = new Literal();
		private Literal ltDefaultTimeToExpire = new Literal();
		private Literal ltDefaultTimeToArchive = new Literal();
		private Literal ltDefaultKeywords = new Literal();
		private Literal ltDefaultMenuLeftIcon = new Literal();
		private Literal ltDefaultMenuRightIcon = new Literal();
		private Literal ltDefaultStatus = new Literal();
		private Literal ltDefaultRating = new Literal();
		private Literal ltDefaultArchive = new Literal();
		private Literal ltTemplates = new Literal();

		#endregion

		#region Private Control Fields for Roles Folder

		private Literal ltAuthorRole = new Literal();
		private Literal ltEditorRole = new Literal();
		private Literal ltReviewerRole = new Literal();

		#endregion

		#region Private Control Fields for Menu Folder

		private Literal ltMenuLabel = new Literal();
		private Literal ltMenuTooltip = new Literal();
		private Literal ltMenuEnabled = new Literal();
		private Literal ltMenuOrder = new Literal();
		private Literal ltMenuLeftIcon = new Literal();
		private Literal ltMenuRightIcon = new Literal();
		private Literal ltMenuBreakImage = new Literal();
		private Literal ltMenuBreakCssClass = new Literal();
		private Literal ltMenuCssClass = new Literal();
		private Literal ltMenuCatalogCssClass = new Literal();
		private Literal ltMenuCatalogSelectedCssClass = new Literal();
		private Literal ltMenuCatalogChildSelectedCssClass = new Literal();
		private Literal ltMenuClipCssClass = new Literal();
		private Literal ltMenuClipSelectedCssClass = new Literal();
		private Literal ltMenuClipChildSelectedCssClass = new Literal();
		private Literal ltMenuClipChildExpandedCssClass = new Literal();
		private Literal ltMenuOverrideFlags = new Literal();
		private Literal ltMenuIconFlags = new Literal();

		#endregion

		private Button btOk = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int DbContentCatalogID
		{
			get { return dbContentCatalogID; }
			set { dbContentCatalogID = value; }
		}

		[Bindable(false), Category("Data"), DefaultValue("")]
		public string ConnectionString
		{
			get { return connectionString; }
			set
			{
				// Parse Connection String
				if (value.StartsWith("<jet40virtual>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(value.Substring(14, value.Length - 14));
				else if (value.StartsWith("<jet40config>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get(value.Substring(13, value.Length - 13)));
				else
					connectionString = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for General Folder

			ltTitle.EnableViewState = false;
			Controls.Add(ltTitle);

			ltDescription.EnableViewState = false;
			Controls.Add(ltDescription);

			ltKeywords.EnableViewState = false;
			Controls.Add(ltKeywords);

			ltStatus.EnableViewState = false;
			Controls.Add(ltStatus);

			ltWorkflowMode.EnableViewState = false;
			Controls.Add(ltWorkflowMode);

			ltCommentsEnabled.EnableViewState = false;
			Controls.Add(ltCommentsEnabled);

			ltNotifyOnComments.EnableViewState = false;
			Controls.Add(ltNotifyOnComments);

			ltEnabled.EnableViewState = false;
			Controls.Add(ltEnabled);

			ltSortOrder.EnableViewState = false;
			Controls.Add(ltSortOrder);

			ltParentCatalog.EnableViewState = false;
			Controls.Add(ltParentCatalog);

			ltChildCatalogs.EnableViewState = false;
			Controls.Add(ltChildCatalogs);

			ltDefaultClip.EnableViewState = false;
			Controls.Add(ltDefaultClip);

			ltIcon.EnableViewState = false;
			Controls.Add(ltIcon);

			#endregion

			#region Child Controls for _system Folder

			ltCreateDate.EnableViewState = false;
			Controls.Add(ltCreateDate);

			ltModifyDate.EnableViewState = false;
			Controls.Add(ltModifyDate);

			#endregion

			#region Child Controls for Defaults Folder

			ltDefaultTimeToPublish.EnableViewState = false;
			Controls.Add(ltDefaultTimeToPublish);

			ltDefaultTimeToExpire.EnableViewState = false;
			Controls.Add(ltDefaultTimeToExpire);

			ltDefaultTimeToArchive.EnableViewState = false;
			Controls.Add(ltDefaultTimeToArchive);

			ltDefaultKeywords.EnableViewState = false;
			Controls.Add(ltDefaultKeywords);

			ltDefaultStatus.EnableViewState = false;
			Controls.Add(ltDefaultStatus);

			ltDefaultRating.EnableViewState = false;
			Controls.Add(ltDefaultRating);

			ltDefaultArchive.EnableViewState = false;
			Controls.Add(ltDefaultArchive);

			ltTemplates.EnableViewState = false;
			Controls.Add(ltTemplates);

			ltDefaultMenuLeftIcon.EnableViewState = false;
			Controls.Add(ltDefaultMenuLeftIcon);

			ltDefaultMenuRightIcon.EnableViewState = false;
			Controls.Add(ltDefaultMenuRightIcon);

			#endregion

			#region Child Controls for Roles Folder

			ltAuthorRole.EnableViewState = false;
			Controls.Add(ltAuthorRole);

			ltEditorRole.EnableViewState = false;
			Controls.Add(ltEditorRole);

			ltReviewerRole.EnableViewState = false;
			Controls.Add(ltReviewerRole);

			#endregion

			#region Child Controls for Menu Folder

			ltMenuLabel.EnableViewState = false;
			Controls.Add(ltMenuLabel);

			ltMenuTooltip.EnableViewState = false;
			Controls.Add(ltMenuTooltip);

			ltMenuEnabled.EnableViewState = false;
			Controls.Add(ltMenuEnabled);

			ltMenuOrder.EnableViewState = false;
			Controls.Add(ltMenuOrder);

			ltMenuLeftIcon.EnableViewState = false;
			Controls.Add(ltMenuLeftIcon);

			ltMenuRightIcon.EnableViewState = false;
			Controls.Add(ltMenuRightIcon);

			ltMenuBreakImage.EnableViewState = false;
			Controls.Add(ltMenuBreakImage);

			ltMenuBreakCssClass.EnableViewState = false;
			Controls.Add(ltMenuBreakCssClass);

			ltMenuCssClass.EnableViewState = false;
			Controls.Add(ltMenuCssClass);

			ltMenuCatalogCssClass.EnableViewState = false;
			Controls.Add(ltMenuCatalogCssClass);

			ltMenuCatalogSelectedCssClass.EnableViewState = false;
			Controls.Add(ltMenuCatalogSelectedCssClass);

			ltMenuCatalogChildSelectedCssClass.EnableViewState = false;
			Controls.Add(ltMenuCatalogChildSelectedCssClass);

			ltMenuClipCssClass.EnableViewState = false;
			Controls.Add(ltMenuClipCssClass);

			ltMenuClipSelectedCssClass.EnableViewState = false;
			Controls.Add(ltMenuClipSelectedCssClass);

			ltMenuClipChildSelectedCssClass.EnableViewState = false;
			Controls.Add(ltMenuClipChildSelectedCssClass);

			ltMenuClipChildExpandedCssClass.EnableViewState = false;
			Controls.Add(ltMenuClipChildExpandedCssClass);

			ltMenuOverrideFlags.EnableViewState = false;
			Controls.Add(ltMenuOverrideFlags);

			ltMenuIconFlags.EnableViewState = false;
			Controls.Add(ltMenuIconFlags);

			#endregion

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btDelete.Text = "Delete";
			btDelete.Width = Unit.Pixel(72);
			btDelete.EnableViewState = false;
			btDelete.Click += new EventHandler(delete_Click);
			Controls.Add(btDelete);

			ChildControlsCreated = true;
		}

		#region ok_Click Save and Update Apply

		protected void ok_Click(object sender, EventArgs e)
		{
			OnOkClicked(EventArgs.Empty);
		}

		#endregion

		protected void delete_Click(object sender, EventArgs e)
		{
			this.OnDeleteClicked(EventArgs.Empty);
		}

		public event EventHandler OkClicked;

		protected virtual void OnOkClicked(EventArgs e)
		{
			if (OkClicked != null)
				OkClicked(this, e);
		}

		public event EventHandler DeleteClicked;

		protected virtual void OnDeleteClicked(EventArgs e)
		{
			if (DeleteClicked != null)
				DeleteClicked(this, e);
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation |
				TableWindowFeatures.WindowPrinter;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (dbContentCatalogID != 0)
			{
				dbContentCatalog = new DbContentCatalog(dbContentCatalogID);

				#region Bind General Folder

				//
				// Set Field Entries
				//

				ltTitle.Text = dbContentCatalog.Title.ToString();
				ltDescription.Text = dbContentCatalog.Description.ToString();
				ltKeywords.Text = dbContentCatalog.Keywords.ToString();
				ltStatus.Text = dbContentCatalog.Status.ToString();
				ltWorkflowMode.Text = dbContentCatalog.WorkflowMode.ToString();
				ltCommentsEnabled.Text = dbContentCatalog.CommentsEnabled.ToString();
				ltNotifyOnComments.Text = dbContentCatalog.NotifyOnComments.ToString();
				ltEnabled.Text = dbContentCatalog.Enabled.ToString();
				ltSortOrder.Text = dbContentCatalog.SortOrder.ToString();
				ltIcon.Text = dbContentCatalog.Icon.ToString();

				//
				// Set Children Selections
				//

				// ParentCatalog

				if (dbContentCatalog.ParentCatalog != null)
					ltParentCatalog.Text = dbContentCatalog.ParentCatalog.ToString();
				else
					ltParentCatalog.Text = string.Empty;

				// DefaultClip

				if (dbContentCatalog.DefaultClip != null)
					ltDefaultClip.Text = dbContentCatalog.DefaultClip.ToString();
				else
					ltDefaultClip.Text = string.Empty;


				#endregion

				#region Bind _system Folder

				//
				// Set Field Entries
				//

				ltCreateDate.Text = dbContentCatalog.CreateDate.ToString();
				ltModifyDate.Text = dbContentCatalog.ModifyDate.ToString();

				//
				// Set Children Selections
				//


				#endregion

				#region Bind Defaults Folder

				//
				// Set Field Entries
				//

				ltDefaultTimeToPublish.Text = dbContentCatalog.DefaultTimeToPublish.ToString();
				ltDefaultTimeToExpire.Text = dbContentCatalog.DefaultTimeToExpire.ToString();
				ltDefaultTimeToArchive.Text = dbContentCatalog.DefaultTimeToArchive.ToString();
				ltDefaultKeywords.Text = dbContentCatalog.DefaultKeywords.ToString();
				ltDefaultMenuLeftIcon.Text = dbContentCatalog.DefaultMenuLeftIcon.ToString();
				ltDefaultMenuRightIcon.Text = dbContentCatalog.DefaultMenuRightIcon.ToString();

				//
				// Set Children Selections
				//

				// DefaultStatus

				if (dbContentCatalog.DefaultStatus != null)
					ltDefaultStatus.Text = dbContentCatalog.DefaultStatus.ToString();
				else
					ltDefaultStatus.Text = string.Empty;

				// DefaultRating

				if (dbContentCatalog.DefaultRating != null)
					ltDefaultRating.Text = dbContentCatalog.DefaultRating.ToString();
				else
					ltDefaultRating.Text = string.Empty;

				// DefaultArchive

				if (dbContentCatalog.DefaultArchive != null)
					ltDefaultArchive.Text = dbContentCatalog.DefaultArchive.ToString();
				else
					ltDefaultArchive.Text = string.Empty;

				// Templates

				if (dbContentCatalog.Templates != null)
					ltTemplates.Text = dbContentCatalog.Templates.ToString();
				else
					ltTemplates.Text = string.Empty;


				#endregion

				#region Bind Roles Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//

				// AuthorRole

				if (dbContentCatalog.AuthorRole != null)
					ltAuthorRole.Text = dbContentCatalog.AuthorRole.ToString();
				else
					ltAuthorRole.Text = string.Empty;

				// EditorRole

				if (dbContentCatalog.EditorRole != null)
					ltEditorRole.Text = dbContentCatalog.EditorRole.ToString();
				else
					ltEditorRole.Text = string.Empty;

				// ReviewerRole

				if (dbContentCatalog.ReviewerRole != null)
					ltReviewerRole.Text = dbContentCatalog.ReviewerRole.ToString();
				else
					ltReviewerRole.Text = string.Empty;


				#endregion

				#region Bind Menu Folder

				//
				// Set Field Entries
				//

				ltMenuLabel.Text = dbContentCatalog.MenuLabel.ToString();
				ltMenuTooltip.Text = dbContentCatalog.MenuTooltip.ToString();
				ltMenuEnabled.Text = dbContentCatalog.MenuEnabled.ToString();
				ltMenuOrder.Text = dbContentCatalog.MenuOrder.ToString();
				ltMenuLeftIcon.Text = dbContentCatalog.MenuLeftIcon.ToString();
				ltMenuRightIcon.Text = dbContentCatalog.MenuRightIcon.ToString();
				ltMenuBreakImage.Text = dbContentCatalog.MenuBreakImage.ToString();
				ltMenuBreakCssClass.Text = dbContentCatalog.MenuBreakCssClass.ToString();
				ltMenuCssClass.Text = dbContentCatalog.MenuCssClass.ToString();
				ltMenuCatalogCssClass.Text = dbContentCatalog.MenuCatalogCssClass.ToString();
				ltMenuCatalogSelectedCssClass.Text = dbContentCatalog.MenuCatalogSelectedCssClass.ToString();
				ltMenuCatalogChildSelectedCssClass.Text = dbContentCatalog.MenuCatalogChildSelectedCssClass.ToString();
				ltMenuClipCssClass.Text = dbContentCatalog.MenuClipCssClass.ToString();
				ltMenuClipSelectedCssClass.Text = dbContentCatalog.MenuClipSelectedCssClass.ToString();
				ltMenuClipChildSelectedCssClass.Text = dbContentCatalog.MenuClipChildSelectedCssClass.ToString();
				ltMenuClipChildExpandedCssClass.Text = dbContentCatalog.MenuClipChildExpandedCssClass.ToString();
				ltMenuOverrideFlags.Text = dbContentCatalog.MenuOverrideFlags.ToString();
				ltMenuIconFlags.Text = dbContentCatalog.MenuIconFlags.ToString();

				//
				// Set Children Selections
				//


				#endregion

				text = "View  - " + dbContentCatalog.ToString();
			}
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderCell("DbContentCatalog ID", "class=\"row1\"");
			RenderCell(dbContentCatalogID.ToString(), "class=\"row1\"");
			output.WriteEndTag("tr");

			renderGeneralFolder(output);

			render_systemFolder(output);

			renderDefaultsFolder(output);

			renderRolesFolder(output);

			renderMenuFolder(output);

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
			if (DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		#region Render General Folder

		private void renderGeneralFolder(HtmlTextWriter output)
		{
			//
			// Render General Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("General");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Title
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Title");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltTitle.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Description
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Description");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDescription.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Keywords
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Keywords");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltKeywords.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Status
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Status");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltStatus.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render WorkflowMode
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Workflow Mode");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltWorkflowMode.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render CommentsEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("CommentsEnabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltCommentsEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotifyOnComments
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("NotifyOnComments");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltNotifyOnComments.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Enabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SortOrder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("SortOrder");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltSortOrder.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ParentCatalog
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ParentCatalog");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltParentCatalog.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ChildCatalogs
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ChildCatalogs");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltChildCatalogs.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultClip
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultClip");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultClip.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Icon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Icon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render _system Folder

		private void render_systemFolder(HtmlTextWriter output)
		{
			//
			// Render _system Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("System Folder");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render CreateDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("CreateDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
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
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ModifyDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltModifyDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Defaults Folder

		private void renderDefaultsFolder(HtmlTextWriter output)
		{
			//
			// Render Defaults Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Defaults");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultTimeToPublish
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Time To Publish");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultTimeToPublish.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultTimeToExpire
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Time To Expiration");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultTimeToExpire.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultTimeToArchive
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Time To Archive");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultTimeToArchive.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultKeywords
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultKeywords");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultKeywords.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultStatus
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultStatus");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultStatus.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultRating
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultRating");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultRating.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultArchive
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Default Archive");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultArchive.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Templates
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Templates");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltTemplates.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultMenuLeftIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultMenuLeftIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultMenuLeftIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DefaultMenuRightIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DefaultMenuRightIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDefaultMenuRightIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Roles Folder

		private void renderRolesFolder(HtmlTextWriter output)
		{
			//
			// Render Roles Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Roles");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AuthorRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Author Roles");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAuthorRole.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EditorRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Editor Roles");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEditorRole.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ReviewerRole
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Reviewer Role");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltReviewerRole.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Menu Folder

		private void renderMenuFolder(HtmlTextWriter output)
		{
			//
			// Render Menu Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Menu");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuLabel
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Label");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuLabel.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuTooltip
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Tooltip");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuTooltip.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enabled");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuOrder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Menu Order");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuOrder.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuLeftIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Left Icon URL");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuLeftIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuRightIcon
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Right Icon URL");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuRightIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuBreakImage
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Break Image URL");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuBreakImage.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuBreakCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Break CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuBreakCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCatalogCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuCatalogCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCatalogSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuCatalogSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuCatalogChildSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog Child Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuCatalogChildSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuClipCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuClipSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipChildSelectedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip Child Selected CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuClipChildSelectedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuClipChildExpandedCssClass
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Clip Child Expanded CSS Class");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuClipChildExpandedCssClass.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuOverrideFlags
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Style Overrides");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuOverrideFlags.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuIconFlags
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuIconFlags.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] myState = (object[]) savedState;
				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
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