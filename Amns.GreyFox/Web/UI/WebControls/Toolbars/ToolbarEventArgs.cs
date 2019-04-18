using System;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Event args for TableWindow button events. Carries the button name.
	/// </summary>
	public class ToolbarEventArgs : System.EventArgs
	{
		private ToolbarItem _selectedToolbarItem;

		public ToolbarItem SelectedToolbarItem
		{
			get { return _selectedToolbarItem; }
		}

		public ToolbarEventArgs(ToolbarItem selectedToolbarItem)
		{
			_selectedToolbarItem = selectedToolbarItem;
		}
	}
}
