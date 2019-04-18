using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Amns.GreyFox.Content.WebControls
{
	/// <summary>
	/// Summary description for ContentGrid.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:ContentGrid runat=server></{0}:ContentGrid>")]
	public class ContentGrid : System.Web.UI.WebControls.WebControl
	{
		private string text;
	
		[Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public string Text 
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
			}
		}

		private string xmlPath;
		[Bindable(true),
		Category("Data"),
		DefaultValue("")]
		public string XmlPath
		{
			get
			{
				return xmlPath;
			}
			set
			{
				xmlPath = value;
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.Write(Text);
		}
	}
}