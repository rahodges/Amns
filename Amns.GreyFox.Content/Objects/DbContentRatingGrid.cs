using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for DbContentRating.
	/// </summary>
	/// </summary>
	[ToolboxData("<{0}:DbContentRatingGrid runat=server></{0}:DbContentRatingGrid>")]
	public class DbContentRatingGrid : TableGrid
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
			DbContentRatingManager m = new DbContentRatingManager();
			DbContentRatingCollection dbContentRatingCollection = m.GetCollection(string.Empty, string.Empty, null);
			// Render Header Row
			this.headerLockEnabled = true;
			RenderRow(this.HeaderRowCssClass, );

			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(DbContentRating dbContentRating in dbContentRatingCollection)
			{
				if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
				rowflag = !rowflag;
				output.WriteBeginTag("tr");
				output.WriteAttribute("i", dbContentRating.ID.ToString());
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
