using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for StyleSheetLinker.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:StyleSheetLinker runat=server></{0}:StyleSheetLinker>")]
	public class StyleSheetLinker : System.Web.UI.WebControls.Literal
	{
		private string cssStyleSheet;
	
		[Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public string CssStyleSheet 
		{
			get
			{
				return cssStyleSheet;
			}

			set
			{
				cssStyleSheet = value;
			}
		}

//		private bool useOldMethod;
//
//		protected override void OnLoad(System.EventArgs e)
//		{
//			useOldMethod = Page.Request.Browser.MinorVersion <= 4;
//		}

		protected override void OnPreRender(System.EventArgs e)
		{
//			if(!useOldMethod)
				this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CSS",
					"<link rel=\"stylesheet\" type=\"text/css\" href=\"" +
					cssStyleSheet + "\" />");
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
//			if(useOldMethod)
//				output.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" +
//					cssStyleSheet + "\" />");
		}
	}
}
