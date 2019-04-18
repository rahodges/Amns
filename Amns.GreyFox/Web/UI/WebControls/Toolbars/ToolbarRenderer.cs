using System;
using System.Drawing;
using System.Web.UI;
using System.Text;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ToolbarRenderer.
	/// </summary>
	public class ToolbarRenderer
	{
		private Control _parentControl			= null;
		private HtmlTextWriter _output			= null;
		private ToolbarStyle _style				= new ToolbarStyle();

		#region Public Properties

		public Control ParentControl
		{
			get { return _parentControl; }
		}

		public HtmlTextWriter Output
		{
			get { return _output; }
		}

		public ToolbarStyle Style
		{
			get { return _style; }
			set { _style = value; }
		}

		#endregion

		public ToolbarRenderer(Control parentControl, HtmlTextWriter output)
		{
			_parentControl = parentControl;
			_output = output;
		}
	}
}