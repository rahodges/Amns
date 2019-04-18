using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for TableWindowViewPane.
	/// </summary>
	public class TableWindowViewPane
	{
		internal TableWindow __parentWindow	= null;

		#region Public Properties

		public TableWindow ParentWindow
		{
			get { return __parentWindow; }
		}

		public string RequestArgs
		{
			get 
			{ 
				if(__parentWindow != null)
				{
					return __parentWindow.Page.Request.QueryString["__" + __parentWindow.ID + "_ID"];
				}
				else
				{
					return string.Empty;
				}
			}
		}

		#endregion

		public TableWindowViewPane()
		{			
		}

		public virtual void Render(HtmlTextWriter output)
		{
			// This class should never be utilized directly, detect this
			// non overriden render method and throw an error.
			output.Write("<h2>Error:</h2>Viewpane is missing, please" +
				"notify technical support regarding this message. " +
				"The viewpane's Render method has not been overridden");
		}

		protected void RenderTableBeginTag(HtmlTextWriter output, string id,
			Unit cellPadding, Unit cellSpacing, Unit width, Unit height, string cssClass)
		{
			output.WriteBeginTag("table");
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(cssClass != null && cssClass != string.Empty)
				output.WriteAttribute("class", cssClass);
			if(!width.IsEmpty)
				output.WriteAttribute("width", width.ToString());
			if(!height.IsEmpty)
				output.WriteAttribute("height", height.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
		}
	}
}
