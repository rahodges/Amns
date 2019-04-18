/* ********************************************************** *
 * AMNS Yari Media Record Grid                                *
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
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.People;
using Amns.GreyFox.Data;

namespace Amns.GreyFox.Yari.WebControls
{
	/// <summary>
	/// A custom grid for YariMediaRecord.
	/// </summary>
	[DefaultProperty("ConnectionString"),
		ToolboxData("<{0}:YariMediaRecordGrid runat=server></{0}:YariMediaRecordGrid>")]
	public class YariMediaRecordGrid : TableGrid
	{
		private string connectionString;

		private string selectionUrl = "";
		private string matchStyle = "background-color: #FFFF00;";
		private bool thumbsEnabled = true;
		private DropDownList ddSearchType = new DropDownList();
		private TextBox tbSearchText = new TextBox();
		private Button btGoSearch = new Button();
		private bool emptySearchEnabled = false;
		private bool cacheEnabled = false;

		#region Public Properties

		public bool CacheEnabled
		{
			get
			{
				return cacheEnabled;
			}
			set
			{
				cacheEnabled = value;
			}
		}

		public bool EmptySearchEnabled
		{
			get
			{
				return emptySearchEnabled;
			}
			set
			{
				emptySearchEnabled = value;
			}
		}

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

		public string SearchType
		{
			get
			{
				if(ddSearchType.SelectedItem != null)
                    return ddSearchType.SelectedItem.Value;
				else
					return "None";
			}
			set
			{
				EnsureChildControls();
				bool valueSet = false;
				for(int x = 0; x < ddSearchType.Items.Count; x++)
					valueSet |=
						ddSearchType.Items[x].Selected =
						ddSearchType.Items[x].Value == value;
				if(!valueSet)
					ddSearchType.Items[0].Selected = true;
			}
		}

		public string SearchQuery
		{
			get
			{
				return tbSearchText.Text;
			}
			set
			{
				tbSearchText.Text = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			if(ddSearchType.Items.Count == 0)
			{
				ddSearchType.Items.Add(new ListItem("Title", "Title"));
				ddSearchType.Items.Add(new ListItem("Author", "Author"));
				ddSearchType.Items.Add(new ListItem("Keywords", "Keywords"));
				ddSearchType.Items.Add(new ListItem("Contents", "Contents"));
				ddSearchType.Items.Add(new ListItem("Abstract", "Abstract"));
			}

			Controls.Add(ddSearchType);
			Controls.Add(tbSearchText);
			btGoSearch.Text = "Go";
			btGoSearch.Width = Unit.Pixel(72);
			Controls.Add(btGoSearch);
			ChildControlsCreated = true;
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation;
			components = TableWindowComponents.Toolbar;
		}

		#region Rendering

		protected override void RenderToolbar(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.SubHeaderCssClass);
			output.WriteAttribute("height", "28px");
			output.WriteAttribute("colspan", this.columnCount.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;	
			output.Write("Search for : ");
			tbSearchText.RenderControl(output);
			output.Write(" using ");
			ddSearchType.RenderControl(output);
			output.Write(" ");
			btGoSearch.RenderControl(output);
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
			string searchText = tbSearchText.Text.Trim().Replace("'", "''");
			string searchType = ddSearchType.SelectedItem.Value;
			string cacheString = string.Empty;
			
			if(!emptySearchEnabled & searchText == string.Empty)
				return;				

			EnsureChildControls();
			
			YariMediaRecordCollection yariMediaRecordCollection = null;

			if(cacheEnabled)
			{
				cacheString = "yariSearchCache_" + 
					searchType + "_" + searchText;
				if(Context.Cache[cacheString] != null)
					yariMediaRecordCollection = (YariMediaRecordCollection) Context.Cache[cacheString];
				else
					yariMediaRecordCollection = null;
			}

			if(yariMediaRecordCollection == null)
			{			
				YariMediaRecordManager m = new YariMediaRecordManager();
			
				switch(searchType)
				{
					case "Title":
						if(searchText == string.Empty)
							goto default;
						yariMediaRecordCollection = m.SearchByTitle(searchText);
						break;
					case "Author":
						yariMediaRecordCollection = m.SearchByAuthor(searchText);
						break;
					case "Keywords":
						yariMediaRecordCollection = m.SearchByKeywords(searchText);
						break;
					case "Contents":
						yariMediaRecordCollection = m.SearchByContents(searchText);
						initializeSearchResult(searchText);
						// Replaces contents text with highlighted search result
						for(int x = 0; x < yariMediaRecordCollection.Count; x++)
							yariMediaRecordCollection[x].ContentsText =
								constructSearchResult(searchText, yariMediaRecordCollection[x].ContentsText);
						break;
					case "Abstract":
						yariMediaRecordCollection = m.SearchByAbstract(searchText);
						initializeSearchResult(searchText);
						// Replaces abstract text with highlighted search result
						for(int x = 0; x < yariMediaRecordCollection.Count; x++)
							yariMediaRecordCollection[x].AbstractText =
								constructSearchResult(searchText, yariMediaRecordCollection[x].AbstractText);
						break;
					default:
						yariMediaRecordCollection = m.GetCollection(string.Empty,
							"Title", null);
						break;
				}

				if(cacheEnabled)
					Context.Cache.Add(cacheString, yariMediaRecordCollection, null,
						DateTime.Now.AddMinutes(5), TimeSpan.Zero, 
						System.Web.Caching.CacheItemPriority.Normal, null);
			}
			
			bool rowflag = false;
			string rowCssClass;

			thumbsEnabled &= yariMediaRecordCollection.Count < 25;

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			if(thumbsEnabled)
				output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(yariMediaRecordCollection.Count);
			output.Write(" results found.");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Records
			//
			YariMediaRecord yariMediaRecord;
			for(int x = 0; x < yariMediaRecordCollection.Count; x++)
			{
				yariMediaRecord = yariMediaRecordCollection[x];

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
					if(yariMediaRecord.ImageUrlSmall != string.Empty)
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
						output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "select_" + yariMediaRecord.ID));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(yariMediaRecord.Title);
					output.WriteEndTag("a");
				}
				else
				{
					output.Write(yariMediaRecord.Title);
				}
				
				output.Write("<br>");
				if(yariMediaRecord.Authors == null)
					output.Write("Unknown");
				else
                    output.Write(yariMediaRecord.Authors.ToString());

				if(searchType == "Abstract")
				{
					output.WriteFullBeginTag("br");
					output.Write(yariMediaRecord.AbstractText);
					output.WriteFullBeginTag("br");
				}
				else if(searchType == "Contents")
				{
					output.WriteFullBeginTag("br");
					output.Write(yariMediaRecord.ContentsText);
					output.WriteFullBeginTag("br");
				}
				
				output.WriteEndTag("td");
				output.WriteEndTag("tr");
			}
		}
		#endregion
		
		Regex hightlightRegex;
		Regex removeHtmlTagsRegEx;
		MatchEvaluator searchMatchEvaluator;
		MatchCollection matchCollection;

		void initializeSearchResult(string keywordString)
		{
			StringBuilder expression = new StringBuilder();
			string[] keywords = keywordString.Replace("+", "").Split(',', ';', ' ');
			for(int x = 0; x < keywords.Length; x++)
			{
				if(expression.Length > 0)
					expression.Append('|');
				expression.Append(keywords[x]);
			}

			searchMatchEvaluator = new MatchEvaluator(highlightEvaluator);
			hightlightRegex = new Regex(expression.ToString(), RegexOptions.IgnoreCase |
				RegexOptions.Compiled);
			removeHtmlTagsRegEx = new Regex("<[^>]+>", RegexOptions.Compiled);
		}

		string constructSearchResult(string keywordString, string text)
		{
			string[] sentances = text.Split('.', ';');
			for(int x = 0; x < sentances.Length; x++)
			{
				matchCollection = hightlightRegex.Matches(sentances[x]);
				
				if(matchCollection.Count == 0)
					continue;

				// remove html tags, highlight search match and add the last period
				text = removeHtmlTagsRegEx.Replace(sentances[x], "");
				text = hightlightRegex.Replace(text, searchMatchEvaluator);
				text += ".";

				return text;
			}

			return string.Empty;
		}

		string highlightEvaluator(System.Text.RegularExpressions.Match match) 
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			s.Append("<span style=\"");
			s.Append(matchStyle);
			s.Append("\">");
			s.Append(match.Value);
			s.Append("</span>");
			return s.ToString();
		}

		#region Viewstate Methods

		protected override void LoadViewState(object savedState) 
		{
			EnsureChildControls();
			// Customize state management to handle saving state of contained objects.
			if (savedState != null) 
			{
				object[] myState = (object[])savedState;
				if (myState[0] != null) base.LoadViewState(myState[0]);
				if (myState[1] != null) selectedID = (int) myState[1];
			}
		}

		protected override object SaveViewState() 
		{
			// Customized state management to handle saving state of contained objects  such as styles.
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = selectedID;
			return myState;
		}
		#endregion
	}
}
