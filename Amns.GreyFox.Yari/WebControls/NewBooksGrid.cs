/* ********************************************************** *
 * AMNS Yari New Nooks Grid                                   *
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
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.People;

namespace Amns.GreyFox.Yari.WebControls
{
	/// <summary>
	/// A custom grid for YariMediaRecord.
	/// </summary>
	[DefaultProperty("ConnectionString"),
		ToolboxData("<{0}:NewBooksGrid runat=server></{0}:NewBooksGrid>")]
	public class NewBooksGrid : TableGrid
	{
		private string connectionString;
		private string selectionUrl = "";
		private bool thumbsEnabled = true;

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

		[Bindable(true),
		Category("Behavior"),
		DefaultValue("")]
		public string SelectionUrl
		{
			get
			{
				return selectionUrl;
			}
			set
			{
				selectionUrl = value;
			}
		}

		#endregion

		public NewBooksGrid() : base()
		{
			columnCount = 2;
			components = 0;
			features = TableWindowFeatures.DisableContentSeparation;			
		}

		#region Rendering

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			YariMediaRecordCollection yariMediaRecordCollection = null;

			EnsureChildControls();			

			if(Page.Cache["Yari_NewBooks"] != null)
			{
				yariMediaRecordCollection = (YariMediaRecordCollection) Page.Cache["Yari_NewBooks"];
			}
			else
			{
				YariMediaRecordManager m = new YariMediaRecordManager();
			
				yariMediaRecordCollection = m.SearchByPublishDate(8, 
					DateTime.Now.Subtract(TimeSpan.FromDays(365)),
					DateTime.Now.Add(TimeSpan.FromDays(365)));

				Page.Cache.Add("Yari_NewBooks",yariMediaRecordCollection, null, 
					DateTime.MaxValue, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal,
					null);
			}			
			
			bool rowflag = false;
			string rowCssClass;

			thumbsEnabled &= yariMediaRecordCollection.Count < 25;

			//
			// Render Records
			//
			foreach(YariMediaRecord yariMediaRecord in yariMediaRecordCollection)
			{
				if(yariMediaRecord.ID == selectedID) rowCssClass = selectedRowCssClass;
				else if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
				rowflag = !rowflag;
				output.WriteFullBeginTag("tr");
				
				//
				// Render Book Image If Available
				//
				if(thumbsEnabled)
				{
					output.WriteBeginTag("td");
					output.WriteAttribute("align", "middle");
					output.WriteAttribute("class", rowCssClass);
					output.Write(HtmlTextWriter.TagRightChar);
					if(yariMediaRecord.ImageUrlSmall.Length > 0)
					{
						output.WriteBeginTag("img");
						output.WriteAttribute("src", yariMediaRecord.ImageUrlSmall);
						output.WriteAttribute("border", "0");
						output.Write(HtmlTextWriter.TagRightChar);
					}
					output.WriteEndTag("td");
				}
				
				//
				// Render Main Representation of Record
				//
				output.WriteBeginTag("td");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("width", "100%");
				output.WriteAttribute("class", rowCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				if(selectEnabled)
				{
					output.WriteBeginTag("a");
					if(selectionUrl != "")
						output.WriteAttribute("href", selectionUrl + "?id=" + yariMediaRecord.ID);
					else
						output.WriteAttribute("href", "javascript:" + 
                            Page.ClientScript.GetPostBackEventReference(this, "select_" + yariMediaRecord.ID));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(yariMediaRecord.Title);
					output.WriteEndTag("a");
				}
				else
				{
					output.Write(yariMediaRecord.Title);
				}
				
				output.WriteEndTag("td");
				output.WriteEndTag("tr");
			}
		}
		#endregion
	}
}
