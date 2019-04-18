using System;
using System.Web;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for CookieUtil.
	/// </summary>
	public class CookieUtil
	{
		public static void SetEncryptedCookie(string key, string text)
		{
			key = CryptoUtil.Encrypt(key);
			text = CryptoUtil.Encrypt(text);
			SetCookie(key, text);
		}

		public static void SetEncryptedCookie(string key, string text, DateTime expires)
		{
			key = CryptoUtil.Encrypt(key);
			text = CryptoUtil.Encrypt(text);
			SetCookie(key, text, expires);
		}

		public static void SetEncryptedCookie(string key, string subkey, string text)
		{
			key = CryptoUtil.Encrypt(key);
			subkey = CryptoUtil.Encrypt(subkey);
			text = CryptoUtil.Encrypt(text);
			SetCookie(key, subkey, text);
		}

		public static void SetTripleDESEncryptedCookie(string key, string text)
		{
            key = CryptoUtil.EncryptTripleDES(key);
			text = CryptoUtil.EncryptTripleDES(text);
			SetCookie(key, text);
		}

		public static void SetTripleDESEncryptedCookie(string key, string subkey, string text)
		{
			key = CryptoUtil.EncryptTripleDES(key);
			subkey = CryptoUtil.EncryptTripleDES(subkey);
			text = CryptoUtil.EncryptTripleDES(text);
			SetCookie(key, subkey, text);
		}

		public static void SetTripleDESEncryptedCookie(string key, string text, DateTime expires)
		{
			key = CryptoUtil.EncryptTripleDES(key);
			text = CryptoUtil.EncryptTripleDES(text);
			SetCookie(key, text, expires);
		}

		public static void SetCookie(string key, string text)
		{
			key = HttpContext.Current.Server.UrlEncode(key);
			text = HttpContext.Current.Server.UrlEncode(text);

			HttpCookie cookie = new HttpCookie(key, text);
			SetCookie(cookie);
		}

		public static void SetCookie(string key, string subkey, string text)
		{
			HttpCookie cookie;

			key = HttpContext.Current.Server.UrlEncode(key);
			subkey = HttpContext.Current.Server.UrlEncode(subkey);
			text = HttpContext.Current.Server.UrlEncode(text);

			try
			{
				cookie = GetCookie(key);
				cookie.Values[subkey] = text;
			}
			catch
			{
				cookie = new HttpCookie(key);
				cookie.Values.Add(subkey, text);
			}

			SetCookie(cookie);			
		}

		public static void SetCookie(string key, string text, DateTime expires)
		{
			key = HttpContext.Current.Server.UrlEncode(key);
			text = HttpContext.Current.Server.UrlEncode(text);

			HttpCookie cookie = new HttpCookie(key, text);
			cookie.Expires = expires;
			SetCookie(cookie);
		}

		public static void SetCookie(string key, string subkey, string text, DateTime expires)
		{
			HttpCookie cookie;

			key = HttpContext.Current.Server.UrlEncode(key);
			subkey = HttpContext.Current.Server.UrlEncode(subkey);
			text = HttpContext.Current.Server.UrlEncode(text);

			try
			{
				cookie = GetCookie(key);
				cookie.Values[subkey] = text;
			}
			catch
			{
				cookie = new HttpCookie(key);
				cookie.Values.Add(subkey, text);
			}

			cookie.Expires = expires;
			SetCookie(cookie);
		}

		public static void SetCookie(HttpCookie cookie)
		{
			HttpContext.Current.Response.Cookies.Set(cookie);
		}

		public static HttpCookie GetCookie(string key)
		{
			key = HttpContext.Current.Server.UrlEncode(key);
			return HttpContext.Current.Request.Cookies.Get(key);
		}

		public static string GetCookieValue(string key)
		{
			try
			{
				string text = GetCookie(key).Value;
				text = HttpContext.Current.Server.UrlDecode(text);
				return text;
			}
			catch
			{
				return string.Empty;
			}
		}

		public static string GetCookieValue(string key, string subkey)
		{
			try
			{
				string text = GetCookie(key).Values[subkey];
				text = HttpContext.Current.Server.UrlDecode(text);
				return text;
			}
			catch
			{
				return string.Empty;
			}
		}

		public static string GetEncryptedCookieValue(string key)
		{
			string text;
			key = CryptoUtil.Encrypt(key);
			text = GetCookieValue(key);
			text = CryptoUtil.Decrypt(text);
			return text;
		}

		public static string GetEncryptedCookieValue(string key, string subkey)
		{
			string text;
			key = CryptoUtil.Encrypt(key);
			subkey = CryptoUtil.Encrypt(subkey);
			text = GetCookieValue(key, subkey);
			text = CryptoUtil.Decrypt(text);
			return text;
		}

		public static string GetTripleDESEncryptedCookieValue(string key)
		{
			string text;
			key = CryptoUtil.EncryptTripleDES(key);
			text = GetCookieValue(key);
			text = CryptoUtil.DecryptTripleDES(text);
			return text;
		}

		public static string GetTripleDESEncryptedCookieValue(string key, string subkey)
		{
			string text;
			key = CryptoUtil.EncryptTripleDES(key);
			subkey = CryptoUtil.EncryptTripleDES(subkey);
			text = GetCookieValue(key, subkey);
			text = CryptoUtil.DecryptTripleDES(text);
			return text;
		}
	}
}
