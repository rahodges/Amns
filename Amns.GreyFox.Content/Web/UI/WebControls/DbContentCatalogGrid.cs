using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for DbContentCatalog.
	/// </summary>
	/// </summary>
	[DefaultProperty("ConnectionString"),
	ToolboxData("<{0}:DbContentCatalogGrid runat=server></{0}:DbContentCatalogGrid>")]
	public class DbContentCatalogGrid : TableGrid
	{
		string connectionString;
		DbContentCatalogCollection catalogs;

		bool isCatalogSelected = true;
		int selectedClipID = -1;
		int copyClipID = -1;
		int copyCatalogID = -1;
		int indent = 0;
		int indentMultiplier = 21;

		string expiredIcon		= "~/amns_greyfox_client/1_75_2088/toolbars/clip_expired.gif";
		string publishedIcon	= "~/amns_greyfox_client/1_75_2088/toolbars/clip_published.gif";
		string prePublishedIcon	= "~/amns_greyfox_client/1_75_2088/toolbars/clip_prepublish.gif";
		string blankIcon		= "~/amns_greyfox_client/1_75_2088/toolbars/blank.gif";
		string plusIcon			= "~/amns_greyfox_client/1_75_2088/toolbars/plus.gif";
		string minusIcon		= "~/amns_greyfox_client/1_75_2088/toolbars/minus.gif";
		string catalogIcon		= "~/amns_greyfox_client/1_75_2088/toolbars/catalog.gif";
		
		bool closeLastCatalog	= false;
		string[] openCatalogs	= new string[0];


		CheckBox cbExpired		= new CheckBox();

//		Menu clipContextMenu	= new Menu();

		/// <summary>
		/// Ensures that the catalogs are loaded.
		/// </summary>
		private void EnsureCatalogs()
		{
			if(catalogs == null)
			{
				DbContentCatalogManager m = new DbContentCatalogManager();
				catalogs = m.GetCollection(string.Empty, string.Empty, null);
				catalogs.Sort(ContentCompareKey.MenuOrder);
			}
		}

		#region Public Properties

		[Bindable(true), Category("Behavior"), DefaultValue("-1")]
		public int SelectedClipID
		{
			get { return selectedClipID; }
			set { selectedClipID = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("-1")]
		public bool IsCatalogSelected
		{
			get { return isCatalogSelected; }
			set { isCatalogSelected = value; }
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
                
                
				// Parse Connection String
				if(value.StartsWith("<jet40virtual>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(value.Substring(14, value.Length - 14));
				else if(value.StartsWith("<jet40config>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get(value.Substring(13, value.Length - 13)));
				else
					connectionString = value;
			}
		}

		#endregion

        protected override void CreateChildControls()
        {
            cbExpired.Text = "Expired";
            cbExpired.EnableViewState = true;
            cbExpired.AutoPostBack = true;
            Controls.Add(cbExpired);

            //			clipContextMenu.AutoPostBackOnSelect = false;
            //			clipContextMenu.ExpandSlide = SlideType.None;
            //			clipContextMenu.ExpandTransition = TransitionType.Fade;
            //			clipContextMenu.ExpandDelay = 250;
            //			clipContextMenu.CollapseSlide = SlideType.None;
            //			clipContextMenu.CollapseTransition = TransitionType.Fade;
            //			clipContextMenu.Orientation = GroupOrientation.Vertical;
            //			clipContextMenu.EnableViewState = false;
            //			clipContextMenu.ContextMenu = ContextMenuType.ControlSpecific;
            //			clipContextMenu.ContextControlId = this.ID;
            //
            //			MenuItem editItem = new MenuItem();
            //			editItem.Text = "Edit";
            //			clipContextMenu.Items.Add(editItem);
            //
            //			Controls.Add(clipContextMenu);

            ToolBarUtility.CommandItem("New Catalog", "New Catalog", "newcatalog.gif");
            ToolBarUtility.CommandItem("Up", "Up", "up.gif");
            ToolBarUtility.CommandItem("Down", "Down", "down.gif");
            ToolBarUtility.CommandItem("Copy", "Copy", "copy.gif");
            ToolBarUtility.CommandItem("Paste", "Paste", "paste.gif");
            ToolBarUtility.CommandItem("rPaste", "rPaste", "rpaste.gif");

            base.CreateChildControls();
        }

        protected override void itemCommand(object sender, ToolBarItemEventArgs e)
        {
            base.itemCommand(sender, e);

            switch (e.Item.Value)
            {
                case "Up":
                    if (IsCatalogSelected)
                    {
                        DbContentCatalogManager.PeerMove(new DbContentCatalog(this.SelectedID), -1, true, false);
                    }
                    else
                    {
                        DbContentClipManager.PeerMove(new DbContentClip(this.SelectedClipID), -1, true, false);
                    }
                    break;
                case "Down":
                    if (IsCatalogSelected)
                    {
                        DbContentCatalogManager.PeerMove(new DbContentCatalog(this.SelectedID), -1, true, false);
                    }
                    else
                    {
                        DbContentClipManager.PeerMove(new DbContentClip(this.SelectedClipID), 1, true, false);
                    }
                    break;
                case "Copy":
                    if (isCatalogSelected)
                    {
                        copyCatalogID = selectedID;
                        copyClipID = -1;
                    }
                    else
                    {
                        copyCatalogID = -1;
                        copyClipID = selectedClipID;
                    }
                    break;
                case "Paste":
                    if (copyCatalogID != -1)
                    {
                        DbContentCatalog srcCatalog;
                        DbContentCatalog destCatalog;

                        srcCatalog = new DbContentCatalog(copyCatalogID);

                        if (isCatalogSelected)
                        {
                            destCatalog = DbContentCatalog.NewPlaceHolder(selectedID);
                        }
                        else
                        {
                            destCatalog = new DbContentClip(selectedClipID).ParentCatalog;
                        }

                        pasteCatalog(srcCatalog, destCatalog, false, false);
                    }
                    else if (copyClipID != -1)
                    {
                        DbContentClip copyClip = new DbContentClip(selectedClipID);
                        DbContentClip newClip = new DbContentClip();
                        newClip.ArchiveDate = copyClip.ArchiveDate;
                        newClip.Authors = copyClip.Authors;
                        newClip.Body = copyClip.Body;
                        newClip.CommentsEnabled = copyClip.CommentsEnabled;
                        newClip.CreateDate = DateTime.Now;
                        newClip.Description = copyClip.Description;
                        newClip.Editors = copyClip.Editors;
                        newClip.ExpirationDate = copyClip.ExpirationDate;
                        newClip.Icon = copyClip.Icon;
                        newClip.Keywords = copyClip.Keywords;
                        newClip.MenuBreak = copyClip.MenuBreak;
                        newClip.MenuEnabled = copyClip.MenuEnabled;
                        newClip.MenuLabel = copyClip.MenuLabel;
                        newClip.MenuLeftIcon = copyClip.MenuLeftIcon;
                        newClip.MenuLeftIconOver = copyClip.MenuLeftIconOver;
                        newClip.MenuRightIcon = copyClip.MenuRightIcon;
                        newClip.MenuRightIconOver = copyClip.MenuRightIconOver;
                        newClip.MenuTooltip = copyClip.MenuTooltip;
                        newClip.ModifyDate = DateTime.Now;
                        newClip.NotifyOnComments = copyClip.NotifyOnComments;
                        newClip.OverrideUrl = copyClip.OverrideUrl;
                        if (isCatalogSelected)
                            newClip.ParentCatalog = DbContentCatalog.NewPlaceHolder(selectedID);
                        else
                            newClip.ParentCatalog = new DbContentClip(selectedClipID).ParentCatalog;
                        newClip.Priority = copyClip.Priority;
                        newClip.PublishDate = copyClip.PublishDate;
                        newClip.Rating = copyClip.Rating;
                        newClip.References = copyClip.References;
                        newClip.SortOrder = copyClip.SortOrder;
                        newClip.Status = copyClip.Status;
                        newClip.Title = copyClip.Title;
                        newClip.WorkingDraft = copyClip.WorkingDraft;
                        newClip.Save();
                    }
                    break;
                case "rPaste":
                    if (copyCatalogID != -1)
                    {
                        DbContentCatalog srcCatalog;
                        DbContentCatalog destCatalog;

                        srcCatalog = new DbContentCatalog(copyCatalogID);

                        if (isCatalogSelected)
                        {
                            destCatalog = DbContentCatalog.NewPlaceHolder(selectedID);
                        }
                        else
                        {
                            destCatalog = new DbContentClip(selectedClipID).ParentCatalog;
                        }

                        pasteCatalog(srcCatalog, destCatalog, true, true);
                    }
                    break;
            }
        }

		public override void ProcessPostBackEvent(string eventArgument)
		{
			int underscoreIndex = eventArgument.IndexOf("_");
			if(underscoreIndex != -1)
			{
				string commandName = eventArgument.Substring(0, underscoreIndex);
				string parameters = eventArgument.Substring(underscoreIndex + 1);			

				switch(commandName)
				{					
					case "csel":
						selectedClipID = int.Parse(parameters);
						isCatalogSelected = false;
						break;
					case "tog":
						toggleCatalog(int.Parse(parameters));
						break;
					case "sel":												
						int newCatalogID = int.Parse(parameters);

						// If the last catalog was opened by a selection
						// instead of clicking "plus", close it only if
						// it is not a child of the parent catalog.
						if(closeLastCatalog)
						{
							EnsureCatalogs();

							// Find Catalog and see if it's parent is the selected catalog
							bool close = true;
							for(int x = 0; x < catalogs.Count; x++)
							{
								DbContentCatalog c = catalogs[x];
								if(c.ID == newCatalogID &&
									c.ParentCatalog != null && c.ParentCatalog.ID == selectedID)
								{
									close = false;
									break;
								}
							}

							if(close)						
								toggleCatalog(selectedID);

							closeLastCatalog = false;							
						}

						selectedID = newCatalogID;
						isCatalogSelected = true;

						// If the catalog is not open, open it.
						if(!catalogIsOpen(selectedID))
						{
							closeLastCatalog = true;
							toggleCatalog(selectedID);
						}
						break;
					default:
						base.ProcessPostBackEvent(eventArgument);
						break;
				}
			}
		}

		private int pasteCatalog(DbContentCatalog srcCatalog, DbContentCatalog destCatalog,
			bool clipPaste, bool recurse)
		{
			DbContentCatalog newCatalog;
			
			newCatalog = new DbContentCatalog();

			newCatalog.AuthorRole = srcCatalog.AuthorRole;
			newCatalog.CommentsEnabled = srcCatalog.CommentsEnabled;
			newCatalog.CreateDate = DateTime.Now;
			newCatalog.DefaultArchive = srcCatalog.DefaultArchive;
			newCatalog.DefaultClip = srcCatalog.DefaultClip;
			newCatalog.DefaultKeywords = srcCatalog.DefaultKeywords;
			newCatalog.DefaultMenuLeftIcon = srcCatalog.DefaultMenuLeftIcon;
			newCatalog.DefaultMenuRightIcon = srcCatalog.DefaultMenuRightIcon;
			newCatalog.DefaultRating = srcCatalog.DefaultRating;
			newCatalog.DefaultStatus = srcCatalog.DefaultStatus;
			newCatalog.DefaultTimeToArchive = srcCatalog.DefaultTimeToArchive;
			newCatalog.DefaultTimeToExpire = srcCatalog.DefaultTimeToExpire;
			newCatalog.DefaultTimeToPublish = srcCatalog.DefaultTimeToPublish;
			newCatalog.Description = srcCatalog.Description;
			newCatalog.EditorRole = srcCatalog.EditorRole;
			newCatalog.Enabled = srcCatalog.Enabled;
			newCatalog.Icon = srcCatalog.Icon;
			newCatalog.Keywords = srcCatalog.Keywords;
			newCatalog.MenuBreakCssClass = srcCatalog.MenuBreakCssClass;
			newCatalog.MenuBreakImage = srcCatalog.MenuBreakImage;
			newCatalog.MenuCatalogChildSelectedCssClass = srcCatalog.MenuCatalogChildSelectedCssClass;
			newCatalog.MenuCatalogCssClass = srcCatalog.MenuCatalogCssClass;
			newCatalog.MenuClipChildExpandedCssClass = srcCatalog.MenuClipChildExpandedCssClass;
			newCatalog.MenuClipChildSelectedCssClass = srcCatalog.MenuClipChildSelectedCssClass;
			newCatalog.MenuClipCssClass = srcCatalog.MenuClipCssClass;
			newCatalog.MenuClipSelectedCssClass = srcCatalog.MenuClipSelectedCssClass;
			newCatalog.MenuCssClass = srcCatalog.MenuCssClass;
			newCatalog.MenuEnabled = srcCatalog.MenuEnabled;
			newCatalog.MenuIconFlags = srcCatalog.MenuIconFlags;
			newCatalog.MenuLabel = srcCatalog.MenuLabel;
			newCatalog.MenuLeftIcon = srcCatalog.MenuLeftIcon;
			newCatalog.MenuOrder = srcCatalog.MenuOrder;
			newCatalog.MenuOverrideFlags = srcCatalog.MenuOverrideFlags;
			newCatalog.MenuRightIcon = srcCatalog.MenuRightIcon;
			newCatalog.MenuTooltip = srcCatalog.MenuTooltip;
			newCatalog.modifyDate = DateTime.Now;
			newCatalog.NotifyOnComments = srcCatalog.NotifyOnComments;
			if(isCatalogSelected)
				newCatalog.ParentCatalog = DbContentCatalog.NewPlaceHolder(selectedID);
			else
				newCatalog.ParentCatalog = new DbContentClip(selectedClipID).ParentCatalog;
			newCatalog.ParentCatalog = destCatalog;
			newCatalog.ReviewerRole = srcCatalog.ReviewerRole;
			newCatalog.SortOrder = srcCatalog.SortOrder;
			newCatalog.Status = srcCatalog.Status;
			newCatalog.Templates = srcCatalog.Templates;
			newCatalog.Title = srcCatalog.Title;
			newCatalog.WorkflowMode = srcCatalog.WorkflowMode;

//			if(clipPaste)
//			{
//				DbContentClip c
//			}
		
			return newCatalog.Save();
		}

		private bool catalogIsOpen(int catalogID)
		{
			// Loop through the catalogs to see if it already exists in the list
			for(int x = 0; x <= openCatalogs.GetUpperBound(0); x++)
				if(openCatalogs[x].ToString() == catalogID.ToString())
					return true;
			return false;
		}

		private void toggleCatalog(int catalogID)
		{			
			string[] newCatalogs;	

			if(catalogIsOpen(catalogID))
			{
				// Loop through the list and copy the catalogs,
				// but skip the existing one to remove it
				int offset = 0;
				newCatalogs = new string[openCatalogs.GetUpperBound(0)];
				for(int x = 0; x <= openCatalogs.GetUpperBound(0); x++)
					if(openCatalogs[x].ToString() == catalogID.ToString())
					{
						offset = -1;
						continue;
					}
					else
					{
						newCatalogs[x + offset] = openCatalogs[x];
					}
			}
			else
			{
				// Add the catalog to the list
				newCatalogs = new string[openCatalogs.GetUpperBound(0) + 2];
				for(int x = 0; x <= openCatalogs.GetUpperBound(0); x++)
					newCatalogs[x] = openCatalogs[x];
				newCatalogs[openCatalogs.GetUpperBound(0) + 1] = catalogID.ToString();
			}

			openCatalogs = newCatalogs;
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 3;
			features = TableWindowFeatures.ClipboardCopier |
				TableWindowFeatures.Scroller |
				TableWindowFeatures.WindowPrinter;
			components = TableWindowComponents.Toolbar;
		}

		#region Rendering

		protected override void OnPreRender(EventArgs e)
		{
			if(isCatalogSelected)
			{
				if(selectedID != -1)
				{
					DbContentCatalog selectedCatalog = new DbContentCatalog(selectedID);

					newButton.Enabled = Page.User.IsInRole(selectedCatalog.EditorRole.Name);
					editButton.Enabled = Page.User.IsInRole(selectedCatalog.EditorRole.Name);
				}
				else
				{
					editButton.Enabled = false;
					viewButton.Enabled = false;
				}				
				//				upButton.Enabled = false;
				//				downButton.Enabled = false;
			}
			else
			{				
				if(selectedClipID != -1)
				{
					DbContentClip clip = new DbContentClip(selectedClipID);
					DbContentCatalog selectedCatalog = clip.ParentCatalog;

					// set edit button based on roles	
					editButton.Enabled = Page.User.IsInRole(clip.ParentCatalog.EditorRole.Name);
                    //upButton.Enabled = editButton.Enabled;
                    //downButton.Enabled = editButton.Enabled;

					newButton.Enabled = selectedID != -1 &&
						(Page.User.IsInRole(selectedCatalog.AuthorRole.Name) |
						Page.User.IsInRole(selectedCatalog.EditorRole.Name));

				}
				else				
				{					
					editButton.Enabled = false;
					viewButton.Enabled = false;
					newButton.Enabled = false;
                    //upButton.Enabled = false;
                    //downButton.Enabled = false;
				}
			}

			ControlExtender.RegisterTooltipScript(this.Page, 1, "grey", 2, "white");
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			EnsureCatalogs();

			//
			// Render Records
			//
			for(int x = 0; x < catalogs.Count; x++)
			{
				DbContentCatalog catalog = catalogs[x];
				if(catalog.ParentCatalog == null)
					renderCatalog(catalog, output);				
			}
		}

		private void renderCatalog(DbContentCatalog catalog, HtmlTextWriter output)
		{	
			float calcSize;									// Calculated size of objects
			bool openCatalog = catalogIsOpen(catalog.ID);
			bool catalogSelect = Page.User.IsInRole("CMS/Administrator") || Page.User.IsInRole(catalog.EditorRole.Name);
			bool clipSelect;			

			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;
						
			output.WriteBeginTag("td");
			output.WriteAttribute("style", "padding-left:" + (indent*indentMultiplier+CellPadding.Value).ToString() + "px;");
			output.WriteAttribute("valign", "middle");
			output.WriteAttribute("width", "100%");
			output.Write(HtmlTextWriter.TagRightChar);

			//			if(catalogSelect)
			//			{
			output.WriteBeginTag("a");
			output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "tog_" + catalog.ID.ToString()));
			output.Write(HtmlTextWriter.TagRightChar);
			//			}

			if(openCatalog)
			{
				output.WriteBeginTag("img");
				output.WriteAttribute("src", Page.ResolveUrl(minusIcon));
				output.WriteAttribute("align", "absmiddle");
				output.WriteAttribute("border", "0");
				output.Write(HtmlTextWriter.TagRightChar);
			}
			else
			{
				output.WriteBeginTag("img");
				output.WriteAttribute("src", Page.ResolveUrl(plusIcon));
				output.WriteAttribute("align", "absmiddle");
				output.WriteAttribute("border", "0");
				output.Write(HtmlTextWriter.TagRightChar);
			}

			//			if(catalogSelect)
			output.WriteEndTag("a");

			//			if(catalogSelect)
			//			{
			output.WriteBeginTag("a");
			output.WriteAttribute("href", "javascript:" + GetSelectReference(catalog.ID));
			output.Write(HtmlTextWriter.TagRightChar);
			//			}

			output.WriteBeginTag("img");
			output.WriteAttribute("src", Page.ResolveUrl(catalogIcon));
			output.WriteAttribute("align", "absmiddle");
			output.WriteAttribute("border", "0");
			output.Write(HtmlTextWriter.TagRightChar);

			if(catalog.ID == selectedID& isCatalogSelected)
				output.Write("<strong>");
			output.Write(catalog.ToString());
			if(catalog.ID == selectedID& isCatalogSelected)
				output.Write("</strong>");

			//			if(catalogSelect)
			output.WriteEndTag("a");
			
			output.WriteEndTag("td");
			output.WriteLine();

			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("&nbsp;");
			output.WriteEndTag("td");

			// Menu Tag
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			if(catalog.MenuEnabled)
				output.Write("Menu");
			else
				output.Write("&nbsp;");
			output.WriteEndTag("td");

			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Catalog");
			output.WriteEndTag("td");

			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(catalog.ModifyDate.ToShortDateString());
			output.WriteEndTag("td");

			//			output.WriteFullBeginTag("td");
			//			output.Write("&nbsp;");
			//			output.WriteEndTag("td");
			//
			//			output.WriteFullBeginTag("td");
			//			output.Write("&nbsp;");
			//			output.WriteEndTag("td");

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();

			indent++;

			if(openCatalog)
			{
				for(int x = 0; x < catalogs.Count; x++)
				{
					DbContentCatalog childCatalog = catalogs[x];
					if(childCatalog.ParentCatalog != null && childCatalog.ParentCatalog.ID == catalog.ID)
						renderCatalog(childCatalog, output);
				}

				DbContentClipCollection clips = catalog.GetClips();
				clips.Sort(ContentCompareKey.MenuEnabled, ContentCompareKey.MenuOrder);

				foreach(DbContentClip clip in clips)
				{
					// Do not display expired clips unless expire checkbox is selected
					if(!cbExpired.Checked && DateTime.Now > clip.ExpirationDate)
						continue;

					// All clips to be selected ONLY if editor, author
					clipSelect = Page.User.IsInRole(catalog.EditorRole.Name);

					output.WriteFullBeginTag("tr");
					output.WriteBeginTag("td");
					output.WriteAttribute("style", "padding-left:" + (indent*indentMultiplier+CellPadding.Value).ToString() + "px;");
					output.WriteAttribute("valign", "middle");
					output.WriteAttribute("width", "100%");
					output.Write(HtmlTextWriter.TagRightChar);

					#region Prefix Icon Output - Should Be Empty For All Clips

					output.WriteBeginTag("div");
					output.WriteAttribute("style", "float:left;");
					output.Write(HtmlTextWriter.TagRightChar);

					output.WriteBeginTag("img");
					output.WriteAttribute("src", Page.ResolveUrl(blankIcon));
					output.WriteAttribute("align", "absmiddle");
					output.WriteAttribute("border", "0");
					output.Write(HtmlTextWriter.TagRightChar);

					output.WriteEndTag("div");

					#endregion

					#region Icon Output

					output.WriteBeginTag("div");
					output.WriteAttribute("style", "float:left;");
					output.Write(HtmlTextWriter.TagRightChar);

					if(clipSelect)
					{
						output.WriteBeginTag("a");
						output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "csel_" + clip.ID.ToString()));
						output.Write(HtmlTextWriter.TagRightChar);
					}

					// Clip Icon
					output.WriteBeginTag("img");
					if(clip.ExpirationDate <= DateTime.Now)
						output.WriteAttribute("src", Page.ResolveUrl(expiredIcon));
					else if(clip.PublishDate <= DateTime.Now)
						output.WriteAttribute("src", Page.ResolveUrl(publishedIcon));
					else
						output.WriteAttribute("src", Page.ResolveUrl(prePublishedIcon));
					output.WriteAttribute("align", "absmiddle");
					output.WriteAttribute("border", "0");
					output.Write(HtmlTextWriter.TagRightChar);

					if(clipSelect)
						output.WriteEndTag("a");
					
					output.WriteEndTag("div");

					#endregion

					#region Title Output

					output.WriteBeginTag("div");
					output.WriteAttribute("style", "padding-top:3px;float:left;");
					
					// Tooltip Properties
					output.WriteAttribute("onmouseover", 
						ControlExtender.GetTooltipStartReference(constructTooltip(clip),"#ffffe0", 0));
					output.WriteAttribute("onmouseout", ControlExtender.GetTooltipEndReference());

					output.Write(HtmlTextWriter.TagRightChar);

					if(clipSelect)
					{
						output.WriteBeginTag("a");
						output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "csel_" + clip.ID.ToString()));
						output.Write(HtmlTextWriter.TagRightChar);
					}
					
					if(selectedClipID == clip.ID & !isCatalogSelected)
						output.Write("<strong>");
					// Be sure to strip html tags!
					output.Write(Regex.Replace(clip.Title, "<[^>]*>", "")); 
					if(selectedClipID == clip.ID & !isCatalogSelected)
						output.Write("</strong>");

					if(clipSelect)
						output.WriteEndTag("a");

					output.WriteEndTag("div");

					#endregion

					output.WriteEndTag("td");

					// Calculate size
					calcSize = ((float) clip.Body.Length) / 1024;
					output.WriteBeginTag("td");
					output.WriteAttribute("nowrap", "true");
					output.WriteAttribute("valign", "top");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(calcSize.ToString("n1"));
					output.Write(" KB");
					output.WriteEndTag("td");

					// Menu Tag
					output.WriteBeginTag("td");
					output.WriteAttribute("valign", "top");
					output.Write(HtmlTextWriter.TagRightChar);
					if(clip.MenuEnabled)
						output.Write("Menu");
					else
						output.Write("&nbsp;");
					output.WriteEndTag("td");

					output.WriteBeginTag("td");
					output.WriteAttribute("valign", "top");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write("Clip");
					output.WriteEndTag("td");

					output.WriteBeginTag("td");
					output.WriteAttribute("valign", "top");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(clip.ModifyDate.ToShortDateString());
					output.WriteEndTag("td");

					//					output.WriteBeginTag("td");
					//					output.WriteAttribute("valign", "top");
					//					output.Write(HtmlTextWriter.TagRightChar);
					//					output.Write(clip.PublishDate.ToShortDateString());
					//					output.WriteEndTag("td");
					//
					//					output.WriteBeginTag("td");
					//					output.WriteAttribute("valign", "top");
					//					output.Write(HtmlTextWriter.TagRightChar);
					//					output.Write(clip.ExpirationDate.ToShortDateString());
					//					output.WriteEndTag("td");

					output.WriteEndTag("tr");
				}
			}

			indent--;
		}

		private string constructTooltip(DbContentClip clip)
		{
			StringBuilder s = new StringBuilder();
			
			s.Append("ID: ");
			s.Append(clip.ID);
			s.Append("<BR>");
			
			s.Append("Published: ");
			s.Append(clip.PublishDate.ToLongDateString());
			s.Append("<BR>");
			
			s.Append("Expires: ");
			s.Append(clip.ExpirationDate.ToLongDateString());

			if(clip.OverrideUrl != string.Empty)
			{
				s.Append("<BR>");
				s.Append("Override: ");
				s.Append(clip.OverrideUrl);
			}

			return s.ToString();
		}

		#endregion

		#region Viewstate Methods

		protected override void LoadViewState(object savedState) 
		{
			EnsureChildControls();
			// Customize state management to handle saving state of contained objects.

			if (savedState != null) 
			{
				object[] myState = (object[])savedState;

				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
					selectedClipID = (int) myState[1];
				if (myState[2] != null)
					isCatalogSelected = (bool) myState[2];
				if (myState[3] != null)
					openCatalogs = myState[3].ToString().Split(',');
				if (myState[4] != null)
					closeLastCatalog = (bool) myState[4];
				if (myState[5] != null)
					copyCatalogID = (int) myState[5];
				if (myState[6] != null)
					copyClipID = (int) myState[6];
			}

			//			loadClientSelection();
		}

		protected override object SaveViewState() 
		{
			// Customized state management to handle saving state of contained objects  such as styles.

			object baseState = base.SaveViewState();
			
			object[] myState = new object[7];
			myState[0] = baseState;
			myState[1] = selectedClipID;
			myState[2] = isCatalogSelected;
			if(openCatalogs != null)
				myState[3] = string.Join(",", openCatalogs);
			else
				myState[3] = string.Empty;
			myState[4] = closeLastCatalog;
			myState[5] = copyCatalogID;
			myState[6] = copyClipID;

			return myState;
		}

		#endregion

	}
}

