using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;
using Amns.GreyFox.Data;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Summary description for MemberListGrid.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:GreyFoxManagerGrid runat=server></{0}:GreyFoxManagerGrid>")]
	public class GreyFoxManagerGrid : TableGrid
	{
		string connectionString;
        string message;
		
		#region Public Properties

		[Bindable(false), Category("Data"), DefaultValue("")]
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

		public GreyFoxManagerGrid() : base()
		{
			this.features |= TableWindowFeatures.ClientSideSelector;
//			this.components |= TableWindowComponents.ViewPane;
			this.deleteEnabled = false;

            message = string.Empty;
		}

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            //controlToolbar.Items.Add(ToolBarUtility.CommandItem("verify", "Verify", "verify.gif"));
        }

        public override void ProcessCommand(string command, string parameters)
        {
            ManagerCore managerCore;
            ExposedManagerCollection managers;
            string report;

            if (command == "Verify")
            {
                managerCore = ManagerCore.GetInstance();
                managers = managerCore.ExposedManagers;

                report = managers[selectedID].Manager.VerifyTable(true);
                if (report.Length == 0)
                {
                    message = managers[selectedID].Name + " is OK.";
                }
                else
                {
                    message = report;
                }
            }
        }

		protected override void OnNewClicked(EventArgs e)
		{
			ManagerCore managerCore = ManagerCore.GetInstance();
			ExposedManagerCollection managers = managerCore.ExposedManagers;
            try
            {
                managers[selectedID].Manager.CreateTable();
                message = managers[selectedID].Name + " is OK.";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			// Pull Exposed Managers
			System.IO.DirectoryInfo dir = 
				new System.IO.DirectoryInfo(Page.Server.MapPath("~/bin"));

			ManagerCore managerCore = ManagerCore.GetInstance();
			
			if(!managerCore.IsInitialized)
			{
				managerCore.ConnectionString = this.ConnectionString;
				managerCore.Initialize(dir.GetFiles("*.dll"));
			}

            if (message != string.Empty)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), 
                    "Error", 
                    "<script language=\"javascript\">\r\n" +
                    "<!--\r\n" +
                    "alert(\"" + message + "\")\r\n" +
                    "//-->\r\n" +
                    ";</script>\r\n");
            }
		}

		#region Rendering
        
		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			EnsureChildControls();

			ManagerCore managerCore = ManagerCore.GetInstance();
			ExposedManagerCollection managers = managerCore.ExposedManagers;

            StringCollection tables = MsJetUtility.GetTables(managerCore.ConnectionString);
			
			bool rowflag = false;
			string rowCssClass;
            bool exists;
	
			this.headerLockEnabled = true;
			RenderRow(this.HeaderRowCssClass, "Extension", "Version", "Exists");

			//
			// Render Records
			//
			for(int i = 0; i < managers.Count; i++)
			{
                exists = false;

                foreach (string tableName in tables)
                {
                    if (string.Equals(tableName, managers[i].Manager.TableName,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }

				if(rowflag)
					rowCssClass = this.defaultRowCssClass;
				else
					rowCssClass = this.alternateRowCssClass;

				rowflag = !rowflag;

				output.WriteBeginTag("tr");
				output.WriteAttribute("i", i.ToString());
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent++;
								
				output.WriteBeginTag("td");
				output.WriteAttribute("valign", "top");
				output.WriteAttribute("class", rowCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write("<strong>");
				output.Write(managers[i].Name);
                output.Write("</strong><span style=\"color:#bbbbbb;\"> - ");
                output.Write(managers[i].Manager.TableName);
				output.Write("</span><br /><em>");
				output.Write(managers[i].Description);
				output.Write("</em>");
				output.WriteEndTag("td");
				output.WriteLine();

				output.WriteBeginTag("td");
				output.WriteAttribute("class", rowCssClass);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(managers[i].VersionMajor);
				output.Write(".");
				output.Write(managers[i].VersionMinor);
				output.Write(".");
				output.Write(managers[i].VersonBuild);
				output.WriteEndTag("td");
				output.WriteLine();

                // Display Existence
                output.WriteBeginTag("td");
                output.WriteAttribute("class", rowCssClass);
                output.Write(HtmlTextWriter.TagRightChar);
                if (exists)
                    output.Write("yes");
                else
                    output.Write("no");
                output.WriteEndTag("td");
                output.WriteLine();
			
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
		}

		#endregion

		#region ViewPane

//		protected override void RenderViewPane(HtmlTextWriter output)
//		{
//			int id = int.Parse(Page.Request.QueryString[0]);
//			ExposedManager manager = null;
//
//			ManagerCore managerCore = ManagerCore.GetInstance();
//			ExposedManagerCollection managers = managerCore.ExposedManagers;
//			foreach(ExposedManager m in managers)
//			{
//				if(m.GetHashCode() == id)
//				{
//					manager = m;					
//				}
//				break;
//			}
//
//			RenderTableBeginTag("_viewPanel", this.CellPadding, this.CellSpacing, Unit.Percentage(100), Unit.Empty, this.CssClass);
//
//			output.WriteFullBeginTag("tr");
//			output.WriteBeginTag("th");
//			output.WriteAttribute("class", this.HeaderCssClass);
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write(manager.Name);
//			output.WriteEndTag("th");
//			output.WriteEndTag("tr");
//
//			output.WriteFullBeginTag("tr");
//			output.WriteBeginTag("td");
//			output.WriteAttribute("class", this.SubHeaderCssClass);
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write("Description");
//			output.WriteEndTag("td");
//			output.WriteEndTag("tr");
//
//			output.WriteFullBeginTag("tr");
//			output.WriteBeginTag("td");
//			output.WriteAttribute("class", this.DefaultRowCssClass);
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write(manager.Description);
//			output.WriteEndTag("td");
//			output.WriteEndTag("tr");
//
//			output.WriteEndTag("table");
//		}

		#endregion
	}
}