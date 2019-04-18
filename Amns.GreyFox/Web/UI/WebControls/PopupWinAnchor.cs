using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Amns.GreyFox.Web.UI.WebControls
{
  /// <summary>
  /// Popup window anchor control
  /// </summary>
  [DefaultProperty("PopupToShow"), ToolboxData("<{0}:PopupWinAnchor runat=server></{0}:PopupWinAnchor>")]
  [Designer(typeof(AnchorDesigner))]
  public class PopupWinAnchor : System.Web.UI.WebControls.WebControl
  {
    #region Variables

    string controlId,controlLink,jsEvent;
    string snMsg,snText,snTitle;
    bool bChangeText=false;

    #endregion
    #region Constructor

    /// <summary>
    /// Create control
    /// </summary>
    public PopupWinAnchor()
    {
      jsEvent="onclick";
      bChangeText=false;
    }

    #endregion
    #region Properties

    /// <summary>
    /// Should texts on PopupWin be replaced with new texts ?
    /// </summary>
    [Bindable(true),Category("PopupWin"),DefaultValue(false)]
    [Description("Should texts on PopupWin be replaced with new texts ?")]
    public bool ChangeTexts
    {
      get { return bChangeText; }
      set { bChangeText=value; }
    }


    /// <summary>
    /// New message text
    /// </summary>
    [Bindable(true),Category("PopupWin"),DefaultValue("")]
    [Description("New message text")]
    public string NewMessage
    {
      get { return snMsg; }
      set { snMsg=value; }
    }


    /// <summary>
    /// New popup title text
    /// </summary>
    [Bindable(true),Category("PopupWin"),DefaultValue("")]
    [Description("New popup title text")]
    public string NewTitle
    {
      get { return snTitle; }
      set { snTitle=value; }
    }


    /// <summary>
    /// New text to show in opened window
    /// </summary>
    [Bindable(true),Category("PopupWin"),DefaultValue("")]
    [Description("New text to show in opened window")]
    public string NewText
    {
      get { return snText; }
      set { snText=value; }
    }


    /// <summary>
    /// JavaScript event to handle
    /// </summary>
    [Bindable(true),Category("Anchor"),DefaultValue("onclick")]
    [Editor(typeof(JavaScriptEventEditor),typeof(UITypeEditor))]
    [Description("JavaScript event to handle")]
    public string HandledEvent
    {
      get { return jsEvent; }
      set { jsEvent=value; }
    }


    /// <summary>
    /// Popup control to show when event occurs
    /// </summary>
    [Bindable(true),Category("Anchor"),DefaultValue("")]
    [Editor(typeof(PopupControlsEditor), typeof(UITypeEditor))]
    [Description("Popup control to show when event occurs")]
    public string PopupToShow
    {
      get { return controlId; }
      set { controlId=value; }
    }

    
    /// <summary>
    /// Contol which event will cause the popup to show
    /// </summary>
    [Bindable(true),Category("Anchor"),DefaultValue("")]
    [Editor(typeof(AllControlsEditor), typeof(UITypeEditor))]
    [Description("Control ")]
    public string LinkedControl
    {
      get { return controlLink; }
      set { controlLink=value; }
    }

    #endregion
    #region Methods

    /// <summary> 
    /// Render script for displaying popup control
    /// </summary>
    /// <param name="output">The HTML writer to write out to</param>
    protected override void Render(HtmlTextWriter output)
    {
      output.Write(@"
        <script type=""text/javascript"">
        //<![CDATA[

        var "+ID+@"oldOnLoad=window.onload;
        window.onload="+ID+@"espopup_anchorInit;
        function "+ID+@"espopup_anchorInit()
        {
          if ("+ID+@"oldOnLoad!=null) "+ID+@"oldOnLoad();
          document.getElementById('"+controlLink+@"')."+jsEvent+@"="+
        ID+@"espopup_anchorEvent;
        }

        function "+ID+@"espopup_anchorEvent()
        {
          ");
      if (bChangeText)
      {
        Control ct=Page.FindControl(controlId);
        if (ct!=null)
        {
          output.Write(controlId+"nText=\""+
            ((PopupWin)ct).GetWinText(snTitle,snText)+"\";\n");
        }
        output.Write(controlId+"nMsg=\""+snMsg+"\";\n");
        output.Write(controlId+"nTitle=\""+snTitle+"\";\n");
        output.Write(controlId+"bChangeTexts=true;\n");
      }
      else
      {
        output.Write(controlId+"bChangeTexts=false;\n");
      }
      output.Write("\n"+controlId+@"espopup_ShowPopup('"+ID+@"');
        }
        //]]>
        </script>");
    }

    #endregion
  }

  
  /// <summary>
  /// Class for displaying PopupWin in designer
  /// </summary>
  public class AnchorDesigner : ControlDesigner
  {
    #region Overriden methods

    /// <summary>
    /// Returns HTML code to show in designer
    /// </summary>
    public override string GetDesignTimeHtml()
    {
      return "<div style=\"padding:2px; background-color: ButtonFace;color:ButtonText; "+
        "border-style:outset; border-width:1px; font: 75% 'Microsoft Sans Serif';\"><b>"+
        "PopupWinAnchor</b> - "+((Control)Component).ID+"</div>";
    }
    
    #endregion
  }


  /// <summary>
  /// Editor for selecting JavaScript event
  /// </summary>
  public class JavaScriptEventEditor : UITypeEditor
  {
    #region Variables

    private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc=null;
    private System.Windows.Forms.ListBox lb;

    #endregion
    #region Methods

    /// <summary>
    /// Overrides the method used to provide basic behaviour for selecting editor.
    /// Shows our custom control for editing the value.
    /// </summary>
    /// <param name="context">The context of the editing control</param>
    /// <param name="provider">A valid service provider</param>
    /// <param name="value">The current value of the object to edit</param>
    /// <returns>The new value of the object</returns>
    public override object EditValue(ITypeDescriptorContext context,
      IServiceProvider provider,object value) 
    {
      if (context!=null&&context.Instance!=null&&provider!=null) 
      {
        edSvc=(System.Windows.Forms.Design.IWindowsFormsEditorService)
          provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
        if (edSvc!=null) 
        {					
          lb=new System.Windows.Forms.ListBox();
          lb.BorderStyle=System.Windows.Forms.BorderStyle.None;
          lb.SelectedIndexChanged+=new EventHandler(lb_SelectedIndexChanged);
          lb.Items.Add("onclick");
          lb.Items.Add("ondblclick");
          lb.Items.Add("onmouseover");
          lb.Items.Add("onfocus");
          lb.Items.Add("oncontextmenu");
          edSvc.DropDownControl(lb);
          if (lb.SelectedIndex==-1) return value;
          return lb.SelectedItem;
        }
      }

      return value;
    }


    /// <summary>
    /// Choose editor type
    /// </summary>
    /// <param name="context">The context of the editing control</param>
    /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
    {
      return UITypeEditorEditStyle.DropDown;			
    }


    /// <summary>
    /// Close the dropdowncontrol when the user has selected a value
    /// </summary>
    private void lb_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (edSvc != null) 
      {
        edSvc.CloseDropDown();
      }
    }

    #endregion
  }


  /// <summary>
  /// Editor for selecting controls from Asp.Net page
  /// </summary>
  public abstract class ControlsEditor : UITypeEditor
  {
    #region Variables

    private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc=null;
    private System.Windows.Forms.ListBox lb;
    private Type typeShow;

    #endregion
    #region Constructor


    /// <summary>
    /// onstructor - show specified types
    /// </summary>
    /// <param name="show">Type descriptor</param>
    public ControlsEditor(Type show)
    {
      typeShow=show;
    }

    #endregion
    #region Methods

    /// <summary>
    /// Overrides the method used to provide basic behaviour for selecting editor.
    /// Shows our custom control for editing the value.
    /// </summary>
    /// <param name="context">The context of the editing control</param>
    /// <param name="provider">A valid service provider</param>
    /// <param name="value">The current value of the object to edit</param>
    /// <returns>The new value of the object</returns>
    public override object EditValue(ITypeDescriptorContext context,
      IServiceProvider provider,object value) 
    {
      if (context!=null&&context.Instance!=null&&provider!=null) 
      {
        edSvc=(System.Windows.Forms.Design.IWindowsFormsEditorService)
          provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
        if (edSvc!=null) 
        {					
          lb=new System.Windows.Forms.ListBox();
          lb.BorderStyle=System.Windows.Forms.BorderStyle.None;
          lb.SelectedIndexChanged+=new EventHandler(lb_SelectedIndexChanged);
          foreach(Control ctrl in ((Control)context.Instance).Page.Controls)
          {
            if (ctrl.GetType().IsSubclassOf(typeShow)||
              ctrl.GetType().FullName==typeShow.FullName) lb.Items.Add(ctrl.ID);
          }
          edSvc.DropDownControl(lb);
          if (lb.SelectedIndex==-1) return value;
          return lb.SelectedItem;
        }
      }

      return value;
    }


    /// <summary>
    /// Choose editor type
    /// </summary>
    /// <param name="context">The context of the editing control</param>
    /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
    {
      return UITypeEditorEditStyle.DropDown;			
    }


    /// <summary>
    /// Close the dropdowncontrol when the user has selected a value
    /// </summary>
    private void lb_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (edSvc != null) 
      {
        edSvc.CloseDropDown();
      }
    }

    #endregion
  }


  /// <summary>
  /// Editor for selecting all Asp.Net controls
  /// </summary>
  public class AllControlsEditor : ControlsEditor
  {
    #region Members

    /// <summary>
    /// Invoke base constructor
    /// </summary>
    public AllControlsEditor() : base(typeof(Control)) {}

    #endregion
  }


  /// <summary>
  /// Editor for selecting PopupWin controls
  /// </summary>
  public class PopupControlsEditor : ControlsEditor
  {
    #region Members

    /// <summary>
    /// Invoke base constructor
    /// </summary>
    public PopupControlsEditor() : base(typeof(PopupWin)) {}

    #endregion
  }
}