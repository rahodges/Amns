using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Tablewindow is a powerful window control for Amns.GreyFox.
	/// 
	/// If ViewPanes are enabled remember that the viewpane renderer can only access properties set on the control
	/// before the RenderViewPane method is executed. Be sure that database connection strings are set properly
	/// before this is fired. RenderViewPane fires during OnInit, don't forget.
	/// </summary>
	[DefaultProperty("Text"),
	ToolboxData("<{0}:TableWindow runat=server></{0}:TableWindow>"),
	Designer(typeof(Amns.GreyFox.Web.UI.WebControls.Design.TableWindowDesigner))]
	public class TableWindow : GreyFoxWebControl, INamingContainer, IPostBackEventHandler
	{
		protected TableWindowComponents components = 0x00;
		protected TableWindowFeatures features = 0x00;
		protected TableWindowLayoutMode layoutMode = TableWindowLayoutMode.Tables;	
		protected int columnCount = 1;		
		protected string text = string.Empty;

		// Toolbar Data (Components 0x01)
        //protected Toolbar controlToolbar = new Toolbar("Controls");
        //protected ToolbarList toolbars;
        //protected ToolbarStyle toolbarStyle = ToolbarStyle.Office2003;

        protected List<ToolBar> toolbars;
        protected ToolBar toolBar;

		// Tab Data (Components 0x04)
		protected TabStrip tabStrip;
		protected TabStripStyle tabStripStyle = TabStripStyle.Default;
		protected string tabCssClass = string.Empty;

		// Scroller Data (Features 0x02)
		bool scrollEnabled = true;
		int scrollTop = 0;
		int scrollLeft = 0;
		Unit scrollHeight = Unit.Percentage(100);
		string scrollDownIconUrl = string.Empty;	// "/themes/scrolldown.gif";
		string scrollUpIconUrl = string.Empty;		// "/themes/scrollup.gif";

		// Printer Data (Features 0x10)
		string secureBlankPage = "blank.htm";		// This page must have NO STYLESHEETS!

		// ViewPane Data (Components 0x??)
		bool contentPaneEnabled	= true;
		bool viewPaneEnabled = true;
		string viewPaneUrl = string.Empty;

		// Appearance
		string cssClass	= string.Empty;
		string headerCssClass = string.Empty;
		string subHeaderCssClass = string.Empty;
		string contentCssClass = string.Empty;
		string viewPaneCssClass = string.Empty;
		string printerCssClass = string.Empty;

		// Layout
		Unit cellPadding						= Unit.Empty;
		Unit cellSpacing						= Unit.Empty;
		Unit borderWidth						= Unit.Empty;
		Unit width								= Unit.Empty;
		Unit height								= Unit.Empty;	
		Unit contentWidth						= Unit.Percentage(50);		// Only for viewpanes
		Unit contentHeight						= Unit.Pixel(450);          // IE
		short tabIndex							= -1;						// Unspecified
	
		TableWindowViewPane viewPane			= null;

		#region Behavior Properties
		
		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public short TabIndex
		{
			get { return tabIndex; }
			set { tabIndex = value; }
		}

		/// <summary>
		/// Specifies the URL to use for SSL requests. Be sure that the
		/// HTML page specified does not have any stylesheets embedded or
		/// the window print feature may not print correctly.
		/// </summary>
		[Bindable(true), Category("Behavior"),  DefaultValue("blank.htm")]
		public string SecureBlankPage
		{
			get { return secureBlankPage; }
			set { secureBlankPage = value; }
		}

		#endregion

		#region Appearance Properties

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string CssClass
		{
			get { return cssClass; }
			set { cssClass = value; }
		}
		
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string HeaderCssClass
		{
			get { return headerCssClass; }
			set { headerCssClass = value; }
		}
		
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ContentCssClass
		{
			get { return contentCssClass; }
			set { contentCssClass = value; }
		}
		
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ViewPaneCssClass
		{
			get { return viewPaneCssClass; }
			set { viewPaneCssClass = value; }
		}

		[Bindable(true), Category("Appearance"),  DefaultValue("")]
		public string SubHeaderCssClass
		{
			get { return subHeaderCssClass; }
			set { subHeaderCssClass = value; }
		}

		#endregion

		#region Layout Properties

		[Bindable(true), Category("Layout"), DefaultValue("1")] 
		public Unit ColumnCount
		{
			get { return columnCount; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("")] 
		public Unit CellPadding
		{
			get { return cellPadding; }
			set { cellPadding = value; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("")] 
		public Unit CellSpacing
		{
			get { return cellSpacing; }
			set { cellSpacing = value; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("")] 
		public Unit BorderWidth
		{
			get { return borderWidth; }
			set { borderWidth = value; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("")] 
		public Unit Width
		{
			get { return width; }
			set { width = value; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("")] 
		public Unit Height
		{
			get { return height; }
			set { height = value; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("300px")] 
		public Unit ContentWidth
		{
			get { return contentWidth; }
			set { contentWidth = value; }
		}

		[Bindable(true), Category("Layout"), DefaultValue("300px")] 
		public Unit ContentHeight
		{
			get { return contentHeight; }
			set { contentHeight = value; }
		}

		#endregion

		#region Printer Properties

		[Bindable(true), Category("Printing"), DefaultValue("")]
		public string PrinterCssClass
		{
			get { return printerCssClass; }
			set { printerCssClass = value; }
		}

		#endregion

		#region View Pane Properties

		[Bindable(true), Category("View Pane"), DefaultValue("")]
		public string ViewPaneUrl
		{
			get { return viewPaneUrl; }
			set { viewPaneUrl = value; }
		}

		#endregion

		#region Scroller Properties

		[Bindable(true), Category("Scroller"), DefaultValue(0),
		Browsable(false)]
		public int ScrollTop
		{
			get { return scrollTop; }
			set { scrollTop = value; }
		}

		[Bindable(true), Category("Scroller"), DefaultValue(0), Browsable(false)]
		public int ScrollLeft
		{
			get { return scrollLeft; }
			set { scrollLeft = value; }
		}

		[Bindable(true), Category("Scroller"), DefaultValue("100%")]
		public Unit ScrollHeight
		{
			get { return scrollHeight; }
			set { scrollHeight = value; }
		}

		[Bindable(true), Category("Scroller"), DefaultValue(true)]
		public bool ScrollEnabled
		{
			get { return scrollEnabled; }
			set { scrollEnabled = value; }
		}

		#endregion

		public TableWindowViewPane ViewPane 
		{
			get 
			{ 
				return viewPane; 
			}
			set 
			{
				viewPane = value;
				
				if(viewPane.__parentWindow != null)
					throw(new Exception("Viewpane is already assigned to '" +
						this.ID + "; cannot be assigned to more than one window."));

				viewPane.__parentWindow = this;
			}
		}

		public string JavascriptObject
		{
			get { return "tableGrid" + ID; }
		}

        public bool FeatureCheck(TableWindowFeatures feature)
		{
			return (features & feature) != 0;
		}

		public bool ComponentCheck(TableWindowComponents component)
		{
			return (components & component) != 0;
		}

		/// <summary>
		/// Registers a hidden firstName making the name and id the same for cross browswer compatible scripts.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void RegisterHiddenField(string name, string value)
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), name + "_" + value, 
				"<input type=\"hidden\" name=\"" + name + "\" id=\"" + name + "\" value=\"" + value + "\" />");
		}

		#region Clipboard Copier

		private void registerClipboardCopier()
		{
			RegisterClientScript("TableWindow.js");
			RegisterClientScript("TableSort.js");


		}

		#endregion

		#region Scroller Scripts

		private void registerScroller()
		{
			RegisterClientScript("TableWindow.js");

			try
			{
				scrollTop = int.Parse(Context.Request.Form["__" + ID + "ScrollTop"]);
				scrollLeft = int.Parse(Context.Request.Form["__" + ID + "ScrollLeft"]);
			}
			catch
			{
			}
				
			RegisterHiddenField("__" + ID + "ScrollTop", scrollTop.ToString());
			RegisterHiddenField("__" + ID + "ScrollLeft", scrollLeft.ToString());
				
			// WARNING... the true false fields controlling the scroller state
			// are case sensitive!

			if(Page.IsPostBack)
				scrollEnabled = Page.Request.Form["__" + ID + "ScrollState"] == "True";
			RegisterHiddenField("__" + ID + "ScrollState", scrollEnabled.ToString());

            
		}

		#endregion

		#region Draggable Layer Scripts
		
		private void registerDragger()
		{
			throw new NotImplementedException("TableWindow Dragger feature not implemented in this version.");

			// TODO: Finish the dragger!

			//			try
			//			{
			//				posX = int.Parse(Context.Request.Form["___" + ID + "X"]);
			//				posY = int.Parse(Context.Request.Form["___" + ID + "Y"]);				
			//			}
			//			catch
			//			{
			//			}
			//
			//			Page.ClientScript.RegisterHiddenField("___" + ID + "X", scrollTop.ToString());
			//			Page.ClientScript.RegisterHiddenField("___" + ID + "Y", scrollTop.ToString());
		}

		#endregion

		#region WindowPrinter Scripts

		/* The WindowPrinter (r) is a special script that opens a browser window that contains only
		 * the controls's contents. Javascript copies the window's contents so that only the contents
		 * will print. */

		private void registerWindowPrinter()
		{
			RegisterClientScript("TableWindow.js");

			// Skip script block registering if the block is already registered
			if(Page.ClientScript.IsClientScriptBlockRegistered("WindowPrinter"))
				return;

			// IE Behavior fix on SSL Pages
			// SRC Attribute has to be set to a dummy page to avoid 
			// "Page displays both secure and nonsecure items..." modal dialog
			if(Page.Request.IsSecureConnection)
			{
				Page.ClientScript.RegisterStartupScript(this.GetType(), "WindowPrinterFrames", "\r\n" +
#if DEBUG
					"<!-- ****************************************** --> \r\n" +
					"<!-- * GreyFox Window Printer Frames v1.0     * --> \r\n" +
					"<!-- * BugFixed + Secure                      * --> \r\n" +
					"<!-- ****************************************** --> \r\n" +
					"\r\n" +
#endif
					"\r\n<iframe id=\"wprintframe\" style=\"Z-INDEX:102;LEFT:1px;WIDTH:1px;POSITION:absolute;TOP:1px;HEIGHT:1px;\" " +
					"name=\"wprintframe\" frameBorder=\"no\" scrolling=\"no\" src=\"" + this.secureBlankPage + "\">" +
					"</iframe>\r\n" +
					"<iframe id=\"bprintframe\" style=\"Z-INDEX:102;LEFT:1px;WIDTH:1px;POSITION:absolute;TOP:1px;HEIGHT:1px;\" " +
					"name=\"bprintframe\" frameBorder=\"no\" scrolling=\"no\" src=\"" + this.secureBlankPage + "\">" +
					"</iframe>");
			}
			else
			{
				Page.ClientScript.RegisterStartupScript(this.GetType(), "WindowPrinterFrames", "\r\n" +
#if DEBUG
					"<!-- ****************************************** --> \r\n" +
					"<!-- * GreyFox Window Printer Frames v1.0     * --> \r\n" +
					"<!-- * BugFixed                               * --> \r\n" +
					"<!-- ****************************************** --> \r\n" +
#endif
					"\r\n<iframe id=\"wprintframe\" style=\"Z-INDEX:102;LEFT:1px;WIDTH:1px;POSITION:absolute;TOP:1px;HEIGHT:1px;\" " +
					"name=\"wprintframe\" frameBorder=\"no\" scrolling=\"no\">" +
					"</iframe>\r\n" +
					"<iframe id=\"bprintframe\" style=\"Z-INDEX:102;LEFT:1px;WIDTH:1px;POSITION:absolute;TOP:1px;HEIGHT:1px;\" " +
					"name=\"bprintframe\" frameBorder=\"no\" scrolling=\"no\">" +
					"</iframe>");
			}
		}

		#endregion

		#region ViewPane Scripts

		protected virtual void ProcessViewPanePostback()
		{
			// For Designer Support
			if(System.Web.HttpContext.Current == null) return;

			if(ComponentCheck(TableWindowComponents.ViewPane))
			{
				System.Web.HttpContext current = System.Web.HttpContext.Current;
				if(current.Request.QueryString["__gfxViewPane_" + this.ID] != null)
				{
					current.Response.ClearContent();
					System.IO.TextWriter tw = new System.IO.StreamWriter(current.Response.OutputStream);
					HtmlTextWriter output = new HtmlTextWriter(tw);
					InitializeRenderHelpers(output);
					output.WriteFullBeginTag("html");
					output.WriteFullBeginTag("head");
					output.WriteEndTag("head");
					output.WriteBeginTag("body");
					output.Write(HtmlTextWriter.TagRightChar);
				
					try
					{
						RenderViewPane(output);
					}
					catch(Exception e)
					{
						RenderTableBeginTag("ErrorPane", Unit.Pixel(5), Unit.Pixel(0), Unit.Pixel(1), Unit.Percentage(100), this.CssClass);
						output.WriteBeginTag("th");
						output.WriteAttribute("class", this.HeaderCssClass);
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write("View Pane Error");
						output.WriteEndTag("th");
						RenderFullRow(e.Message, this.SubHeaderCssClass);
						output.WriteFullBeginTag("tr");
						output.WriteBeginTag("td");
						output.WriteAttribute("valign", "top");
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write(e.StackTrace + "<br />" + e.Source);
						output.WriteEndTag("td");
						output.WriteEndTag("tr");
						output.WriteEndTag("table");
					}

					output.WriteEndTag("body");
					output.WriteEndTag("html");
					output.Flush();

					current.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
					current.Response.Cache.SetExpires(DateTime.Now.Subtract(TimeSpan.FromDays(2)));
					current.Response.Cache.SetNoStore();
					current.Response.Cache.SetAllowResponseInBrowserHistory(false);
					current.Response.End();
				}
			}
		}

		/// <summary>
		/// Registers the viewpane scripts with the webbrowser and initializes the controls used in the
		/// viewpane.
		/// </summary>
		private void registerViewPane()
		{
			if(Page.IsPostBack)
			{
				viewPaneEnabled = Page.Request.Form["__" + ID + "_VP"] == "True";
				contentPaneEnabled = Page.Request.Form["__" + ID + "_CP"] == "True";
			}

			RegisterHiddenField("__" + ID + "_VP", viewPaneEnabled.ToString());
			RegisterHiddenField("__" + ID + "_CP", contentPaneEnabled.ToString());
		}
	
		#endregion

		#region Tabstrip Methods

		private void registerTabStrip()
		{
			if(tabStrip != null)
			{
				tabStripStyle.RegisterClientScriptBlock(this.Page);
				tabStrip.RegisterClientScriptBlock(this);
			}
		}

		protected void RenderTabTableStart(HtmlTextWriter output)
		{
			output.WriteBeginTag("table");
			if(CssClass != "")
				output.WriteAttribute("class", tabCssClass);
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(!BorderWidth.IsEmpty)
				output.WriteAttribute("border", BorderWidth.ToString());
			if(!Width.IsEmpty)
				output.WriteAttribute("width", Width.ToString());
			if(Height == Unit.Percentage(100))
				output.WriteAttribute("height", "100%");
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
		}

		protected void RenderTabTableEnd(HtmlTextWriter output)
		{
			output.Indent--;
			output.WriteEndTag("table");
			output.WriteLine();
		}

		protected void RenderTabPanels(HtmlTextWriter output)
		{
			TabStripRenderer r = new TabStripRenderer(this, output);
			r.RenderDivTableStart += new TabRenderHandler(this.RenderTabTableStart);
			r.RenderDivTableEnd += new TabRenderHandler(this.RenderTabTableEnd);

			output.WriteFullBeginTag("tr");
			output.WriteLine();

			output.Indent++;
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan", "2");
			output.WriteAttribute("valign", "top");
			if(height == Unit.Percentage(100))
				output.WriteAttribute("height", "100%");
			if(contentCssClass != string.Empty)
				output.WriteAttribute("class", contentCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			
			output.Indent++;
			for(int x = 0; x < tabStrip.Tabs.Count; x++)	
				tabStrip.Tabs[x].RenderPanel(r);
			
			output.Indent--;
			output.WriteEndTag("td");

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();
		}

		#endregion

		#region Toolbar Methods

        //private void registerToolbar()
        //{
        //    toolbarStyle.RegisterClientScriptBlock(this.Page);
			
        //    if(toolbars != null)
        //        toolbars[0].RegisterClientScriptBlock(this);
        //    else
        //        controlToolbar.RegisterClientScriptBlock(this);
        //}

		#endregion

		#region TableWindowButton Events

        //public ToolbarEventHandler ToolbarClicked;
        //protected virtual void OnToolbarClicked(ToolbarEventArgs e)
        //{
        //    if(ToolbarClicked != null)
        //        ToolbarClicked(this, e);
        //}

		#endregion

		#region Postback Handler

        public virtual void RaisePostBackEvent(string eventArgument)
        {
            // Search toolbars for a command that matches the event argument
            //if (toolbars != null)
            //{
            //    foreach (Toolbar toolbar in toolbars)
            //    {
            //        foreach (object toolbarItem in toolbar.Items)
            //        {
            //            ToolbarItem i = (ToolbarItem)toolbarItem;

            //            if (eventArgument == i.Command)
            //            {
            //                ToolbarEventArgs e = new ToolbarEventArgs(i);
            //                OnToolbarClicked(e);
            //                ProcessPostBackEvent(eventArgument);
            //                return;
            //            }
            //        }
            //    }
            //}

            ProcessPostBackEvent(eventArgument);
        }

        public virtual void ProcessPostBackEvent(string eventArgument) { }

        public virtual void ProcessClientSideSelection(string selection) { }

		#endregion

		#region OnLoad

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            toolbars = new List<ToolBar>();

            if (FeatureCheck(TableWindowFeatures.ClipboardCopier)) 
                registerClipboardCopier();
            if (FeatureCheck(TableWindowFeatures.Scroller)) 
                registerScroller();
            if (FeatureCheck(TableWindowFeatures.Dragger)) 
                registerDragger();
            if (FeatureCheck(TableWindowFeatures.WindowPrinter)) 
                registerWindowPrinter();
            //if(ComponentCheck(TableWindowComponents.Toolbar))		registerToolbar();		
            if (ComponentCheck(TableWindowComponents.Tabs)) 
                registerTabStrip();
            if (ComponentCheck(TableWindowComponents.ViewPane)) 
                registerViewPane();
        }

        protected virtual void CreateToolBarControls()
        {
            if (FeatureCheck(TableWindowFeatures.ClipboardCopier))
            {
                ToolBarItem copy = ToolBarUtility.Copy();
                if ((features & TableWindowFeatures.Scroller) != 0)
                    copy.ClientSideCommand = "javascript:gfx_clipcopy('" + this.ID + "_ContentDiv" + "');";
                else
                    copy.ClientSideCommand = "javascript:gfx_clipcopy('" + this.ID + "');";
                toolBar.Items.Add(copy);
            }
            if (FeatureCheck(TableWindowFeatures.Scroller))
            {
                ToolBarItem expand = ToolBarUtility.Expand();
                expand.ClientSideCommand = "javascript:gfx_ToggleView('" + ID + "','" + height.ToString() + "');";
                toolBar.Items.Add(expand);
            }
            if (ComponentCheck(TableWindowComponents.ViewPane))
            {
                ToolBarItem viewPane = ToolBarUtility.ViewPane();
                viewPane.ClientSideCommand = this.JavascriptObject + ".toggleViewPane()";
                toolBar.Items.Add(viewPane);
            }
            if (FeatureCheck(TableWindowFeatures.WindowPrinter))
            {
                toolBar.Items.Add(ToolBarUtility.CommandItem("print",
                    Localization.Strings.Print, "print.gif", this.JavascriptObject + ".print();"));
            }
        }

		protected override void OnLoad(EventArgs e)
		{
            base.OnLoad(e);			
		}

		#endregion

		#region TabStrip

		protected virtual void RenderTabStrip(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;

			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.SubHeaderCssClass);
			output.WriteAttribute("colspan", this.columnCount.ToString());
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
			
			if(tabStrip != null)
			{
				TabStripRenderer r = new TabStripRenderer(this, output);
				r.Style = this.tabStripStyle;

				tabStrip.Render(r);
			}

			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();
		}

		#endregion

		#region Toolbar

		protected virtual void RenderToolbar(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			output.WriteLine();

			output.Indent++;
			output.WriteBeginTag("td");
			if(this.columnCount != 1)
				output.WriteAttribute("colspan", this.columnCount.ToString());
			if(this.SubHeaderCssClass != string.Empty)
				output.WriteAttribute("class", this.SubHeaderCssClass);
			output.WriteAttribute("height", "26px");			
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

            //if (toolbar != null)
            //{
            if (toolbars != null)
            {
                foreach (ToolBar toolbar in toolbars)
                {
                    toolbar.RenderControl(output);
                }
            }
            //}
            //else
            //{
            //    if (toolbars != null)
            //    {
            //        output.WriteBeginTag("div");
            //        output.WriteAttribute("id", ID + "_ToolbarDIV");
            //        output.WriteLine(HtmlTextWriter.TagRightChar);
            //        output.Indent++;

            //        ToolbarRenderer r = new ToolbarRenderer(this, output);
            //        r.Style = toolbarStyle;

            //        for (int i = 0; i < toolbars.Count; i++)
            //            toolbars[i].Render(r);

            //        output.Indent--;
            //        output.WriteEndTag("div");
            //        output.WriteLine();
            //    }
            //}

			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();
		}

		#endregion

		public virtual void RegisterClientObject()
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "gfx_TableGrid_" + ID,
				"<script language=\"javascript\">\r\n" +
				"<!--\r\n" +
				"var " + this.JavascriptObject + " = new gfx_TableGridObject('" + ID + "'," +
				"'" + this.ContentWidth.ToString() + "'," +
				FeatureCheck(TableWindowFeatures.ClientSideSelector).ToString().ToLower() + "," +
				ComponentCheck(TableWindowComponents.ViewPane).ToString().ToLower() + "," +
				FeatureCheck(TableWindowFeatures.Scroller).ToString().ToLower() + ");" +
				this.JavascriptObject + ".printCssClass = '" + this.PrinterCssClass + "';\r\n" +				
				this.JavascriptObject + ".bind();\r\n" +
				"// -->\r\n" +
				"</script>\r\n");
		}

		#region  Render Methods

		#region Render

		/// <summary>
		/// Renders the grid.
		/// </summary>
		/// <param name="output">The HTML writer to write out to.</param>
		protected override void Render(HtmlTextWriter output)
		{
			// Set protected variable output for helper functions
			this.InitializeRenderHelpers(output);                       

			#region Control Comments
#if DEBUG
			string featureString = string.Empty;
			if(ComponentCheck(TableWindowComponents.Toolbar))
				featureString += " Toolbar";
			if(ComponentCheck(TableWindowComponents.ViewPane))
				featureString += " ViewPane";
			if(FeatureCheck(TableWindowFeatures.DisableContentSeparation))
				featureString += " DCS";
			if(FeatureCheck(TableWindowFeatures.ClientSideSelector))
				featureString += " ClientSelect";
			if(FeatureCheck(TableWindowFeatures.WindowPrinter))
				featureString += " Print";
			if(FeatureCheck(TableWindowFeatures.Scroller))
				featureString += " Scroll";


			// Start Window Output Notice
			output.WriteLine();
			output.WriteLine("<!-- ******************************************************* -->");
			output.WriteLine("<!-- * GreyFox TableWindow v1.0                            * -->");
			output.WriteLine("<!-- * Window: " + this.ID.PadRight(43, ' ') +     " * -->");
			output.WriteLine("<!-- *" + featureString.PadRight(52, ' ') +              " * -->");
			output.WriteLine("<!-- ******************************************************* -->");
#endif
			#endregion

			// Window Div ======================================================
			output.WriteLine();
			output.WriteBeginTag("div");
			output.WriteAttribute("id", ID + "_Window");
			if(!Width.IsEmpty | !Height.IsEmpty)
			{
				output.Write(" style=\"");
				if(!Width.IsEmpty)
					output.WriteStyleAttribute("width", Width.ToString());
				if(!Height.IsEmpty)
					output.WriteStyleAttribute("height", Height.ToString());
				output.Write("\"");
			}
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

			// Detect and execute layout mode ==================================
			if(this.layoutMode == TableWindowLayoutMode.Divs)
				renderDivLayout(output);
			else
				renderTableLayout(output);

			// WINDOW PRINTER BORDER DIV END TAG ===============================
			output.Indent--;
			output.WriteEndTag("div");
			output.WriteLine();

			if(FeatureCheck(TableWindowFeatures.ClientSideSelector) |
				FeatureCheck(TableWindowFeatures.Scroller) |
				ComponentCheck(TableWindowComponents.ViewPane))
			{
				RegisterClientObject();
			}
		}

		/// <summary>
		/// Renders the grid with DIV tags
		/// </summary>
		/// <param name="output">The HTML writer to write out to.</param>
		private void renderDivLayout(HtmlTextWriter output)
		{
			output.WriteBeginTag("div");
			output.WriteAttribute("class", CssClass);
			if(!Height.IsEmpty)								// A table window MUST have a height if there is a scroller
			{
				output.WriteAttribute("style", "height:" + height.ToString() + ";");
				output.WriteAttribute("height", Height.ToString());
			}	
			// This identifies a table as a tablewindow for script functions
			output.WriteAttribute("isRootTableWindow", "true");
			output.WriteLine(HtmlTextWriter.TagRightChar);
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output">The HTML writer to write out to.</param>
		private void renderTableLayout(HtmlTextWriter output)
		{			
			// TABLE DEFINE TAG =======================================
			output.WriteBeginTag("table");
			output.WriteAttribute("id", ID);
			if(CssClass != "")
				output.WriteAttribute("class", CssClass);
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(!BorderWidth.IsEmpty)
				output.WriteAttribute("border", BorderWidth.ToString());
			if(!Width.IsEmpty)
				output.WriteAttribute("width", Width.ToString());
			if(!Height.IsEmpty)			// A table window MUST have a height if there is a scroller
			{
				output.WriteAttribute("style", "height:" + height.ToString() + ";");
				output.WriteAttribute("height", Height.ToString());
			}			
			// This identifies a table as a tablewindow for script functions
			output.WriteAttribute("isRootTableWindow", "true");
			output.WriteLine(HtmlTextWriter.TagRightChar);

			// Indent to begin table rows
			output.Indent++;

			#region Header Text

			// RENDER TEXT HEADER=======================================
			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;

			output.WriteBeginTag("th");
			if(columnCount != 1) 
				output.WriteAttribute("colspan", columnCount.ToString());
			if(headerCssClass != string.Empty) 
				output.WriteAttribute("class", headerCssClass);
			output.WriteLine(HtmlTextWriter.TagRightChar);
            output.Indent++;
			output.Write(text);
			output.WriteLine();
			
			output.Indent--;
			output.WriteEndTag("th");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();

			#endregion

			// RENDER TOOLBAR ==========================================
			if((components & TableWindowComponents.Toolbar) != 0)
				RenderToolbar(output);

			// RENDER TABS =============================================
			if((components & TableWindowComponents.Tabs) != 0)
				RenderTabStrip(output);

			// RENDER CONTENT HEADER ===================================
			if((components & TableWindowComponents.ContentHeader) != 0)
				RenderContentHeader(output);
			
			// RENDER CONTENTS OF TABLE ================================
			if((features & TableWindowFeatures.Scroller) != 0)
			{
				output.WriteFullBeginTag("tr");
				output.WriteLine();
				output.Indent++;

				output.WriteBeginTag("td");
				if(contentCssClass != string.Empty)
					output.WriteAttribute("class", contentCssClass);
				if(this.columnCount != 1)
					output.WriteAttribute("colspan", columnCount.ToString());
				output.WriteAttribute("id", ID + "_ScrollTD");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("height", this.ContentHeight.ToString());
				output.WriteAttribute("width", "100%");
				// output.WriteAttribute("style", "padding:0px");
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent++;

				if(layoutMode == TableWindowLayoutMode.TableTabs)
				{
					renderTableLayoutContent(output);
				}
				else
				{
					renderTableLayoutContent(output);
				}

				output.Indent--;
				output.WriteEndTag("td");
				output.WriteLine();

				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
				
			}
			else if((features & TableWindowFeatures.DisableContentSeparation) != 0)
			{
				RenderContent(output);
			}
			else
			{
                output.WriteFullBeginTag("tr");
				output.WriteLine();
			
				output.Indent++;
				output.WriteBeginTag("td");
				if(this.columnCount != 1)
					output.WriteAttribute("colspan", columnCount.ToString());
				if(contentCssClass != string.Empty)
					output.WriteAttribute("class", contentCssClass);
				if(!height.IsEmpty)
				{
					output.WriteAttribute("height", "100%");
					output.WriteAttribute("valign", "top");		// must be top aligned for windows
				}
				output.Write(HtmlTextWriter.TagRightChar);		// with custom heights.
				output.WriteLine();
			
				output.Indent++;
				RenderContent(output);
			
				output.Indent--;
				output.WriteEndTag("td");
				output.WriteLine();

				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}

			if((components & TableWindowComponents.Footer) != 0)
				RenderFooter(output);

			// TABLE END TAG ===========================================
			output.Indent--;
			output.WriteEndTag("table");
			output.WriteLine();
		}

		public void renderTableLayoutContent(HtmlTextWriter output)
		{
			#region Content DIV
 
			output.WriteBeginTag("div");
			output.WriteAttribute("id", ID + "_ContentDiv");
			output.Write("style=\"");

			#region Content DIV Styles

			// Set ViewPane settings for DIV
			if(ComponentCheck(TableWindowComponents.ViewPane))
			{
				// Float the content pane to the left hand side of window
				output.WriteStyleAttribute("float", "left");

				if(contentPaneEnabled & viewPaneEnabled)
					output.WriteStyleAttribute("width", ContentWidth.ToString());
				else if(contentPaneEnabled & !viewPaneEnabled)
					output.WriteStyleAttribute("width", "100%");
				else
					output.WriteStyleAttribute("width", "0px");
			}
			else
			{
				output.WriteStyleAttribute("width", "100%");
			}

			// Set Scroll Settings for DIV
			if(scrollEnabled)
				output.WriteStyleAttribute("overflow", "scroll");
			else
				output.WriteStyleAttribute("overflow", "visible");

			// Set Height
			output.WriteStyleAttribute("height", this.ContentHeight.ToString());
			output.Write("\"");

			#endregion

			output.WriteAttribute("class", this.contentCssClass);
			output.WriteAttribute("onscroll", this.JavascriptObject + ".scroll();");
			output.WriteLine(HtmlTextWriter.TagRightChar);				
			output.Indent++;

			output.WriteBeginTag("table");
			output.WriteAttribute("id", ID + "_datatable");
			output.WriteAttribute("cellpadding", cellPadding.ToString());
			output.WriteAttribute("cellspacing", cellSpacing.ToString());
			output.WriteAttribute("class", contentCssClass);
			output.WriteAttribute("width", "100%");
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

			output.WriteFullBeginTag("thead");
			output.WriteLine();
			output.Indent++;
			RenderContentHeader(output);
			output.Indent--;
			output.WriteEndTag("thead");
			output.WriteLine();

			output.WriteFullBeginTag("tbody");
			output.WriteLine();
			output.Indent++;
			RenderContent(output);
			output.Indent--;
			output.WriteEndTag("tbody");
			output.WriteLine();			
				
			output.Indent--;
			output.WriteEndTag("table");
			output.WriteLine();
				
			output.Indent--;
			output.WriteEndTag("div");
			output.WriteLine();

			#endregion

			#region ViewPane DIV

			// Render ViewPane DIV
			if(ComponentCheck(TableWindowComponents.ViewPane))
			{
				output.WriteBeginTag("div");
				output.WriteAttribute("id", "__gfxViewPane_" + this.ID);
				if(viewPaneEnabled)
				{
					output.WriteAttribute("style", "overflow:scroll;width:100%;" +
						//"visibility:visible;" +
						"height=" + this.contentHeight.ToString() + ";");
				}
				else
				{
					output.WriteAttribute("style", "overflow:scroll;width=100%;" + 
						//"visibility:hidden;" +
						"height=" + this.contentHeight.ToString() + ";");
				}
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("div");
				output.WriteLine();
			}

			#endregion
		}

		#endregion

 		#region Render Stub Methods

		protected virtual void RenderContentHeader(HtmlTextWriter output) { }

		protected virtual void RenderContent(HtmlTextWriter output) { }

		/// <summary>
		/// Renders a specific footer row to the grid. This method is not encapsulated by tr's and td's.
		/// </summary>
		/// <param name="output"></param>
		protected virtual void RenderFooter(HtmlTextWriter output) { }

		protected virtual void RenderViewPane(HtmlTextWriter output) 
		{ 
			if(viewPane != null)
				viewPane.Render(output);
		}

		#endregion

		#endregion

		#region Table Rendering Helper Methods

		protected void RenderFullRow(string text, string cssClass)
		{
			__output.WriteFullBeginTag("tr");
			__output.WriteLine();
			__output.Indent++;
			__output.WriteBeginTag("td");
			__output.WriteAttribute("colspan", columnCount.ToString());
			__output.WriteAttribute("class", cssClass);
			__output.Write(HtmlTextWriter.TagRightChar);
			if(text == "")
				__output.Write("&nbsp;");
			else
                __output.Write(text);
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
			__output.WriteEndTag("tr");
			__output.WriteLine();
		}

		protected void RenderPropertyRows(string col1CssClass, string col2CssClass, string[] labels, Control[] controls)
		{
			if(labels.GetUpperBound(0) != controls.GetUpperBound(0))
				throw(new Exception("Cannot render cell pairs, labels array dimension does not match controls array dimension."));

			for(int x = 0; x <= labels.GetUpperBound(0); x++)
			{
				__output.WriteFullBeginTag("tr");
				__output.WriteLine();

				__output.Indent++;
				__output.WriteBeginTag("td");
				__output.WriteAttribute("class", col1CssClass);
				__output.Write(HtmlTextWriter.TagRightChar);
				if(labels[x] == "")
					__output.Write("&nbsp;");
				else
                    __output.Write(labels[x]);
				__output.WriteEndTag("td");
				__output.WriteLine();
				
				__output.WriteBeginTag("td");
				__output.WriteAttribute("class", col2CssClass);
				if(columnCount > 2)
					__output.WriteAttribute("colspan", (columnCount - 1).ToString());
				__output.Write(HtmlTextWriter.TagRightChar);
				controls[x].RenderControl(__output);
				__output.WriteEndTag("td");
				__output.WriteLine();
				__output.Indent--;
				
				__output.WriteEndTag("tr");
				__output.WriteLine();
			}
		}

		protected void RenderPropertyRows(int pairCount, string col1CssClass, string col2CssClass, string[] labels, Control[] controls)
		{
			if(labels.GetUpperBound(0) != controls.GetUpperBound(0))
				throw(new Exception("Cannot render cell pairs, labels array dimension does not match controls array dimension."));

			int x = 0;
			while(true)
			{
				__output.WriteFullBeginTag("tr");
				__output.WriteLine();

				for(int y = 0; y < pairCount; y++)
				{
					//
					// Render Label
					//
					__output.Indent++;
					__output.WriteBeginTag("td");
					__output.WriteAttribute("class", col1CssClass);
					__output.Write(HtmlTextWriter.TagRightChar);
					if(x > labels.GetUpperBound(0))
						__output.Write("&nbsp;");
					else
					{
						if(labels[x] == "")
							__output.Write("&nbsp;");
						else
							__output.Write(labels[x]);
					}
					__output.WriteEndTag("td");
					__output.WriteLine();
				
					//
					// Render Control
					//
					__output.WriteBeginTag("td");
					__output.WriteAttribute("class", col2CssClass);
					__output.Write(HtmlTextWriter.TagRightChar);
					if(x > labels.GetUpperBound(0))
						__output.Write("&nbsp;");
					else
						if(controls[x] != null)
						controls[x].RenderControl(__output);
					__output.WriteEndTag("td");
					__output.WriteLine();
					__output.Indent--;

					x++;
				}
				
				__output.WriteEndTag("tr");
				__output.WriteLine();

				if(x > labels.GetUpperBound(0))
					break;
			}
		}

		#endregion

		#region Designer Helper

		internal string __designInit()
		{
			try
			{
				this.OnInit(EventArgs.Empty);
				return string.Empty;
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}

		#endregion
	}
}