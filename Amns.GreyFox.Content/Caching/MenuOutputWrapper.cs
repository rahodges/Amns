using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Caching
{
	/// <summary>
	/// Summary description for MenuWrapper.
	/// </summary>
	public class MenuOutputWrapper
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

		public MenuOutputWrapper()
		{
//			_isUnbound = true;
		}

		public void Bind(Menu menu)
		{
			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter output = new HtmlTextWriter(stringWriter);

			// Render
			menu.RenderControl(output);
			_output = stringWriter.ToString();
			output.Close();
			stringWriter.Close();
//			_isUnbound = false;
		}
	}
}
