using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Security.Web.UI.WebControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for GreyFoxRole.
	/// </summary>
	/// </summary>
	[ToolboxData("<{0}:GreyFoxRoleGrid runat=server></{0}:GreyFoxRoleGrid>")]
	public class GreyFoxRoleGrid : TableGrid
	{

		#region Public Properties
		#endregion

		protected override void OnInit(EventArgs e)
		{
			features = TableWindowFeatures.ClipboardCopier | 
				TableWindowFeatures.Scroller |
				TableWindowFeatures.WindowPrinter |
				TableWindowFeatures.ClientSideSelector;
		}

		#region Rendering

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			GreyFoxRoleManager m = new GreyFoxRoleManager();
			GreyFoxRoleCollection greyFoxRoleCollection = m.GetCollection(string.Empty, string.Empty);
			// Render Header Row
			this.headerLockEnabled = true;
			RenderRow(this.HeaderRowCssClass, );

			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(GreyFoxRole greyFoxRole in greyFoxRoleCollection)
			{
				if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
				rowflag = !rowflag;
				output.WriteBeginTag("tr");
				output.WriteAttribute("i", greyFoxRole.ID.ToString());
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent++;

				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
		}

		#endregion

	}
}
