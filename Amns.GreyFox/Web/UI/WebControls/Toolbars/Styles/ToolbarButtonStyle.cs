using System;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls {	
	
	/// <summary>
	/// Contains all the style parameters for ToolbarButtons
	/// </summary>
	public class ToolbarButtonStyle : IStateManager
	{	
		private Color _borderColor				= Color.Empty;
        private Color _borderColorLight			= Color.Empty;
		private Color _borderColorDark			= Color.Empty;

		private Color _overBackColor			= Color.Empty;
		private string _overBackgroundImage		= string.Empty;
		private Color _overBorderColorLight		= Color.Empty;
		private Color _overBorderColorDark		= Color.Empty;
		
		private Color _downBackColor			= Color.Empty;
		private string _downBackgroundImage		= string.Empty;
		private Color _downBorderColorLight		= Color.Empty;
		private Color _downBorderColorDark		= Color.Empty;

		private Unit _width						= Unit.Empty;
		private Unit _height					= Unit.Empty;

		bool _viewStateTracked = false;

		public ToolbarButtonStyle()
		{
		}

		/// <summary>
		/// Constructor for colors only
		/// </summary>
		public ToolbarButtonStyle(Color borderColorLight, Color borderColorDark,
			Color overBackColor, Color overBorderColorLight, Color overBorderColorDark,
			Color downBackColor, Color downBorderColorLight, Color downBorderColorDark) 
		{
			_borderColorLight		= borderColorLight;
			_borderColorDark		= borderColorDark;
			_overBackColor			= overBackColor;
			_overBorderColorLight	= overBorderColorLight;
			_overBorderColorDark	= overBorderColorDark;
			_downBackColor			= downBackColor;
			_downBorderColorLight	= downBorderColorLight;
			_downBorderColorDark	= downBorderColorDark;
			
		}

		/// <summary>
		/// Constructor with colors and images
		/// </summary>
		public ToolbarButtonStyle(Color borderColorLight,Color borderColorDark,string overBackgroundImage,Color overBorderColorLight,Color overBorderColorDark,string downBackgroundImage,Color downBorderColorLight,Color downBorderColorDark) {
			_borderColorLight		= borderColorLight;
			_borderColorDark		= borderColorDark;
			_overBackgroundImage	= overBackgroundImage;
			_overBorderColorLight	= overBorderColorLight;
			_overBorderColorDark	= overBorderColorDark;
			_downBackgroundImage	= downBackgroundImage;
			_downBorderColorLight	= downBorderColorLight;
			_downBorderColorDark	= downBorderColorDark;
		}

		/// <summary>
		/// Sets BorderColorLight and BorderColorDark.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color BorderColor 
		{
			get { return _borderColor; }
			set { _borderColor = value; _borderColorLight = value; _borderColorDark = value; }
		}

		/// <summary>
		/// The default light (top and left) border color of buttons.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color BorderColorLight
		{
			get { return _borderColorLight; }
			set { _borderColorLight = value; }
		}

		/// <summary>
		/// The default dark (right and bottom) border color of buttons.
		/// </summary>		
		[ NotifyParentProperty(true) ]
		public Color BorderColorDark
		{
			get { return _borderColorDark; }
			set { _borderColorDark = value; }
		}

		/// <summary>
		/// The default onMouseOver back color of buttons.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color OverBackColor 
		{
			get { return _overBackColor; }
			set { _overBackColor = value; }
		}

		/// <summary>
		/// The default onMouseOver light (top and left) border color of buttons.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color OverBorderColorLight
		{
			get { return _overBorderColorLight; }
			set { _overBorderColorLight = value; }
		}

		/// <summary>
		/// The default onMouseOver dark (right and bottom) border color of buttons.
		/// </summary>		
		[ NotifyParentProperty(true) ]
		public Color OverBorderColorDark
		{
			get { return _overBorderColorLight; }
			set { _overBorderColorLight = value; }
		}

		/// <summary>
		/// The background image onMouseDown.
		/// </summary>
		[ NotifyParentProperty(true) ]
		public string OverBackgroundImage
		{
			get { return _overBackgroundImage; }
			set { _overBackgroundImage = value; }
		}

		/// <summary>
		/// The default onMouseDown back color of buttons.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color DownBackColor
		{
			get { return _downBackColor; }
			set { _downBackColor = value; }
		}

		/// <summary>
		/// The default onMouseDown light (top and left) border color of buttons onMouseDown.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color DownBorderColorLight
		{
			get { return _downBorderColorLight; }
			set { _downBorderColorLight = value; }
		}

		/// <summary>
		/// The default onMouseDown dark (right and bottom) border color of buttons onMouseDown.
		/// </summary>	
		[ NotifyParentProperty(true) ]
		public Color DownBorderColorDark
		{
			get { return _downBorderColorDark; }
			set { _downBorderColorDark = value; }
		}

		/// <summary>
		/// The background image onMouseDown.
		/// </summary>
		[ NotifyParentProperty(true) ]
		public string DownBackgroundImage
		{
			get { return _downBackgroundImage; }
			set { _downBackgroundImage = value; }
		}

		/// <summary>
		/// Height
		/// </summary>
		[ NotifyParentProperty(true) ]
		public Unit Height
		{
			get { return _height; }
			set { _height = value; }
		}

		/// <summary>
		/// Width
		/// </summary>
		[ NotifyParentProperty(true) ]
		public Unit Width
		{
			get { return _width; }
			set { _width = value; }
		}		

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
            object[] myState = new object[13];
			
			myState[0] = _borderColor;
			myState[1] = _borderColorLight;
			myState[2] = _borderColorDark;
			myState[3] = _overBackColor;
			myState[4] = _overBackgroundImage;
			myState[5] = _overBorderColorLight;
			myState[6] = _overBorderColorDark;
			myState[7] = _downBackColor;
			myState[8] = _downBackgroundImage;
			myState[9] = _downBorderColorLight;
			myState[10] = _downBorderColorDark;
			myState[11] = _width;
			myState[12] = _height;

			return myState;
		}

		void IStateManager.LoadViewState(object savedSate)
		{
			if(savedSate != null)
			{
				object[] myState = (object[]) savedSate;
				_borderColor = (Color) myState[0];
				_borderColorLight = (Color) myState[1];
				_borderColorDark = (Color) myState[2];
				_overBackColor = (Color) myState[3];
				_overBackgroundImage = (string) myState[4];
				_overBorderColorLight = (Color) myState[5];
				_overBorderColorDark = (Color) myState[6];
				_downBackColor = (Color) myState[7];
				_downBackgroundImage = (string) myState[8];
				_downBorderColorLight = (Color) myState[9];
				_downBorderColorDark = (Color) myState[10];
				_width = (Unit) myState[11];
				_height = (Unit) myState[12];
			}
		}
	}	
}