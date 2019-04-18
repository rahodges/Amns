using System;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// This contains all default Amns.GreyFox toolbar Items.
	/// </summary>
	public class ToolBarUtility
	{
		#region File Buttons

        public static ToolBar DefaultToolBar(string id)
        {
            ToolBar toolbar = new ToolBar();

            toolbar.ID = id;
            toolbar.CssClass = "toolbar";
            toolbar.ImagesBaseUrl = "images/toolbar";
            toolbar.DefaultItemCssClass = "item";
            toolbar.DefaultItemHoverCssClass = "itemHover";
            toolbar.DefaultItemActiveCssClass = "itemActive";
            toolbar.DefaultItemCheckedCssClass = "itemChecked";
            toolbar.DefaultItemCheckedHoverCssClass = "itemActive";
            toolbar.DefaultItemTextImageSpacing = 5;
            toolbar.DefaultItemTextImageRelation = ToolBarTextImageRelation.ImageBeforeText;
            toolbar.DefaultItemImageHeight = Unit.Pixel(16);
            toolbar.DefaultItemImageWidth = Unit.Pixel(16);
            toolbar.ItemSpacing = Unit.Pixel(1);
            toolbar.Orientation = GroupOrientation.Horizontal;
            toolbar.EnableViewState = false;

            return toolbar;
        }

        public static void AddControlItem(ToolBar toolbar, System.Web.UI.Control control)
        {
            ToolBarItem item = new ToolBarItem();
            item.ItemType = ToolBarItemType.Separator;
            item.CustomContentId = toolbar.ID + "_" + control.ID;
            toolbar.Items.Add(item);

            ToolBarItemContent c = new ToolBarItemContent();
            c.ID = toolbar.ID + "_" + control.ID;
            c.Controls.Add(control);
            toolbar.Content.Add(c);
        }

        public static ToolBarItem Break()
        {
            ToolBarItem item = new ToolBarItem();
            item.ItemType = ToolBarItemType.Separator;
            item.ImageUrl = "break.gif";
            item.ImageHeight = Unit.Pixel(16);
            item.ImageWidth = Unit.Pixel(2);
            return item;
        }

        public static ToolBarItem CommandItem(string command, string text, string imageUrl, string clientSideCommand)
        {
            ToolBarItem item = CommandItem(command, text, imageUrl);
            item.ID = command;
            item.ClientSideCommand = clientSideCommand;
            return item;
        }

        public static ToolBarItem CommandItem(string command, string text, string imageUrl)
        {
            ToolBarItem item = new ToolBarItem();
            item.ID = command;
            item.ItemType = ToolBarItemType.Command;
            item.ToolTip = text;
            item.Value = command;
            item.ImageUrl = imageUrl;
            item.AutoPostBackOnSelect = true;
            return item;
        }

		public static ToolBarItem New()
		{
            return CommandItem("new", Localization.Strings.New, "new.gif");
		}

        public static ToolBarItem Close()
		{
            return CommandItem("close", Localization.Strings.Close, "close.gif");
		}

        public static ToolBarItem Edit()
		{
            return CommandItem("edit", Localization.Strings.Edit, "edit.gif");
		}

        public static ToolBarItem Delete()
		{
            return CommandItem("delete", Localization.Strings.Delete, "delete.gif");
		}

        public static ToolBarItem Save()
		{
            return CommandItem("save", Localization.Strings.Save, "save.gif");
		}

		#endregion

		#region Edit Buttons

        public static ToolBarItem Copy()
		{
            return CommandItem("copy", Localization.Strings.Copy, "copy.gif");
		}

        public static ToolBarItem Paste()
		{
            return CommandItem("paste", Localization.Strings.Paste, "paste.gif");
		}

		#endregion

		#region View

        public static ToolBarItem View()
		{
            return CommandItem("view", Localization.Strings.View, "view.gif");
		}

        public static ToolBarItem ViewPane()
		{
            return CommandItem("viewpane", Localization.Strings.Viewpane, "viewpane.gif");
		}

        public static ToolBarItem Expand()
        {
            return CommandItem("expand", Localization.Strings.Viewpane, "expand.gif");
        }

        public static ToolBarItem Comments()
		{
            return CommandItem("comments", Localization.Strings.Comments, "comments.gif");
		}

        public static ToolBarItem Filter()
		{
            return CommandItem("filter", Localization.Strings.Filter, "filter.gif");
		}

		#endregion
	}
}
