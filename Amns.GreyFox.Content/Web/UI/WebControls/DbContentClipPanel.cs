using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.Content;
using Amns.GreyFox.Security;
using Amns.GreyFox.Scheduling;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for DbContentClip.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
	ToolboxData("<{0}:DbContentClipPanel runat=server></{0}:DbContentClipPanel>")]
	public class DbContentClipPanel : Control
	{
		string		_connectionString				= string.Empty;
		int			_dbContentClipID				= -1;
		bool		_contentOnly					= false;
		bool		_inlineEditEnabled				= false;					// Allows content to be edited inline
		bool		_hitCounterEnabled				= false;
		
		string		_glossTableCssClass				= string.Empty;
		string		_glossTitleCssClass				= string.Empty;
		
		string		_titleCssClass					= "h1";
		string		_bodyCssClass					= string.Empty;
		string		_contributorsCssClass			= "h2";
		string		_publishDateCssClass			= "h2";

		string		_referenceTableCssClass			= string.Empty;
		string		_referenceTitleCssClass			= string.Empty;
		
		string		_designModeCssClass				= string.Empty;

		string		_linkFormat						= "?ref={0}";
		string		_rewriteLinkKey					= string.Empty;			// ?ref={0}
		string		_rewriteLinkFormat				= string.Empty;			// ~/gfxc_{0}.aspx
        string      _editorUserControlUrl           = "~/_skins/editor/editor.ascx";
        		    
		System.Web.UI.WebControls.TextBox		tbTitle;					// Editor Title
		System.Web.UI.WebControls.LinkButton	btEdit;						// Editor Edit Button
		System.Web.UI.WebControls.Button		btSave;						// Editor Save Button
		System.Web.UI.WebControls.Button		btCancel;					// Editor Cancel Button
		System.Web.UI.WebControls.TextBox		tbDescription;				// Editor Description Button

        Control userEditor;
        ComponentArt.Web.UI.Editor caEditor;

		bool adminView		= false;
		DbContentClip clip	= null;

		// Allow panel to only display content in the root catalog or
		// catalogs descending from the root catalog.

//		int			rootCatalogID						= -1;
//		string		_onThisPageCssClass					= string.Empty;
//		string		_onThisPageIconUrl					= string.Empty;
//		CatalogTransversalMode _transversalMode			= CatalogTransversalMode.Tree;

		#region Public Properties

		[Bindable(true), Category("Data"), DefaultValue(-1)]
		public int DbContentClipID
		{
			get { return _dbContentClipID; }
			set { _dbContentClipID = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string DesignModeCssClass
		{
			get { return _designModeCssClass; }
			set { _designModeCssClass = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool InlineEditEnabled
		{
			get { return _inlineEditEnabled; }
			set { _inlineEditEnabled = value; }
		}

        [Bindable(true), Category("Behavior"), DefaultValue("~/_skins/editor/editor.ascx")]
        public string EditorUserControlUrl
        {
            get { return _editorUserControlUrl; }
            set { _editorUserControlUrl = value; }
        }

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool HitCounterEnabled
		{
			get { return _hitCounterEnabled; }
			set { _hitCounterEnabled = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("h1")]
		public string TitleCssClass
		{
			get { return _titleCssClass; }
			set { _titleCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string BodyCssClass
		{
			get { return _bodyCssClass; }
			set { _bodyCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("h2")]
		public string ContributorsCssClass
		{
			get { return _contributorsCssClass; }
			set { _contributorsCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("h2")]
		public string PublishDateCssClass
		{
			get { return _publishDateCssClass; }
			set { _publishDateCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ReferenceTableCssClass
		{
			get { return _referenceTableCssClass; }
			set { _referenceTableCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ReferenceTitleCssClass
		{
			get { return _referenceTitleCssClass; }
			set { _referenceTitleCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string GlossTableCssClass
		{
			get { return _glossTableCssClass; }
			set { _glossTableCssClass = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string GlossTitleCssClass
		{
			get { return _glossTitleCssClass; }
			set { _glossTitleCssClass = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool ContentOnly
		{
			get { return _contentOnly; }
			set { _contentOnly = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("?ref={0}")]
		public string LinkFormat
		{
			get { return _linkFormat; }
			set { _linkFormat = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string RewriteLinkKey
		{
			get { return _rewriteLinkKey; }
			set { _rewriteLinkKey = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string RewriteLinkPath
		{
			get { return _rewriteLinkFormat; }
			set { _rewriteLinkFormat = value; }
		}

		#endregion

		protected override void CreateChildControls()
		{
			if(adminView)
			{
				btEdit = new LinkButton();
				btEdit.Text = "Edit";
				btEdit.Click += new EventHandler(btEdit_Click);
				btEdit.EnableViewState = false;
				btEdit.Visible = true;
				Controls.Add(btEdit);

				tbTitle = new TextBox();
				tbTitle.Width = Unit.Pixel(400);
				Controls.Add(tbTitle);

				tbDescription = new TextBox();
				tbDescription.Rows = 3;
				tbDescription.TextMode = TextBoxMode.MultiLine;
				tbDescription.Width = Unit.Pixel(400);
				Controls.Add(tbDescription);

                userEditor = Page.LoadControl(_editorUserControlUrl);
                userEditor.Visible = false;                
                Controls.Add(userEditor);                
                caEditor = ((IEditor)userEditor).Editor;
                ((IEditor)userEditor).SkinPath = userEditor.TemplateSourceDirectory;
                ((IEditor)userEditor).Editor.DesignCssClass = DesignModeCssClass;

				btSave = new Button();
				btSave.Text = "Save";
				btSave.Click += new EventHandler(btSave_Click);
				btSave.EnableViewState = false;
				btSave.Visible = false;
				Controls.Add(btSave);

				btCancel = new Button();
				btCancel.Text = "Cancel";
				btCancel.Click += new EventHandler(btCancel_Click);
				btCancel.EnableViewState = false;
				btCancel.Visible = false;
				Controls.Add(btCancel);
			}

			ChildControlsCreated = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			try { clip = new DbContentClip(_dbContentClipID); }
			catch { return; }

			// adminMode must be set before child controls are created!
			if(_inlineEditEnabled)
				adminView = Page.User.IsInRole(clip.ParentCatalog.EditorRole.Name);

			EnsureChildControls();

			Amns.GreyFox.Web.Handlers.AssemblyResourceHandler.RegisterScript(this.Page, "Base.js");
		}

		protected void btEdit_Click(object sender, System.EventArgs e)
		{			
			btEdit.Visible = false;
			btSave.Visible = true;
			btCancel.Visible = true;
			tbTitle.Visible = true;
            userEditor.Visible = true;
            //caEditor.Visible = true;
			//ftbEditor.Visible = true;
			tbTitle.Text = clip.Title;
			tbDescription.Text = clip.Description;
            caEditor.ContentHTML = clip.Body;
			//ftbEditor.Text = clip.Body;
		}

		protected void btSave_Click(object sender, System.EventArgs e)
		{
			clip.Title = tbTitle.Text;
			clip.Description = tbDescription.Text;
            clip.Body = caEditor.ContentHTML;
			//clip.Body = ftbEditor.Text;
			clip.Save();
            userEditor.Visible = false;
            //caEditor.Visible = false;
			//ftbEditor.Visible = false;
			btSave.Visible = false;
			btCancel.Visible = false;
		}

		protected void btCancel_Click(object sender, System.EventArgs e)
		{
            userEditor.Visible = false;
			//ftbEditor.Visible = false;
			btSave.Visible = false;
			btCancel.Visible = false;
		}

		protected override void OnPreRender(EventArgs e)
		{
			// For server side event processing to correctly fire
			// this code changes the clip AFTER OnLoad(...) has been called.
			// Administration Mode must be disabled since some clips may not
			// use it. This prevents a null reference exception when the
			// child controls have not already been created.
			// 
			// Child control events will not fire if the controls are created here,
			// therefore admin mode is disabled for Late ClipID binding.
			if(clip == null || clip.ID != _dbContentClipID)
			{
				try
				{
					clip = new DbContentClip(_dbContentClipID);					
				}
				catch
				{
					clip = null;
				}
			}

			if(clip!=null && clip.ParentCatalog != null && clip.ParentCatalog.EditorRole != null)
			{
				adminView = Page.User.IsInRole(clip.ParentCatalog.EditorRole.Name);
			}
		}

		private void RenderException(HtmlTextWriter output, string text)
		{
			output.WriteBeginTag("div");
			output.Write(" style=\"");
			output.WriteStyleAttribute("padding", "5px");
			output.WriteStyleAttribute("margin", "5px");
			output.WriteStyleAttribute("border-top", "#dcdcdc thin solid");
			output.WriteStyleAttribute("border-bottom", "#a9a9a9 thin solid");
			output.WriteStyleAttribute("border-left", "#dcdcdc thin solid");
			output.WriteStyleAttribute("border-right", "#a9a9a9 thin solid");
			output.WriteStyleAttribute("font-family", "Arial");
			output.WriteStyleAttribute("font-size", "x-small");
			output.WriteStyleAttribute("font-weight", "bold");			
			output.WriteStyleAttribute("color", "#000000");				
			output.WriteStyleAttribute("background-color", "#cccccc");
			output.Write("\">");
			output.Write(text);
			output.WriteEndTag("div");
		}


		protected override void Render(HtmlTextWriter output)
		{
			#region Test for Errors

			if(clip == null)
			{
				RenderException(output, "The requested content could not be found.");
				return;
			}

			if(!adminView)
			{
				if(!clip.ParentCatalog.Enabled)
				{
					RenderException(output, "The content's requested catalog is disabled.");
					return;
				}

				if(clip.ExpirationDate < DateTime.Now)
				{
					RenderException(output, "The requested content is expired.");
					return;
				}

				if(clip.PublishDate > DateTime.Now)
				{
					RenderException(output, "The requested content is awaiting publishing.");
					return;
				}
			}

			#endregion

			#region Editor Rendering
			
            if (userEditor != null && userEditor.Visible)
            //if (caEditor != null && caEditor.Visible)
			//if (ftbEditor != null && ftbEditor.Visible)
			{
				output.Write("<div style=\"margin-bottom:5px\">Title:</div><div style=\"margin-bottom:5px\">");
				tbTitle.RenderControl(output);
				output.Write("</div><div style=\"margin-bottom:5px\">Description:</div><div style=\"margin-bottom:5px\">");
				tbDescription.RenderControl(output);
				output.Write("</div><div style=\"margin-bottom:5px\">");
                userEditor.RenderControl(output);
				//ftbEditor.RenderControl(output);
				output.Write("</div><div>");
				btSave.RenderControl(output);
				output.Write("&nbsp;");
				btCancel.RenderControl(output);
				output.Write("</div>");
				return;
			}

			#endregion

			#region Content Rendering

			if(_contentOnly)
			{
				// Resolve home references! Whahoo!
				output.Write(clip.Body.Replace("=\"~", Page.ResolveUrl("~")));

				// Display Edit Link
				if(btEdit != null && btEdit.Visible)
				{
					output.Write("<br>");
					btEdit.RenderControl(output);
				}

				return;
			}

			#endregion

			// Compile Clip
			Amns.GreyFox.Content.Support.ContentBuilder b = new Amns.GreyFox.Content.Support.ContentBuilder(clip);
			b.Compile();

			this.renderTextTag(output, _titleCssClass, "h3", clip.Title);

			if(clip.Authors.Count != 0)
			{
				output.WriteBeginTag("h4");
				if(_contributorsCssClass != "")
					output.WriteAttribute("class", _contributorsCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				for(int i = 0; i < clip.Authors.Count; i++)
				{
					output.Write(clip.Authors[i].Contact.ConstructName("P F L S"));
					if(i + 1 < clip.Authors.Count)
						output.Write(", ");
				}
				output.WriteEndTag("h4");

				// Output Dates
				this.renderTextTag(output, _publishDateCssClass, "h5", clip.PublishDate.ToString("MMMM yyyy"));
			}

			output.WriteBeginTag("div");
			if(_bodyCssClass != string.Empty)
				output.WriteAttribute("class", _bodyCssClass);
            output.Write(HtmlTextWriter.TagRightChar);

			#region Reference, Administrator and Additional Side Panels

			output.WriteBeginTag("div");
			output.WriteAttribute("style", "float:right;");
			output.Write(HtmlTextWriter.TagRightChar);

			#region Reference Panel

			if(clip.References.Count != 0)
			{				
				output.WriteBeginTag("table");
				output.WriteAttribute("border", "0");
				output.WriteAttribute("cellPadding", "5");
				output.WriteAttribute("cellSpacing", "0");	
				output.WriteAttribute("width", "150px");
				if(_referenceTableCssClass != string.Empty)
					output.WriteAttribute("class", _referenceTableCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteFullBeginTag("tr");
				output.WriteBeginTag("td");
				if(_referenceTitleCssClass != string.Empty)
					output.WriteAttribute("class", _referenceTitleCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write("References");
				output.WriteEndTag("td");
				output.WriteEndTag("tr");
				foreach(DbContentClip reference in clip.References)
				{
					output.WriteFullBeginTag("tr");
					output.WriteFullBeginTag("td");
					output.WriteBeginTag("a");
					if(reference.OverrideUrl != string.Empty)
						output.WriteAttribute("href", reference.OverrideUrl);
					else
						output.WriteAttribute("href", Page.ResolveUrl(string.Format(_linkFormat, reference.ID)));
					if(reference.MenuTooltip != string.Empty)
						output.WriteAttribute("title", reference.MenuTooltip);
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(reference.Title);
					output.WriteEndTag("a");
					output.WriteEndTag("td");
					output.WriteEndTag("tr");
				}
				output.WriteEndTag("table");
			}

			#endregion

			#region Administration Panel

			if(btEdit != null && btEdit.Visible)
			{
				output.WriteBeginTag("table");
				output.WriteAttribute("border", "0");
				output.WriteAttribute("cellPadding", "5");
				output.WriteAttribute("cellSpacing", "0");	
				output.WriteAttribute("width", "150px");
				if(_referenceTableCssClass != string.Empty)
					output.WriteAttribute("class", _referenceTableCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteFullBeginTag("tr");
				output.WriteBeginTag("td");
				if(_referenceTitleCssClass != string.Empty)
					output.WriteAttribute("class", _referenceTitleCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write("Administration");
				output.WriteEndTag("td");
				output.WriteEndTag("tr");
				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");
				btEdit.RenderControl(output);
				output.WriteEndTag("td");
				output.WriteEndTag("tr");

				#region Hit Counter Display

				// TODO: Finish Counter
				int totalHits = 0;
				int uniqueHits = 0;

				DbContentHitManager hm = new DbContentHitManager();
				DbContentHitCollection hits = hm.GetCollection("RequestContentID=" + clip.ID.ToString(), "RequestDate", null);

				totalHits = hits.Count;

				// =================================================
				// BAR GRAPH
				// =================================================

				DateTime now = DateTime.Now;
				
				DateTime weekStart6 = DateManipulator.FirstOfWeek(now);
                DateTime weekStart5 = DateManipulator.FirstOfWeek(now.Subtract(TimeSpan.FromDays(7)));
                DateTime weekStart4 = DateManipulator.FirstOfWeek(now.Subtract(TimeSpan.FromDays(14)));
                DateTime weekStart3 = DateManipulator.FirstOfWeek(now.Subtract(TimeSpan.FromDays(21)));
                DateTime weekStart2 = DateManipulator.FirstOfWeek(now.Subtract(TimeSpan.FromDays(28)));
                DateTime weekStart1 = DateManipulator.FirstOfWeek(now.Subtract(TimeSpan.FromDays(35)));
				
				int[] weekCounts = new int[6];

				// Count Unique Hits
				foreach(DbContentHit hit in hits)
				{
					if(hit.IsUnique)
					{
						if(hit.RequestDate > weekStart1 & hit.RequestDate < weekStart2)
							weekCounts[0]++;
						if(hit.RequestDate > weekStart2 & hit.RequestDate < weekStart3)
							weekCounts[1]++;
						if(hit.RequestDate > weekStart3 & hit.RequestDate < weekStart4)
							weekCounts[2]++;
						if(hit.RequestDate > weekStart4 & hit.RequestDate < weekStart5)
							weekCounts[3]++;
						if(hit.RequestDate > weekStart5 & hit.RequestDate < weekStart6)
							weekCounts[4]++;
						if(hit.RequestDate > weekStart6)
							weekCounts[5]++;

						uniqueHits++;
					}
				}

				int graphWidth = 138;
				int graphHeight = 100;
				int graphPadding = 1;
				int plotWidth = graphWidth - (graphPadding * 2);
				int plotHeight = graphHeight - (graphPadding * 2);
				int maxBarHeight = plotHeight;
				int minBarHeight = 20;
				int barMargin = 2;
				int barWidth = 21;

				// Find Highest Count
				int maxCount = 0;
				for(int i = 0; i < weekCounts.Length; i++)
					if(weekCounts[i] > maxCount)
						maxCount = weekCounts[i];

				// Find Height per Unit
				double pixelUnit = Convert.ToDouble(maxBarHeight) / Convert.ToDouble(maxCount);

				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");
				output.Write("<strong>Hits:</strong>");
				output.WriteEndTag("td");
				output.WriteEndTag("tr");

				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");

				output.WriteFullBeginTag("style");
				output.WriteLine("#vertgraph { width : " + graphWidth.ToString() + "px; " +
					"height: " + graphHeight.ToString() + "px; " +
					"position: relative; " +
					"padding: " + graphPadding.ToString() + "px; " +
					"background: #aaaaaa; }");
				output.WriteLine("#vertgraph .graph-area { width: " + plotWidth.ToString() + "px; " +
					"height: " + plotHeight.ToString() + "px; " +
					"padding: 0; " +
					"margin: 0; " +
					"position: relative; " +
					"overflow: hidden; } ");
				output.WriteLine("#vertgraph ul { width: " + plotWidth.ToString() + "px; " +
					"height: " + plotHeight.ToString() + "px; " +
					"margin: 0; " +
					"padding: 0; " +
					"position: absolute; " +
					"bottom: 0; " +
					"list-style-type: none; }");
				output.Write("#vertgraph ul li { position: absolute; " +
					"width: " + barWidth.ToString() + "px; " +
					"height: " + maxBarHeight.ToString() + "px; " +
					"bottom: 0; " +
					"padding 0; " +
					"margin 0; " +
					"background: no-repeat; " +
					"writing-mode:tb-rl; " +
					"text-align: center; " +
					"font-size: xx-small; " +
					//					"font-weight: bold; " +
					"vertical-align: middle; " +
					"color: white; }");
				output.WriteLine("#vertgraph li.week1 { background: #999999; left: 0px; }");
				output.WriteLine("#vertgraph li.week2 { background: #888888; left: " + ((barMargin + barWidth) * 1).ToString() + "px; }");
				output.WriteLine("#vertgraph li.week3 { background: #777777; left: " + ((barMargin + barWidth) * 2).ToString() + "px; }");
				output.WriteLine("#vertgraph li.week4 { background: #666666; left: " + ((barMargin + barWidth) * 3).ToString() + "px; }");
				output.WriteLine("#vertgraph li.week5 { background: #555555; left: " + ((barMargin + barWidth) * 4).ToString() + "px; }");
				output.WriteLine("#vertgraph li.week6 { background: #444444; left: " + ((barMargin + barWidth) * 5).ToString() + "px; }");
				output.WriteEndTag("style");

				output.WriteBeginTag("div");
				output.WriteAttribute("id", "vertgraph");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("div");
				output.WriteAttribute("id", "graph-area");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteFullBeginTag("ul");

				int barHeight;
				double barHeightD = 0;

				for(int i = 0; i < 6; i++)
				{
					barHeightD = Convert.ToDouble(weekCounts[i]) * pixelUnit;
					if(barHeightD < minBarHeight)
						barHeight = minBarHeight;
					else if(double.IsNaN(barHeightD))
						barHeight = minBarHeight;
					else
						barHeight = Convert.ToInt32(barHeightD);

					output.WriteBeginTag("li");
					output.WriteAttribute("class", "week" + (i + 1).ToString());
					output.WriteAttribute("style", "height: " + barHeight + "px;");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(weekCounts[i]);
					output.WriteEndTag("li");
					output.WriteLine();
				}
				
				output.WriteEndTag("ul");
				output.WriteEndTag("div");
				output.WriteEndTag("div");

				output.WriteEndTag("td");
				output.WriteEndTag("tr");
				// ===============================================
				// END BAR GRAPH
				// ===============================================

				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");
				output.Write("<strong>Unique Hits:</strong><br />");
				output.Write(uniqueHits);
				output.WriteEndTag("td");
				output.WriteEndTag("tr");

				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");
				output.Write("<strong>Total Hits:</strong><br />");
				output.Write(totalHits);
				output.WriteEndTag("td");
				output.WriteEndTag("tr");

				if(hits.Count > 0)
				{
					output.WriteFullBeginTag("tr");
					output.WriteFullBeginTag("td");
					output.Write("<strong>Last Hit:</strong><br />");
					output.Write(hits[hits.Count-1].RequestDate.ToString());
					output.WriteEndTag("td");
					output.WriteEndTag("tr");
				}

				#endregion

				#region Update Display

				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");
				output.Write("<strong>Last Updated:</strong><br />");
				output.Write(clip.ModifyDate.ToString());
				output.WriteEndTag("td");
				output.WriteEndTag("tr");

				#endregion

				#region Admin Notices

				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");

				if(!clip.ParentCatalog.Enabled)
					output.Write(string.Format("<div><font color=\"ff0000\">Admin Notice: The requested clip's parent catalog '{0}' is disabled.</font></div>", clip.ParentCatalog.Title));
				if(clip.ExpirationDate < DateTime.Now)
					output.Write(string.Format("<div><font color=\"ff0000\">Admin Notice: The requested clip expired on {0}.</font></div>", clip.ExpirationDate));
				if(clip.PublishDate > DateTime.Now)
					output.Write(string.Format("<div><font color=\"ff0000\">Admin Notice: The requested clip will be published on {0}.</font></div>", clip.PublishDate));

				output.WriteEndTag("td");
				output.WriteEndTag("tr");

				#endregion

				output.WriteEndTag("table");
			}

			#endregion

			#region Gloss Word Panel

			if(b.FaqCompiled)
			{
				output.Write("\r\n" +
					"<script language=\"javascript\">\r\n" +
					"function gfx_faqToggle(id) {\r\n" +
					"	q = new getObj(id); \r\n" +
					"	if(q.style.display == 'block') \r\n" +
					"		q.style.display = 'none'; \r\n" +
					"	else \r\n" +
					"		q.style.display = 'block'; \r\n" +
					"}\r\n" +
					"</script>\r\n");
			}

			if(b.GlossWordsCompiled)
			{
				output.Write("\r\n" +
					"<script language=\"javascript\">\r\n" +
					"function gfx_glossCopy(id) {\r\n" +
					"	document.getElementById('glosscontainer').innerHTML = \r\n" +
					"		document.getElementById(id).innerHTML;\r\n" +
					"}\r\n" +
					"</script>\r\n");

				output.WriteBeginTag("table");
				output.WriteAttribute("border", "0");
				output.WriteAttribute("cellPadding", "5");
				output.WriteAttribute("cellSpacing", "0");	
				output.WriteAttribute("width", "150px");
				if(_glossTableCssClass != string.Empty)
					output.WriteAttribute("class", _glossTableCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteFullBeginTag("tr");
				output.WriteBeginTag("td");
				if(_glossTitleCssClass != string.Empty)
					output.WriteAttribute("class", _glossTitleCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write("Glossary");
				output.WriteEndTag("td");
				output.WriteEndTag("tr");
				output.WriteFullBeginTag("tr");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("div");
				output.WriteAttribute("id", "glosscontainer");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write("Please hover over a keyword to view glossary content.");
				output.WriteEndTag("div");
				output.Write(b.GlossBlock);
				output.WriteEndTag("td");
				output.WriteEndTag("tr");
				output.WriteEndTag("table");
			}

			#endregion

			output.WriteEndTag("div");

			#endregion

            output.Write(b.Output);

			output.WriteEndTag("div");

			// DO NOT RECORD HITS ON ADMIN VIEW OR IF USER HAS ALREADY
			// HIT THIS PAGE UNIQUELY.
			if(!adminView & _hitCounterEnabled)
			{
				if(Page.Session["GFX_HIT_" + clip.ID.ToString()] == null)
				{
					clip.SaveHit(Page.Request, true);
					Page.Session["GFX_HIT_" + clip.ID.ToString()] = true;
				}
				else
				{
					clip.SaveHit(Page.Request, false);
				}
			}
		}

		private void renderTextTag(HtmlTextWriter output, string cssClass, string defaultCssClass, string text)
		{
			string c;

			if(cssClass != string.Empty)
				c = cssClass;
			else if(defaultCssClass != string.Empty)
				c = defaultCssClass;
			else
				c = "p";

			switch(c)
			{
				case "h1":
					output.WriteFullBeginTag(cssClass);
					output.Write(text);
					output.WriteEndTag(cssClass);
					break;
				case "h2":
					goto case "h1";
				case "h3":
					goto case "h1";
				case "h4":
					goto case "h1";
				case "h5":
					goto case "h1";
				case "h6":
					goto case "h1";
				case "p":
					goto case "p";
				default:
					output.WriteBeginTag("div");
					output.WriteAttribute("class", cssClass);
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(text);
					output.WriteEndTag("div");
					output.WriteLine();
					break;
			}
		}

		#region ViewState

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null) base.LoadViewState(myState[0]);
				if(myState[1] != null) _dbContentClipID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = _dbContentClipID;
			return myState;
		}

		#endregion
	}
}