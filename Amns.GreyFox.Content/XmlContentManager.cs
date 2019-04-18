using System;
using System.Data;
using System.Collections;
using System.Web;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Provides a consolidated management system for content based on XML. Also
	/// provides caching for clips.
	/// </summary>
	public sealed class XmlContentManager
	{
		private string xmlPath;

		public XmlContentManager(string xmlPath)
		{
			this.xmlPath = xmlPath;
		}

		public static XmlClip GetClip(string xmlPath, string groupID, string clipID)
		{
			string cacheKey = string.Format("___CLIPCACHE{0}{1}{2}", xmlPath, groupID, clipID);
			if(HttpContext.Current.Cache[cacheKey] != null)
				return (XmlClip) HttpContext.Current.Cache[cacheKey];

			XmlClip xclip = new XmlClip(xmlPath, groupID, clipID);
			xclip.Load();

			if(!xclip.CacheDisabled)
			{
				System.Web.Caching.CacheDependency Dependencies;
				Dependencies = new System.Web.Caching.CacheDependency(xclip.XmlPath);
				HttpContext.Current.Cache.Insert(cacheKey, xclip, Dependencies);
			}

			return xclip;
		}

		public XmlClip GetClip(string groupID, string clipID)
		{
			return XmlContentManager.GetClip(xmlPath, groupID, clipID);
		}
	}
}
