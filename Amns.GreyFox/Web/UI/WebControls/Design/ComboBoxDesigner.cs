using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls.Design {
	/// <summary>
	/// Summary description for ComboBoxDesigner.
	/// </summary>
	public class ComboBoxDesigner: ControlDesigner {
		public ComboBoxDesigner() {
			//
			// TODO: Add constructor logic here
			//
		}
		public override string GetDesignTimeHtml() {
			ComboBox component = (ComboBox) base.Component;
			if (component.Items.Count >=1) {
				return "<select><option>" + component.Items[0].Text + "</option></select>";
			}
			return "<select><option>" + component.ID + "</option></select>";
		}
	}
}
