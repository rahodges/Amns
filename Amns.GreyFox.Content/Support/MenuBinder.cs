using System;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Support
{
	/// <summary>
	/// Summary description for MenuBinder.
	/// </summary>
	public class MenuBinder
	{
		bool				__looksEnabled		= true;
		string				__linkFormat		= "?ref={0}";
		DbContentCatalog	__rootCatalog		= null;
		ComponentArt.Web.UI.Menu	__menu				= null;

		#region Public Properties

		public int CatalogId
		{
			get { return CatalogId; }
		}

		public bool LooksEnabled
		{
			get { return __looksEnabled; }
			set { __looksEnabled = value; }
		}

		public string LinkFormat
		{
			get { return __linkFormat; }
			set { __linkFormat = value; }
		}

		public DbContentCatalog RootCatalog
		{
			get { return __rootCatalog; }
			set { __rootCatalog = value; }
		}

		public ComponentArt.Web.UI.Menu MenuMenuBinder
		{
			get { return __menu; }
			set { __menu = value; }
		}

		#endregion

		#region Instantiation

		public MenuBinder()
		{

		}

		public MenuBinder(DbContentCatalog rootCatalog, ComponentArt.Web.UI.Menu menu) : this()
		{
            __rootCatalog = rootCatalog;
			__menu = menu;
		}

        public MenuBinder(string connectionString, int catalogId, ComponentArt.Web.UI.Menu menu)
            : this()
		{
			OpenCatalog(catalogId);
			__menu = menu;
		}

		#endregion

		public void OpenCatalog(int catalogId)
		{
			__rootCatalog = new DbContentCatalog(catalogId);
		}

		#region MenuBinder

		public void Bind()
		{
			DbContentClipCollection rootCatalogClips;
			CssInfo rootCssInfo;

			// Load the catalog to start building a menu from
			rootCatalogClips = __rootCatalog.GetClips();
			rootCatalogClips.Sort(ContentCompareKey.MenuOrder);
			
			// Set menu css to catalog css properties
			rootCssInfo = new CssInfo(__rootCatalog.MenuCssClass);

			if(__looksEnabled)
				rootCssInfo.SetLook(__menu);

			DbContentCatalogCollection catalogs = __rootCatalog.GetCatalogs();
			catalogs.Sort(ContentCompareKey.MenuOrder);
			
			for(int catalogIndex = 0; catalogIndex < catalogs.Count; catalogIndex++)
				addCatalogToMenu(__menu.Items, catalogs[catalogIndex]);

			addClipsToMenu(__menu.Items, __rootCatalog, rootCatalogClips);
		}

        private void addCatalogToMenu(ComponentArt.Web.UI.MenuItemCollection parentItemCollection, DbContentCatalog catalog)
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
					catalogMenuItem.NavigateUrl = string.Format(__linkFormat, catalog.DefaultClip.ID);
				
				catalogMenuItem.ID = catalog.DefaultClip.ID.ToString();
			}		

			if(catalog.MenuCssClass == "[rootcatalog]")
				catalogCssInfo = new CssInfo(__rootCatalog.MenuCssClass);
			else
				catalogCssInfo = new CssInfo(catalog.MenuCssClass);

			if(catalog.MenuLeftIcon == "[rootcatalog]")
				leftIconInfo = new IconInfo(__rootCatalog.MenuLeftIcon);
			else
				leftIconInfo = new IconInfo(catalog.MenuLeftIcon);

			if(catalog.MenuRightIcon == "[rootcatalog]")
				rightIconInfo = new IconInfo(__rootCatalog.MenuRightIcon);
			else
				rightIconInfo = new IconInfo(catalog.MenuRightIcon);

			if(__looksEnabled)
			{
				catalogCssInfo.SetLook(catalogMenuItem);
				leftIconInfo.SetLeftLook(catalogMenuItem);
				rightIconInfo.SetRightLook(catalogMenuItem);
			}

			DbContentCatalogCollection childCatalogs = catalog.GetCatalogs();;
			childCatalogs.Sort(ContentCompareKey.SortOrder);

			foreach(DbContentCatalog childCatalog in childCatalogs)
				addCatalogToMenu(catalogMenuItem.Items, childCatalog);

			addClipsToMenu(catalogMenuItem.Items, catalog, clips);
		}

        private void addClipsToMenu(ComponentArt.Web.UI.MenuItemCollection parentItems,
			DbContentCatalog catalog, DbContentClipCollection clips)
		{
			foreach(DbContentClip clip in clips)
			{
				if(!clip.MenuEnabled | clip.ExpirationDate <= DateTime.Now | clip.PublishDate > DateTime.Now)
					continue;

				// Check for a break and use the current catalog's menu break system
				if(clip.MenuBreak)
				{
                    ComponentArt.Web.UI.MenuItem menuBreak = 
                        new ComponentArt.Web.UI.MenuItem();					
					menuBreak.Look.ImageUrl = __rootCatalog.MenuBreakImage;
					menuBreak.Look.CssClass = __rootCatalog.MenuBreakCssClass;
					menuBreak.Look.ImageHeight = Unit.Pixel(2);
					menuBreak.Look.ImageWidth = Unit.Percentage(100);
					parentItems.Add(menuBreak);
				}

                ComponentArt.Web.UI.MenuItem clipMenuItem = 
                    new ComponentArt.Web.UI.MenuItem();
				IconInfo leftIconInfo, rightIconInfo;
				//				CssInfo clipCssInfo;
					
				clipMenuItem.ID = clip.ID.ToString();

#if DEBUG
				clipMenuItem.Text = clip.MenuLabel + " (" + clip.MenuOrder.ToString() + ")";
#else
				clipMenuItem.Text = clip.MenuLabel;
#endif

				if(clip.OverrideUrl != string.Empty)
					clipMenuItem.NavigateUrl = clip.OverrideUrl;
				else
					clipMenuItem.NavigateUrl = __menu.Page.ResolveUrl(string.Format(__linkFormat, clip.ID.ToString()));
				clipMenuItem.ToolTip = clip.MenuTooltip;
							
				//				if(clip.MenuCssClass != string.Empty)
				//				{
				//					clipCssInfo = new CssInfo(clip.MenuCssClass);
				//					clipMenuItem.Look.CssClass = clipCssInfo.Default;
				//					clipMenuItem.Look.HoverCssClass = clipCssInfo.Over;
				//					clipMenuItem.Look.ActiveCssClass = clipCssInfo.Down;
				//				}
				
				if(clip.MenuLeftIcon == "[rootcatalog]")
					leftIconInfo = new IconInfo(__rootCatalog.MenuLeftIcon);
				else if(clip.MenuLeftIcon == "[catalog]")
					leftIconInfo = new IconInfo(catalog.MenuLeftIcon);
				else
					leftIconInfo = new IconInfo(clip.MenuLeftIcon);

				if(clip.MenuRightIcon == "[rootcatalog]")
					rightIconInfo = new IconInfo(__rootCatalog.MenuRightIcon);
				else if(clip.MenuRightIcon == "[catalog]")
					rightIconInfo = new IconInfo(catalog.MenuRightIcon);
				else
					rightIconInfo = new IconInfo(clip.MenuRightIcon);

				leftIconInfo.SetLeftLook(clipMenuItem);
				rightIconInfo.SetRightLook(clipMenuItem);

				parentItems.Add(clipMenuItem);
			}
		}

		#endregion
	}
}
