using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.People.Web.UI.WebControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for GreyFoxContact.
	/// </summary>
	/// </summary>
	[ToolboxData("<{0}:GreyFoxContactGrid runat=server></{0}:GreyFoxContactGrid>")]
	public class GreyFoxContactGrid : TableGrid
	{
		private string tableName = "sysGlobal_Contacts";

		#region Public Properties
		[Bindable(false),
			Category("Data"),
			DefaultValue("sysGlobal_Contacts")]
		public string TableName
		{
			get
			{
				return tableName;
			}
			set
			{
				tableName = value;
			}
		}

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
			GreyFoxContactManager m = new GreyFoxContactManager(tableName);
			GreyFoxContactCollection greyFoxContactCollection = m.GetCollection(string.Empty, string.Empty);
			// Render Header Row
			this.headerLockEnabled = true;
			RenderRow(this.HeaderRowCssClass, );

			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(GreyFoxContact greyFoxContact in greyFoxContactCollection)
			{
				if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
				rowflag = !rowflag;
				output.WriteBeginTag("tr");
				output.WriteAttribute("i", greyFoxContact.ID.ToString());
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
