using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Resources;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using Amns.GreyFox.Web.Handlers;

namespace Amns.GreyFox.Web.UI.WebControls {
	/// <summary>
	/// A Toolbar for FreeTextBox
	/// </summary>
	public class Toolbarp 
	{
		private string _name							= "";
		private bool _movable							= false;
		private bool _visible							= true;

		private ToolbarItemsList _items		= new ToolbarItemsList();

		#region Public Properties

		/// <summary>
		/// The name of the Toolbar
		/// </summary>
		public string Name  
		{
			get { return _name; }
			set { _name = value; }
		}
		/// <summary>
		/// Whether the toolbar will be rendered visible
		/// </summary>
		public bool Visible  
		{
			get { return _visible; }
			set { _visible = value; }
		}

		/// <summary>
		/// Not Implimented
		/// </summary>
		public bool Moveable  
		{
			get { return _movable; }
			set { _movable = value; }
		}		

		public ToolbarItemsList Items
		{
			get { return _items; }
			set { _items = value; }
		}

		#endregion

		public Toolbarp() 
		{
		}

		public Toolbarp(string name) 
		{
			this.Name = name;
		}

		public Toolbarp(string name, bool moveable) 
		{
			Name = name;
			Moveable = moveable;
		}

		#region Render Methods

		public virtual void Render(ToolbarRenderer r)
		{
			if(!_visible)
				return;

			HtmlTextWriter output = r.Output;

			output.WriteBeginTag("div");
			output.WriteAttribute("id", r.ParentControl.ID + "_ToolBar_" + _name);
			output.WriteAttribute("class", r.Style.Name + "_Toolbar");
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

			output.WriteBeginTag("table");
			output.WriteAttribute("border", "0");
			output.WriteAttribute("cellpadding", "0");
			output.WriteAttribute("cellspacing", "0");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;
			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;
			
			if(r.Style.StartImage)
			{
				output.WriteFullBeginTag("td");
				output.WriteLine();
				output.Indent++;
				output.WriteBeginTag("img");
				output.WriteAttribute("src", r.ParentControl.ResolveUrl(r.Style.ButtonPath) +
					"toolbar.start." + 
					r.Style.ButtonExtension);
				output.WriteAttribute("border", "0");
				output.WriteAttribute("unselectable", "on");
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent--;
				output.WriteEndTag("td");
				output.WriteLine();
			}
			
			output.WriteFullBeginTag("td");
			output.WriteLine();
			output.Indent++;
			output.WriteBeginTag("table");
			output.WriteAttribute("border", "0");
			output.WriteAttribute("cellpadding", "0");
			output.WriteAttribute("cellspacing", "0");

			if(!r.Style.BackColor.IsEmpty) 
				output.WriteAttribute("bgcolor", ColorTranslator.ToHtml(r.Style.BackColor));
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;
            for(int x = 0; x < _items.Count; x++)
				_items[x].Render(r);
			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("table");
			output.WriteLine();
			
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			
			if(r.Style.EndImage)
			{
				output.WriteFullBeginTag("td");
				output.WriteLine();
				output.Indent++;
				output.WriteBeginTag("img");
				output.WriteAttribute("src", r.ParentControl.ResolveUrl(r.Style.ButtonPath) +
					"toolbar.end." + 
					r.Style.ButtonExtension);
				output.WriteAttribute("border", "0");
				output.WriteAttribute("unselectable", "on");
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent--;
				output.WriteEndTag("td");
				output.WriteLine();
			}

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("table");
			output.WriteLine();
			
			output.Indent--;
			output.WriteEndTag("div");
			output.WriteLine();
		}

		#endregion

		#region Toolbar Script

		public void RegisterClientScriptBlock(Control parentControl)
		{
			AssemblyResourceHandler.RegisterScript(parentControl.Page, "Toolbar.js");
		}

		#endregion

	}
}