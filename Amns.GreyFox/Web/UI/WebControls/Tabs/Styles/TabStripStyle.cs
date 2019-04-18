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
	public class TabStripStyle : IStateManager
	{	
		string _name = "Default";

//		bool _slidingDoorsEnabled = false;
//		int _slidingDoorsHeight = 21;
//		
//		string _defaultLeftImage = string.Empty;
//		string _defaultRightImage = string.Empty;
//		string _hoverLeftImage = string.Empty;
//		string _hoverRightImage = string.Empty;
//		string _activeLeftImage = string.Empty;
//		string _activeRightImage = string.Empty;		
//
//		string _imagePath = "~/amns_greyfox_client/1_75_2088/tabs/";
//		string _imageExtension	= "gif";

		bool _viewStateTracked = false;

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

//		public bool SlidingDoorsEnabled
//		{
//			get { return _slidingDoorsEnabled; }
//			set { _slidingDoorsEnabled = value; }
//		}
//
//		public int SlidingDoorsHeight
//		{
//			get { return _slidingDoorsHeight; }
//			set { _slidingDoorsHeight = value; }
//		}
//
//		public string DefaultLeftImage
//		{
//			get { return _defaultLeftImage; }
//			set { _defaultLeftImage = value; }
//		}
//
//		public string DefaultRightImage
//		{
//			get { return _defaultRightImage; }
//			set { _defaultRightImage = value; }
//		}
//
//		public string HoverLeftImage
//		{
//			get { return _hoverLeftImage; }
//			set { _hoverLeftImage = value; }
//		}
//
//		public string HoverRightImage
//		{
//			get { return _hoverRightImage; }
//			set { _hoverRightImage = value; }
//		}
//
//		public string ActiveLeftImage
//		{
//			get { return _activeLeftImage; }
//			set { _activeLeftImage = value; }
//		}
//
//		public string ActiveRightImage
//		{
//			get { return _activeRightImage; }
//			set { _activeRightImage = value; }
//		}

		#endregion

		public TabStripStyle()
		{
		}

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
			object[] myState = new object[1];

			return myState;
		}

		void IStateManager.LoadViewState(object savedSate)
		{
			if(savedSate != null)
			{
				object[] myState = (object[]) savedSate;
			}
		}

		#endregion

		#region Render Methods

//		private string ResolveImageUrl(string image, Page page)
//		{
//			return page.ResolveUrl(_imagePath + image + _imageExtension);
//		}

		public void RegisterClientScriptBlock(Page page)
		{
//			if(_slidingDoorsEnabled)
//			{
//				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "gfx_tabstyle_" + _name, 
//					"<style> \r\n" +
//					".gfxtab { \r\n" +
//					"	background:url(\"" + ResolveImageUrl(this._defaultRightImage, page) + "\") no-repeat left top;\r\n" +
//					"	margin:0;\r\n" +
//					"	padding:0 0 0 6px;\r\n" +
//					"	font:x-small/1.5em Verdana,Arial,Helvetica;\r\n" +
//					"}\r\n" +
//					".gfxtableft { \r\n" +
//					"	background:url(\"" + ResolveImageUrl(this._defaultLeftImage, page) + "\") no-repeat left top;\r\n" +
//					"	height:
//</style>
//");
//			}
//			else
//			{
            page.ClientScript.RegisterClientScriptBlock(this.GetType(), "gfx_tabstyle_" + _name, @"
<style>
.gfxtaboff {
	color:#444;
	display:block;
	width:auto;
	float:left;
	text-decoration:none;
	background:#dddddd;
	margin:0;
	padding: 2px 10px;
	border-left: 1px solid #fff;
	border-top: 1px solid #fff;
	border-right: 1px solid #aaa;
}
.gfxtabon {
	color:#444;
	display:block;
	width:auto;
	float:left;
	text-decoration:none;
	background:#cccccc;
	margin:0;
	padding: 2px 10px;
	border-left: 1px solid #fff;
	border-top: 1px solid #fff;
	border-right: 1px solid #aaa;
}
</style>");
//			}
		}

		#endregion

		#region ToolbarStyles

		public static TabStripStyle Default
		{
			get 
			{ 
				TabStripStyle style = new TabStripStyle();
				style.Name = "Default";
				return style;
			}
		}

//		public static TabStripStyle Office2003
//		{
//			get
//			{
//				TabStripStyle style = new TabStripStyle();
//				style.Name = "Office 2003";
//				style.DefaultLeftImage = "default_right";
//				style.DefaultRightImage = "default_right";
//				style.HoverLeftImage = "hover_left";
//				style.HoverRightImage = "hover_right";
//				style.ActiveLeftImage = "current_left";
//				style.ActiveRightImage = "current_right";
//				return style;
//			}
//		}

		#endregion

	}	
}