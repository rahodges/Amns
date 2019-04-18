using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.Content;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ContentPanel.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:ContentPanel runat=server></{0}:ContentPanel>")]
	public class ContentPanel : System.Web.UI.Control
	{
		private string xmlPath;
	
		[Bindable(true), 
			Category("Source"), 
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

		private string groupID;
	
		[Bindable(true), 
		Category("Source"), 
		DefaultValue("")] 
		public string GroupID
		{
			get
			{
				return groupID;
			}

			set
			{
				groupID = value;
			}
		}

		private string clipID;
	
		[Bindable(true), 
		Category("Source"), 
		DefaultValue("")] 
		public string ClipID
		{
			get
			{
				return clipID;
			}

			set
			{
				clipID = value;
			}
		}

		private XmlClip content;
		private string errorMessage;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			
		}

		protected override void CreateChildControls() 
		{
			try
			{
				content = XmlContentManager.GetClip(Context.Server.MapPath(xmlPath), groupID, clipID);
			}
			catch (Exception ex)
			{
				errorMessage = ex.ToString();
			}

			if(content != null)
			{
				if(content.HasClientScriptBlock)
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(), ID + "_ClientScript", content.ClientScriptBlock);
				if(content.clientCssLink != string.Empty)
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(), ID + "_ClientCssLink", 
						"<link rel=\"stylesheet\" type=\"text/css\" href=\"" +
						Page.ResolveUrl(content.clientCssLink) + "\" >");
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			try
			{
				if(content.IsOverriden)
				{
					StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(content.OverrideUrl), System.Text.Encoding.UTF8);
					output.Write(sr.ReadToEnd());
					sr.Close();
				}
				else
					output.Write(content.ToHtml());
			}
			catch (Exception e)
			{
				output.Write(e.Message);
			}			
		}

		protected override void LoadViewState(object savedState) 
		{
			// Customize state management to handle saving state of contained objects.

			if (savedState != null) 
			{
				object[] myState = (object[])savedState;

				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
                    xmlPath = myState[1].ToString();					
				if (myState[2] != null)
					groupID = myState[2].ToString();
				if (myState[3] != null)
					clipID = myState[3].ToString();
			}
		}

		protected override object SaveViewState() 
		{
			// Customized state management to handle saving state of contained objects  such as styles.

			object baseState = base.SaveViewState();
			
			object[] myState = new object[4];
			myState[0] = baseState;
			myState[1] = xmlPath;
			myState[2] = groupID;
			myState[3] = clipID;

			return myState;
		}
	}
}