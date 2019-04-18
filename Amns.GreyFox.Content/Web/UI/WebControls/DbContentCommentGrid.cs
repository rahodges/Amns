using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for DbContentComment.
	/// </summary>
	/// </summary>
	[DefaultProperty("ConnectionString"),
		ToolboxData("<{0}:DbContentCommentGrid runat=server></{0}:DbContentCommentGrid>")]
	public class DbContentCommentGrid : TableGrid
	{
		private string connectionString;

		#region Public Properties
		[Bindable(false),
			Category("Data"),
			DefaultValue("")]
		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				// Parse Connection String
				if(value.StartsWith("<jet40virtual>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(value.Substring(14, value.Length - 14));
				else if(value.StartsWith("<jet40config>") & Context != null)
					connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA SOURCE=" +
						Context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings.Get(value.Substring(13, value.Length - 13)));
				else
					connectionString = value;
			}
		}

		#endregion

		#region Rendering

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			DbContentCommentManager m = new DbContentCommentManager();
			DbContentCommentCollection dbContentCommentCollection = m.GetCollection(string.Empty, string.Empty);
			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(DbContentComment dbContentComment in dbContentCommentCollection)
			{
				if(dbContentComment.ID == selectedID) rowCssClass = selectedRowCssClass;
				else if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
				rowflag = !rowflag;
				output.WriteFullBeginTag("tr");
				output.WriteLine();
				output.Indent++;
				//
				// Render ID of Record
				//
				// output.WriteBeginTag("td");
				// output.WriteAttribute("class", rowCssClass);
				// output.Write(HtmlTextWriter.TagRightChar);
				// output.Write(dbContentComment.ID);
				// output.WriteEndTag("td");
				// output.WriteLine();
				//
				// Render Main Representation of Record
				//
				output.WriteBeginTag("td");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("class", rowCssClass);
				output.Write(HtmlTextWriter.TagRightChar);

				if(selectEnabled)
				{
					output.WriteBeginTag("a");
					output.WriteAttribute("href", "javascript:" + GetSelectReference(dbContentComment.ID));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(dbContentComment.ToString());
					output.WriteEndTag("a");
				}
				else
				{
					output.Write(dbContentComment.ToString());
				}

				output.WriteEndTag("td");
				output.WriteLine();
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
		}

		#endregion

	}
}
