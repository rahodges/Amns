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
	public class TableWindowDesigner : ControlDesigner 
	{
		public TableWindowDesigner() 
		{

		}

		public override string GetDesignTimeHtml() 
		{
			string initMessage = string.Empty;

			System.Text.StringBuilder s = new System.Text.StringBuilder();

			TableWindow component = (TableWindow) base.Component;

//			initMessage = component.__designInit();

			s.Append("<table id=\"" + component.ClientID + "\"");
			if(component.CssClass != "")
				s.Append(" class=\"" + component.CssClass + "\"");
			if(!component.CellPadding.IsEmpty)
				s.Append(" cellPadding=\"" + component.CellPadding.ToString() + "\"");
			if(!component.CellSpacing.IsEmpty)
				s.Append(" cellSpacing=\"" + component.CellSpacing.ToString() + "\"");
			if(!component.BorderWidth.IsEmpty)
				s.Append(" border=\"" + component.BorderWidth.ToString() + "\"");
			if(!component.Width.IsEmpty)
				s.Append(" width=\"" + component.Width.ToString() + "\"");
			if(!component.Height.IsEmpty)
			{
				s.Append(" height=\"200px\"");
				s.Append(" style=\"height:200px;\"");

//				s.Append(" height=\"" + component.Height.ToString() + "\"");
//				s.Append(" style=\"height:" + component.Height.ToString() + ";\"");
			}
			s.Append(">");

			// Title

			s.Append("<tr><th");
			if(component.ColumnCount != 1)
				s.Append(" colspan=\"" + component.ColumnCount + "\"");
			if(component.HeaderCssClass != string.Empty)
				s.Append(" class=\"" + component.HeaderCssClass + "\"");
			s.Append(">");
			s.Append(component.Text + " (" + component.ID + ")");
			s.Append("</th></tr>");

			// Toolbar

			if(component.ComponentCheck(TableWindowComponents.Toolbar))
			{
				s.Append("<tr>");
				s.Append("<td ");
				if(component.ColumnCount != 1)
					s.Append(" colspan=\"" + component.ColumnCount + "\"");
				if(component.SubHeaderCssClass != string.Empty)
					s.Append(" class=\"" + component.SubHeaderCssClass + "\"");
				s.Append(" height=\"28px\"");
				s.Append(">");
				s.Append("Toolbar");
				s.Append("</td></tr>");
			}			
            
			// Content

//			if(component.ComponentCheck(TableWindowComponents.ContentHeader))
//			{
//				s.Append("<tr>");
//				s.Append("<td ");
//				if(component.ColumnCount != 1)
//					s.Append(" colspan=\"" + component.ColumnCount + "\"");
//				if(component.ContentCssClass != string.Empty)
//					s.Append(" class=\"" + component.ContentCssClass + "\"");
//				s.Append(" valign=\"top\"");
//				s.Append(" height=\"100%\"");
//				s.Append(" width=\"100%\"");
//				s.Append(">");
//				s.Append("Content Header");
//				s.Append("</td></tr>");
//			}

			s.Append("<tr>");
			s.Append("<td ");
			if(component.ColumnCount != 1)
				s.Append(" colspan=\"" + component.ColumnCount + "\"");
			if(component.ContentCssClass != string.Empty)
				s.Append(" class=\"" + component.ContentCssClass + "\"");
			s.Append(" valign=\"top\"");
			s.Append(" height=\"100%\"");
			s.Append(" width=\"100%\"");
			s.Append(">");

			s.Append("<div");
			s.Append(" id=" + component.ID + "_ContentDiv");
			s.Append(" style=\"");
			if(component.ComponentCheck(TableWindowComponents.ViewPane))
			{
				s.Append("float:left;visibility:visible;");
				s.Append("width:" + component.ContentWidth.ToString() + ";");
			}
			else
			{
				s.Append(" width:100%;");
			}
			if(component.FeatureCheck(TableWindowFeatures.Scroller))
				s.Append("overflow:scroll;");
			s.Append("height:100%;");
			s.Append("\"");
			if(component.ContentCssClass != string.Empty)
				s.Append(" class=\"" + component.ContentCssClass + "\"");
			s.Append(">");

			// Fake Table
			s.Append("<table id=\"" + component.ClientID + "_datatable\"");
			s.Append(" cellpadding=\"" + component.CellPadding.ToString() + "\"");
			s.Append(" cellspacing=\"" + component.CellSpacing.ToString() + "\"");
			s.Append(" class=\"" + component.ContentCssClass.ToString() + "\"");
			s.Append(" width=\"100%\"");
			s.Append(">");

			if(component is TableGrid)
			{
				TableGrid g = (TableGrid) component;
				s.Append("<tr><td class=\"" + g.HeaderRowCssClass + "\">Item</td>" +
					"<td class=\"" + g.HeaderRowCssClass + "\">Quantity</td></tr>");
				s.Append("<tr><td class=\"" + g.SelectedRowCssClass + "\">Object One</td>" +
					"<td class=\"" + g.SelectedRowCssClass + "\">34</td></tr>");
				s.Append("<tr><td class=\"" + g.DefaultRowCssClass + "\">Object Two</td>" +
					"<td class=\"" + g.DefaultRowCssClass + "\">17</td></tr>");
				s.Append("<tr><td class=\"" + g.AlternateRowCssClass + "\">Object Three</td>" +
					"<td class=\"" + g.AlternateRowCssClass + "\">22</td></tr>");
			}
			else
			{
				s.Append("<tr><td>Item 1</td></tr>");
				s.Append("<tr><td>Item 2</td></tr>");
				s.Append("<tr><td>Item 3</td></tr>");
				s.Append("<tr><td>Item 4</td></tr>");
			}

			s.Append("</table>");

			s.Append("</div>");

			// Viewpane Div
			if(component.ComponentCheck(TableWindowComponents.ViewPane))
			{
				s.Append("<div id=\"__gfxViewPane_" + component.ID + "\"");
				s.Append(" style=\"overflow:scroll;visibility:visible;width=100%;height=100%;\"");
				s.Append(">");
				if(initMessage != string.Empty)
					s.Append(initMessage);
				else
					s.Append("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Cras eget eros. " +
						"Etiam vel ante. Donec vehicula.");
				s.Append("</div>");
			}

			s.Append("</td></tr>");

//			if(component.ComponentCheck(TableWindowComponents.Footer))
//			{
//				s.Append("<tr>");
//				s.Append("<td ");
//				if(component.ColumnCount != 1)
//					s.Append(" colspan=\"" + component.ColumnCount + "\"");
//				if(component.ContentCssClass != string.Empty)
//					s.Append(" class=\"" + component.ContentCssClass + "\"");
//				s.Append(" valign=\"top\"");
//				s.Append(" height=\"100%\"");
//				s.Append(" width=\"100%\"");
//				s.Append(">");
//				s.Append("Footer");
//				s.Append("</td></tr>");
//			}

			// End Table

			s.Append("</table>");

			return s.ToString();
		}
	}
}