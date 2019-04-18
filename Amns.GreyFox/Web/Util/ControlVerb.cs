using System;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Control verbs are verbs available for a class that allow automated menu creation
	/// for controls on a page. For example, a grid with a "Copy" verb exposes a copy method
	/// that can be used to populate an "Edit" menu bar automatically. When "Edit" is selected
	/// in the menu bar, the grid's edit function is executed.
	/// 
	/// In addition, a web utility can populate service functions for members selected. For
	/// example, a Member Grid on a page can expose and add a "Members" menu item. When this
	/// "Members" menu item is created with a "MemberSelect" verb, verbs with the MemberSelect
	/// tag can populate menu items based on the MemberSelect Verb. 
	/// </summary>
	public class Verb
	{
		string name;			// Member Edit					Fix Attendance
		string description;		// Edits a selected member		Fix Member's Attendance
		string verb;			// Edit							FixAttendance
		string[] nouns;			// DojoMember					DojoMember
		string[] roles;			// Tessen/Administrator			Tessen/Administrator

		public string Name { get { return name; } set { name = value; } }
		public string Description { get { return description; } set { description = value; } }
		public string Verb { get { return verb; } set { verb = value; } }
		public string Nouns { get { return nouns; } set { nouns = value; } }

		public Verb()
		{
		}
	}
}