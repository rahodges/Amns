using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	public enum DbContentClientGridSortType { PublishDate, OrderNum, MenuOrder };
	public enum DbContentClientGridDateStyle { None, TitleRight, TitleBottom, DescriptionUnder, DescriptionEnd, HeaderFloatRight };

	/// <summary>
	/// <summary>
	/// A custom grid for DbContentClip.
	/// </summary>
	/// </summary>
	[DefaultProperty("ConnectionString"),
		ToolboxData("<{0}:DbContentClientGrid runat=server></{0}:DbContentClientGrid>")]
	public class DbContentClientGrid : TableWindow
	{
		private int catalogID							= 4;
		private string bulletIcon						= string.Empty;
		private bool leftIcons							= false;
		private bool titles								= true;
		private bool descriptions						= true;
        private bool bodies								= false;
		private string linkFormat						= "?ref={0}";
		private string dateFormat						= "(MMMM d)";
		private DbContentClientGridSortType sortType	= DbContentClientGridSortType.PublishDate;
		private DbContentClientGridDateStyle dateStyle	= DbContentClientGridDateStyle.HeaderFloatRight;
		private DateTime maxPublishDate					= DateTime.Now;
		private DateTime minPublishDate					= DateTime.MinValue;

		private DbContentClipCollection clips;

		#region Public Properties
		
		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int CatalogID
		{
			get { return catalogID; }
			set { catalogID = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string BulletIcon
		{
			get { return bulletIcon; }
			set { bulletIcon = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue(false)]
		public bool LeftIcons
		{
			get { return leftIcons; }
			set { leftIcons = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true)]
		public bool Titles
		{
			get { return titles; }
			set { titles = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue(true)]
		public bool Descriptions
		{
			get { return descriptions; }
			set { descriptions = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue(false)]
		public bool Bodies
		{
			get { return bodies; }
			set { bodies = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("?ref={0}")]
		public string LinkFormat
		{
			get { return linkFormat; }
			set { linkFormat = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("MMMM d")]
		public string DateFormat
		{
			get { return dateFormat; }
			set { dateFormat = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue(DbContentClientGridSortType.PublishDate)]
		public DbContentClientGridSortType SortType
		{
			get { return sortType; }
			set { sortType = value; }
		}
		
		[Bindable(true), Category("Behavior"), DefaultValue(DbContentClientGridDateStyle.HeaderFloatRight)]
		public DbContentClientGridDateStyle DateStyle
		{
			get { return dateStyle; }
			set { dateStyle = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public DateTime MinPublishDate
		{
			get { return minPublishDate; }
			set { minPublishDate = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public DateTime MaxPublishDate
		{
			get { return maxPublishDate; }
			set { maxPublishDate = value; }
		}

		#endregion

		#region Rendering

		protected override void OnInit(EventArgs e)
		{
			columnCount = 1;
			features = TableWindowFeatures.DisableContentSeparation;
			components = 0;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(dateStyle == DbContentClientGridDateStyle.TitleRight)
				columnCount++;
			if(leftIcons == true || bulletIcon != string.Empty)
				columnCount++;
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			string sortToken;
			
			switch(sortType)
			{
				case DbContentClientGridSortType.PublishDate:
					sortToken = "PublishDate DESC";
					break;
				default:
					sortToken = sortType.ToString();
					break;
			}

			DbContentClipManager m = new DbContentClipManager();
			clips =	m.GetCollection("ParentCatalogID=" + catalogID.ToString(), sortToken, null);

			// Find the latest publish date
			DateTime latestPublishDate = DateTime.MinValue;
			foreach(DbContentClip clip in clips)
				if(clip.PublishCheck(this.minPublishDate, this.maxPublishDate) &&
					clip.PublishDate > latestPublishDate)
					latestPublishDate = clip.PublishDate;

//			if(dateStyle == DbContentClientGridDateStyle.HeaderFloatRight)
//			{
//				this.text = "<div style=\"float:right;\">" + 
//					latestPublishDate.ToString(dateFormat) + "</div>" +
//					this.text;
//			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			//
			// Render Records
			//
			foreach(DbContentClip dbContentClip in clips)
			{
				if(!dbContentClip.PublishCheck(this.minPublishDate, this.maxPublishDate))
                    continue;

//				if(dbContentClip.ID == selectedID) rowCssClass = selectedRowCssClass;
//				else if(rowflag) rowCssClass = defaultRowCssClass;
//				else rowCssClass = alternateRowCssClass;
//				rowflag = !rowflag;

				if(titles)
				{
					output.WriteFullBeginTag("tr");
					output.WriteLine();
					output.Indent++;

					if(bulletIcon != string.Empty || leftIcons)
					{
						output.WriteBeginTag("td");
						output.WriteAttribute("valign", "top");
						output.WriteAttribute("class", SubHeaderCssClass);
						if(descriptions)
							output.WriteAttribute("rowspan", "2");
						output.Write(HtmlTextWriter.TagRightChar);
						output.WriteLine();
						output.Indent++;
						output.WriteBeginTag("img");
						output.WriteAttribute("src", bulletIcon);
						output.Write(HtmlTextWriter.TagRightChar);
						output.WriteLine();
						output.Indent--;
						output.WriteEndTag("td");
						output.WriteLine();
					}
					
					output.WriteBeginTag("td");
					output.WriteAttribute("valign", "top");
					output.WriteAttribute("class", SubHeaderCssClass);
					output.WriteAttribute("width", "100%");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteLine();
					output.Indent++;

					int refNum = -1;

					if(linkFormat != string.Empty & dbContentClip.Body != string.Empty)
					{
						if(dbContentClip.Body.StartsWith("<?xml"))
						{
							Extensions.DbCommandClip c = new Extensions.DbCommandClip(dbContentClip);
							DbContentClip forward = c.ForwardReference;
							if(forward != null)
								refNum = forward.ID;
						}
						else
						{
							refNum = dbContentClip.ID;
						}
					}

					if(dbContentClip.OverrideUrl != string.Empty)
					{
						output.WriteBeginTag("a");
						output.WriteAttribute("href", Page.ResolveUrl(dbContentClip.OverrideUrl));
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write(dbContentClip.Title);
						output.WriteEndTag("a");
					}
					else if(refNum != -1)
					{
						output.WriteBeginTag("a");
						output.WriteAttribute("href", Page.ResolveUrl(string.Format(linkFormat, refNum)));
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write(dbContentClip.Title);
						output.WriteEndTag("a");
					}
					else
					{
						output.Write(dbContentClip.Title);
					}
                    
					if(dateStyle == DbContentClientGridDateStyle.TitleBottom)
					{
//						output.WriteFullBeginTag("tr");
//						output.WriteFullBeginTag("td");
						output.Write("<br>");
						output.Write(dbContentClip.PublishDate.ToString(dateFormat));
//						output.WriteEndTag("td");
//						output.WriteEndTag("tr");
					}

					output.WriteLine();
					output.Indent--;
                    output.WriteEndTag("td");
					output.WriteLine();

					if(dateStyle == DbContentClientGridDateStyle.TitleRight)
					{
						output.WriteBeginTag("td");
						output.WriteAttribute("valign", "top");
						output.WriteAttribute("align", "right");
						output.WriteAttribute("nowrap", "true");
						if(SubHeaderCssClass != string.Empty)
							output.WriteAttribute("class", SubHeaderCssClass);
						output.Write(HtmlTextWriter.TagRightChar);
						output.WriteLine();
						output.Indent++;
						output.Write(dbContentClip.PublishDate.ToString(dateFormat));
						output.WriteLine();
						output.Indent--;
						output.WriteEndTag("td");
					}
					output.WriteLine();

					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				if(descriptions)
				{
					output.WriteFullBeginTag("tr");
					output.WriteLine();
					output.Indent++;

//					if(bulletIcon != string.Empty || leftIcons)
//					{
//						output.WriteFullBeginTag("td");
//						output.Write("&nbsp;");
//						output.WriteEndTag("td");
//                        output.WriteLine();						
//					}

					output.WriteBeginTag("td");
					output.WriteAttribute("valign", "top");
//					output.WriteAttribute("class", rowCssClass);
					if(columnCount != 1) output.WriteAttribute("colspan", columnCount.ToString());
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteLine();
					output.Indent++;
					output.Write(dbContentClip.Description); 
					if(dateStyle == DbContentClientGridDateStyle.DescriptionUnder)
					{
						output.WriteBeginTag("div");
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write(dbContentClip.PublishDate.ToString(dateFormat));
						output.WriteEndTag("div");
					}
					else if(dateStyle == DbContentClientGridDateStyle.DescriptionEnd)
					{
						output.Write(dbContentClip.PublishDate.ToString(dateFormat));
					}
					output.WriteLine();
					output.Indent--;
					output.WriteEndTag("td");
					output.WriteLine();

					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				if(bodies)
				{
					output.WriteFullBeginTag("tr");
					output.WriteLine();
					output.Indent++;

					if(bulletIcon != string.Empty || leftIcons)
					{
						output.WriteFullBeginTag("td");
						output.Write("&nbsp;");
						output.WriteEndTag("td");
						output.WriteLine();						
					}

					output.WriteBeginTag("td");
					output.WriteAttribute("valign", "top");
//					output.WriteAttribute("class", rowCssClass);
					if(columnCount != 1) output.WriteAttribute("colspan", columnCount.ToString());
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteLine();
					output.Indent++;
					output.Write(dbContentClip.Body);
					output.WriteLine();
					output.Indent--;
					output.WriteEndTag("td");
					output.WriteLine();

					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}
			}
		}

		#endregion
	}
}
