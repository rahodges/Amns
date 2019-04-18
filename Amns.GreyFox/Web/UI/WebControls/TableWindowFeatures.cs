using System;

namespace Amns.GreyFox.Web.UI.WebControls
{
	[Flags]
	public enum TableWindowFeatures : byte
	{
		DisableContentSeparation = 0x01,	// Window contents are inline instead of separated.
		Scroller = 0x02,					// Window contents are scrolled.
		// Reserved = 0x04,
		Dragger = 0x08,						// Window is draggable.
		WindowPrinter = 0x10,				// Window is printable.
		ClientSideSelector = 0x20,			// Window will use client side selector.
		// use 'BuildSelectorLink(selectionID)' for selection links
		// use 'BuildSelectorRow(selectionID)' for cell toggles
		ClipboardCopier= 0x40,
		//		Reserved = 0x80,
	}
}
