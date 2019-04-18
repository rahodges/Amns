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
	[ToolboxData("<DbContentClip:DbContentClipView runat=server></{0}:DbContentClipView>")]
	public class DbContentClipView : TableWindow, INamingContainer
	{
		private int dbContentClipID;
		private DbContentClip dbContentClip;

		#region Private Control Fields for _system Folder

		private Literal ltCreateDate = new Literal();
		private Literal ltModifyDate = new Literal();

		#endregion

		#region Private Control Fields for General Folder

		private Literal ltTitle = new Literal();
		private Literal ltDescription = new Literal();
		private Literal ltKeywords = new Literal();
		private Literal ltIcon = new Literal();
		private Literal ltStatus = new Literal();

		#endregion

		#region Private Control Fields for Body Folder

		private Literal ltBody = new Literal();

		#endregion

		#region Private Control Fields for Publishing Folder

		private Literal ltPublishDate = new Literal();
		private Literal ltExpirationDate = new Literal();
		private Literal ltArchiveDate = new Literal();
		private Literal ltPriority = new Literal();
		private Literal ltSortOrder = new Literal();
		private Literal ltCommentsEnabled = new Literal();
		private Literal ltNotifyOnComments = new Literal();
		private Literal ltOverrideUrl = new Literal();
		private Literal ltParentCatalog = new Literal();
		private Literal ltRating = new Literal();
		private Literal ltReferences = new Literal();
		private Literal ltWorkingDraft = new Literal();

		#endregion

		#region Private Control Fields for Contributors Folder

		private Literal ltAuthors = new Literal();
		private Literal ltEditors = new Literal();

		#endregion

		#region Private Control Fields for Menu Folder

		private Literal ltMenuLabel = new Literal();
		private Literal ltMenuTooltip = new Literal();
		private Literal ltMenuEnabled = new Literal();
		private Literal ltMenuOrder = new Literal();
		private Literal ltMenuLeftIcon = new Literal();
		private Literal ltMenuLeftIconOver = new Literal();
		private Literal ltMenuRightIcon = new Literal();
		private Literal ltMenuRightIconOver = new Literal();
		private Literal ltMenuBreak = new Literal();

		#endregion

		#region Private Control Fields for Security Folder


		#endregion

		private Button btOk = new Button();
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
				dbContentClipID = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for _system Folder

			ltCreateDate.EnableViewState = false;
			Controls.Add(ltCreateDate);

			ltModifyDate.EnableViewState = false;
			Controls.Add(ltModifyDate);

			#endregion

			#region Child Controls for General Folder

			ltTitle.EnableViewState = false;
			Controls.Add(ltTitle);

			ltDescription.EnableViewState = false;
			Controls.Add(ltDescription);

			ltKeywords.EnableViewState = false;
			Controls.Add(ltKeywords);

			ltIcon.EnableViewState = false;
			Controls.Add(ltIcon);

			ltStatus.EnableViewState = false;
			Controls.Add(ltStatus);

			#endregion

			#region Child Controls for Body Folder

			ltBody.EnableViewState = false;
			Controls.Add(ltBody);

			#endregion

			#region Child Controls for Publishing Folder

			ltParentCatalog.EnableViewState = false;
			Controls.Add(ltParentCatalog);

			ltPublishDate.EnableViewState = false;
			Controls.Add(ltPublishDate);

			ltExpirationDate.EnableViewState = false;
			Controls.Add(ltExpirationDate);

			ltArchiveDate.EnableViewState = false;
			Controls.Add(ltArchiveDate);

			ltPriority.EnableViewState = false;
			Controls.Add(ltPriority);

			ltSortOrder.EnableViewState = false;
			Controls.Add(ltSortOrder);

			ltRating.EnableViewState = false;
			Controls.Add(ltRating);

			ltReferences.EnableViewState = false;
			Controls.Add(ltReferences);

			ltCommentsEnabled.EnableViewState = false;
			Controls.Add(ltCommentsEnabled);

			ltNotifyOnComments.EnableViewState = false;
			Controls.Add(ltNotifyOnComments);

			ltWorkingDraft.EnableViewState = false;
			Controls.Add(ltWorkingDraft);

			ltOverrideUrl.EnableViewState = false;
			Controls.Add(ltOverrideUrl);

			#endregion

			#region Child Controls for Contributors Folder

			ltAuthors.EnableViewState = false;
			Controls.Add(ltAuthors);

			ltEditors.EnableViewState = false;
			Controls.Add(ltEditors);

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

			ltMenuLeftIconOver.EnableViewState = false;
			Controls.Add(ltMenuLeftIconOver);

			ltMenuRightIcon.EnableViewState = false;
			Controls.Add(ltMenuRightIcon);

			ltMenuRightIconOver.EnableViewState = false;
			Controls.Add(ltMenuRightIconOver);

			ltMenuBreak.EnableViewState = false;
			Controls.Add(ltMenuBreak);

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

		#region ok_Click Save and Update Method

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
			if(OkClicked != null)
				OkClicked(this, e);
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
			features = TableWindowFeatures.DisableContentSeparation | 
				TableWindowFeatures.WindowPrinter;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if(dbContentClipID != 0)
			{
				dbContentClip = new DbContentClip(dbContentClipID);

				#region Bind _system Folder

				//
				// Set Field Entries
				//

				ltCreateDate.Text = dbContentClip.CreateDate.ToString();
				ltModifyDate.Text = dbContentClip.ModifyDate.ToString();

				//
				// Set Children Selections
				//


				#endregion

				#region Bind General Folder

				//
				// Set Field Entries
				//

				ltTitle.Text = dbContentClip.Title.ToString();
				ltDescription.Text = dbContentClip.Description.ToString();
				ltKeywords.Text = dbContentClip.Keywords.ToString();
				ltIcon.Text = dbContentClip.Icon.ToString();

				//
				// Set Children Selections
				//

				// Status

				if(dbContentClip.Status != null)
					ltStatus.Text = dbContentClip.Status.ToString();
				else
					ltStatus.Text = string.Empty;


				#endregion

				#region Bind Body Folder

				//
				// Set Field Entries
				//

				ltBody.Text = dbContentClip.Body.ToString();

				//
				// Set Children Selections
				//


				#endregion

				#region Bind Publishing Folder

				//
				// Set Field Entries
				//

				ltPublishDate.Text = dbContentClip.PublishDate.ToString();
				ltExpirationDate.Text = dbContentClip.ExpirationDate.ToString();
				ltArchiveDate.Text = dbContentClip.ArchiveDate.ToString();
				ltPriority.Text = dbContentClip.Priority.ToString();
				ltSortOrder.Text = dbContentClip.SortOrder.ToString();
				ltCommentsEnabled.Text = dbContentClip.CommentsEnabled.ToString();
				ltNotifyOnComments.Text = dbContentClip.NotifyOnComments.ToString();
				ltOverrideUrl.Text = dbContentClip.OverrideUrl.ToString();

				//
				// Set Children Selections
				//

				// ParentCatalog

				if(dbContentClip.ParentCatalog != null)
					ltParentCatalog.Text = dbContentClip.ParentCatalog.ToString();
				else
					ltParentCatalog.Text = string.Empty;

				// Rating

				if(dbContentClip.Rating != null)
					ltRating.Text = dbContentClip.Rating.ToString();
				else
					ltRating.Text = string.Empty;

				// References

				if(dbContentClip.References != null)
					ltReferences.Text = dbContentClip.References.ToString();
				else
					ltReferences.Text = string.Empty;

				// WorkingDraft

				if(dbContentClip.WorkingDraft != null)
					ltWorkingDraft.Text = dbContentClip.WorkingDraft.ToString();
				else
					ltWorkingDraft.Text = string.Empty;


				#endregion

				#region Bind Contributors Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//

				// Authors

				if(dbContentClip.Authors != null)
					ltAuthors.Text = dbContentClip.Authors.ToString();
				else
					ltAuthors.Text = string.Empty;

				// Editors

				if(dbContentClip.Editors != null)
					ltEditors.Text = dbContentClip.Editors.ToString();
				else
					ltEditors.Text = string.Empty;


				#endregion

				#region Bind Menu Folder

				//
				// Set Field Entries
				//

				ltMenuLabel.Text = dbContentClip.MenuLabel.ToString();
				ltMenuTooltip.Text = dbContentClip.MenuTooltip.ToString();
				ltMenuEnabled.Text = dbContentClip.MenuEnabled.ToString();
				ltMenuOrder.Text = dbContentClip.MenuOrder.ToString();
				ltMenuLeftIcon.Text = dbContentClip.MenuLeftIcon.ToString();
				ltMenuLeftIconOver.Text = dbContentClip.MenuLeftIconOver.ToString();
				ltMenuRightIcon.Text = dbContentClip.MenuRightIcon.ToString();
				ltMenuRightIconOver.Text = dbContentClip.MenuRightIconOver.ToString();
				ltMenuBreak.Text = dbContentClip.MenuBreak.ToString();

				//
				// Set Children Selections
				//


				#endregion

				#region Bind Security Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//


				#endregion

				text = "View  - " + dbContentClip.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "DbContentClip ID", dbContentClipID.ToString());
			output.WriteEndTag("tr");

			render_systemFolder(output);

			renderGeneralFolder(output);

			renderBodyFolder(output);

			renderPublishingFolder(output);

			renderContributorsFolder(output);

			renderMenuFolder(output);

			renderSecurityFolder(output);

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
			if(DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

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

		}

		#endregion

		#region Render Body Folder

		private void renderBodyFolder(HtmlTextWriter output)
		{
			//
			// Render Body Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Body");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Body
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Content");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltBody.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Publishing Folder

		private void renderPublishingFolder(HtmlTextWriter output)
		{
			//
			// Render Publishing Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Publishing");
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
			output.Write("Parent Catalog");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltParentCatalog.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PublishDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Publish Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPublishDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ExpirationDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Expiration Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltExpirationDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ArchiveDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Archive Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltArchiveDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Priority
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Priority");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPriority.RenderControl(output);
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
			// Render Rating
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Rating");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltRating.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render References
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Cross References");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltReferences.RenderControl(output);
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
			// Render OverrideUrl
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("OverrideUrl");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltOverrideUrl.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Contributors Folder

		private void renderContributorsFolder(HtmlTextWriter output)
		{
			//
			// Render Contributors Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Contributors");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Authors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Authors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Editors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Editors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEditors.RenderControl(output);
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
			output.Write("MenuLabel");
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
			output.Write("MenuTooltip");
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
			output.Write("MenuEnabled");
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
			output.Write("MenuOrder");
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
			output.Write("MenuLeftIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuLeftIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuLeftIconOver
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuLeftIconOver");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuLeftIconOver.RenderControl(output);
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
			output.Write("MenuRightIcon");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuRightIcon.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuRightIconOver
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuRightIconOver");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuRightIconOver.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MenuBreak
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("MenuBreak");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMenuBreak.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Security Folder

		private void renderSecurityFolder(HtmlTextWriter output)
		{
			//
			// Render Security Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Security");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

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
