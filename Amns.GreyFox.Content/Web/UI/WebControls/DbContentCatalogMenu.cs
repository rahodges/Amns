using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.Content.Caching;
using Amns.GreyFox.Content.Support;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for DbContentCatalogMenu.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:DbContentCatalogMenu runat=server></{0}:DbContentCatalogMenu>")]
	public class DbContentCatalogMenu : System.Web.UI.Control
	{
		ComponentArt.Web.UI.Menu _menu;
		string		_connectionString;
		int			_catalogID					= 3;
		int			_clipID						= -1;
		TimeSpan	_cacheTime					= TimeSpan.FromHours(5);
		bool		_cached						= false;
		string		_linkFormat					= "?ref={0}";
		Unit		_width;

		#region Public Properties

		[Bindable(false), Category("Data"), DefaultValue("")]
		public string ConnectionString
		{
			get { return _connectionString; }
			set
			{
				// Parse Connection String
				if(value.StartsWith("<jet40virtual>") & Context != null)
					_connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(value.Substring(14, value.Length - 14));
				else if(value.StartsWith("<jet40config>") & Context != null)
					_connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get(value.Substring(13, value.Length - 13)));
				else
					_connectionString = value;
			}
		}

		[Bindable(false), Category("Data"), DefaultValue(1)]
		public int CatalogID
		{
			get { return _catalogID; }
			set { _catalogID = value; }
		}

		[Bindable(false), Category("Data"), DefaultValue(1)]
		public int ClipID
		{
			get { return _clipID; }
			set { _clipID = value; }
		}

		[Bindable(false), Category("Appearance"), DefaultValue(1)]
		public Unit Width
		{
			get { return _width; }
			set { _width = value; }
		}

		[Bindable(false), Category("Behavior")]
		public TimeSpan CacheTime
		{
			get { return _cacheTime; }
			set { _cacheTime = value; }
		}

		[Bindable(true), Category("Appearance"), DefaultValue("?ref={0}")]
		public string LinkFormat
		{
			get { return _linkFormat; }
			set { _linkFormat = value; }
		}

		#endregion

		protected override void CreateChildControls()
		{
			if(MenuCacheControl.MenuExists(this._catalogID, this._linkFormat))
			{
				_cached = true;
			}
			else
			{
				_menu = new ComponentArt.Web.UI.Menu();

				_menu.ID = this.ID + "_menu";
				_menu.CssClass = "TopGroup";
				_menu.EnableViewState = false;
				_menu.Orientation = GroupOrientation.Vertical;
				_menu.DefaultGroupCssClass = "MenuGroup";
				_menu.DefaultGroupItemSpacing = 1;
				_menu.ImagesBaseUrl = "~/";
				_menu.EnableViewState = false;
				_menu.Width = _width;
//				_menu.ClientScriptLocation = "~/componentart_webui_client";

				// Create default Item Look
				ItemLook itmLook = new ItemLook();
				itmLook.LookId = "MenuItem";
				itmLook.CssClass = "MenuItem";
				itmLook.HoverCssClass = "MenuItemHover";
				itmLook.ActiveCssClass = "MenuItemDown";
				itmLook.ExpandedCssClass = "MenuItemDown";
				itmLook.LabelPaddingLeft = Unit.Pixel(5);
				itmLook.LabelPaddingRight = Unit.Pixel(15);
				itmLook.LabelPaddingTop = Unit.Pixel(2);
				itmLook.LabelPaddingBottom = Unit.Pixel(2);
				
				_menu.ItemLooks.Add(itmLook);
				
				_menu.DefaultItemLookId = "MenuItem";                				

				Controls.Add(_menu);				
			}
		
			ChildControlsCreated = true;
		}

		protected override void OnLoad(EventArgs e)
		{

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			EnsureChildControls();

			// Cached menus are already Loaded
			if(_cached)
				return;

			DbContentCatalog rootCatalog;
			DbContentClipCollection rootCatalogClips;
			CssInfo rootCssInfo;

			// Load the catalog to start building a menu from
			rootCatalog = new DbContentCatalog(_catalogID);
			rootCatalogClips = rootCatalog.GetClips();
			rootCatalogClips.Sort(ContentCompareKey.MenuOrder);
			
			// Set menu css to catalog css properties
			rootCssInfo = new CssInfo(rootCatalog.MenuCssClass);
			rootCssInfo.SetLook(_menu);

			DbContentCatalogCollection catalogs = rootCatalog.GetCatalogs();
			catalogs.Sort(ContentCompareKey.MenuOrder);
			
			for(int catalogIndex = 0; catalogIndex < catalogs.Count; catalogIndex++)
				addCatalogToMenu(_menu.Items, rootCatalog, catalogs[catalogIndex]);

			addClipsToMenu(_menu.Items, rootCatalog, rootCatalog, rootCatalogClips);

			_menu.Width = _width;

			MenuCacheControl.SetMenu(_catalogID, _linkFormat, _menu);
		}

		
		/// <summary> 
		/// Render rootCatalog control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
