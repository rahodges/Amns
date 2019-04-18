using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Caching
{
	/// <summary>
	/// Summary description for SiteMapWrapper.
	/// </summary>
	public class SiteMapOutputWrapper
	{
//		private string _script;
		private string _output;
//		private bool _isUnbound;

//		public string Script
//		{
//			get { return _script; }
//		}

		public string Output
		{
			get { return _output; }
		}

		public SiteMapOutputWrapper()
		{
//			_isUnbound = true;
		}

        public void Bind(ComponentArt.Web.UI.SiteMap siteMap)
		{
			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter output = new HtmlTextWriter(stringWriter);

			// Render
			siteMap.RenderControl(output);
			_output = stringWriter.ToString();
			output.Close();
			stringWriter.Close();
//			_isUnbound = false;
		}
	}
}
