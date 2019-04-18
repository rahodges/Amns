using System;
using System.Web;
using System.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls {
	/// <summary>
	/// Summary description for ComboBoxBuilder.
	/// </summary>
	internal class ComboBoxBuilder : ControlBuilder {
		public ComboBoxBuilder(){
			//
			// TODO: Add constructor logic here
			//
		}
		public override bool AllowWhitespaceLiterals() {
			return false;
		} 
		
		public override Type GetChildControlType(string tagName, System.Collections.IDictionary attribs) {
			string szTagName = tagName.ToLower();
			int colon = szTagName.IndexOf(':');
			if ((colon >= 0) && (colon < (szTagName.Length + 1))) {
				// Separate the tagname from the namespace
				szTagName = szTagName.Substring(colon + 1, szTagName.Length - colon - 1);
			}
			if (String.Compare(szTagName, "option", true, System.Globalization.CultureInfo.InvariantCulture) == 0 ||
				String.Compare(szTagName, "listitem", true, System.Globalization.CultureInfo.InvariantCulture) == 0 ) {
				return typeof(System.Web.UI.WebControls.ListItem); 
			}
			// No Type was found, throw an exception
			throw new Exception(String.Format("Invalid child with tagname \"{0}\"", tagName));
		}

	}
}