using System;
using System.Configuration;
using System.Web;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for DbContentPageServer.
	/// </summary>
	public sealed class ContentServer
	{
		#region Singleton Fields and Methods

		static readonly ContentServer _instance = new ContentServer();

		static ContentServer()
		{
		}

		public static ContentServer CurrentContext
		{
			get { return _instance; }
		}

		#endregion

		bool _isAccess = false;
		string _connectionString = string.Empty;
		string _pagePathOverrideQueryKey = "ref";						// If this query key is found in the requested url, the content id specified by it will replace the one in the PagePath
		string _transferPathFormat = "~/content.aspx?ref={0}";
		string _leftPagePathFormat = "~/gfxc_";
		string _rightPagePathFormat = ".aspx";
		string _pathPrefix = string.Empty;

		public string PagePathFormat
		{
			get 
			{ 
				return _leftPagePathFormat + "{0}" + _rightPagePathFormat; 
			}
			set 
			{
				int tokenIndex = value.IndexOf("{0}");
				if(tokenIndex == -1)
					throw(new ContentException("Cannot find '{0}' token in PagePathFormat.", 1093));
				_leftPagePathFormat = value.Substring(0, tokenIndex);
				_rightPagePathFormat = value.Substring(tokenIndex + 3, value.Length - _leftPagePathFormat.Length - 3);
			}
		}

		ContentServer()
		{
		}

		public void Bind(HttpApplication application)
		{
			if(application.Context == null)
				throw(new ContentException("Application HttpContext is null, be sure application is running under IIS.", 1002));
			if(application.Server == null)
				throw(new ContentException("Application Server is null.", 1003));

			if(ConfigurationManager.AppSettings["GreyFoxContent_IsAccess"] == null)
				throw(new ContentException("DbContentServer cannot find 'GreyFoxContent_IsAccess' key.", 1006));

            try { _isAccess = bool.Parse(ConfigurationManager.AppSettings["GreyFoxContent_IsAccess"]); }
			catch { throw(new ContentException("DbContentServer cannot parse 'GreyFoxContent_IsAccess' key. Check to make sure the value is set to 'true' of 'false'.", 1007)); }
			
			if(ConfigurationManager.AppSettings["GreyFoxContent_Database"] == null)
				throw(new ContentException("DbContentServer cannot find 'GreyFoxContent_Database' key.", 1008));

			if(_isAccess)
			{
				_connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
					application.Server.MapPath(ConfigurationManager.AppSettings["GreyFoxContent_Database"]);
			}
			else
			{
                _connectionString = ConfigurationManager.AppSettings["GreyFoxContent_Database"];
			}

			// This line wasn't working :(
			application.BeginRequest += new EventHandler(this.ProcessRequest);
		}

		public void ProcessRequest(object sender, EventArgs e)
		{
			this.ProcessRequest();
		}

		public void ProcessRequest()
		{
			HttpContext context = HttpContext.Current;
			string requestPath = context.Request.Url.AbsolutePath;
			string applicationPath = context.Request.ApplicationPath;
			string leftPath;

			// Replace Page Path with Application Directory
			if(applicationPath == "/")
				leftPath = _leftPagePathFormat.Replace("~", "");
			else
				leftPath = _leftPagePathFormat.Replace("~", applicationPath);
				
			if(requestPath.StartsWith(leftPath) && requestPath.EndsWith(_rightPagePathFormat))
			{
				string pageId;				
				string filePath;
				string requestQuery = context.Request.Url.Query;
				int overrideKeyQueryIndex = requestQuery.IndexOf(_pagePathOverrideQueryKey);

				if(overrideKeyQueryIndex != -1)
				{
					// Extract the key value, don't forget to trim the equals sign with '+1'
					string keyValue = requestQuery.Substring(overrideKeyQueryIndex + _pagePathOverrideQueryKey.Length + 1);
					if(keyValue.IndexOf("&") != -1)
						keyValue = keyValue.Substring(0, keyValue.Length - keyValue.IndexOf("&"));
					pageId = keyValue;
				}
				else
				{
					// Extract the page id by getting the value from the requested page file name
					pageId = requestPath.Substring(leftPath.Length, requestPath.Length - leftPath.Length - _rightPagePathFormat.Length);
				}
					
				// Rewrite the file path using the transfer format with the specified id
				// ======================================================================
				// THERE WAS A FORWARD SLASH AFTER THE TILDE IN THE REPLACE METHOD BELOW,
				// IT HAS BEEN REMOVED FOR COMPATABILITY WITH APPLICATION DIRECTORIES
				// UNDER ROOT IIS INETPUB DIRECTORY.
				// ======================================================================
				filePath = string.Format(_transferPathFormat.Replace("~", context.Request.ApplicationPath), pageId);

				// Rewrite the path to context - thus serving the virtual page with an existing one.
				context.RewritePath(filePath);
			}
		}
	}
}
