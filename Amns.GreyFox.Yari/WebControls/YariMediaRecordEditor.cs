/* ********************************************************** *
 * AMNS Yari Media Record Editor                              *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Yari.WebControls
{
	/// <summary>
	/// Default web editor for YariMediaRecord.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
		ToolboxData("<{0}:YariMediaRecordEditor runat=server></{0}:YariMediaRecordEditor>")]
	public class YariMediaRecordEditor : TableWindow, INamingContainer
	{
		private int yariMediaRecordID;
		private YariMediaRecord editYariMediaRecord;
		private string connectionString;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for Default Folder

		private TextBox tbEndNoteReferenceID = new TextBox();
		private TextBox tbPublishYear = new TextBox();
		private TextBox tbTitle = new TextBox();
		private TextBox tbPages = new TextBox();
		private TextBox tbEdition = new TextBox();
		private TextBox tbIsbn = new TextBox();
		private TextBox tbLabel = new TextBox();
		private Editor ftbAbstractText = new Editor();
        private Editor ftbContentsText = new Editor();
		private TextBox tbNotesText = new TextBox();
		private DateEditor deAmazonFillDate = new DateEditor();
		private DateEditor deAmazonRefreshDate = new DateEditor();
		private TextBox tbImageUrlSmall = new TextBox();
		private TextBox tbImageUrlMedium = new TextBox();
		private TextBox tbImageUrlLarge = new TextBox();
		private Literal ltAmazonListPrice = new Literal();
		private Literal ltAmazonOurPrice = new Literal();
		private Literal ltAmazonAvailability = new Literal();
		private Literal ltAmazonMedia = new Literal();
		private Literal ltAmazonReleaseDate = new Literal();
		private Literal ltAmazonAsin = new Literal();
		private CheckBox cbAbstractEnabled = new CheckBox();
		private CheckBox cbContentsEnabled = new CheckBox();
		private CheckBox cbNotesEnabled = new CheckBox();
		private TextBox tbAuthors = new TextBox();
		private TextBox tbSecondaryAuthors = new TextBox();
		private TextBox tbPublisher = new TextBox();
		private MultiSelectBox msMediaType = new MultiSelectBox();
		private MultiSelectBox msKeywords = new MultiSelectBox();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int YariMediaRecordID
		{
			get
			{
				return yariMediaRecordID;
			}
			set
			{
				loadFlag = true;
				yariMediaRecordID = value;
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

		protected override void CreateChildControls()
		{
			Controls.Clear();
			bindDropDownLists();

			#region Child Controls for Default Folder

			tbEndNoteReferenceID.EnableViewState = false;
			Controls.Add(tbEndNoteReferenceID);

			tbPublishYear.EnableViewState = false;
			Controls.Add(tbPublishYear);

			tbTitle.EnableViewState = false;
			Controls.Add(tbTitle);

			tbPages.EnableViewState = false;
			Controls.Add(tbPages);

			tbEdition.EnableViewState = false;
			Controls.Add(tbEdition);

			tbIsbn.EnableViewState = false;
			Controls.Add(tbIsbn);

			tbLabel.EnableViewState = false;
			Controls.Add(tbLabel);

			ftbAbstractText.ID = this.ID + "_AbstractText";
			ftbAbstractText.Width = Unit.Percentage(100);
			ftbAbstractText.EnableViewState = false;
			Controls.Add(ftbAbstractText);

			ftbContentsText.ID = this.ID + "_ContentsText";
			ftbContentsText.Width = Unit.Percentage(100);
			ftbContentsText.EnableViewState = false;
			Controls.Add(ftbContentsText);

			tbNotesText.EnableViewState = false;
			Controls.Add(tbNotesText);

			deAmazonFillDate.ID = this.ID + "_AmazonFillDate";
			deAmazonFillDate.AutoAdjust = true;
			deAmazonFillDate.EnableViewState = false;
			Controls.Add(deAmazonFillDate);

			deAmazonRefreshDate.ID = this.ID + "_AmazonRefreshDate";
			deAmazonRefreshDate.AutoAdjust = true;
			deAmazonRefreshDate.EnableViewState = false;
			Controls.Add(deAmazonRefreshDate);

			tbImageUrlSmall.EnableViewState = false;
			Controls.Add(tbImageUrlSmall);

			tbImageUrlMedium.EnableViewState = false;
			Controls.Add(tbImageUrlMedium);

			tbImageUrlLarge.EnableViewState = false;
			Controls.Add(tbImageUrlLarge);

			ltAmazonListPrice.EnableViewState = false;
			Controls.Add(ltAmazonListPrice);

			ltAmazonOurPrice.EnableViewState = false;
			Controls.Add(ltAmazonOurPrice);

			ltAmazonAvailability.EnableViewState = false;
			Controls.Add(ltAmazonAvailability);

			ltAmazonMedia.EnableViewState = false;
			Controls.Add(ltAmazonMedia);

			ltAmazonReleaseDate.EnableViewState = false;
			Controls.Add(ltAmazonReleaseDate);

			ltAmazonAsin.EnableViewState = false;
			Controls.Add(ltAmazonAsin);

			cbAbstractEnabled.EnableViewState = false;
			Controls.Add(cbAbstractEnabled);

			cbContentsEnabled.EnableViewState = false;
			Controls.Add(cbContentsEnabled);

			cbNotesEnabled.EnableViewState = false;
			Controls.Add(cbNotesEnabled);

			tbAuthors.EnableViewState = false;
			Controls.Add(tbAuthors);

			tbSecondaryAuthors.EnableViewState = false;
			Controls.Add(tbSecondaryAuthors);

			tbPublisher.EnableViewState = false;
			Controls.Add(tbPublisher);

			msMediaType.Mode = MultiSelectBoxMode.DropDownList;
			Controls.Add(msMediaType);

			msKeywords.Mode = MultiSelectBoxMode.DualSelect;
			Controls.Add(msKeywords);

			#endregion

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btCancel.Text = "Cancel";
			btCancel.Width = Unit.Pixel(72);
			btCancel.EnableViewState = false;
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

			YariMediaTypeManager mediaTypeManager = new YariMediaTypeManager();
			YariMediaTypeCollection mediaTypeCollection = mediaTypeManager.GetCollection(string.Empty, string.Empty);
			foreach(YariMediaType mediaType in mediaTypeCollection)
			{
				ListItem i = new ListItem(mediaType.ToString(), mediaType.ID.ToString());
				msMediaType.Items.Add(i);
			}

			YariMediaKeywordManager keywordsManager = new YariMediaKeywordManager();
			YariMediaKeywordCollection keywordsCollection = keywordsManager.GetCollection(string.Empty, "Keyword");
			foreach(YariMediaKeyword keywords in keywordsCollection)
			{
				ListItem i = new ListItem(keywords.ToString(), keywords.ID.ToString());
				msKeywords.Items.Add(i);
			}

			#endregion

		}
		#region ok_Click Save and Update Apply

		protected void ok_Click(object sender, EventArgs e)
		{
			if(yariMediaRecordID == 0)
				editYariMediaRecord = new YariMediaRecord();
			else
				editYariMediaRecord = new YariMediaRecord(yariMediaRecordID);

			editYariMediaRecord.EndNoteReferenceID = int.Parse(tbEndNoteReferenceID.Text);
			editYariMediaRecord.PublishYear = int.Parse(tbPublishYear.Text);
			editYariMediaRecord.Title = tbTitle.Text;
			editYariMediaRecord.Pages = int.Parse(tbPages.Text);
			editYariMediaRecord.Edition = tbEdition.Text;
			editYariMediaRecord.Isbn = tbIsbn.Text;
			editYariMediaRecord.Label = tbLabel.Text;
			editYariMediaRecord.AbstractText = ftbAbstractText.ContentHTML;
            editYariMediaRecord.ContentsText = ftbContentsText.ContentHTML;
			editYariMediaRecord.NotesText = tbNotesText.Text;
			editYariMediaRecord.AmazonFillDate = deAmazonFillDate.Date;
			editYariMediaRecord.AmazonRefreshDate = deAmazonRefreshDate.Date;
			editYariMediaRecord.ImageUrlSmall = tbImageUrlSmall.Text;
			editYariMediaRecord.ImageUrlMedium = tbImageUrlMedium.Text;
			editYariMediaRecord.ImageUrlLarge = tbImageUrlLarge.Text;
			editYariMediaRecord.AbstractEnabled = cbAbstractEnabled.Checked;
			editYariMediaRecord.ContentsEnabled = cbContentsEnabled.Checked;
			editYariMediaRecord.NotesEnabled = cbNotesEnabled.Checked;
			editYariMediaRecord.Authors = tbAuthors.Text;
			editYariMediaRecord.SecondaryAuthors = tbSecondaryAuthors.Text;
			editYariMediaRecord.Publisher = tbPublisher.Text;

			if(msMediaType.SelectedItem != null)
				editYariMediaRecord.MediaType = YariMediaType.NewPlaceHolder( 
					int.Parse(msMediaType.SelectedItem.Value));
			else
				editYariMediaRecord.MediaType = null;

			if(msKeywords.IsChanged)
			{
				editYariMediaRecord.Keywords = new YariMediaKeywordCollection();
				foreach(ListItem i in msKeywords.Items)
					if(i.Selected)
						editYariMediaRecord.Keywords.Add(YariMediaKeyword.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(editOnAdd)
				yariMediaRecordID = editYariMediaRecord.Save();
			else
				editYariMediaRecord.Save();

			if(resetOnAdd)
			{
				tbEndNoteReferenceID.Text = string.Empty;
				tbPublishYear.Text = string.Empty;
				tbTitle.Text = string.Empty;
				tbPages.Text = string.Empty;
				tbEdition.Text = string.Empty;
				tbIsbn.Text = string.Empty;
				tbLabel.Text = string.Empty;
                ftbAbstractText.ContentHTML = string.Empty;
                ftbContentsText.ContentHTML = string.Empty;
				tbNotesText.Text = string.Empty;
				deAmazonFillDate.Date = DateTime.Now;
				deAmazonRefreshDate.Date = DateTime.Now;
				tbImageUrlSmall.Text = string.Empty;
				tbImageUrlMedium.Text = string.Empty;
				tbImageUrlLarge.Text = string.Empty;
				cbAbstractEnabled.Checked = false;
				cbContentsEnabled.Checked = false;
				cbNotesEnabled.Checked = false;
				tbAuthors.Text = string.Empty;
				tbSecondaryAuthors.Text = string.Empty;
				tbPublisher.Text = string.Empty;
				msMediaType.SelectedIndex = 0;
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
		}

		protected override void OnPreRender(EventArgs e)
		{
			if(yariMediaRecordID != 0 & loadFlag)
			{
				editYariMediaRecord = new YariMediaRecord(yariMediaRecordID);

				//
				// Set Field Entries
				//
				tbEndNoteReferenceID.Text = editYariMediaRecord.EndNoteReferenceID.ToString();
				tbPublishYear.Text = editYariMediaRecord.PublishYear.ToString();
				tbTitle.Text = editYariMediaRecord.Title;
				tbPages.Text = editYariMediaRecord.Pages.ToString();
				tbEdition.Text = editYariMediaRecord.Edition;
				tbIsbn.Text = editYariMediaRecord.Isbn;
				tbLabel.Text = editYariMediaRecord.Label;
                ftbAbstractText.ContentHTML = editYariMediaRecord.AbstractText;
                ftbContentsText.ContentHTML = editYariMediaRecord.ContentsText;
				tbNotesText.Text = editYariMediaRecord.NotesText;
				deAmazonFillDate.Date = editYariMediaRecord.AmazonFillDate;
				deAmazonRefreshDate.Date = editYariMediaRecord.AmazonRefreshDate;
				tbImageUrlSmall.Text = editYariMediaRecord.ImageUrlSmall;
				tbImageUrlMedium.Text = editYariMediaRecord.ImageUrlMedium;
				tbImageUrlLarge.Text = editYariMediaRecord.ImageUrlLarge;
				ltAmazonListPrice.Text = editYariMediaRecord.AmazonListPrice.ToString();
				ltAmazonOurPrice.Text = editYariMediaRecord.AmazonOurPrice.ToString();
				ltAmazonAvailability.Text = editYariMediaRecord.AmazonAvailability.ToString();
				ltAmazonMedia.Text = editYariMediaRecord.AmazonMedia.ToString();
				ltAmazonReleaseDate.Text = editYariMediaRecord.AmazonReleaseDate.ToString();
				ltAmazonAsin.Text = editYariMediaRecord.AmazonAsin.ToString();
				cbAbstractEnabled.Checked = editYariMediaRecord.AbstractEnabled;
				cbContentsEnabled.Checked = editYariMediaRecord.ContentsEnabled;
				cbNotesEnabled.Checked = editYariMediaRecord.NotesEnabled;
				tbAuthors.Text = editYariMediaRecord.Authors;
				tbSecondaryAuthors.Text = editYariMediaRecord.SecondaryAuthors;
				tbPublisher.Text = editYariMediaRecord.Publisher;

				//
				// Set Children Selections
				//
				if(editYariMediaRecord.MediaType != null)
					foreach(ListItem item in msMediaType.Items)
						item.Selected = editYariMediaRecord.MediaType.ID.ToString() == item.Value;

				foreach(ListItem i in msKeywords.Items)
					foreach(YariMediaKeyword yariMediaKeyword in editYariMediaRecord.Keywords)
						if(i.Value == yariMediaKeyword.ID.ToString())
						{
							i.Selected = true;
							break;
						}
				Text = "Edit  - " + editYariMediaRecord.ToString();
			}
			else
				Text = "Add ";
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			this.InitializeRenderHelpers(output);

			output.WriteFullBeginTag("tr");
			RenderCell("YariMediaRecord ID", "class=\"row1\"");
			RenderCell(yariMediaRecordID.ToString(), "class=\"row1\"");
			output.WriteEndTag("tr");

			renderDefaultFolder(output);

			render_systemFolder(output);

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
			// Render Default Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Default");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EndNoteReferenceID
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EndNoteReferenceID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEndNoteReferenceID.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PublishYear
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Year");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPublishYear.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Title
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Title");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbTitle.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Pages
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Page Count");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPages.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Edition
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Edition");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEdition.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Isbn
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Isbn");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbIsbn.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Label
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Label");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLabel.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AbstractText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AbstractText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ftbAbstractText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContentsText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ContentsText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ftbContentsText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotesText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("NotesText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbNotesText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonFillDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonFillDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			deAmazonFillDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonRefreshDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonRefreshDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			deAmazonRefreshDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlSmall
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlSmall");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbImageUrlSmall.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlMedium
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlMedium");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbImageUrlMedium.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlLarge
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlLarge");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbImageUrlLarge.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonListPrice
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonListPrice");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonListPrice.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonOurPrice
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonOurPrice");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonOurPrice.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonAvailability
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonAvailability");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonAvailability.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonMedia
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonMedia");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonMedia.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonReleaseDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonReleaseDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonReleaseDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonAsin
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonAsin");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonAsin.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AbstractEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Abstract?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			cbAbstractEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContentsEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Contents?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			cbContentsEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotesEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Notes?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			cbNotesEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Authors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Authors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SecondaryAuthors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Secondary Authors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSecondaryAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Publisher
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Publisher");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPublisher.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MediaType
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Reference Type");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			msMediaType.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Keywords
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Keywords");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			msKeywords.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

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

		}

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null)
					base.LoadViewState(myState[0]);
				if(myState[1] != null)
					yariMediaRecordID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = yariMediaRecordID;
			return myState;
		}
	}
}
