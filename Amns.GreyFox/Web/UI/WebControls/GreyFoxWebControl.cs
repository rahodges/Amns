using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.Handlers;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for Amns.GreyFoxWebControl.
	/// </summary>
	public abstract class GreyFoxWebControl : System.Web.UI.Control
	{
		protected HtmlTextWriter __output;

		public GreyFoxWebControl()
		{

		}

		/// <summary>
		/// Registers client scripts for GreyFox Webcontrols using Resource Manifest.
		/// </summary>
		/// <param name="scriptName"></param>
		protected void RegisterClientScript(string scriptName)
		{
			// BE SURE TO REGISTER BASE.JS FIRST!
			if(scriptName != "Base.js")
				AssemblyResourceHandler.RegisterScript(this.Page, "Base.js");

			AssemblyResourceHandler.RegisterScript(this.Page, scriptName);
		}

		protected void InitializeRenderHelpers(HtmlTextWriter output)
		{
			__output = output;
		}

		/// <summary>
		/// Renders a table row with TR and TD elements with css Class attributes.
		/// </summary>
		/// <param name="cssClass">Css Class to apply to cells.</param>
		/// <param name="headerText">Text to use in the cells.</param>
		protected void RenderRow(string cssClass, params string[] cellText)
		{
			__output.WriteFullBeginTag("tr");
			__output.WriteLine();
			__output.Indent++;
			for(int i = 0; i <= cellText.GetUpperBound(0); i++)
			{
				__output.WriteBeginTag("td");
				__output.WriteAttribute("class", cssClass);
				__output.Write(HtmlTextWriter.TagRightChar);
				__output.Write(cellText[i]);
				__output.WriteEndTag("td");
				__output.WriteLine();
			}
			__output.Indent--;
			__output.WriteEndTag("tr");
			__output.WriteLine();
		}

		protected void RenderCell(string s)
		{
			__output.Indent++;
			__output.WriteBeginTag("td");			
			__output.WriteLine(HtmlTextWriter.TagRightChar);
			__output.Indent++;
			__output.Write(s);
			__output.WriteLine();
			__output.Indent--;
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
		}

		protected void RenderCell(params Control[] controls)
		{
			__output.Indent++;
			__output.WriteBeginTag("td");			
			__output.WriteLine(HtmlTextWriter.TagRightChar);

			__output.Indent++;
			foreach(Control c in controls)
				c.RenderControl(__output);
			__output.WriteLine();

			__output.Indent--;
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
		}

		protected void RenderCell(Control control, string attributes)
		{
			__output.Indent++;
			__output.WriteBeginTag("td");
			__output.Write(" ");
			__output.Write(attributes);
			__output.WriteLine(HtmlTextWriter.TagRightChar);
			__output.Indent++;
			control.RenderControl(__output);
			__output.WriteLine();
			__output.Indent--;
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
		}

		protected void RenderCell(string s, string attributes)
		{
			__output.Indent++;
			__output.WriteBeginTag("td");
			__output.Write(" ");
			__output.Write(attributes);
			__output.WriteLine(HtmlTextWriter.TagRightChar);
			__output.Indent++;
			__output.Write(s);
			__output.WriteLine();
			__output.Indent--;
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
		}

		protected void RenderCell(string s, string align, string colspan)
		{
			__output.Indent++;
			__output.WriteBeginTag("td");
			__output.WriteAttribute("align", align);
			__output.WriteAttribute("colspan", colspan);
			__output.WriteLine(HtmlTextWriter.TagRightChar);
			__output.Indent++;
			__output.Write(s);
			__output.WriteLine();
			__output.Indent--;
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
		}

		protected void RenderCell(string s, string align, string colspan, string rowspan)
		{
			__output.Indent++;
			__output.WriteBeginTag("td");
			__output.WriteAttribute("align", align);
			__output.WriteAttribute("colspan", colspan);
			__output.WriteAttribute("rowspan", rowspan);
			__output.WriteLine(HtmlTextWriter.TagRightChar);
			__output.Indent++;
			__output.Write(s);
			__output.WriteLine();
			__output.Indent--;
			__output.WriteEndTag("td");
			__output.WriteLine();
			__output.Indent--;
		}

		protected void RenderTableBeginTag(string id,
			Unit cellPadding, Unit cellSpacing, Unit width, Unit height, string cssClass)
		{
			__output.WriteBeginTag("table");
			if(!cellPadding.IsEmpty)
				__output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				__output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(cssClass != null && cssClass != string.Empty)
				__output.WriteAttribute("class", cssClass);
			if(!width.IsEmpty)
				__output.WriteAttribute("width", width.ToString());
			if(!height.IsEmpty)
				__output.WriteAttribute("height", height.ToString());
			__output.Write(HtmlTextWriter.TagRightChar);
		}
	}
}