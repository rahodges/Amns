using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for DbContentClip.
	/// </summary>
	/// </summary>
	[DefaultProperty("ConnectionString"),
		ToolboxData("<{0}:DbContentClipGrid runat=server></{0}:DbContentClipGrid>")]
	public class DbContentClipGrid : TableGrid
	{
		private int catalogID;
		private string connectionString;	
		private DropDownList ddSortType = new DropDownList();

		#region Public Properties
		
		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int CatalogID
		{
			get
			{
				return catalogID;
			}
			set
			{
				catalogID = value;
			}
		}

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
			DbContentClipManager m = new DbContentClipManager();
			DbContentClipCollection dbContentClipCollection =
				m.GetCollection("ParentCatalogID=" + catalogID.ToString(), ddSortType.SelectedItem.Value, null);
			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(DbContentClip dbContentClip in dbContentClipCollection)
			{
				if(dbContentClip.ID == selectedID) rowCssClass = selectedRowCssClass;
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
				// output.Write(dbContentClip.ID);
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
					output.WriteAttribute("href", "javascript:" + GetSelectReference(dbContentClip.ID));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(dbContentClip.ToString());
					output.WriteEndTag("a");
				}
				else
				{
					output.Write(dbContentClip.ToString());
				}

				output.WriteEndTag("td");
				output.WriteLine();
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
		}

		#endregion

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null) base.LoadViewState(myState[0]);
				if(myState[1] != null) catalogID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = catalogID;
			return myState;
		}
	}
}
