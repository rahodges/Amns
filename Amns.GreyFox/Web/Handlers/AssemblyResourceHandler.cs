//=============================================================================
// File    : ResSrvHandler.cs
// Author  : Eric Woodruff
// Updated : Mon 04/07/03
// Compiler: Microsoft Visual C#
//
// This file contains a derived System.Web.IHttpHandler class that acts as a
// resource server to send resources to the client browser such as scripts,
// images, etc.
//
// Add the following to the <system.web> section of the Web.Config file in
// applications that make use of the custom control library.  Modify it to
// match your control library names.
//
// <!-- Custom Control Resource Server Handler
//      Add this section to map the resource requests to the resource
//      handler class in the custom control assembly. -->
// <httpHandlers>
// <add verb="*" path="GreyFoxWebResource.axd"
// type="Amns.GreyFox.Web.Handlers.GreyFoxWebResource, Amns.GreyFox.Web.Handlers"
// />
// </httpHandlers>
//
// This code may be used in compiled form in any way you desire.  This
// file may be redistributed by any means in modified or unmodified form
// PROVIDING that this notice and the author's name and any copyright
// notices remain intact.
//
// This file is provided "as is" with no warranty either express or
// implied.  The author accepts no liability for any damage or loss of
// business that this product may cause.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    03/25/2003  EFW  Created the code
// 1.0.1    04/07/2003  EFW  Converted from Web.UI.Page to Web.IHttpHandler
// 1.0.2    07/19/2003  EFW  Added notes to docs about use with forms-based
//                           authentication and updated XML code documentation.
// 1.0.3	08/24/2005	RAH  Updated code for GreyFox
//=============================================================================

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;

namespace Amns.GreyFox.Web.Handlers
{
	/// <summary>
	/// This is a derived <see cref="System.Web.IHttpHandler"/> class that
	/// acts as a resource server to send resources to the client browser
	/// such as scripts, images, etc.
	/// </summary>
	/// <remarks>This allows resources to be embedded in the control assembly
	/// so that they do not have to be distributed and installed separately.
	/// For more information about how this class works, see
	/// <a href="http://www.codeproject.com/useritems/ResSrvPage.asp">
	/// A Resource Server Handler Class For Custom Controls</a> at
	/// <b>The Code Project</b>.</remarks>
	public class AssemblyResourceHandler : System.Web.IHttpHandler
	{
		//=====================================================================
		// Constants

		// TODO: Modify this constant to name the ASPX page that will be
		// referenced in the application Web.Config file to invoke this
		// handler class.

		/// <summary>
		/// The AXD page name that will cause requests to get routed
		/// to this handler.  The default value is
		/// <b>GreyFoxWebResource.axd</b>.
		/// </summary>
		public const string cResSrvHandlerPageName =
			"GreyFoxWebResource.axd";

		// TODO: Modify these two constants to match your control's
		// namespace and the folder names of your resources.  Add any
		// additional constants as needed for other resource types.

		/// <summary>
		/// The path to the image resources.  The default value is
		/// "<b>ResServerTest.Web.Controls.Images.</b>"
		/// </summary>
		private const string cImageResPath =
			"Amns.GreyFox.Web.WebControls.Images.";

		/// <summary>
		/// The path to the script resources.  The default value is
		/// "<b>ResServerTest.Web.Controls.Scripts.</b>"
		/// </summary>
		private const string cScriptResPath =
			"Amns.GreyFox.Web.UI.WebControls.Scripts.";

		//=====================================================================
		// Helper methods

		/// <summary>
		/// Registers an embedded resource script from default assembly's script
		/// location.
		/// </summary>
		/// <param name="page">The page to register the script to.</param>
		/// <param name="scriptName">The name of the script with .js extension.</param>
		public static void RegisterScript(System.Web.UI.Page page, string scriptName)
		{	
			if(!page.ClientScript.IsClientScriptBlockRegistered("gfx_" + scriptName))
			{
				string sourceUrl = ResourceUrl(scriptName, true);

				// Map browser path to script			
				page.ClientScript.RegisterClientScriptBlock(typeof(AssemblyResourceHandler), scriptName, 
					"<script language=\"javascript\" type=\"text/javascript\" src=\"" + 
					sourceUrl + "\"></script>");
			}
		}

		/// <summary>
		/// This can be called to format a URL to a resource name that is
		/// embedded within the assembly.
		/// </summary>
		/// <param name="strResourceName">The name of the resource</param>
		/// <param name="bCacheResource">Specify true to have the
		/// resource cached on the client, false to never cache it.</param>
		/// <returns>A string containing the URL to the resource</returns>
		public static string ResourceUrl(string strResourceName,
			bool bCacheResource)
		{
			return String.Format("{0}?Res={1}{2}", cResSrvHandlerPageName,
				strResourceName, (bCacheResource) ? "" : "&NoCache=1");
		}

