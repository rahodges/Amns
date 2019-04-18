using System;
using System.Drawing;
using System.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for WebTab.
	/// </summary>
	public class Tab
	{
		string	_name					= string.Empty;
		Color	_foregroundColor		= Color.Empty;
		Color	_backgroundColor		= Color.Empty;
		string	_cssClass				= string.Empty;
		string	_style					= string.Empty;
		bool	_visible				= false;
		bool	_linkTab				= true;

		public TabRenderHandler RenderDiv;        
		public virtual void OnRenderDiv(HtmlTextWriter output)
		{	
			if(RenderDiv != null)
				RenderDiv(output);
		}

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

		public bool Visible
		{
			get { return _visible; } 
			set { _visible = value; }
		}

		#endregion

		public Tab() { }

		public Tab(string name)
		{
			_name = name;
		}

		public void RenderTab(TabStripRenderer r)
		{
//			HtmlTextWriter output = r.Output;
//			output.WriteFullBeginTag("li");
//			output.WriteBeginTag("a");
//			if(_linkTab)
//				output.WriteAttribute("href", "javascript:" + "gfx_setTab(" + r.ParentControl.ID + "_TabArray, '" + r.ParentControl.ID + "_Tab_" + _name + "');");
//			if(_visible)
//				output.WriteAttribute("class", "here");
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write(_name);
//			output.WriteEndTag("li");
//			output.WriteLine();
            
			HtmlTextWriter output = r.Output;
//			output.WriteBeginTag("div");			
//			if(_style != string.Empty)
//				output.WriteAttribute("style", _style);
//			else
//				output.WriteAttribute("style", "padding-right=5px;float:left;");
//			if(_cssClass != string.Empty)
//				output.WriteAttribute("class", _cssClass);
//			else
//				output.WriteAttribute("class", "gfxtaba");
//			if(!_linkTab)
//				output.WriteAttribute("onclick", 
//					"gfx_setTab(" + r.ParentControl.ID + "_TabArray, '" + r.ParentControl.ID + "_Tab_" + _name + "');");
//			output.WriteLine(HtmlTextWriter.TagRightChar);
//			output.Indent++;
			if(_linkTab)
			{
				output.WriteBeginTag("a");
				output.WriteAttribute("id", r.ParentControl.ID + "_Tab_" + _name);
				output.WriteAttribute("href", "#");
				if(_linkTab)
					output.WriteAttribute("onclick", "gfx_setTab('" + r.ParentControl.ID + "', '" + _name + "');");
				if(_visible)
					output.WriteAttribute("class", "gfxtabon");
				else
					output.WriteAttribute("class", "gfxtaboff");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(_name);
				output.WriteEndTag("a");
			}
			else
				output.WriteLine(_name);
//			output.Indent--;
//			output.WriteEndTag("div");
//			output.WriteLine();
		}

		public void RenderPanel(TabStripRenderer r)
		{
			HtmlTextWriter output = r.Output;

			output.WriteBeginTag("div");
			output.WriteAttribute("id", r.ParentControl.ID + "_TabPage_" + _name);
			if(_visible)
				output.WriteAttribute("style", "display:block;visibility:visible;");
			else
				output.WriteAttribute("style", "display:none;visibility:hidden;");
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

			r.OnRenderDivTableStart(output);
            OnRenderDiv(r.Output);
			r.RenderDivTableEnd(output);
			
			output.Indent--;
			output.WriteEndTag("div");
			output.WriteLine();
		}
	}
}