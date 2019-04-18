using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls 
{	
	
	/// <summary>
	/// Contains all the style parameters for ToolbarButtons
	/// </summary>
	public class ToolbarStyle : IStateManager
	{	
		private string _name						= "Default";

		private bool _buttonOverImage				= false;
		private bool _buttonDownImage				= false;
		private bool _buttonOverStyle				= false;
		private bool _buttonDownStyle				= false;
		private bool _startImage					= false;
		private bool _endImage						= false;

		private string _buttonPath					= "~/amns_greyfox_client/1_75_2088/toolbars/";
		private string _buttonExtension				= "gif";
	
		private ToolbarButtonType _buttonType		= ToolbarButtonType.Image;
		private ToolbarButtonStyle _buttonStyle		= new ToolbarButtonStyle();
		
		private bool _backgroundImage				= false;
		private Color _backColor					= Color.Empty;
		private Color _gutterBackColor				= Color.Empty;	
		private Color _gutterBorderColorLight		= Color.Empty;
		private Color _gutterBorderColorDark		= Color.Empty;

		private FontUnit _fontSize					= FontUnit.XXSmall;

		bool _viewStateTracked = false;

		public ToolbarStyle()
		{
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		#region Toolbar DIV Properties

		[ NotifyParentProperty(true) ]
		public bool StartImage
		{
			get { return _startImage; }
			set { _startImage = value; }
		}		

		[ NotifyParentProperty(true) ]
		public bool EndImage
		{
			get { return _endImage; }
			set { _endImage = value; }
		}		

		[ NotifyParentProperty(true) ]
		public bool BackgroundImage
		{
			get { return _backgroundImage; }
			set { _backgroundImage = value; }
		}		

		#endregion

		#region Button Properties

		[ NotifyParentProperty(true) ]
		public bool ButtonOverImage
		{
			get { return _buttonOverImage; }
			set { _buttonOverImage = value; }
		}		

		[ NotifyParentProperty(true) ]
		public bool ButtonDownImage
		{
			get { return _buttonDownImage; }
			set { _buttonDownImage = value; }
		}		

		[ NotifyParentProperty(true) ]
		public bool ButtonOverStyle
		{
			get { return _buttonOverStyle; }
			set { _buttonOverStyle = value; }
		}		

		[ NotifyParentProperty(true) ]
		public bool ButtonDownStyle
		{
			get { return _buttonDownStyle; }
			set { _buttonDownStyle = value; }
		}		

		[ NotifyParentProperty(true) ]
		public string ButtonPath
		{
			get { return _buttonPath; }
			set { _buttonPath = value; }
		}		

		[ NotifyParentProperty(true) ]
		public string ButtonExtension
		{
			get { return _buttonExtension; }
			set { _buttonExtension = value; }
		}		

		[ NotifyParentProperty(true) ]
		public ToolbarButtonStyle ButtonStyle
		{
			get { return _buttonStyle; }
			set { _buttonStyle = value; }
		}		

		[ NotifyParentProperty(true) ]
		public ToolbarButtonType ButtonType
		{
			get { return _buttonType; }
			set { _buttonType = value; }
		}		

		#endregion

		#region Color Properties

		[ NotifyParentProperty(true) ]
		public Color BackColor
		{
			get { return _backColor; }
			set { _backColor = value; }
		}
	
		[ NotifyParentProperty(true) ]
		public Color GutterBackColor
		{
			get { return _gutterBackColor; }
			set { _gutterBackColor = value; }
		}		

		[ NotifyParentProperty(true) ]
		public Color GutterBorderColorLight
		{
			get { return _gutterBorderColorLight; }
			set { _gutterBorderColorLight = value; }
		}		

		[ NotifyParentProperty(true) ]
		public Color GutterBorderColorDark
		{
			get { return _gutterBorderColorDark; }
			set { _gutterBorderColorDark = value; }
		}		

		#endregion

		#region IStateManager Methods

		bool IStateManager.IsTrackingViewState
		{ 
			get { return _viewStateTracked; }
		}

		void IStateManager.TrackViewState()
		{
			_viewStateTracked = true;
		}

		object IStateManager.SaveViewState()
		{
			object[] myState = new object[15];
			
			myState[0] = _buttonOverImage;
			myState[1] = _buttonDownImage;
			myState[2] = _buttonOverStyle;
			myState[3] = _buttonDownStyle;
			myState[4] = _buttonPath;
			myState[5] = _buttonExtension;
			myState[6] = _buttonType;
			myState[7] = ((IStateManager) _buttonStyle).SaveViewState();
			myState[8] = _backgroundImage;
			myState[9] = _backColor;
			myState[10] = _gutterBackColor;
			myState[11] = _gutterBorderColorLight;
			myState[12] = _gutterBorderColorDark;
			myState[13] = _startImage;
			myState[14] = _endImage;

			return myState;
		}

		void IStateManager.LoadViewState(object savedSate)
		{
			if(savedSate != null)
			{
				object[] myState = (object[]) savedSate;
				_buttonOverImage = (bool) myState[0];
				_buttonDownImage = (bool) myState[1];
				_buttonOverStyle = (bool) myState[2];
				_buttonDownStyle = (bool) myState[3];
				_buttonPath = (string) myState[4];
				_buttonExtension = (string) myState[5];
				_buttonType = (ToolbarButtonType) myState[6];
				((IStateManager) _buttonStyle).LoadViewState(myState[7]);
				_backgroundImage = (bool) myState[8];
				_backColor = (Color) myState[9];
				_gutterBackColor = (Color) myState[10];
				_gutterBorderColorLight = (Color) myState[11];
				_gutterBorderColorDark = (Color) myState[12];
				_startImage = (bool) myState[13];
				_endImage = (bool) myState[14];
			}
		}

		#endregion

		#region Render Methods

		public void RegisterClientScriptBlock(Page page)
		{
			page.ClientScript.RegisterClientScriptBlock(this.GetType(), "gfx_toolbarstyle_" + _name, @"
<script language=""javascript"">
var " + _name + @"_OverImage = new Image();
" + _name + @"_OverImage.src = '" + page.ResolveUrl(_buttonPath) + _buttonStyle.OverBackgroundImage + _buttonExtension + @"';
var " + _name + @"_DownImage = new Image(); 
" + _name + @"_DownImage.src = '" + page.ResolveUrl(_buttonPath) + _buttonStyle.DownBackgroundImage + _buttonExtension + @"';
</script>
<style>
td." + _name + @"_StartTabOn {
	font-size: " + _fontSize.ToString() + @";
	padding:1px;
	border-left: 1 solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(this.GutterBorderColorLight) + @";
	border-top: 1 solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
td." + _name + @"_StartTabOff {
	font-size: " + _fontSize.ToString() + @";
	padding:1px;
	border-left: 1 solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-top: 1 solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
td." + _name + @"_TabOn {
	font-size: " + _fontSize.ToString() + @";
	padding:1px;
	padding-left:5px;
	padding-right:5px;
	border-left: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorLight) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-top: 1 solid " + ColorTranslator.ToHtml(this._backColor) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	background-color: " + ColorTranslator.ToHtml(this._backColor) + @";
}
td." + _name + @"_TabOffRight {
	font-size: " + _fontSize.ToString() + @";
	padding:1px;
	padding-left:5px;
	padding-right:5px;
	border-left: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-top: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
}
td." + _name + @"_TabOffLeft {
	font-size: " + _fontSize.ToString() + @";
	padding:1px;
	padding-left:5px;
	padding-right:5px;
	border-left: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorLight) + @";
	border-top: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(this._backColor) + @";
	background-color: " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
}
td." + _name + @"_EndTab {
	font-size: " + _fontSize.ToString() + @";
	width: 100%;
	padding:1px;
	border-left: 1 solid " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
	border-top: 1 solid " + ColorTranslator.ToHtml(this._gutterBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this._gutterBackColor) + @";
}
td." + _name + @"_None {
}
td." + _name + @"_ButtonNormal {
	" + ((_backColor != Color.Empty) ? "border: 1 solid " + ColorTranslator.ToHtml(_backColor) + ";" : "padding: 1px;") + @"
	" + ((_backColor != Color.Empty) ? "background-color:  " + ColorTranslator.ToHtml(_backColor) + ";" : "") + @"
	font-family: MS Sans Serif;
	font-size: " + _fontSize.ToString() + @";
}
td." + _name + @"_ButtonOver {
	border-top: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorLight) + @";	
	border-left: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorLight) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorDark) + @";
	" + ((ButtonStyle.OverBackColor != Color.Empty) ? "background-color: " + ColorTranslator.ToHtml(_buttonStyle.OverBackColor) + ";" : "") + @"
	font-family: MS Sans Serif;
	font-size: " + _fontSize.ToString() + @";
}
td." + _name + @"_ButtonDown {
	border-top: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.DownBorderColorLight) + @";	
	border-left: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.DownBorderColorLight) + @";
	border-right: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.DownBorderColorDark) + @";
	border-bottom: 1 solid " + ColorTranslator.ToHtml(ButtonStyle.DownBorderColorDark) + @";
	" + ((ButtonStyle.DownBackColor != Color.Empty) ? "background-color: " + ColorTranslator.ToHtml(_buttonStyle.DownBackColor) + ";" : "") + @"
	font-family: MS Sans Serif;
	font-size: " + _fontSize.ToString() + @";
}
div." + _name + @"_Toolbar {
	margin-bottom: 1px;
	margin-right: 2px;
	float: left;
	" + ((_backgroundImage) ? 
				"background-image: url(" + page.ResolveUrl(_buttonPath) + "toolbar.background." + _buttonExtension + ");" :
				"background-color: " + ColorTranslator.ToHtml(BackColor) + ";")
				+ @"
}
</style>");
		}

		#endregion

		#region ToolbarStyles

		public static ToolbarStyle Default
		{
			get 
			{ 
				ToolbarStyle style = new ToolbarStyle();
				style.Name = "Default";
				return style;
			}
		}

		public static ToolbarStyle Office2003
		{
			get
			{
				ToolbarStyle style = new ToolbarStyle();

				style.Name = "Office2003";
				style.GutterBackColor = ColorTranslator.FromHtml("#81A9E2");
				style.ButtonOverStyle = true;
				style.ButtonOverImage = false;
				style.ButtonDownStyle = true;
				style.ButtonDownImage = false;
				style.StartImage = true;
				style.EndImage = true;
				style.ButtonStyle.OverBackColor = Color.Empty;
				style.ButtonStyle.OverBackgroundImage = "toolbarbutton.over.";
				style.ButtonStyle.OverBorderColorLight = ColorTranslator.FromHtml("#000080");
				style.ButtonStyle.OverBorderColorDark = ColorTranslator.FromHtml("#000080");
				style.ButtonStyle.DownBackColor = Color.Empty;
				style.ButtonStyle.DownBackgroundImage = "toolbarbutton.down.";
				style.ButtonStyle.DownBorderColorLight = ColorTranslator.FromHtml("#000080");
				style.ButtonStyle.DownBorderColorDark = ColorTranslator.FromHtml("#000080");
				style.BackColor = System.Drawing.Color.Empty;
				style.BackgroundImage = true;

				return style;
			}
		}

		public static ToolbarStyle OfficeXP
		{
			get
			{
				ToolbarStyle style = new ToolbarStyle();

				style.Name = "OfficeXP";
				style.GutterBackColor = ColorTranslator.FromHtml("#BFBCB6");
				style.ButtonOverStyle = true;
				style.ButtonOverImage = true;
				style.ButtonDownStyle = true;
				style.ButtonDownImage = false;
				style.StartImage = true;
				style.EndImage = true;
				style.ButtonStyle.OverBackColor = ColorTranslator.FromHtml("#B6BDD2");
				style.ButtonStyle.OverBorderColorLight = ColorTranslator.FromHtml("#3169C6");
				style.ButtonStyle.OverBorderColorDark = ColorTranslator.FromHtml("#3169C6");
				style.ButtonStyle.DownBackColor = ColorTranslator.FromHtml("#8592B5");
				style.ButtonStyle.DownBorderColorLight = ColorTranslator.FromHtml("#3169C6");
				style.ButtonStyle.DownBorderColorDark = ColorTranslator.FromHtml("#3169C6");						
				style.BackgroundImage = false;

				return style;
			}
		}

		public static ToolbarStyle Office2000
		{
			get
			{
				ToolbarStyle style = new ToolbarStyle();

				style.Name = "Office2000";
				style.GutterBackColor = ColorTranslator.FromHtml("#BFBCB6");	
				style.ButtonOverStyle = true;
				style.ButtonOverImage = false;
				style.ButtonDownStyle = true;
				style.ButtonDownImage = false;
				style.StartImage = true;
				style.EndImage = true;
				style.ButtonStyle.OverBackColor = ColorTranslator.FromHtml("#D4D0C8");
				style.ButtonStyle.OverBorderColorLight = ColorTranslator.FromHtml("#FFFFFF");
				style.ButtonStyle.OverBorderColorDark = ColorTranslator.FromHtml("#808080");
				style.ButtonStyle.DownBackColor = ColorTranslator.FromHtml("#D4D0C8");
				style.ButtonStyle.DownBorderColorLight = ColorTranslator.FromHtml("#808080");
				style.ButtonStyle.DownBorderColorDark = ColorTranslator.FromHtml("#FFFFFF");
				style.BackgroundImage = false;

				return style;
			}
		}

		#endregion

	}	
}