using System;
using System.Collections;
using System.Web;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Caching
{
	/// <summary>
	/// Summary description for SiteMapCacheControl.
	/// </summary>
	public class SiteMapCacheControl
	{
		public SiteMapCacheControl()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Generates the siteMap id for the cache. Insures that siteMaps are isolated by user.
		/// </summary>
		/// <param name="catalogId"></param>
		/// <param name="linkFormat"></param>
		/// <returns></returns>
		private static string generateSiteMapId(int catalogId, string linkFormat)
		{
			int hashCode = catalogId.GetHashCode() +
				linkFormat.GetHashCode() +
				HttpContext.Current.User.Identity.Name.GetHashCode() +
				HttpContext.Current.Request.UserAgent.GetHashCode();

			return "gfxcms_siteMap_" + hashCode.ToString();
		}

		public static bool SiteMapExists(int catalogId, string linkFormat)
		{
			// Manages siteMaps based on client string
			return HttpContext.Current.Cache[generateSiteMapId(catalogId, linkFormat)] !=  null;
		}

		public static SiteMapOutputWrapper GetSiteMap(int catalogId, string linkFormat)
		{
			string cacheId = generateSiteMapId(catalogId, linkFormat);
			if(HttpContext.Current.Cache[cacheId] != null)
				return ((SiteMapOutputWrapper) HttpContext.Current.Cache[cacheId]);
			return null;
		}

        public static void SetSiteMap(int catalogId, string linkFormat, ComponentArt.Web.UI.SiteMap siteMap)
		{
			string cacheId = generateSiteMapId(catalogId, linkFormat);
			SiteMapOutputWrapper siteMapOutputWrapper = new SiteMapOutputWrapper();
			siteMapOutputWrapper.Bind(siteMap);
			HttpContext.Current.Cache.Insert(cacheId, siteMapOutputWrapper);
		}

		public static void ClearSiteMaps()
		{
			siteMapClear();
		}

		/// <summary>
		/// Triggers siteMap clearing processes that reiterates through the cache to remove siteMaps.
		/// </summary>
		private static void siteMapClear()
		{
			string key				= string.Empty;
			IDictionaryEnumerator i	= HttpContext.Current.Cache.GetEnumerator();
			
			while(i.MoveNext())
			{
				if(i.Key.ToString().StartsWith("gfxcms_siteMap_"))
				{
					key = i.Key.ToString();
					break;
				}
			}

			if(key != string.Empty)
			{
				HttpContext.Current.Cache.Remove(key);
				ClearSiteMaps();
			}
		}
	}
}
