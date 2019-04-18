using System;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for MultiselectorMode.
	/// </summary>
	public enum MultiSelectBoxMode 
	{
		/// <summary>
		/// Renders two select boxes and uses OptionTransfer.js client javascript.
		/// </summary>
		DualSelect, 
		/// <summary>
		/// Renders a checkbox list.
		/// </summary>
		CheckBoxList, 
		/// <summary>
		/// Renders a listbox control.
		/// </summary>
		ListBox, 
		/// <summary>
		/// Renders a radiobox control.
		/// </summary>
		RadioBoxList, 
		/// <summary>
		/// Renders a dropdown list control.
		/// </summary>
		DropDownList,
		/// <summary>
		/// Renders a dropdown list with search features.
		/// </summary>
		SearchableDropDownList
	}
}