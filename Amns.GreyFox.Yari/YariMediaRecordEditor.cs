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
	[ToolboxData("<{0}:YariMediaRecordEditor runat=server></{0}:YariMediaRecordEditor>")]
	public class YariMediaRecordEditor : TableWindow, INamingContainer
	{
		private int yariMediaRecordID;
		private YariMediaRecord obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for Default Folder

		private TextBox tbEndNoteReferenceID = new TextBox();
		private RegularExpressionValidator revEndNoteReferenceID = new RegularExpressionValidator();
		private TextBox tbPublishYear = new TextBox();
		private RegularExpressionValidator revPublishYear = new RegularExpressionValidator();
		private TextBox tbTitle = new TextBox();
		private TextBox tbPages = new TextBox();
		private RegularExpressionValidator revPages = new RegularExpressionValidator();
		private TextBox tbEdition = new TextBox();
		private TextBox tbIsbn = new TextBox();
		private TextBox tbLabel = new TextBox();
		private FreeTextBox ftbAbstractText = new FreeTextBox();
		private FreeTextBox ftbContentsText = new FreeTextBox();
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

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();
			bindDropDownLists();

			#region Child Controls for Default Folder

			tbEndNoteReferenceID.ID = this.ID + "_EndNoteReferenceID";
			tbEndNoteReferenceID.EnableViewState = false;
			Controls.Add(tbEndNoteReferenceID);
			revEndNoteReferenceID.ControlToValidate = tbEndNoteReferenceID.ID;
			revEndNoteReferenceID.ValidationExpression = "^(\\+|-)?\\d+$";
			revEndNoteReferenceID.ErrorMessage = "*";
			revEndNoteReferenceID.Display = ValidatorDisplay.Dynamic;
			revEndNoteReferenceID.EnableViewState = false;
			Controls.Add(revEndNoteReferenceID);

			tbPublishYear.ID = this.ID + "_PublishYear";
			tbPublishYear.EnableViewState = false;
			Controls.Add(tbPublishYear);
			revPublishYear.ControlToValidate = tbPublishYear.ID;
			revPublishYear.ValidationExpression = "^(\\+|-)?\\d+$";
			revPublishYear.ErrorMessage = "*";
			revPublishYear.Display = ValidatorDisplay.Dynamic;
			revPublishYear.EnableViewState = false;
			Controls.Add(revPublishYear);

			tbTitle.EnableViewState = false;
			Controls.Add(tbTitle);

			tbPages.ID = this.ID + "_Pages";
			tbPages.EnableViewState = false;
			Controls.Add(tbPages);
			revPages.ControlToValidate = tbPages.ID;
			revPages.ValidationExpression = "^(\\+|-)?\\d+$";
			revPages.ErrorMessage = "*";
			revPages.Display = ValidatorDisplay.Dynamic;
			revPages.EnableViewState = false;
			Controls.Add(revPages);

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
			#region Bind Default Child Data

			msMediaType.Items.Add(new ListItem("Null", "Null"));
			YariMediaTypeManager mediaTypeManager = new YariMediaTypeManager();
			YariMediaTypeCollection mediaTypeCollection = mediaTypeManager.GetCollection(string.Empty, string.Empty);
			foreach(YariMediaType mediaType in mediaTypeCollection)
			{
				ListItem i = new ListItem(mediaType.ToString(), mediaType.ID.ToString());
				msMediaType.Items.Add(i);
			}

			msKeywords.Items.Add(new ListItem("Null", "Null"));
			YariMediaKeywordManager keywordsManager = new YariMediaKeywordManager();
			YariMediaKeywordCollection keywordsCollection = keywordsManager.GetCollection(string.Empty, string.Empty);
			foreach(YariMediaKeyword keywords in keywordsCollection)
			{
				ListItem i = new ListItem(keywords.ToString(), keywords.ID.ToString());
				msKeywords.Items.Add(i);
			}

			#endregion

		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(yariMediaRecordID == 0)
				obj = new YariMediaRecord();
			else
				obj = new YariMediaRecord(yariMediaRecordID);

			obj.EndNoteReferenceID = int.Parse(tbEndNoteReferenceID.Text);
			obj.PublishYear = int.Parse(tbPublishYear.Text);
			obj.Title = tbTitle.Text;
			obj.Pages = int.Parse(tbPages.Text);
			obj.Edition = tbEdition.Text;
			obj.Isbn = tbIsbn.Text;
			obj.Label = tbLabel.Text;
			obj.AbstractText = ftbAbstractText.Text;
			obj.ContentsText = ftbContentsText.Text;
			obj.NotesText = tbNotesText.Text;
			obj.AmazonFillDate = deAmazonFillDate.Date;
			obj.AmazonRefreshDate = deAmazonRefreshDate.Date;
			obj.ImageUrlSmall = tbImageUrlSmall.Text;
			obj.ImageUrlMedium = tbImageUrlMedium.Text;
			obj.ImageUrlLarge = tbImageUrlLarge.Text;
			obj.AbstractEnabled = cbAbstractEnabled.Checked;
			obj.ContentsEnabled = cbContentsEnabled.Checked;
			obj.NotesEnabled = cbNotesEnabled.Checked;
			obj.Authors = tbAuthors.Text;
			obj.SecondaryAuthors = tbSecondaryAuthors.Text;
			obj.Publisher = tbPublisher.Text;

			if(msMediaType.SelectedItem != null && msMediaType.SelectedItem.Value != "Null")
				obj.MediaType = YariMediaType.NewPlaceHolder(
					int.Parse(msMediaType.SelectedItem.Value));
			else
				obj.MediaType = null;

			if(msKeywords.IsChanged)
			{
				obj.Keywords = new YariMediaKeywordCollection();
				foreach(ListItem i in msKeywords.Items)
					if(i.Selected)
						obj.Keywords.Add(YariMediaKeyword.NewPlaceHolder(int.Parse(i.Value)));
			}

			if(editOnAdd)
				yariMediaRecordID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbEndNoteReferenceID.Text = string.Empty;
				tbPublishYear.Text = string.Empty;
				tbTitle.Text = string.Empty;
				tbPages.Text = string.Empty;
				tbEdition.Text = string.Empty;
				tbIsbn.Text = string.Empty;
				tbLabel.Text = string.Empty;
				ftbAbstractText.Text = string.Empty;
				ftbContentsText.Text = string.Empty;
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
			components = TableWindowComponents.Tabs;
			tabStrip = new TabStrip();
			tabStrip.Tabs = new TabList();

			Tab DefaultTab = new Tab("Default");
			DefaultTab.Visible = true;
			DefaultTab.RenderDiv += new TabRenderHandler(renderDefaultFolder);
			DefaultTab.Visible = true;
			tabStrip.Tabs.Add(DefaultTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(yariMediaRecordID > 0)
				{
					obj = new YariMediaRecord(yariMediaRecordID);
					text = "Edit  - " + obj.ToString();
				}
				else if(yariMediaRecordID <= 0)
				{
					obj = new YariMediaRecord();
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbEndNoteReferenceID.Text = obj.EndNoteReferenceID.ToString();
				tbPublishYear.Text = obj.PublishYear.ToString();
				tbTitle.Text = obj.Title;
				tbPages.Text = obj.Pages.ToString();
				tbEdition.Text = obj.Edition;
				tbIsbn.Text = obj.Isbn;
				tbLabel.Text = obj.Label;
				ftbAbstractText.Text = obj.AbstractText;
				ftbContentsText.Text = obj.ContentsText;
				tbNotesText.Text = obj.NotesText;
				deAmazonFillDate.Date = obj.AmazonFillDate;
				deAmazonRefreshDate.Date = obj.AmazonRefreshDate;
				tbImageUrlSmall.Text = obj.ImageUrlSmall;
				tbImageUrlMedium.Text = obj.ImageUrlMedium;
				tbImageUrlLarge.Text = obj.ImageUrlLarge;
				ltAmazonListPrice.Text = obj.AmazonListPrice.ToString();
				ltAmazonOurPrice.Text = obj.AmazonOurPrice.ToString();
				ltAmazonAvailability.Text = obj.AmazonAvailability.ToString();
				ltAmazonMedia.Text = obj.AmazonMedia.ToString();
				ltAmazonReleaseDate.Text = obj.AmazonReleaseDate.ToString();
				ltAmazonAsin.Text = obj.AmazonAsin.ToString();
				cbAbstractEnabled.Checked = obj.AbstractEnabled;
				cbContentsEnabled.Checked = obj.ContentsEnabled;
				cbNotesEnabled.Checked = obj.NotesEnabled;
				tbAuthors.Text = obj.Authors;
				tbSecondaryAuthors.Text = obj.SecondaryAuthors;
				tbPublisher.Text = obj.Publisher;

				//
				// Set Children Selections
				//
				if(obj.MediaType != null)
					foreach(ListItem item in msMediaType.Items)
						item.Selected = obj.MediaType.ID.ToString() == item.Value;
					else
						msMediaType.SelectedIndex = 0;

				foreach(ListItem i in msKeywords.Items)
					foreach(YariMediaKeyword yariMediaKeyword in obj.Keywords)
						if(i.Value == yariMediaKeyword.ID.ToString())
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

		private void renderDefaultFolder(HtmlTextWriter output)
		{
			//
			// Render EndNoteReferenceID
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EndNoteReferenceID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEndNoteReferenceID.RenderControl(output);
			revEndNoteReferenceID.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PublishYear
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Year");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPublishYear.RenderControl(output);
			revPublishYear.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

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
			// Render Pages
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Page Count");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPages.RenderControl(output);
			revPages.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Edition
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Edition");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEdition.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Isbn
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Isbn");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbIsbn.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Label
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Label");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLabel.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AbstractText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AbstractText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			ftbAbstractText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContentsText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ContentsText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			ftbContentsText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotesText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("NotesText");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbNotesText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonFillDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonFillDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			deAmazonFillDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonRefreshDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonRefreshDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			deAmazonRefreshDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlSmall
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlSmall");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbImageUrlSmall.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlMedium
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlMedium");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbImageUrlMedium.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ImageUrlLarge
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ImageUrlLarge");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbImageUrlLarge.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render AmazonListPrice
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonListPrice");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
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
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonOurPrice");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
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
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonAvailability");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
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
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonMedia");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
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
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonReleaseDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
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
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("AmazonAsin");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
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
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Abstract?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbAbstractEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContentsEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Contents?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbContentsEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render NotesEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable Notes?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbNotesEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

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
			tbAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SecondaryAuthors
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Secondary Authors");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSecondaryAuthors.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Publisher
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Publisher");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPublisher.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MediaType
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Reference Type");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			msMediaType.RenderControl(output);
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
			msKeywords.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void render_systemFolder(HtmlTextWriter output)
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

