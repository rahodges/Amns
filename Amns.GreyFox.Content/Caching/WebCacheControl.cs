using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Caching
{
	/// <summary>
	/// Summary description for WebCacheControl.
	/// </summary>
	public sealed class WebCacheControl
	{
		public static Cache CurrentCache
		{
			get { return HttpContext.Current.Cache; }
		}

		/// <summary>
		/// Generates the menu id for the cache. Insures that menus are isolated by user.
		/// </summary>
		/// <param name="catalogId"></param>
		/// <param name="linkFormat"></param>
		/// <returns></returns>
		private static string generateMenuId(int catalogId, string linkFormat)
		{
			int hashCode = catalogId.GetHashCode() +
				linkFormat.GetHashCode() +
				HttpContext.Current.User.Identity.Name.GetHashCode();

			return "gfxcms_menu_" + hashCode.ToString();
		}

		public static bool MenuExists(int catalogId, string linkFormat)
		{
			return CurrentCache[generateMenuId(catalogId, linkFormat)] !=  null;
		}

		public static Menu GetMenu(int catalogId, string linkFormat)
		{
			string cacheId = generateMenuId(catalogId, linkFormat);
			if(CurrentCache[cacheId] != null)
			{
				Menu menu;
				menu = new Menu();
				menu.LoadXml(CurrentCache[cacheId].ToString());
				return menu;
			}
			return null;
		}

		public static void SetMenu(int catalogId, Menu menu, string linkFormat)
		{
			string cacheId = generateMenuId(catalogId, linkFormat);
			CurrentCache.Insert(cacheId, menu.GetXml());			
		}

		public static void ClearMenus()
		{
			menuClear();
		}

		/// <summary>
		/// Triggers menu clearing processes that reiterates through the cache to remove menus.
		/// </summary>
		private static void menuClear()
		{
			string key				= string.Empty;
			IDictionaryEnumerator i	= CurrentCache.GetEnumerator();
			
			while(i.MoveNext())
			{
				if(i.Key.ToString().StartsWith("gfxcms_menu_"))
				{
					key = i.Key.ToString();
					break;
				}
			}

			if(key != string.Empty)
			{
				CurrentCache.Remove(key);
				ClearMenus();
			}
		}
	}
}
