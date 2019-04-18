using System;
using System.Drawing;
using System.Web.UI;
using System.Text;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ToolbarRenderer.
	/// </summary>
	public class TabStripRenderer
	{
		private Control _parentControl			= null;
		private HtmlTextWriter _output			= null;
		private TabStripStyle _style			= new TabStripStyle();

		#region Public Properties

		public Control ParentControl
		{
			get { return _parentControl; }
		}

		public HtmlTextWriter Output
		{
			get { return _output; }
		}

		public TabStripStyle Style
		{
			get { return _style; }
			set { _style = value; }
		}

		#endregion

		public TabStripRenderer(Control parentControl, HtmlTextWriter output)
		{
			_parentControl = parentControl;
			_output = output;
		}

		public TabRenderHandler RenderDivTableStart;        
		public virtual void OnRenderDivTableStart(HtmlTextWriter output)
		{
			if(RenderDivTableStart != null)
				RenderDivTableStart(output);
		}

		public TabRenderHandler RenderDivTableEnd;        
		public virtual void OnRenderDivTableEnd(HtmlTextWriter output)
		{
			if(RenderDivTableEnd != null)
				RenderDivTableEnd(output);
		}
	}
}