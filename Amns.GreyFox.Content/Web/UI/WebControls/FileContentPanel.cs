using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for FileContentPanel.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:FileContentPanel runat=server></{0}:FileContentPanel>")]
	public class FileContentPanel : System.Web.UI.Control
	{
		private string contentRoot;
	
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string ContentRoot
		{
			get
			{
				return contentRoot;
			}

			set
			{
				if(value != null)
					contentRoot = value.Replace('/', '\\');
			}
		}

		private string filePath;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string FilePath
		{
			get
			{
				return filePath;
			}

			set
			{
				if(value != null)
					filePath = value.Replace('/', '\\');
			}
		}

		private bool errorPathEnabled;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public bool ErrorPathEnabled
		{
			get
			{
				return errorPathEnabled;
			}

			set
			{
				errorPathEnabled = value;
			}
		}

		private string errorPath;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string ErrorPath
		{
			get
			{
				return errorPath;
			}

			set
			{
				if(value != null)
					errorPath = value.Replace('/', '\\');
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			string tempPath;

			if(contentRoot == null)
			{
				throw(new Exception("Root not specified."));
			}

			if(filePath == null)
			{
				return;
			}

			if(!contentRoot.EndsWith("/") || !contentRoot.EndsWith("\\"))
				tempPath = contentRoot + "\\";
			else
				tempPath = contentRoot;

			if(filePath.StartsWith("/") || filePath.StartsWith("\\"))
				tempPath += filePath.Substring(1, filePath.Length);
			else
				tempPath += filePath;

			try
			{
				StreamReader sr = new StreamReader(Context.Server.MapPath(tempPath));
				string text = sr.ReadToEnd();
				text = text.Replace("=\"~", "=\"" + Page.ResolveUrl("~"));
				output.Write(text);
				sr.Close();
			}
			catch
			{
				renderError(output);
			}
		}

		private void renderError(HtmlTextWriter output)
		{
			if(!errorPathEnabled)
			{
				output.Write("404 - Could not find reference specified.");
				return;
			}

			try
			{
				StreamReader sr = new StreamReader(Context.Server.MapPath(errorPath));
				output.Write(sr.ReadToEnd());
				sr.Close();
			}
			catch
			{
				output.Write("Error path not specified.");
			}
		}
	}
}