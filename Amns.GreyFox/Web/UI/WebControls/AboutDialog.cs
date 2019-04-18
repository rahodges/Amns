using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	// THIS CODE IS HARD BOUND TO DATABASE!

	/// <summary>
	/// Summary description for MyMembershipPanel.
	/// </summary>
	[DefaultProperty("Text"), 
	ToolboxData("<{0}:AboutDialog runat=server></{0}:AboutDialog>")]
	public class AboutDialog : TableWindow
	{
		protected string strProduct;
		protected string strTitle;		
		protected string strVersion;
		protected string strCopyright;
		protected string strCompany;
		protected Assembly assm;

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation;			
		}

		protected void EnsureAssembly()
		{
			assm = Assembly.GetCallingAssembly();

			AssemblyCopyrightAttribute copyright;
			AssemblyCompanyAttribute company;
			AssemblyProductAttribute product;
			AssemblyTitleAttribute title;

			strVersion = assm.GetName().Version.ToString(4);
			object[] customAttributes = assm.GetCustomAttributes(false);
				
			for(int i = 0; i < customAttributes.Length; i++)
			{
				switch(customAttributes[i].GetType().ToString())
				{
					case "System.Reflection.AssemblyCopyrightAttribute":
						copyright = (AssemblyCopyrightAttribute) customAttributes[i];
						strCopyright = copyright.Copyright;			
						break;
					case "System.Reflection.AssemblyCompanyAttribute":	
						company = (AssemblyCompanyAttribute) customAttributes[i];
						strCompany = company.Company;			
						break;
					case "System.Reflection.AssemblyProductAttribute":
						product = (AssemblyProductAttribute) customAttributes[i];
						strProduct = product.Product;			
						break;
					case "System.Reflection.AssemblyTitleAttribute":
						title = (AssemblyTitleAttribute) customAttributes[i];
						strTitle = title.Title;
						break;
				}
			}
		}

		protected virtual void RenderDetails(HtmlTextWriter output)
		{

		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{

			// Output Product
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(strProduct);
			output.Write(" - ");
			output.Write(strTitle);
			output.Write(" Version ");
			output.Write(strVersion);
			output.WriteFullBeginTag("br");
			output.Write(strCopyright);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

//			// Output Title
//			output.WriteFullBeginTag("tr");
//			output.WriteBeginTag("td");
//			output.WriteAttribute("class", "row1");
//			output.WriteAttribute("colspan", "2");
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write(strTitle);
//			output.Write(" Version ");
//			output.Write(strVersion);
//			output.WriteEndTag("td");
//			output.WriteEndTag("tr");
//
//			// Output Copyright
//			output.WriteFullBeginTag("tr");
//			output.WriteBeginTag("td");
//			output.WriteAttribute("class", "row1");
//			output.WriteAttribute("colspan", "2");
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write(strCopyright);
//			output.WriteEndTag("td");
//			output.WriteEndTag("tr");

			RenderDetails(output);
		}
	}
}