using System;
using System.Collections;
using System.Web;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Caching
{
	/// <summary>
	/// Summary description for MenuCacheControl.
	/// </summary>
	public class MenuCacheControl
	{
		public MenuCacheControl()
		{
		}

		/// <summary>
		/// Generates the menu id for the cache. Insures that menus are isolated by user.
		/// </summary>
		/// <param name="catalogId"></param>
		/// <param name="linkFormat"></param>
		/// <returns></returns>
		private static string generateMenuId(int catalogId, string linkFormat)
		{
			// Exceptions
			if(linkFormat == null) 
				throw(new GreyFoxContentException("LinkFormat cannot be null."));
			if(HttpContext.Current == null)
				throw(new GreyFoxContentException("HttpContext.Current cannot be null."));
			if(HttpContext.Current.Request == null)
				throw(new GreyFoxContentException("HttpContext.Current.Request cannot be null."));

			// Encode CatalogID and LinkFormat
			int hashCode = catalogId.GetHashCode() + linkFormat.GetHashCode();
			
			// Encode UserAgent to Code
			if(HttpContext.Current.Request.UserAgent != null)
				hashCode += HttpContext.Current.Request.UserAgent.GetHashCode();

			// Encode User
			if(HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
				hashCode += HttpContext.Current.User.Identity.Name.GetHashCode();

			return "gfxcms_menu_" + hashCode.ToString();
		}

		public static bool MenuExists(int catalogId, string linkFormat)
		{
			string cacheId = generateMenuId(catalogId, linkFormat);
			object o = HttpContext.Current.Cache.Get(cacheId);
			return o != null;
		}

		public static MenuOutputWrapper GetMenu(int catalogId, string linkFormat)
		{
			// Exceptions
			if(linkFormat == null) 
				throw(new GreyFoxContentException("LinkFormat cannot be null."));

			string cacheId = generateMenuId(catalogId, linkFormat);
			object o = HttpContext.Current.Cache.Get(cacheId);
			if(o != null)
				return ((MenuOutputWrapper) HttpContext.Current.Cache[cacheId]);
			return null;
		}

		public static void SetMenu(int catalogId, string linkFormat, Menu menu)
		{
			// Exceptions
			if(linkFormat == null) throw(new GreyFoxContentException("LinkFormat cannot be null."));
			if(menu == null) throw(new GreyFoxContentException("Menu cannot be null."));

			string cacheId = generateMenuId(catalogId, linkFormat);
			MenuOutputWrapper menuOutputWrapper = new MenuOutputWrapper();
			menuOutputWrapper.Bind(menu);
			HttpContext.Current.Cache.Insert(cacheId, menuOutputWrapper);
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
			IDictionaryEnumerator i	= HttpContext.Current.Cache.GetEnumerator();
			
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
				HttpContext.Current.Cache.Remove(key);
				ClearMenus();
			}
		}
	}
}