//			if(this._cached)
//			{
				output.Write(MenuCacheControl.GetMenu(this._catalogID, this._linkFormat).Output);
//			}
//			else
//			{
				// Change the selected item if it doesn't match the current clips item id
//				if(_menu.SelectedItem != null && _menu.SelectedItem.ID != _clipID.ToString())
//					recurseSelection(_menu);
//
//				_menu.RenderControl(output);
//			}
		}

        private void recurseSelection(ComponentArt.Web.UI.Menu menu)
		{
            for(int x = 0; x < menu.Items.Count; x++)
				recurseSelection(menu.Items[x]);
		}

        private void recurseSelection(ComponentArt.Web.UI.MenuItem menuItem)
		{
			string id = _clipID.ToString();
			if(menuItem.ID == id)
				_menu.SelectedItem = menuItem;
			else
				for(int x = 0; x < menuItem.Items.Count; x++)
					recurseSelection(menuItem.Items[x]);
		}

        private void addCatalogToMenu(ComponentArt.Web.UI.MenuItemCollection parentItemCollection, 
			DbContentCatalog rootCatalog, DbContentCatalog catalog)
		{
			if(!catalog.Enabled || !catalog.MenuEnabled)
				return;

			DbContentClipCollection clips;
			ComponentArt.Web.UI.MenuItem catalogMenuItem;
			IconInfo leftIconInfo, rightIconInfo;
			CssInfo catalogCssInfo;

			clips = catalog.GetMenuEnabledClips();
			clips.Sort(ContentCompareKey.MenuOrder);

            catalogMenuItem = new ComponentArt.Web.UI.MenuItem();
			parentItemCollection.Add(catalogMenuItem);

			catalogMenuItem.Text = catalog.MenuLabel;
			catalogMenuItem.ToolTip = catalog.MenuTooltip;

			if(catalog.DefaultClip != null)
			{
				if(catalog.DefaultClip.OverrideUrl != string.Empty)
					catalogMenuItem.NavigateUrl = catalog.DefaultClip.OverrideUrl;
				else
                    catalogMenuItem.NavigateUrl = string.Format(_linkFormat, catalog.DefaultClip.ID);
				
				catalogMenuItem.ID = catalog.DefaultClip.ID.ToString();
			}		

			if(catalog.MenuCssClass == "[rootcatalog]" |
				catalog.MenuCssClass == "")
				catalogCssInfo = new CssInfo(rootCatalog.MenuCssClass);
			else
                catalogCssInfo = new CssInfo(catalog.MenuCssClass);

			if(catalog.MenuLeftIcon == "[rootcatalog]" |
				catalog.MenuCssClass == "")
				leftIconInfo = new IconInfo(rootCatalog.MenuLeftIcon);
			else
				leftIconInfo = new IconInfo(catalog.MenuLeftIcon);

			if(catalog.MenuRightIcon == "[rootcatalog]" |
				catalog.MenuCssClass == "")
				rightIconInfo = new IconInfo(rootCatalog.MenuRightIcon);
			else
				rightIconInfo = new IconInfo(catalog.MenuRightIcon);

			catalogCssInfo.SetLook(catalogMenuItem);
			leftIconInfo.SetLeftLook(catalogMenuItem);
			rightIconInfo.SetRightLook(catalogMenuItem);

			DbContentCatalogCollection childCatalogs = catalog.GetCatalogs();;
				childCatalogs.Sort(ContentCompareKey.SortOrder);

            foreach(DbContentCatalog childCatalog in childCatalogs)
				addCatalogToMenu(catalogMenuItem.Items, rootCatalog, childCatalog);

            addClipsToMenu(catalogMenuItem.Items, rootCatalog, catalog, clips);
		}

		private void addClipsToMenu(ComponentArt.Web.UI.MenuItemCollection parentItems, DbContentCatalog rootCatalog, 
			DbContentCatalog catalog, DbContentClipCollection clips)
		{
			foreach(DbContentClip clip in clips)
			{
				if(!clip.MenuEnabled | clip.ExpirationDate <= DateTime.Now | clip.PublishDate > DateTime.Now)
					continue;

				// Check for a break and use the current catalog's _menu break system
				if(clip.MenuBreak)
				{
                    ComponentArt.Web.UI.MenuItem menuBreak = 
                        new ComponentArt.Web.UI.MenuItem();					
					menuBreak.Look.ImageUrl = rootCatalog.MenuBreakImage;
					menuBreak.Look.CssClass = rootCatalog.MenuBreakCssClass;
					menuBreak.Look.ImageHeight = Unit.Pixel(2);
					menuBreak.Look.ImageWidth = Unit.Percentage(100);
					parentItems.Add(menuBreak);
				}

                ComponentArt.Web.UI.MenuItem clipMenuItem = 
                    new ComponentArt.Web.UI.MenuItem();
				IconInfo leftIconInfo, rightIconInfo;
				CssInfo clipCssInfo;
					
				clipMenuItem.ID = clip.ID.ToString();

#if DEBUG
				clipMenuItem.Text = clip.MenuLabel + " (" + clip.MenuOrder.ToString() + ")";
#else
				clipMenuItem.Text = clip.MenuLabel;
#endif

				if(clip.OverrideUrl != string.Empty)
					clipMenuItem.NavigateUrl = clip.OverrideUrl;
				else
					clipMenuItem.NavigateUrl = Page.ResolveUrl(string.Format(_linkFormat, clip.ID.ToString()));
				clipMenuItem.ToolTip = clip.MenuTooltip;
							
				if(catalog.MenuCssClass == "[rootcatalog]" |
					catalog.MenuCssClass == "")
					clipCssInfo = new CssInfo(rootCatalog.MenuCssClass);
				else 
                    clipCssInfo = new CssInfo(catalog.MenuCssClass);
				clipCssInfo.SetLook(clipMenuItem);
				
				if(clip.MenuLeftIcon == "[rootcatalog]")
					leftIconInfo = new IconInfo(rootCatalog.MenuLeftIcon);				
				else if(clip.MenuLeftIcon == "[catalog]")
					leftIconInfo = new IconInfo(catalog.MenuLeftIcon);				
				else
                    leftIconInfo = new IconInfo(clip.MenuLeftIcon);

				leftIconInfo.SetLeftLook(clipMenuItem);

				if(clip.MenuRightIcon == "[rootcatalog]")
					rightIconInfo = new IconInfo(rootCatalog.MenuRightIcon);
				else if(clip.MenuRightIcon == "[catalog]")
					rightIconInfo = new IconInfo(catalog.MenuRightIcon);
				else
					rightIconInfo = new IconInfo(clip.MenuRightIcon);
				
				rightIconInfo.SetRightLook(clipMenuItem);

				parentItems.Add(clipMenuItem);
			}
		}
	}
}