using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Summary description for MemberListGrid.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:GreyFoxUserGrid runat=server></{0}:GreyFoxUserGrid>")]
	public class GreyFoxUserGrid : TableGrid
	{
		public GreyFoxUserGrid() : base()
		{
			this.features |= TableWindowFeatures.ClientSideSelector;
			this.components |= TableWindowComponents.ViewPane;
		}

		#region Rendering
        
		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			GreyFoxUserManager m = new GreyFoxUserManager();
			GreyFoxUserCollection userCollection = m.GetCollection(string.Empty, "Username", GreyFoxUserFlags.Contact);

			bool rowflag = false;
			string rowCssClass;		

			//
			// Render Records
			//
			foreach(GreyFoxUser user in userCollection)
			{
				if(!Page.Response.IsClientConnected)
					return;

				if(rowflag)
					rowCssClass = this.DefaultRowCssClass;
				else
					rowCssClass = this.AlternateRowCssClass;

				rowflag = !rowflag;

				output.WriteBeginTag("tr");
				output.WriteAttribute("i", user.ID.ToString());
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
				output.WriteFullBeginTag("strong");
				output.Write(user.userName);
				output.WriteEndTag("strong");	
				output.Write("<br>");
				output.Write(user.Contact.FullName);
				output.WriteEndTag("td");
				output.WriteLine();

				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
		}

		#endregion

		protected override void RenderViewPane(HtmlTextWriter output)
		{
			GreyFoxUser u = new GreyFoxUser(int.Parse(Page.Request.QueryString[0]));
		
			RenderTableBeginTag("_viewPanel", this.CellPadding, this.CellSpacing, Unit.Percentage(100), Unit.Empty, this.CssClass);
           
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("th");
			output.WriteAttribute("class", this.HeaderCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(u.Contact.FullName);
			output.WriteEndTag("th");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.SubHeaderCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(Localization.PeopleStrings.ContactTab);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.defaultRowCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(u.Contact.ConstructAddress("<br />"));
			output.Write("<br />");
			if(u.Contact.HomePhone != string.Empty)
				output.Write(u.Contact.HomePhone + " (h)<br />");
			if(u.Contact.WorkPhone != string.Empty)
				output.Write(u.Contact.WorkPhone + " (w)<br />");
			if(u.Contact.Email1 != string.Empty)
			{
				output.Write("<a href=\"mailto:");
				output.Write(u.Contact.Email1);
				output.Write("\">");
				output.Write(u.Contact.Email1);
				output.Write("</a>");
				output.Write("<br />");
			}
			if(u.Contact.ValidationMemo != null && u.Contact.ValidationMemo.Length > 0)
			{
                output.Write("<br /><strong>" + Localization.PeopleStrings.ValidationMemo + "</strong><br />");
				output.Write(Localization.PeopleStrings.Invalid + 
                    Localization.PeopleStrings.Space + u.Contact.ValidationFlagsToString().ToLower());
				output.Write(u.Contact.ValidationMemo.Replace("\n", "<br />"));
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.SubHeaderCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(Localization.SecurityStrings.SecurityTab);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.defaultRowCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("<strong>" + Localization.SecurityStrings.Username + "</strong> : ");
			output.Write(u.UserName);
			output.Write("<br />");
			output.Write("<strong>" + Localization.SecurityStrings.LastAccess + "</strong> : ");
			if(u.LoginDate != DateTime.MinValue)
				output.Write(u.LoginDate);
			else
				output.Write("&nbsp;");
			output.Write("<br />");
			output.Write("<strong>" + Localization.SecurityStrings.LoginCount + "</strong> : ");
			output.Write(u.LoginCount);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.SubHeaderCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(Localization.SecurityStrings.Roles);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", this.defaultRowCssClass);
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(string.Join("; ", u.Roles.RolesNamesToArray()));
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteEndTag("table");

		}

	}
}