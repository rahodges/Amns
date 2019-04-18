using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.EventLog;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// <summary>
	/// A custom grid for GreyFoxEvent.
	/// </summary>
	/// </summary>
	[DefaultProperty("ConnectionString"),
	ToolboxData("<{0}:GreyFoxEventGrid runat=server></{0}:GreyFoxEventGrid>")]
	public class GreyFoxEventGrid : TableGrid
	{
		private string tableName = "sysGlobal_EventLog";

		private DropDownList ddSource;
		private DropDownList ddCategory;

		#region Public Properties

		[Bindable(false),
		Category("Data"),
		DefaultValue("sysGlobal_EventLog")]
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

		public GreyFoxEventGrid() : base()
		{
			this.features |= TableWindowFeatures.ClientSideSelector;
		}

		protected override void CreateChildControls()
		{
            base.CreateChildControls();

			ddSource = new DropDownList();
			ddSource.AutoPostBack = true;
			ddSource.Items.Add(new ListItem("Auditor", "AUDITOR"));
			ddSource.Items.Add(new ListItem("Generic", "Generic"));
			ddSource.Items.Add(new ListItem("All", "null"));
			Controls.Add(ddSource);

			//sToolbar.Items.Add(new ToolbarLiteral("Source : "));
			//sToolbar.Items.Add(new ToolbarControl(ddSource));

			ddCategory = new DropDownList();
			ddCategory.AutoPostBack = true;
			ddCategory.Items.Add(new ListItem("Login", "Login"));
			ddCategory.Items.Add(new ListItem("Web", "Web"));
			ddCategory.Items.Add(new ListItem("All", "null"));
			Controls.Add(ddCategory);
			//sToolbar.Items.Add(new ToolbarLiteral("Category : "));
			//sToolbar.Items.Add(new ToolbarControl(ddCategory));

			this.ChildControlsCreated = true;
		}

		#region Rendering

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			string whereQuery = string.Empty;

			#region Construct Where Query

			if(ddSource.SelectedValue != "null")
			{
				if(whereQuery != string.Empty)
				{
					whereQuery += " AND";
				}

				whereQuery += " " + tableName + ".Source='" + ddSource.SelectedValue + "'";
			}

			if(ddCategory.SelectedValue != "null")
			{
				if(whereQuery != string.Empty)
				{
					whereQuery += " AND";
				}

				whereQuery += " " + tableName + ".Category='" + ddCategory.SelectedValue + "'";
			}

			#endregion

			EnsureChildControls();
			GreyFoxEventManager m = new GreyFoxEventManager(tableName);
			GreyFoxEventCollection GreyFoxEventCollection = m.GetCollection(50, whereQuery, tableName + ".EventDate DESC", null);
			bool rowflag = false;
			string rowCssClass;
			//
			// Render Records
			//
			foreach(GreyFoxEvent greyFoxEvent in GreyFoxEventCollection)
			{
				if(!Page.Response.IsClientConnected)
					return;

				if(greyFoxEvent.ID == selectedID) rowCssClass = selectedRowCssClass;
				else if(rowflag) rowCssClass = defaultRowCssClass;
				else rowCssClass = alternateRowCssClass;
				rowflag = !rowflag;
				output.WriteBeginTag("tr");
				output.WriteAttribute("i", greyFoxEvent.ID.ToString());
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteLine();
				output.Indent++;

				//
				// Render Main Representation of Record
				//
				output.WriteBeginTag("td");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("class", rowCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(greyFoxEvent.EventDate.ToString());
				output.Write("<br>");
				output.Write(greyFoxEvent.Source);
				output.Write("<br>");
				output.Write(greyFoxEvent.Category);
				output.WriteEndTag("td");
				output.WriteLine();

				//
				// Render ID of Record
				//
				output.WriteBeginTag("td");
				output.WriteAttribute("class", rowCssClass);
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("style", "word-wrap:break-word");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(greyFoxEvent.Description.Replace("\n", "<br>"));
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
