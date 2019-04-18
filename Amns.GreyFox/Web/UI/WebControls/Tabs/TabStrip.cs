using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using Amns.GreyFox.Web.Handlers;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for WebTab.
	/// </summary>
	public class TabStrip
	{
		private string	_name;
		private Color	_foregroundColor;
		private Color	_backgroundColor;
		private string	_cssClass;
		private string	_style;

		private TabList _tabs;

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public Color ForegroundColor
		{
			get { return _foregroundColor; }
			set { _foregroundColor = value;}		  
		}

		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { _backgroundColor = value;}		  
		}

		public string CssClass
		{
			get { return _cssClass; }
			set { _cssClass = value; }
		}

		public string Style
		{
			get { return _style; } 
			set { _style = value; }
		}

		public TabList Tabs
		{
			get { return _tabs; }
			set { _tabs = value; }
		}

		#endregion
		
		public TabStrip()
		{
		}

		public void ConfigureTabs(string[] names)
		{
            _tabs = new TabList();
			for(int x = 0; x <= names.GetUpperBound(0); x++)
				_tabs.Add(new Tab(names[x]));
		}

		public void Render(TabStripRenderer r)
		{
			HtmlTextWriter output = r.Output;		
	
			for(int x = 0; x < _tabs.Count; x++)
				_tabs[x].RenderTab(r);
		}

		public void RegisterClientScriptBlock(Control parentControl)
		{
			AssemblyResourceHandler.RegisterScript(parentControl.Page, "Tabstrip.js");

			StringBuilder tabArray = new StringBuilder();
			for(int x = 0; x < _tabs.Count; x++)
			{
				tabArray.Append("\"");
				tabArray.Append(_tabs[x].Name);
				tabArray.Append("\"");
				if(x < _tabs.Count - 1)
					tabArray.Append(",");
			}
			
			parentControl.Page.ClientScript.RegisterArrayDeclaration(parentControl.ID + "_Tabs", tabArray.ToString());
		}
	}
}