		/// <summary>
		/// This can be called to format a URL to a resource name that is
		/// embedded within a different assembly.
		/// </summary>
		/// <param name="strAssemblyName">The name of the assembly that
		/// contains the resource</param>
		/// <param name="strResourceHandlerName">The name of the resource
		/// handler that can retrieve it (i.e. the ASPX page name)</param>
		/// <param name="strResourceName">The name of the resource</param>
		/// <param name="bCacheResource">Specify true to have the
		/// resource cached on the client, false to never cache it.</param>
		/// <returns>A string containing the URL to the resource</returns>
		public static string ResourceUrl(string strAssemblyName,
			string strResourceHandlerName, string strResourceName,
			bool bCacheResource)
		{
			return String.Format("{0}?Assembly={1}&Res={2}{3}",
				strResourceHandlerName,
				HttpContext.Current.Server.UrlEncode(strAssemblyName),
				strResourceName, (bCacheResource) ? "" : "&NoCache=1");
		}

		//=====================================================================
		// System.Web.IHttpHandler methods

		/// <summary>
		/// This property is used to indicate that the object instance can
		/// be used by other requests.  It always returns true.
		/// </summary>
		public bool IsReusable
		{
			get { return true; }
		}

		/// <summary>
		/// Load the resource specified in the query string and return it
		/// as the HTTP response.
		/// </summary>
		/// <param name="hc">The context object for the request</param>
		public void ProcessRequest(HttpContext hc)
		{
			Assembly asm = null;
			StreamReader sr = null;
			Stream s = null;

			string strResName, strType;
			byte[] byImage;
			int nLen;
			bool bUseInternalPath = true;

			// TODO: Be sure to adjust the QueryString names if you are
			// using something other than Res and NoCache.

			// Get the resource name and base the type on the extension
			strResName = hc.Request.QueryString["Res"];
			strType = strResName.Substring(strResName.LastIndexOf('.') + 1).ToLower();

			try
			{
				hc.Response.Clear();

				// If caching is not disabled, set the cache parameters so that
				// the response is cached on the client for up to one day.
				if(hc.Request.QueryString["NoCache"] == null)
				{
					// TODO: Adjust caching length as needed.

					hc.Response.Cache.SetExpires(DateTime.Now.AddDays(1));
					hc.Response.Cache.SetCacheability(HttpCacheability.Public);
					hc.Response.Cache.SetValidUntilExpires(false);

					// Vary by parameter name.  Note that if you have more
					// than one, add additional lines to specify them.
					hc.Response.Cache.VaryByParams["Res"] = true;
				}
				else
				{
					// The response is not cached
					hc.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
					hc.Response.Cache.SetCacheability(HttpCacheability.NoCache);
				}

				// Get the resource from this assembly or another?
				if(hc.Request.QueryString["Assembly"] == null)
				{
					asm = Assembly.GetExecutingAssembly();
				}
				else
				{
					Assembly[] asmList = AppDomain.CurrentDomain.GetAssemblies();
					string strSearchName = hc.Request.QueryString["Assembly"];

					foreach(Assembly a in asmList)
						if(a.GetName().Name == strSearchName)
						{
							asm = a;
							break;
						}

					if(asm == null)
						throw new ArgumentOutOfRangeException("Assembly",
							strSearchName, "Assembly not found");

					// Now get the resources listed in the assembly manifest
					// and look for the filename.  Note the fact that it is
					// matched on the filename and not necessarily the path
					// within the assembly.  This may restricts you to using
					// a filename only once, but it also prevents the problem
					// that the VB.NET compiler has where it doesn't seem to
					// output folder names on resources.
					foreach(string strResource in asm.GetManifestResourceNames())
						if(strResource.EndsWith(strResName))
						{
							strResName = strResource;
							bUseInternalPath = false;
							break;
						}
				}

				switch(strType)
				{
					case "gif":     // Image types
					case "jpg":
					case "jpeg":
					case "bmp":
					case "png":
					case "tif":
					case "tiff":
						if(strType == "jpg")
							strType = "jpeg";
						else
							if(strType == "png")
							strType = "x-png";
						else
							if(strType == "tif")
							strType = "tiff";

						hc.Response.ContentType = "image/" + strType;

						if(bUseInternalPath == true)
							strResName = cImageResPath + strResName;

						s = asm.GetManifestResourceStream(strResName);
						ReadBinaryResource(s, out byImage, out nLen);
						hc.Response.OutputStream.Write(byImage, 0, nLen);
						break;

					case "js":      // Script types
					case "vb":
					case "vbs":
						if(strType == "js")
							hc.Response.ContentType = "text/javascript";
						else
							hc.Response.ContentType = "text/vbscript";

						if(bUseInternalPath == true)
							strResName = cScriptResPath + strResName;

						sr = new StreamReader(
							asm.GetManifestResourceStream(strResName));
						hc.Response.Write(sr.ReadToEnd());
						break;

					case "css":     // Some style sheet info
						// Not enough to embed so we'll write it out from here
						hc.Response.ContentType = "text/css";

						if(bUseInternalPath == true)
							hc.Response.Write(".Style1 { font-weight: bold; " +
								"color: #dc143c; font-style: italic; " +
								"text-decoration: underline; }\n" +
								".Style2 { font-weight: bold; color: navy; " +
								"text-decoration: underline; }\n");
						else
						{
							// CSS from some other source
							sr = new StreamReader(
								asm.GetManifestResourceStream(strResName));
							hc.Response.Write(sr.ReadToEnd());
						}
						break;

					case "htm":     // Maybe some html
					case "html":
						hc.Response.ContentType = "text/html";

						sr = new StreamReader(
							asm.GetManifestResourceStream(strResName));
						hc.Response.Write(sr.ReadToEnd());
						break;

					case "xml":     // Even some XML
						hc.Response.ContentType = "text/xml";

						sr = new StreamReader(
							asm.GetManifestResourceStream(
							"ResServerTest.Web.Controls." + strResName));

						// This is used to demonstrate the NoCache option.
						// We'll modify the XML to show the current server
						// date and time.
						string strXML = sr.ReadToEnd();

						hc.Response.Write(strXML.Replace("DATETIME",
							DateTime.Now.ToString()));
						break;

					default:    // Unknown resource type
						throw new Exception("Unknown resource type");
				}
			}
			catch(Exception excp)
			{
				XmlDocument xml;
				XmlNode node, element;

				string strMsg = excp.Message.Replace("\r\n", " ");

				hc.Response.Clear();
				hc.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
				hc.Response.Cache.SetCacheability(HttpCacheability.NoCache);

				// For script, write out an alert describing the problem.
				// For XML, send an XML response containing the exception.
				// For all other resources, just let it display a broken link
				// or whatever.
				switch(strType)
				{
					case "js":
						hc.Response.ContentType = "text/javascript";
						hc.Response.Write("alert(\"Could not load resource '" +
							strResName + "': " + strMsg + "\");");
						break;

					case "vb":
					case "vbs":
						hc.Response.ContentType = "text/vbscript";
						hc.Response.Write("MsgBox \"Could not load resource '" +
							strResName + "': " + strMsg + "\"");
						break;

					case "xml":
						xml = new XmlDocument();
						node = xml.CreateElement("ResourceError");

						element = xml.CreateElement("Resource");
						element.InnerText = "Could not load resource: " +
							strResName;
						node.AppendChild(element);

						element = xml.CreateElement("Exception");
						element.InnerText = strMsg;
						node.AppendChild(element);

						xml.AppendChild(node);
						hc.Response.Write(xml.InnerXml);
						break;
				}
			}
			finally
			{
				if(sr != null)
					sr.Close();

				if(s != null)
					s.Close();
			}
		}

		/// <summary>
		/// Read a binary resource such as an image in from the stream.  Since
		/// the size of the resource cannot be obtained, it is read in a block
		/// at a time until the end of the stream is reached.
		/// </summary>
		/// <param name="sIn">The stream from which to read the resource</param>
		/// <param name="byBuffer">An out parameter to receive the resource bytes</param>
		/// <param name="nLen">An out parameter to receive the resource length</param>
		public void ReadBinaryResource(Stream sIn, out Byte[] byBuffer,
			out int nLen)
		{
			byte[] byTemp;
			int nBytesRead;

			byBuffer = new Byte[1024];
			nLen = 0;

			do
			{
				if(byBuffer.Length < nLen + 1024)
				{
					byTemp = new Byte[byBuffer.Length * 2];
					Array.Copy(byBuffer, byTemp, byBuffer.Length);
					byBuffer = byTemp;
				}

				nBytesRead = sIn.Read(byBuffer, nLen, 1024);
				nLen += nBytesRead;

			} while(nBytesRead == 1024);
		}
	}
}