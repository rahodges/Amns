using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Summary description for MemberListGrid.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:GreyFoxRoleGrid runat=server></{0}:GreyFoxRoleGrid>")]
	public class GreyFoxRoleGrid : TableGrid
	{
		#region Rendering
        
		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			EnsureChildControls();

			GreyFoxRoleManager m = new GreyFoxRoleManager();
			GreyFoxRoleCollection roles = m.GetCollection(string.Empty, string.Empty);

			bool rowflag = false;
			string rowCssClass;		

			//
			// Render Records
			//
			foreach(GreyFoxRole role in roles)
			{				
				if(rowflag)
					rowCssClass = this.defaultRowCssClass;
				else
					rowCssClass = this.alternateRowCssClass;

				rowflag = !rowflag;

				output.WriteFullBeginTag("tr");
				output.WriteLine();
				output.Indent++;
	
				//
				// Render ID of Record
				//
//				output.WriteBeginTag("td");
//				output.WriteAttribute("class", rowCssClass);
//				output.Write(HtmlTextWriter.TagRightChar);
//				output.Write(role.iD);
//				output.WriteEndTag("td");
//				output.WriteLine();

				//
				// Render Main Representation of Record
				//				
				output.WriteBeginTag("td");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("class", rowCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				
				if(selectEnabled)
				{
					output.WriteBeginTag("a");
					output.WriteAttribute("href", "javascript:" + 
                        Page.ClientScript.GetPostBackEventReference(this, "sel_" + role.iD));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(role.Name);					
					output.WriteEndTag("a");
				}
				else
				{
					output.WriteFullBeginTag("strong");
					output.Write(role.name);
					output.WriteEndTag("strong");
				}
				output.WriteEndTag("td");
				output.WriteLine();

				//
				// Render Disabled Status
				//
				output.WriteBeginTag("td");
				output.WriteAttribute("class", rowCssClass);
				output.WriteAttribute("valign", "top");
				output.Write(HtmlTextWriter.TagRightChar);
				if(role.isDisabled)
					output.Write("Disabled");
				else
					output.Write("&nbsp;");
				output.WriteEndTag("td");
				output.WriteLine();

				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
		}

		#endregion
	}
}