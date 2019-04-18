// DBMODEL MODE: READONLY

/* ********************************************************** *
 * AMNS Yari Media Record View                                *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.People;
using Amns.GreyFox.Yari.Amazon;

namespace Amns.GreyFox.Yari.WebControls
{
	/// <summary>
	/// Default web editor for YariMediaRecord.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
		ToolboxData("<{0}:YariMediaRecordView runat=server></{0}:YariMediaRecordView>")]
	public class YariMediaRecordView : System.Web.UI.Control
	{
		private int yariMediaRecordID;
		private string connectionString;
		protected string cssClass;
		protected string titleCssClass;
		protected Unit cellPadding;
		protected Unit cellSpacing;
		protected Unit borderWidth;
		protected Unit width;
		protected Unit height;

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")]
		public string CssClass
		{
			get
			{
				return cssClass;
			}
			set
			{
				cssClass = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit CellPadding
		{
			get
			{
				return cellPadding;
			}
			set
			{
				cellPadding = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit CellSpacing
		{
			get
			{
				return cellSpacing;
			}
			set
			{
				cellSpacing = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit BorderWidth
		{
			get
			{
				return borderWidth;
			}
			set
			{
				borderWidth = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		[Bindable(true), Category("Data"), DefaultValue(0)]
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

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public string TitleCssClass
		{
			get
			{
				return titleCssClass;
			}
			set
			{
				titleCssClass = value;
			}
		}	

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int YariMediaRecordID
		{
			get
			{
				return yariMediaRecordID;
			}
			set
			{
				yariMediaRecordID = value;
			}
		}	

		YariMediaRecord r;

		protected override void OnPreRender(EventArgs e)
		{			
			r = new YariMediaRecord(yariMediaRecordID);		
			AmazonHandler h = new AmazonHandler(r);
			h.Update();
		}

		protected override void Render(HtmlTextWriter output)
		{
			output.WriteBeginTag("table");
			if(CssClass != string.Empty)
				output.WriteAttribute("class", CssClass);
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(!BorderWidth.IsEmpty)
				output.WriteAttribute("border", BorderWidth.ToString());
			if(!Width.IsEmpty)
				output.WriteAttribute("width", Width.ToString());
			if(!Height.IsEmpty)
				output.WriteAttribute("height", Height.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteFullBeginTag("tr");
			if(r.ImageUrlMedium != string.Empty)
			{
				output.WriteBeginTag("td");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("rowspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("img");
				output.WriteAttribute("src", r.ImageUrlMedium);
				output.Write(HtmlTextWriter.SelfClosingTagEnd);
				output.WriteEndTag("td");

				output.WriteBeginTag("td");
				output.WriteAttribute("width", "100%");
				output.Write(HtmlTextWriter.TagRightChar);
			}
			else
			{
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("width", "100%");
				output.Write(HtmlTextWriter.TagRightChar);
			}
			
			output.WriteBeginTag("div");
			output.WriteAttribute("class", this.titleCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(r.Title);
			output.WriteEndTag("div");
			output.Write("by ");
			output.Write(r.Authors);
			output.WriteFullBeginTag("hr");

			if(r.AmazonReleaseDate != DateTime.MinValue && 
				r.AmazonReleaseDate != DateTime.Parse("12/30/1899"))
			{
				output.Write("<strong>Released:</strong> ");
				output.Write(r.AmazonReleaseDate.ToString("MMMM d, yyyy"));
			}
			else
			{
				output.Write("<strong>Published:</strong> ");
				output.Write(r.PublishYear);
			}

			if(r.Publisher != string.Empty)
			{
				output.Write("<br>");
				output.Write("<strong>Publisher:</strong> ");
				output.Write(r.Publisher);
			}

			if(r.Isbn != string.Empty)
			{
				output.Write("<br><strong>ISBN:</strong> ");
				output.Write(r.Isbn);
			}
			output.WriteEndTag("td");

//			output.WriteBeginTag("td");
//			output.WriteAttribute("valign", "top");
//			output.Write(HtmlTextWriter.TagRightChar);
//
////			if(r.AmazonListPrice != decimal.Zero)
//				output.Write("<strong>List Price:</strong> {0:c}<br>", r.AmazonListPrice);
////			if(r.AmazonListPrice != decimal.Zero)
//				output.Write("<strong>Amazon Price:</strong> {0:c}<br>", r.AmazonOurPrice);
//			if(r.AmazonAvailability != string.Empty)
//				output.Write("<strong>Availability:</strong> {0:c}", r.AmazonAvailability);
//			output.WriteEndTag("td");
			
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan", "3");
			output.Write(HtmlTextWriter.TagRightChar);
			if(r.AmazonAsin != string.Empty)
				output.Write(AmazonFunctions.RenderBuyButton(r.AmazonAsin));
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan", "3");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteBeginTag("a");
			output.WriteAttribute("href", "javascript://");
			output.WriteAttribute("onclick", "divAbstract.style.display='block';divContents.style.display='none';");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Abstract");
			output.WriteEndTag("a");
			output.Write(" | ");
			output.WriteBeginTag("a");
			output.WriteAttribute("href", "javascript://");
			output.WriteAttribute("onclick", "divAbstract.style.display='none';divContents.style.display='block';");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Contents");
			output.WriteEndTag("a");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan", "3");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteBeginTag("div");
			output.WriteAttribute("id", "divAbstract");
			output.WriteAttribute("style", "width:100%;");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(r.AbstractText);
			output.WriteEndTag("div");
			output.WriteBeginTag("div");
			output.WriteAttribute("id", "divContents");
			output.WriteAttribute("style", "width:100%;display='none';");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(r.ContentsText);
			output.WriteEndTag("div");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteEndTag("table");
		}

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null)
					base.LoadViewState(myState[0]);
				if(myState[1] != null)
					yariMediaRecordID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = yariMediaRecordID;
			return myState;
		}
	}
}
