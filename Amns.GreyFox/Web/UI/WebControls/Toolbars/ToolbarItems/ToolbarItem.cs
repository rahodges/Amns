using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ToolbarItem.
	/// </summary>
	public class ToolbarItem
	{
		private string _name				= string.Empty;			
		private string _command				= string.Empty;
		private string _toolTip				= string.Empty;		
		private string _text				= string.Empty;
		private bool _visible				= true;
		private bool _enabled				= true;

		private string _function			= string.Empty;		// Overrides post back!
		private string _scriptBlock			= string.Empty;
	
		private string _icon				= string.Empty;
		private string _mouseOverIcon		= string.Empty;
		private string _mouseDownIcon		= string.Empty;

		private Unit _width					= new Unit("21px");
		private Unit _height				= new Unit("21px");

		#region Public Properties

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string Command
		{
			get { return _command; }
			set { _command = value; }				
		}
        
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ToolTip
		{
			get { return _toolTip; }
			set { _toolTip = value; }				
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string Function
		{
			get { return _function; }
			set { _function = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string ScriptBlock
		{
			get { return _scriptBlock; }
			set { _scriptBlock = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue(true)]
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue(true)]
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string Icon
		{
			get { return _icon; }
			set { _icon = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string MouseOverIcon
		{
			get { return _mouseOverIcon; }
			set { _mouseOverIcon = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string MouseDownIcon
		{
			get { return _mouseDownIcon; }
			set { _mouseDownIcon = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public Unit Width
		{
			get { return _width; }
			set { _width = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public Unit Height
		{
			get { return _height; }
			set { _height = value; }
		}

		#endregion

		public ToolbarItem()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public virtual void Render(ToolbarRenderer r)
		{
			return;
		}
	}
}
