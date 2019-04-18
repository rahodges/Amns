using System;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// This contains all default Amns.GreyFox toolbar Items.
	/// </summary>
	public class ToolbarItems
	{
		#region File Buttons

		public static ToolbarButton New()
		{
			return new ToolbarButton("New", "new", Localization.Strings.New);
		}

		public static ToolbarButton Close()
		{
			return new ToolbarButton("Close", "close", Localization.Strings.Close);
		}

		public static ToolbarButton Edit()
		{
			return new ToolbarButton("Edit", "edit", Localization.Strings.Edit);
		}

		public static ToolbarButton Delete()
		{
			return new ToolbarButton("Delete", "delete", Localization.Strings.Delete);
		}

		public static ToolbarButton Save()
		{
			return new ToolbarButton("Save", "save", Localization.Strings.Save);
		}

		#endregion

		#region Edit Buttons

		public static ToolbarButton Copy()
		{
			return new ToolbarButton("Copy", "copy", "Copy", false);
		}

		public static ToolbarButton Paste()
		{
			return new ToolbarButton("Paste", "paste", "Paste", false);
		}

		#endregion

		#region View

		public static ToolbarButton View()
		{
			return new ToolbarButton("View", "view", Localization.Strings.View);
		}

		public static ToolbarButton ViewPane()
		{
			return new ToolbarButton("Viewpane", "viewpane", "Viewpane", false);
		}

		public static ToolbarButton Comments()
		{
			return new ToolbarButton("Comments", "comments", Localization.Strings.Comments);
		}

		public static ToolbarButton Filter()
		{
			return new ToolbarButton("Filter", "filter", Localization.Strings.Filter);
		}

		#endregion
	}
}
