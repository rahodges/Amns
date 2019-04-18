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
		ToolboxData("<{0}:DbContentCatalogSiteMap runat=server></{0}:DbContentCatalogSiteMap>")]
	public class DbContentCatalogSiteMap : System.Web.UI.Control
	{
		private SiteMap		_siteMap;
		private string		_connectionString;
		private int			_catalogID					= 3;
		private int			_clipID						= -1;
		private TimeSpan	_cacheTime					= TimeSpan.FromHours(5);
		private bool		_cached						= false;
		private string		_linkFormat					= "?ref={0}";
		private Unit		_width;

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
			if(SiteMapCacheControl.SiteMapExists(this._catalogID, this._linkFormat))
			{
				_cached = true;
			}
			else
			{
				_siteMap = new SiteMap();
				_siteMap.ID = this.ID + "_siteMap";
				_siteMap.SiteMapLayout = SiteMapLayoutType.Tree;
				_siteMap.TreeShowLines = true;
				_siteMap.TreeLineImagesFolderUrl = "/images/lines2";
				_siteMap.TreeLineImageWidth = 19;
				_siteMap.TreeLineImageHeight = 20;
				_siteMap.CssClass = "SiteMap";
				_siteMap.RootNodeCssClass = "RootNode";
				_siteMap.ParentNodeCssClass = "ParentNode";
				_siteMap.LeafNodeCssClass = "LeafNode";
				_siteMap.Width = this._width;
			
				SiteMapTable t = _siteMap.Table;
				SiteMapTableRow a = new SiteMapTableRow();
				SiteMapTableCell a1 = new SiteMapTableCell();
				a1.RootNodes = 2;
				a1.VerticalAlign = VerticalAlign.Top;
				a.Cells.Add(a1);
				SiteMapTableCell a2 = new SiteMapTableCell();
				a2.RootNodes = 2;
				a2.VerticalAlign = VerticalAlign.Top;
				a.Cells.Add(a2);
				SiteMapTableCell a3 = new SiteMapTableCell();
				a3.VerticalAlign = VerticalAlign.Top;
				a.Cells.Add(a3);
				t.Rows.Add(a);

				Controls.Add(_siteMap);				
			}
		
			ChildControlsCreated = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			EnsureChildControls();

			// Cached menus are already Loaded
			if(_cached)
				return;

			DbContentCatalog rootCatalog;		
//			CssInfo rootCssInfo;

			// Load the catalog to start building a menu from
			rootCatalog = new DbContentCatalog(_catalogID);

			DbContentCatalogCollection catalogs = rootCatalog.GetCatalogs();;
			catalogs.Sort(ContentCompareKey.MenuOrder);
			
			for(int catalogIndex = 0; catalogIndex < catalogs.Count; catalogIndex++)
				addCatalogToSiteMap(_siteMap.Nodes, rootCatalog, catalogs[catalogIndex]);

			DbContentClipCollection clips = rootCatalog.GetClips();
			clips.Sort(ContentCompareKey.MenuOrder);

			addClipsToSiteMap(_siteMap.Nodes, rootCatalog, rootCatalog, clips);

			_siteMap.Width = _width;

			SiteMapCacheControl.SetSiteMap(_catalogID, _linkFormat, _siteMap);
		}
		
		/// <summary> 
		/// Render rootCatalog control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
//			_siteMap.RenderControl(output);
//			if(this._cached)
//			{
				output.Write(SiteMapCacheControl.GetSiteMap(this._catalogID, this._linkFormat).Output);
//			}
//			else
//			{
				// Change the selected item if it doesn't match the current clips item id
