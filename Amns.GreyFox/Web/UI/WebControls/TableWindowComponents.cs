using System;

namespace Amns.GreyFox.Web.UI.WebControls
{
	[Flags]
	public enum TableWindowComponents : byte
	{
		Toolbar = 0x01,
		Footer = 0x02,
		Tabs = 0x04,
		ContentHeader = 0x08,
//		Buttons = 0x10,
		ViewPane = 0x20,
//		ViewPaneHeader = 0x40,
//		ViewPaneFooter = 0x80,
	}
}
