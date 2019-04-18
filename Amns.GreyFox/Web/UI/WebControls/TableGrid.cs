using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for MemberListGrid.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:TableGrid runat=server></{0}:TableGrid>"),
		Designer(typeof(Amns.GreyFox.Web.UI.WebControls.Design.TableWindowDesigner))]
	public class TableGrid : TableWindow  
	{
		#region constants

//		private const string selectionSuffix		= "_sid";

		#endregion

		protected int selectedID					= -1;
		protected bool headerLockEnabled			= false;
		protected bool columnLockEnabled			= false;
		protected bool headerSortEnabled			= false;

		protected bool selectEnabled				= true;
		protected bool addEnabled					= false;
		protected bool deleteEnabled				= false;
		protected bool editEnabled					= false;

		protected string headerRowCssClass			= string.Empty;
		protected string defaultRowCssClass			= string.Empty;
		protected string alternateRowCssClass		= string.Empty;
		protected string selectedRowCssClass		= string.Empty;
		protected string indexRowCssClass			= string.Empty;

        protected ToolBarItem newButton				= ToolBarUtility.New();
        protected ToolBarItem editButton			= ToolBarUtility.Edit();
        protected ToolBarItem viewButton			= ToolBarUtility.View();
        protected ToolBarItem deleteButton			= ToolBarUtility.Delete();

        string callBackControlID                    = string.Empty; // <-- for componentart        
		
		#region Public Properties

		[Bindable(true), Category("Behavior"), DefaultValue("-1")]
		public int SelectedID { get { return selectedID; } set { selectedID = value; } }
		
		[Bindable(true), Category("Behavior"), DefaultValue(true)]
		public bool SelectEnabled { get { return selectEnabled; } set { selectEnabled = value; } }

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool AddEnabled { get { return addEnabled; } set { addEnabled = value; } }
		
		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool DeleteEnabled { get { return deleteEnabled; } set { deleteEnabled = value; } }

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool EditEnabled { get { return editEnabled; } set { editEnabled = value; } }

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string HeaderRowCssClass { get { return headerRowCssClass; } set { headerRowCssClass = value; } }
		
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string DefaultRowCssClass { get { return defaultRowCssClass; } set { defaultRowCssClass = value; } }
		
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string AlternateRowCssClass { get { return alternateRowCssClass; } set { alternateRowCssClass = value; } }

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string SelectedRowCssClass { get { return selectedRowCssClass; } set { selectedRowCssClass = value; } }

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string IndexRowCssClass { get { return indexRowCssClass; } set { indexRowCssClass = value; } }

		#endregion

		#region Postback Handler

        //public override void ProcessPostBackEvent(string eventArgument)
        //{				
        //    string commandName;
        //    string parameters;

        //    // Parse command parameters using an underscore sas the separator
        //    int underscoreIndex = eventArgument.IndexOf("_");
        //    if(underscoreIndex == -1)
        //    {
        //        commandName = eventArgument;
        //        parameters = string.Empty;
        //    }
        //    else
        //    {
        //        commandName = eventArgument.Substring(0, underscoreIndex);
        //        parameters = eventArgument.Substring(underscoreIndex + 1);				
        //    }

        //    switch(commandName)
        //    {
        //        case "new":
        //            OnNewClicked(EventArgs.Empty);
        //            break;
        //        case "view":
        //            OnViewClicked(EventArgs.Empty);
        //            break;
        //        case "edit":
        //            OnEditClicked(EventArgs.Empty);
        //            break;
        //        case "delete":
        //            OnDeleteClicked(EventArgs.Empty);
        //            break;
        //        case "sel":
        //            selectedID = int.Parse(parameters);
        //            OnSelectionChanged(System.EventArgs.Empty);
        //            break;
        //        default:
        //            ProcessCommand(commandName, parameters);
        //            break;
        //    }
        //}

		public virtual void ProcessCommand(string command, string parameters)
		{
		}

		#endregion

		#region Events

		public event EventHandler SelectionChanged;
		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if(SelectionChanged != null)
				SelectionChanged(this, e);
		}

		public event EventHandler NewClicked;
		protected virtual void OnNewClicked(EventArgs e)
		{
			if(NewClicked != null)
				NewClicked(this, e);
		}

		public event EventHandler ViewClicked;
		protected virtual void OnViewClicked(EventArgs e)
		{
			if(ViewClicked != null)
				ViewClicked(this, e);
		}

		public event EventHandler EditClicked;
		protected virtual void OnEditClicked(EventArgs e)
		{
			if(EditClicked != null)
				EditClicked(this, e);
		}

		public event EventHandler DeleteClicked;
		protected virtual void OnDeleteClicked(EventArgs e)
		{
			if(DeleteClicked != null)
				DeleteClicked(this, e);
		}

		#endregion

        [Bindable(true), Category("View Pane"), 
        Description("ASP.net AJAX Callback Control")]
        public string CallBackControlID
        {
            get { return callBackControlID; }
            set { callBackControlID = value; }
        }

		public TableGrid()
		{
			features = TableWindowFeatures.ClipboardCopier |
				TableWindowFeatures.Scroller |
				TableWindowFeatures.WindowPrinter;
			components = TableWindowComponents.Toolbar;
		}

		#region Init

		protected override void OnInit(EventArgs e)
		{
            base.OnInit(e);
			if(ComponentCheck(TableWindowComponents.ViewPane))
				this.ProcessViewPanePostback();
		}

		#endregion

        protected override void CreateChildControls()
        {
            base.CreateChildControls();                       

            toolBar = ToolBarUtility.DefaultToolBar("toolBar");
            toolbars.Add(toolBar);
            CreateToolBarControls();
            Controls.Add(toolBar); // <--- MUST BE CALLED AFTER CreateToolBarControls() for Event Handling
            toolBar.ItemCommand += new ToolBar.ItemCommandEventHandler(itemCommand);
        }

        /// <summary>
        /// Creates controls on the toolbar. This is necissary to avoid ComponentArt Toolbar's
        /// inability to process postbacks when the ToolBarItem has not yet been instantiated
        /// into the ToolBarItem list.
        /// </summary>
        protected override void CreateToolBarControls()
        {
            toolBar.Items.Add(ToolBarUtility.New());
            toolBar.Items.Add(ToolBarUtility.Edit());
            toolBar.Items.Add(ToolBarUtility.Delete());
            toolBar.Items.Add(ToolBarUtility.Break());
            base.CreateToolBarControls();
        }

        protected virtual void itemCommand(object sender, ToolBarItemEventArgs e)
        {
            OnToolBarItemCommand(e);
        }

        public event ToolBar.ItemCommandEventHandler ToolBarItemCommand;

        protected virtual void OnToolBarItemCommand(ToolBarItemEventArgs e)
        {
            if (ToolBarItemCommand != null)
                ToolBarItemCommand(this, e);
        }

		#region Selector Methods

		private void registerHiddenFields()
		{
			if(FeatureCheck(TableWindowFeatures.ClientSideSelector))
			{
				string priorCss;

				if(Page.Request.Form["__" + ID + "_CSS"] == null)
					priorCss = string.Empty;
				else
					priorCss = Page.Request.Form["__" + ID + "_CSS"];

				RegisterHiddenField("__" + ID + "_ID", this.selectedID.ToString());
				RegisterHiddenField("__" + ID + "_CSS", priorCss);
			}
		}

		public string GetSelectReference(int selectionID)
		{
			return GetSelectReference(selectionID, selectedRowCssClass);
		}
		
		public string GetSelectReference(int selectionID, string rowSelectCssClass)
		{
			if(!FeatureCheck(TableWindowFeatures.ClientSideSelector))
				return Page.ClientScript.GetPostBackEventReference(this, "sel_" + selectionID.ToString());

			System.Text.StringBuilder s = new System.Text.StringBuilder();

			s.Append(ID + "_sel(this);");

			return s.ToString();
		}

		#endregion

		public override void RegisterClientObject()
		{
			string printCode = string.Empty;

			if(FeatureCheck(TableWindowFeatures.WindowPrinter))
			{
				printCode = this.JavascriptObject + ".printCssClass = '" +
					this.PrinterCssClass + "';\r\n";
			}

			Page.ClientScript.RegisterStartupScript(this.GetType(), "gfx_TableGrid_" + ID,
				"\r\n<script language=\"javascript\">\r\n" +
				"var " + this.JavascriptObject + " = new gfx_TableGridObject('" + ID + "'," +
				"'" + this.ContentWidth.ToString() + "'," +
				FeatureCheck(TableWindowFeatures.ClientSideSelector).ToString().ToLower() + "," +
				ComponentCheck(TableWindowComponents.ViewPane).ToString().ToLower() + "," +
				FeatureCheck(TableWindowFeatures.Scroller).ToString().ToLower() + ");\r\n" +
				this.JavascriptObject + ".selectCssClass = '" + this.SelectedRowCssClass + "';\r\n" + 
				this.JavascriptObject + ".headerLockEnabled = " + this.headerLockEnabled.ToString().ToLower() + ";\r\n" +
				this.JavascriptObject + ".headerSortEnabled = " + this.headerSortEnabled.ToString().ToLower() + ";\r\n" +
				this.JavascriptObject + ".columnLockEnabled = " + this.columnLockEnabled.ToString().ToLower() + ";\r\n" +
				this.JavascriptObject + ".printCssClass = '" + this.PrinterCssClass + "';\r\n" +
                (this.callBackControlID.Length > 0 ?
                    this.JavascriptObject + ".callBackControl = " + this.callBackControlID + ";\r\n" :
                    string.Empty) +
				this.JavascriptObject + ".bind();\r\n" +
			//	this.JavascriptObject + ".hidePanes();\r\n" +
				"</script>\r\n");
		}

		protected override void OnPreRender(EventArgs e)
		{
            base.OnPreRender(e);

			registerHiddenFields();
				

			// Disable selected toolbar buttons if no item is selected
			if(!FeatureCheck(TableWindowFeatures.ClientSideSelector) && selectedID == -1)
			{
				editButton.Enabled = false;
				deleteButton.Enabled = false;
			}

			base.OnPreRender (e);
		}

		private bool __rowflag;

		public void RenderIdRow(string id, params string[] rowText)
		{
			if(__rowflag) 
				RenderIdCssRow(defaultRowCssClass, id, rowText);
			else 
				RenderIdCssRow(alternateRowCssClass, id, rowText);
		}

		/// <summary>
		/// Renders a table row with TR and TD elements with css Class attributes.
		/// </summary>
		/// <param name="cssClass">Css Class to apply to cells.</param>
		/// <param name="headerText">Text to use in the cells. First string is the id.</param>
		public void RenderIdCssRow(string cssClass, string id, params string[] rowText)
		{
			// Track the rowflag on every id row
			__rowflag = !__rowflag;

			__output.WriteBeginTag("tr");
			__output.WriteAttribute("i", id);
			__output.WriteLine(HtmlTextWriter.TagRightChar);
			__output.Indent++;
			for(int i = 0; i <= rowText.GetUpperBound(0); i++)
			{
				__output.WriteBeginTag("td");
				__output.WriteAttribute("class", cssClass);
				__output.Write(HtmlTextWriter.TagRightChar);
				if(rowText[i] != "")
				{
					__output.Write(rowText[i]);
				}
				else
				{
					__output.Write("&nbsp;");
				}
				__output.WriteEndTag("td");
				__output.WriteLine();
			}
			__output.Indent--;
			__output.WriteEndTag("tr");
			__output.WriteLine();
		}

		#region Viewstate Methods

		protected override void LoadViewState(object savedState) 
		{

			// Customize state management to handle saving state of contained objects.

			if (savedState != null) 
			{
				object[] myState = (object[])savedState;

				if (myState[0] != null) base.LoadViewState(myState[0]);
				if (myState[1] != null) selectedID = (int) myState[1];
			}

			if(this.FeatureCheck(TableWindowFeatures.ClientSideSelector))
			{
				string selectionID = Page.Request.Form["__" + ID + "_ID"];
				if(selectionID != null)
				{
					int newId = int.Parse(selectionID); 
					if(selectedID != newId)					
					{
						selectedID = newId;
						this.OnSelectionChanged(EventArgs.Empty);
					}
				}
			}
			
			EnsureChildControls();

		}

		protected override object SaveViewState() 
		{
			// Customized state management to handle saving state of contained objects  such as styles.

			object baseState = base.SaveViewState();
			
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = selectedID;

			return myState;
		}

		#endregion
	}
}