//				if(_siteMap.SelectedItem != null && _siteMap.SelectedItem.ID != _clipID.ToString())
//					recurseSelection(_siteMap);
//
//				_siteMap.RenderControl(output);
//			}
		}

		private void addCatalogToSiteMap(SiteMapNodeCollection parentNodeCollection, 
			DbContentCatalog rootCatalog, DbContentCatalog catalog)
		{
			if(!catalog.Enabled || !catalog.MenuEnabled)
				return;

			DbContentClipCollection clips;
			SiteMapNode catalogMenuNode;
			IconInfo leftIconInfo, rightIconInfo;
			CssInfo catalogCssInfo;

			clips = catalog.GetMenuEnabledClips();
			clips.Sort(ContentCompareKey.MenuOrder);

			catalogMenuNode = new SiteMapNode();
			parentNodeCollection.Add(catalogMenuNode);

			catalogMenuNode.Text = catalog.MenuLabel;
			catalogMenuNode.ToolTip = catalog.MenuTooltip;

			if(catalog.DefaultClip != null)
			{
				if(catalog.DefaultClip.OverrideUrl != string.Empty)
					catalogMenuNode.NavigateUrl = catalog.DefaultClip.OverrideUrl;
				else
                    catalogMenuNode.NavigateUrl = string.Format(_linkFormat, catalog.DefaultClip.ID);
				
				catalogMenuNode.ID = catalog.DefaultClip.ID.ToString();
			}		

			if(catalog.MenuCssClass == "[rootcatalog]")
				catalogCssInfo = new CssInfo(rootCatalog.MenuCssClass);
			else
                catalogCssInfo = new CssInfo(catalog.MenuCssClass);

			if(catalog.MenuLeftIcon == "[rootcatalog]")
				leftIconInfo = new IconInfo(rootCatalog.MenuLeftIcon);
			else
				leftIconInfo = new IconInfo(catalog.MenuLeftIcon);

			if(catalog.MenuRightIcon == "[rootcatalog]")
				rightIconInfo = new IconInfo(rootCatalog.MenuRightIcon);
			else
				rightIconInfo = new IconInfo(catalog.MenuRightIcon);

			DbContentCatalogCollection childCatalogs = catalog.GetCatalogs();
				childCatalogs.Sort(ContentCompareKey.MenuOrder);

            foreach(DbContentCatalog childCatalog in childCatalogs)
				addCatalogToSiteMap(catalogMenuNode.Nodes, rootCatalog, childCatalog);

            addClipsToSiteMap(catalogMenuNode.Nodes, rootCatalog, catalog, clips);
		}

		private void addClipsToSiteMap(SiteMapNodeCollection parentNodes, DbContentCatalog rootCatalog, 
			DbContentCatalog catalog, DbContentClipCollection clips)
		{
			foreach(DbContentClip clip in clips)
			{
				if(!clip.MenuEnabled | clip.ExpirationDate <= DateTime.Now | clip.PublishDate > DateTime.Now)
					continue;

//				// Check for a break and use the current catalog's _siteMap break system
//				if(clip.MenuBreak)
//				{
//					MenuItem menuBreak = new MenuItem();					
//					menuBreak.Look.ImageUrl = rootCatalog.MenuBreakImage;
//					menuBreak.Look.CssClass = rootCatalog.MenuBreakCssClass;
//					menuBreak.Look.ImageHeight = Unit.Pixel(2);
//					menuBreak.Look.ImageWidth = Unit.Percentage(100);
//					parentNodes.Add(menuBreak);
//				}

				SiteMapNode clipMenuNode = new SiteMapNode();
				IconInfo leftIconInfo, rightIconInfo;
//				CssInfo clipCssInfo;
					
				clipMenuNode.ID = clip.ID.ToString();
				clipMenuNode.Text = clip.MenuLabel;
				if(clip.OverrideUrl != string.Empty)
					clipMenuNode.NavigateUrl = clip.OverrideUrl;
				else
					clipMenuNode.NavigateUrl = Page.ResolveUrl(string.Format(_linkFormat, clip.ID.ToString()));
				clipMenuNode.ToolTip = clip.MenuTooltip;
							
//				if(clip.MenuCssClass != string.Empty)
//				{
//					clipCssInfo = new CssInfo(clip.MenuCssClass);
//					clipMenuItem.Look.CssClass = clipCssInfo.Default;
//					clipMenuItem.Look.HoverCssClass = clipCssInfo.Over;
//					clipMenuItem.Look.ActiveCssClass = clipCssInfo.Down;
//				}
				
				if(clip.MenuLeftIcon == "[rootcatalog]")
					leftIconInfo = new IconInfo(rootCatalog.MenuLeftIcon);
				else if(clip.MenuLeftIcon == "[catalog]")
					leftIconInfo = new IconInfo(catalog.MenuLeftIcon);
				else
					leftIconInfo = new IconInfo(clip.MenuLeftIcon);

				if(clip.MenuRightIcon == "[rootcatalog]")
					rightIconInfo = new IconInfo(rootCatalog.MenuRightIcon);
				else if(clip.MenuRightIcon == "[catalog]")
					rightIconInfo = new IconInfo(catalog.MenuRightIcon);
				else
					rightIconInfo = new IconInfo(clip.MenuRightIcon);

//				leftIconInfo.SetLeftLook(clipMenuNode);
//				rightIconInfo.SetRightLook(clipMenuNode);

				parentNodes.Add(clipMenuNode);
			}
		}
	}
}