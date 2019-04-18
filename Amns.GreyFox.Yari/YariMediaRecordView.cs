using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using FreeTextBoxControls;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Yari.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for YariMediaRecord.
	/// </summary>
	[ToolboxData("<YariMediaRecord:YariMediaRecordView runat=server></{0}:YariMediaRecordView>")]
	public class YariMediaRecordView : TableWindow, INamingContainer
	{
		private int yariMediaRecordID;
		private YariMediaRecord yariMediaRecord;

		#region Private Control Fields for Default Folder

		private Literal ltEndNoteReferenceID = new Literal();
		private Literal ltPublishYear = new Literal();
		private Literal ltTitle = new Literal();
		private Literal ltPages = new Literal();
		private Literal ltEdition = new Literal();
		private Literal ltIsbn = new Literal();
		private Literal ltLabel = new Literal();
		private Literal ltAbstractText = new Literal();
		private Literal ltContentsText = new Literal();
		private Literal ltNotesText = new Literal();
		private Literal ltAmazonFillDate = new Literal();
		private Literal ltAmazonRefreshDate = new Literal();
		private Literal ltImageUrlSmall = new Literal();
		private Literal ltImageUrlMedium = new Literal();
		private Literal ltImageUrlLarge = new Literal();
		private Literal ltAmazonListPrice = new Literal();
		private Literal ltAmazonOurPrice = new Literal();
		private Literal ltAmazonAvailability = new Literal();
		private Literal ltAmazonMedia = new Literal();
		private Literal ltAmazonReleaseDate = new Literal();
		private Literal ltAmazonAsin = new Literal();
		private Literal ltAbstractEnabled = new Literal();
		private Literal ltContentsEnabled = new Literal();
		private Literal ltNotesEnabled = new Literal();
		private Literal ltAuthors = new Literal();
		private Literal ltSecondaryAuthors = new Literal();
		private Literal ltPublisher = new Literal();
		private Literal ltMediaType = new Literal();
		private Literal ltKeywords = new Literal();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
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
				yariMediaRecordID = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for Default Folder

			ltEndNoteReferenceID.EnableViewState = false;
			Controls.Add(ltEndNoteReferenceID);

			ltPublishYear.EnableViewState = false;
			Controls.Add(ltPublishYear);

			ltTitle.EnableViewState = false;
			Controls.Add(ltTitle);

			ltPages.EnableViewState = false;
			Controls.Add(ltPages);

			ltEdition.EnableViewState = false;
			Controls.Add(ltEdition);

			ltIsbn.EnableViewState = false;
			Controls.Add(ltIsbn);

			ltLabel.EnableViewState = false;
			Controls.Add(ltLabel);

			ltAbstractText.EnableViewState = false;
			Controls.Add(ltAbstractText);

			ltContentsText.EnableViewState = false;
			Controls.Add(ltContentsText);

			ltNotesText.EnableViewState = false;
			Controls.Add(ltNotesText);

			ltAmazonFillDate.EnableViewState = false;
			Controls.Add(ltAmazonFillDate);

			ltAmazonRefreshDate.EnableViewState = false;
			Controls.Add(ltAmazonRefreshDate);

			ltImageUrlSmall.EnableViewState = false;
			Controls.Add(ltImageUrlSmall);

			ltImageUrlMedium.EnableViewState = false;
			Controls.Add(ltImageUrlMedium);

			ltImageUrlLarge.EnableViewState = false;
			Controls.Add(ltImageUrlLarge);

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

			ltAbstractEnabled.EnableViewState = false;
			Controls.Add(ltAbstractEnabled);

			ltContentsEnabled.EnableViewState = false;
			Controls.Add(ltContentsEnabled);

			ltNotesEnabled.EnableViewState = false;
			Controls.Add(ltNotesEnabled);

			ltAuthors.EnableViewState = false;
			Controls.Add(ltAuthors);

			ltSecondaryAuthors.EnableViewState = false;
			Controls.Add(ltSecondaryAuthors);

			ltPublisher.EnableViewState = false;
			Controls.Add(ltPublisher);

			ltMediaType.EnableViewState = false;
			Controls.Add(ltMediaType);

			ltKeywords.EnableViewState = false;
			Controls.Add(ltKeywords);

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
			if(yariMediaRecordID != 0)
			{
				yariMediaRecord = new YariMediaRecord(yariMediaRecordID);

				#region Bind Default Folder

				//
				// Set Field Entries
				//

				ltEndNoteReferenceID.Text = yariMediaRecord.EndNoteReferenceID.ToString();
				ltPublishYear.Text = yariMediaRecord.PublishYear.ToString();
				ltTitle.Text = yariMediaRecord.Title.ToString();
				ltPages.Text = yariMediaRecord.Pages.ToString();
				ltEdition.Text = yariMediaRecord.Edition.ToString();
				ltIsbn.Text = yariMediaRecord.Isbn.ToString();
				ltLabel.Text = yariMediaRecord.Label.ToString();
				ltAbstractText.Text = yariMediaRecord.AbstractText.ToString();
				ltContentsText.Text = yariMediaRecord.ContentsText.ToString();
				ltNotesText.Text = yariMediaRecord.NotesText.ToString();
				ltAmazonFillDate.Text = yariMediaRecord.AmazonFillDate.ToString();
				ltAmazonRefreshDate.Text = yariMediaRecord.AmazonRefreshDate.ToString();
				ltImageUrlSmall.Text = yariMediaRecord.ImageUrlSmall.ToString();
				ltImageUrlMedium.Text = yariMediaRecord.ImageUrlMedium.ToString();
				ltImageUrlLarge.Text = yariMediaRecord.ImageUrlLarge.ToString();
				ltAmazonListPrice.Text = yariMediaRecord.AmazonListPrice.ToString();
				ltAmazonOurPrice.Text = yariMediaRecord.AmazonOurPrice.ToString();
				ltAmazonAvailability.Text = yariMediaRecord.AmazonAvailability.ToString();
				ltAmazonMedia.Text = yariMediaRecord.AmazonMedia.ToString();
				ltAmazonReleaseDate.Text = yariMediaRecord.AmazonReleaseDate.ToString();
				ltAmazonAsin.Text = yariMediaRecord.AmazonAsin.ToString();
				ltAbstractEnabled.Text = yariMediaRecord.AbstractEnabled.ToString();
				ltContentsEnabled.Text = yariMediaRecord.ContentsEnabled.ToString();
				ltNotesEnabled.Text = yariMediaRecord.NotesEnabled.ToString();
				ltAuthors.Text = yariMediaRecord.Authors.ToString();
				ltSecondaryAuthors.Text = yariMediaRecord.SecondaryAuthors.ToString();
				ltPublisher.Text = yariMediaRecord.Publisher.ToString();

				//
				// Set Children Selections
				//

				// MediaType

				if(yariMediaRecord.MediaType != null)
					ltMediaType.Text = yariMediaRecord.MediaType.ToString();
				else
					ltMediaType.Text = string.Empty;

				// Keywords

				if(yariMediaRecord.Keywords != null)
					ltKeywords.Text = yariMediaRecord.Keywords.ToString();
				else
					ltKeywords.Text = string.Empty;


				#endregion

				#region Bind _system Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//


				#endregion

				text = "View  - " + yariMediaRecord.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "YariMediaRecord ID", yariMediaRecordID.ToString());
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
			if(DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		#region Render Default Folder

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
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EndNoteReferenceID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEndNoteReferenceID.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PublishYear
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Year");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPublishYear.RenderControl(output);
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
			// Render Pages
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Page Count");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPages.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Edition
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Edition");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEdition.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Isbn
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Isbn");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltIsbn.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Label
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
			ltLabel.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AbstractText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AbstractText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAbstractText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContentsText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ContentsText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltContentsText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotesText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("NotesText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltNotesText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonFillDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonFillDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonFillDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonRefreshDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonRefreshDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAmazonRefreshDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlSmall
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlSmall");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltImageUrlSmall.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlMedium
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlMedium");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltImageUrlMedium.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlLarge
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlLarge");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltImageUrlLarge.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonListPrice
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
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
			output.WriteAttribute("valign", "top");
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
			output.WriteAttribute("valign", "top");
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
			output.WriteAttribute("valign", "top");
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
			output.WriteAttribute("valign", "top");
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
			output.WriteAttribute("valign", "top");
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
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Abstract?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAbstractEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContentsEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Contents?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltContentsEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotesEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Notes?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltNotesEnabled.RenderControl(output);
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
			// Render SecondaryAuthors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Secondary Authors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltSecondaryAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Publisher
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Publisher");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPublisher.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MediaType
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Reference Type");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMediaType.RenderControl(output);
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
