using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for DbContentCatalogMenu.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:DbContentBreadcrumbsLabel runat=server></{0}:DbContentBreadcrumbsLabel>")]
	public class DbContentBreadcrumbsLabel : System.Web.UI.Control
	{
		string _homeTitle = string.Empty;
		string _homeUrl = string.Empty;

		int _catalogID = 3;
		int _clipID = -1;
		int _maxDepth = 5;
		string _linkFormat = "?ref={0}";
		bool _includeClip = false;
        
		#region Public Properties

		[Bindable(false), Category("Data"), DefaultValue(1)]
		public int CatalogID
		{
			get { return _catalogID; }
			set { _catalogID = value; }
		}

		[Bindable(false), Category("Data"), DefaultValue(1)]
		public int ClipID
		{
			get { return _clipID; }
			set { _clipID = value; }
		}

		[Bindable(false), Category("Appearance"), DefaultValue("")]
		public string HomeTitle
		{
			get { return _homeTitle; }
			set { _homeTitle = value; }
		}

		[Bindable(false), Category("Appearance"), DefaultValue("")]
		public string HomeUrl
		{
			get { return _homeUrl; }
			set { _homeUrl = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue("?ref={0}")]
		public string LinkFormat
		{
			get { return _linkFormat; }
			set { _linkFormat = value; }
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool IncludeClip
		{
			get { return _includeClip; }
			set { _includeClip = value; }
		}

		#endregion

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			int i = 0;
			int catalogDepth = _maxDepth;
			string[] titles = new string[_maxDepth];
			string[] urls = new string[_maxDepth];

			if(_homeTitle != string.Empty)
				catalogDepth--;

			DbContentClip clip = null;

			try
			{
				clip = new DbContentClip(_clipID);
			}
			catch
			{
				output.Write("<div>Clip unavailable for navigation.</div>");
				return;
			}

			DbContentCatalog catalog = clip.ParentCatalog;
	
			while(i < catalogDepth && catalog != null && catalog.ID != _catalogID)
			{
				titles[i] = catalog.Title;
				if(catalog.DefaultClip != null)
				{
					if(catalog.DefaultClip.OverrideUrl != null &&
						catalog.DefaultClip.OverrideUrl != string.Empty)
						urls[i] = Page.ResolveUrl(catalog.DefaultClip.OverrideUrl);
					else
                        urls[i] = Page.ResolveUrl(string.Format(_linkFormat, catalog.DefaultClip.ID));
				}
				else
					urls[i] = string.Empty;

                catalog = catalog.ParentCatalog;
							
				i++;
			}

			i--;

			if(_homeTitle != string.Empty)
			{
				i++; 
				titles[i] = _homeTitle;
				if(_homeUrl != string.Empty)
					urls[i] = Page.ResolveUrl(_homeUrl);
				else
					urls[i] = Page.ResolveUrl("~/");
			}

			while(i != -1)
			{
				output.WriteBeginTag("a");
				output.WriteAttribute("href", urls[i]);
				output.Write(HtmlTextWriter.TagRightChar);
                output.Write(titles[i]);
				output.WriteEndTag("a");
				if(i != 0)
					output.Write(" > ");
				i--;
			}

			if(_includeClip)
			{
				output.Write(" > ");
                output.Write(clip.Title);
			}
		}
	}
}