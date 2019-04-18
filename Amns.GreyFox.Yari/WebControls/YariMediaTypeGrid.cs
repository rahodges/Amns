/* ********************************************************** *
 * AMNS Yari Media Type Grid                                 *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Yari.WebControls
{
	/// <summary>
	/// A custom grid for YariMediaType.
	/// </summary>
	[DefaultProperty("ConnectionString"),
	ToolboxData("<{0}:YariMediaTypeGrid runat=server></{0}:YariMediaTypeGrid>")]
	public class YariMediaTypeGrid : TableGrid
	{
		private string connectionString;

		#region Public Properties
		[Bindable(false),
		Category("Data"),
		DefaultValue("")]
		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				connectionString = value;
			}
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			columnCount = 1;
			features = TableWindowFeatures.DisableContentSeparation |
				TableWindowFeatures.Scroller;
			components = TableWindowComponents.Toolbar;
		}

		#region Rendering

		protected override void RenderToolbar(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;
			output.WriteBeginTag("td");
			output.WriteAttribute("align", "right");
			output.WriteAttribute("class", this.SubHeaderCssClass);
			output.WriteAttribute("height", "28px");
			output.WriteAttribute("colspan", this.columnCount.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;	
			// RENDER OBJECTS
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			EnsureChildControls();
			YariMediaTypeManager m = new YariMediaTypeManager();
			YariMediaTypeCollection yariMediaTypeCollection = m.GetCollection(string.Empty, string.Empty);
			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(YariMediaType yariMediaType in yariMediaTypeCollection)
			{
				if(yariMediaType.ID == selectedID) rowCssClass = selectedRowCssClass;
				else if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
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
				//				output.Write(yariMediaType.ID);
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
					output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "select_" + yariMediaType.ID));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(yariMediaType.ToString());
					output.WriteEndTag("a");
				}
				else
				{
					output.Write(yariMediaType.ToString());
				}
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
