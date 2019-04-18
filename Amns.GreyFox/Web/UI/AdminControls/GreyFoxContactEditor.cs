using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.Web.UI.WebControls;
using Amns.GreyFox.People;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Default web editor for GreyFoxContact.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
		ToolboxData("<{0}:GreyFoxContactEditor runat=server></{0}:GreyFoxContactEditor>")]
	public class GreyFoxContactEditor : TableWindow, INamingContainer
	{
		private int greyFoxContactID;
		private string greyFoxContactTable = "sysGlobal_Contacts";
		private GreyFoxContact editGreyFoxContact;

		private TextBox tbPrefix = new TextBox();
		private TextBox tbFirstName = new TextBox();
		private TextBox tbMiddleName = new TextBox();
		private TextBox tbLastName = new TextBox();
		private TextBox tbSuffix = new TextBox();
		private TextBox tbSuffixCommaEnabled = new TextBox();
		private TextBox tbBusinessName = new TextBox();
		private TextBox tbTitle = new TextBox();
		private TextBox tbAddress1 = new TextBox();
		private TextBox tbAddress2 = new TextBox();
		private TextBox tbCity = new TextBox();
		private TextBox tbStateProvince = new TextBox();
		private TextBox tbPostalCode = new TextBox();
		private TextBox tbCountry = new TextBox();
		private TextBox tbHomePhone = new TextBox();
		private TextBox tbWorkPhone = new TextBox();
		private TextBox tbMobilePhone = new TextBox();
		private TextBox tbPager = new TextBox();
		private TextBox tbFax = new TextBox();
		private TextBox tbEmail1 = new TextBox();
		private TextBox tbEmail2 = new TextBox();
		private TextBox tbUrl = new TextBox();
		private TextBox tbMemoText = new TextBox();
		private TextBox tbBirthDate = new TextBox();
		private TextBox tbContactMethod = new TextBox();

		private Button btOk = new Button();
		private Button btCancel = new Button();

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxContactID
		{
			get
			{
				return greyFoxContactID;
			}
			set
			{
				greyFoxContactID = value;
			}
		}

		[Bindable(true), Category("Data"), DefaultValue("sysGlobal_Contacts")]
		public string GreyFoxContactTable
		{
			get
			{
				return greyFoxContactTable;
			}
			set
			{
				greyFoxContactTable = value;
			}
		}

		protected override void CreateChildControls()
		{
			Controls.Clear();
			bindDropDownLists();

			tbPrefix.Width = Unit.Pixel(175);
			tbPrefix.EnableViewState = false;
			Controls.Add(tbPrefix);

			tbFirstName.Width = Unit.Pixel(175);
			tbFirstName.EnableViewState = false;
			Controls.Add(tbFirstName);

			tbMiddleName.Width = Unit.Pixel(175);
			tbMiddleName.EnableViewState = false;
			Controls.Add(tbMiddleName);

			tbLastName.Width = Unit.Pixel(175);
			tbLastName.EnableViewState = false;
			Controls.Add(tbLastName);

			tbSuffix.Width = Unit.Pixel(175);
			tbSuffix.EnableViewState = false;
			Controls.Add(tbSuffix);

			tbSuffixCommaEnabled.Width = Unit.Pixel(175);
			tbSuffixCommaEnabled.EnableViewState = false;
			Controls.Add(tbSuffixCommaEnabled);

			tbBusinessName.Width = Unit.Pixel(175);
			tbBusinessName.EnableViewState = false;
			Controls.Add(tbBusinessName);

			tbTitle.Width = Unit.Pixel(175);
			tbTitle.EnableViewState = false;
			Controls.Add(tbTitle);

			tbAddress1.Width = Unit.Pixel(175);
			tbAddress1.EnableViewState = false;
			Controls.Add(tbAddress1);

			tbAddress2.Width = Unit.Pixel(175);
			tbAddress2.EnableViewState = false;
			Controls.Add(tbAddress2);

			tbCity.Width = Unit.Pixel(175);
			tbCity.EnableViewState = false;
			Controls.Add(tbCity);

			tbStateProvince.Width = Unit.Pixel(175);
			tbStateProvince.EnableViewState = false;
			Controls.Add(tbStateProvince);

			tbPostalCode.Width = Unit.Pixel(175);
			tbPostalCode.EnableViewState = false;
			Controls.Add(tbPostalCode);

			tbCountry.Width = Unit.Pixel(175);
			tbCountry.EnableViewState = false;
			Controls.Add(tbCountry);

			tbHomePhone.Width = Unit.Pixel(175);
			tbHomePhone.EnableViewState = false;
			Controls.Add(tbHomePhone);

			tbWorkPhone.Width = Unit.Pixel(175);
			tbWorkPhone.EnableViewState = false;
			Controls.Add(tbWorkPhone);

			tbMobilePhone.Width = Unit.Pixel(175);
			tbMobilePhone.EnableViewState = false;
			Controls.Add(tbMobilePhone);

			tbPager.Width = Unit.Pixel(175);
			tbPager.EnableViewState = false;
			Controls.Add(tbPager);

			tbFax.Width = Unit.Pixel(175);
			tbFax.EnableViewState = false;
			Controls.Add(tbFax);

			tbEmail1.Width = Unit.Pixel(175);
			tbEmail1.EnableViewState = false;
			Controls.Add(tbEmail1);

			tbEmail2.Width = Unit.Pixel(175);
			tbEmail2.EnableViewState = false;
			Controls.Add(tbEmail2);

			tbUrl.Width = Unit.Pixel(175);
			tbUrl.EnableViewState = false;
			Controls.Add(tbUrl);

			tbMemoText.Width = Unit.Pixel(175);
			tbMemoText.EnableViewState = false;
			Controls.Add(tbMemoText);

			tbBirthDate.Width = Unit.Pixel(175);
			tbBirthDate.EnableViewState = false;
			Controls.Add(tbBirthDate);

			tbContactMethod.Width = Unit.Pixel(175);
			tbContactMethod.EnableViewState = false;
			Controls.Add(tbContactMethod);

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btCancel.Text = "Cancel";
			btCancel.Width = Unit.Pixel(72);
			btCancel.EnableViewState = false;
			btCancel.Click += new EventHandler(cancel_Click);
			Controls.Add(btCancel);

			ChildControlsCreated = true;
		}

		private void bindDropDownLists()
		{
		}
		protected void ok_Click(object sender, EventArgs e)
		{
			if(greyFoxContactID == 0)
				editGreyFoxContact = new GreyFoxContact(GreyFoxContactTable);
			else
				editGreyFoxContact = new GreyFoxContact(GreyFoxContactTable, GreyFoxContactID);

			editGreyFoxContact.Prefix = tbPrefix.Text;
			editGreyFoxContact.FirstName = tbFirstName.Text;
			editGreyFoxContact.MiddleName = tbMiddleName.Text;
			editGreyFoxContact.LastName = tbLastName.Text;
			editGreyFoxContact.Suffix = tbSuffix.Text;
			editGreyFoxContact.SuffixCommaEnabled = bool.Parse(tbSuffixCommaEnabled.Text);
			editGreyFoxContact.BusinessName = tbBusinessName.Text;
			editGreyFoxContact.Title = tbTitle.Text;
			editGreyFoxContact.Address1 = tbAddress1.Text;
			editGreyFoxContact.Address2 = tbAddress2.Text;
			editGreyFoxContact.City = tbCity.Text;
			editGreyFoxContact.StateProvince = tbStateProvince.Text;
			editGreyFoxContact.PostalCode = tbPostalCode.Text;
			editGreyFoxContact.Country = tbCountry.Text;
			editGreyFoxContact.HomePhone = tbHomePhone.Text;
			editGreyFoxContact.WorkPhone = tbWorkPhone.Text;
			editGreyFoxContact.MobilePhone = tbMobilePhone.Text;
			editGreyFoxContact.Pager = tbPager.Text;
			editGreyFoxContact.Fax = tbFax.Text;
			editGreyFoxContact.Email1 = tbEmail1.Text;
			editGreyFoxContact.Email2 = tbEmail2.Text;
			editGreyFoxContact.Url = tbUrl.Text;
			editGreyFoxContact.MemoText = tbMemoText.Text;
			editGreyFoxContact.BirthDate = DateTime.Parse(tbBirthDate.Text);
			editGreyFoxContact.ContactMethod = 
                (GreyFoxContactMethod) byte.Parse(tbContactMethod.Text);

			greyFoxContactID = editGreyFoxContact.Save();

			OnUpdated(EventArgs.Empty);
		}

		protected void cancel_Click(object sender, EventArgs e)
		{
			this.OnCancelled(EventArgs.Empty);
		}

		public event EventHandler Cancelled;
		protected virtual void OnCancelled(EventArgs e)
		{
			if(Cancelled != null)
				Cancelled(this, e);
		}

		public event EventHandler Updated;
		protected virtual void OnUpdated(EventArgs e)
		{
			if(Updated != null)
				Updated(this, e);
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation;;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if(greyFoxContactID != 0)
			{
				editGreyFoxContact = new GreyFoxContact(GreyFoxContactTable, GreyFoxContactID);
				tbPrefix.Text = editGreyFoxContact.Prefix.ToString();
				tbFirstName.Text = editGreyFoxContact.FirstName.ToString();
				tbMiddleName.Text = editGreyFoxContact.MiddleName.ToString();
				tbLastName.Text = editGreyFoxContact.LastName.ToString();
				tbSuffix.Text = editGreyFoxContact.Suffix.ToString();
				tbSuffixCommaEnabled.Text = editGreyFoxContact.SuffixCommaEnabled.ToString();
				tbBusinessName.Text = editGreyFoxContact.BusinessName.ToString();
				tbTitle.Text = editGreyFoxContact.Title.ToString();
				tbAddress1.Text = editGreyFoxContact.Address1.ToString();
				tbAddress2.Text = editGreyFoxContact.Address2.ToString();
				tbCity.Text = editGreyFoxContact.City.ToString();
				tbStateProvince.Text = editGreyFoxContact.StateProvince.ToString();
				tbPostalCode.Text = editGreyFoxContact.PostalCode.ToString();
				tbCountry.Text = editGreyFoxContact.Country.ToString();
				tbHomePhone.Text = editGreyFoxContact.HomePhone.ToString();
				tbWorkPhone.Text = editGreyFoxContact.WorkPhone.ToString();
				tbMobilePhone.Text = editGreyFoxContact.MobilePhone.ToString();
				tbPager.Text = editGreyFoxContact.Pager.ToString();
				tbFax.Text = editGreyFoxContact.Fax.ToString();
				tbEmail1.Text = editGreyFoxContact.Email1.ToString();
				tbEmail2.Text = editGreyFoxContact.Email2.ToString();
				tbUrl.Text = editGreyFoxContact.Url.ToString();
				tbMemoText.Text = editGreyFoxContact.MemoText.ToString();
				tbBirthDate.Text = editGreyFoxContact.BirthDate.ToString();
				tbContactMethod.Text = editGreyFoxContact.ContactMethod.ToString();

				Text = "Edit GreyFoxContact - " + editGreyFoxContact.FullName;
			}
			else
				Text = "Add GreyFoxContact";
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderCell("GreyFoxContact ID", "class=\"row1\"");
			RenderCell(GreyFoxContactID.ToString(), "class=\"row1\"");
			output.WriteEndTag("tr");

			//
			// Render Prefix
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Prefix");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPrefix.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render FirstName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("First");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbFirstName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MiddleName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Middle");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMiddleName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LastName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Last");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLastName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Suffix
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Suffix");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSuffix.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SuffixCommaEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Use comma after suffix?");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSuffixCommaEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render BusinessName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Business Name");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbBusinessName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Title
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Title");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbTitle.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Address1
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address Line 1");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbAddress1.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Address2
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address Line 2");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbAddress2.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render City
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("City");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbCity.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render StateProvince
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("State or Province");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbStateProvince.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PostalCode
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Postal Code");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPostalCode.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Country
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Country");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbCountry.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render HomePhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Home Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbHomePhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render WorkPhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Work Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbWorkPhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MobilePhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Mobile Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMobilePhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Pager
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Pager");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPager.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Fax
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Fax");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbFax.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Email1
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("First Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEmail1.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Email2
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Second Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEmail2.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Url
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Url");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUrl.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MemoText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Memo Text");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMemoText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render BirthDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Birth Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbBirthDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ContactMethod
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Preferred Contact Apply");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbContactMethod.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render OK/Cancel Buttons
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan", "2");
			output.WriteAttribute("align", "right");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			btOk.RenderControl(output);
			output.Write("&nbsp;");
			btCancel.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		protected override void LoadViewState(object savedState)
		{
			if(savedState != null)
			{
				object[] myState = (object[]) savedState;
				if(myState[0] != null)
					base.LoadViewState(myState[0]);
				if(myState[1] != null)
					greyFoxContactID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = GreyFoxContactID;
			return myState;
		}
	}
}
