using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.People.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxContact.
	/// </summary>
	[ToolboxData("<GreyFoxContact:GreyFoxContactView runat=server></{0}:GreyFoxContactView>")]
	public class GreyFoxContactView : TableWindow, INamingContainer
	{
		private int greyFoxContactID;
		private string greyFoxContactTable = "sysGlobal_Contacts";
		private GreyFoxContact greyFoxContact;

		#region Private Control Fields for Name Folder

		private Literal ltDisplayName = new Literal();
		private Literal ltPrefix = new Literal();
		private Literal ltFirstName = new Literal();
		private Literal ltMiddleName = new Literal();
		private Literal ltLastName = new Literal();
		private Literal ltSuffixCommaEnabled = new Literal();
		private Literal ltSuffix = new Literal();
		private Literal ltTitle = new Literal();
		private Literal ltValidationFlags = new Literal();
		private Literal ltValidationMemo = new Literal();

		#endregion

		#region Private Control Fields for Address Folder

		private Literal ltAddress1 = new Literal();
		private Literal ltAddress2 = new Literal();
		private Literal ltCity = new Literal();
		private Literal ltStateProvince = new Literal();
		private Literal ltCountry = new Literal();
		private Literal ltPostalCode = new Literal();

		#endregion

		#region Private Control Fields for Voice Folder

		private Literal ltHomePhone = new Literal();
		private Literal ltWorkPhone = new Literal();
		private Literal ltFax = new Literal();
		private Literal ltPager = new Literal();
		private Literal ltMobilePhone = new Literal();

		#endregion

		#region Private Control Fields for Internet Folder

		private Literal ltEmail1 = new Literal();
		private Literal ltEmail2 = new Literal();
		private Literal ltUrl = new Literal();

		#endregion

		#region Private Control Fields for Business Folder

		private Literal ltBusinessName = new Literal();

		#endregion

		#region Private Control Fields for Default Folder

		private Literal ltMemoText = new Literal();
		private Literal ltBirthDate = new Literal();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

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

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for Name Folder

			ltDisplayName.EnableViewState = false;
			Controls.Add(ltDisplayName);

			ltPrefix.EnableViewState = false;
			Controls.Add(ltPrefix);

			ltFirstName.EnableViewState = false;
			Controls.Add(ltFirstName);

			ltMiddleName.EnableViewState = false;
			Controls.Add(ltMiddleName);

			ltLastName.EnableViewState = false;
			Controls.Add(ltLastName);

			ltSuffixCommaEnabled.EnableViewState = false;
			Controls.Add(ltSuffixCommaEnabled);

			ltSuffix.EnableViewState = false;
			Controls.Add(ltSuffix);

			ltTitle.EnableViewState = false;
			Controls.Add(ltTitle);

			ltValidationFlags.EnableViewState = false;
			Controls.Add(ltValidationFlags);

			ltValidationMemo.EnableViewState = false;
			Controls.Add(ltValidationMemo);

			#endregion

			#region Child Controls for Address Folder

			ltAddress1.EnableViewState = false;
			Controls.Add(ltAddress1);

			ltAddress2.EnableViewState = false;
			Controls.Add(ltAddress2);

			ltCity.EnableViewState = false;
			Controls.Add(ltCity);

			ltStateProvince.EnableViewState = false;
			Controls.Add(ltStateProvince);

			ltCountry.EnableViewState = false;
			Controls.Add(ltCountry);

			ltPostalCode.EnableViewState = false;
			Controls.Add(ltPostalCode);

			#endregion

			#region Child Controls for Voice Folder

			ltHomePhone.EnableViewState = false;
			Controls.Add(ltHomePhone);

			ltWorkPhone.EnableViewState = false;
			Controls.Add(ltWorkPhone);

			ltFax.EnableViewState = false;
			Controls.Add(ltFax);

			ltPager.EnableViewState = false;
			Controls.Add(ltPager);

			ltMobilePhone.EnableViewState = false;
			Controls.Add(ltMobilePhone);

			#endregion

			#region Child Controls for Internet Folder

			ltEmail1.EnableViewState = false;
			Controls.Add(ltEmail1);

			ltEmail2.EnableViewState = false;
			Controls.Add(ltEmail2);

			ltUrl.EnableViewState = false;
			Controls.Add(ltUrl);

			#endregion

			#region Child Controls for Business Folder

			ltBusinessName.EnableViewState = false;
			Controls.Add(ltBusinessName);

			#endregion

			#region Child Controls for Default Folder

			ltMemoText.EnableViewState = false;
			Controls.Add(ltMemoText);

			ltBirthDate.EnableViewState = false;
			Controls.Add(ltBirthDate);

			#endregion

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btDelete.Text = "Delete";
			btDelete.Width = Unit.Pixel(72);
			btDelete.EnableViewState = false;
			btDelete.Click += new EventHandler(delete_Click);
			Controls.Add(btDelete);

			ChildControlsCreated = true;
		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			OnOkClicked(EventArgs.Empty);
		}

		#endregion

		protected void delete_Click(object sender, EventArgs e)
		{
			this.OnDeleteClicked(EventArgs.Empty);
		}

		public event EventHandler OkClicked;
		protected virtual void OnOkClicked(EventArgs e)
		{
			if(OkClicked != null)
				OkClicked(this, e);
		}

		public event EventHandler DeleteClicked;
		protected virtual void OnDeleteClicked(EventArgs e)
		{
			if(DeleteClicked != null)
			DeleteClicked(this, e);
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation | 
				TableWindowFeatures.WindowPrinter;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if(greyFoxContactID != 0)
			{
				greyFoxContact = new GreyFoxContact(greyFoxContactTable, greyFoxContactID);

				#region Bind Name Folder

				//
				// Set Field Entries
				//

				ltDisplayName.Text = greyFoxContact.DisplayName.ToString();
				ltPrefix.Text = greyFoxContact.Prefix.ToString();
				ltFirstName.Text = greyFoxContact.FirstName.ToString();
				ltMiddleName.Text = greyFoxContact.MiddleName.ToString();
				ltLastName.Text = greyFoxContact.LastName.ToString();
				ltSuffixCommaEnabled.Text = greyFoxContact.SuffixCommaEnabled.ToString();
				ltSuffix.Text = greyFoxContact.Suffix.ToString();
				ltTitle.Text = greyFoxContact.Title.ToString();
				ltValidationFlags.Text = greyFoxContact.ValidationFlags.ToString();
				ltValidationMemo.Text = greyFoxContact.ValidationMemo.ToString();


				#endregion

				#region Bind Address Folder

				//
				// Set Field Entries
				//

				ltAddress1.Text = greyFoxContact.Address1.ToString();
				ltAddress2.Text = greyFoxContact.Address2.ToString();
				ltCity.Text = greyFoxContact.City.ToString();
				ltStateProvince.Text = greyFoxContact.StateProvince.ToString();
				ltCountry.Text = greyFoxContact.Country.ToString();
				ltPostalCode.Text = greyFoxContact.PostalCode.ToString();


				#endregion

				#region Bind Voice Folder

				//
				// Set Field Entries
				//

				ltHomePhone.Text = greyFoxContact.HomePhone.ToString();
				ltWorkPhone.Text = greyFoxContact.WorkPhone.ToString();
				ltFax.Text = greyFoxContact.Fax.ToString();
				ltPager.Text = greyFoxContact.Pager.ToString();
				ltMobilePhone.Text = greyFoxContact.MobilePhone.ToString();


				#endregion

				#region Bind Internet Folder

				//
				// Set Field Entries
				//

				ltEmail1.Text = greyFoxContact.Email1.ToString();
				ltEmail2.Text = greyFoxContact.Email2.ToString();
				ltUrl.Text = greyFoxContact.Url.ToString();


				#endregion

				#region Bind Business Folder

				//
				// Set Field Entries
				//

				ltBusinessName.Text = greyFoxContact.BusinessName.ToString();


				#endregion

				#region Bind Default Folder

				//
				// Set Field Entries
				//

				ltMemoText.Text = greyFoxContact.MemoText.ToString();
				ltBirthDate.Text = greyFoxContact.BirthDate.ToString();


				#endregion

				#region Bind _system Folder

				//
				// Set Field Entries
				//



				#endregion

				text = "View  - " + greyFoxContact.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "GreyFoxContact ID", greyFoxContactID.ToString());
			output.WriteEndTag("tr");

			renderNameFolder(output);

			renderAddressFolder(output);

			renderVoiceFolder(output);

			renderInternetFolder(output);

			renderBusinessFolder(output);

			renderDefaultFolder(output);

			render_systemFolder(output);

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
			if(DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		#region Render Name Folder

		private void renderNameFolder(HtmlTextWriter output)
		{
			//
			// Render Name Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Name");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render DisplayName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DisplayName");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDisplayName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Prefix
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Prefix");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPrefix.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render FirstName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("First");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltFirstName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MiddleName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Middle");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMiddleName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LastName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Last");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltLastName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SuffixCommaEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable comma before suffix.");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltSuffixCommaEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Suffix
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Suffix");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltSuffix.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Title
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Title");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltTitle.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ValidationFlags
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ValidationFlags");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltValidationFlags.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ValidationMemo
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ValidationMemo");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltValidationMemo.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Address Folder

		private void renderAddressFolder(HtmlTextWriter output)
		{
			//
			// Render Address Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Address1
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address Line 1");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAddress1.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Address2
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address Line 2");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltAddress2.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render City
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("City");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltCity.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render StateProvince
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("State / Province");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltStateProvince.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Country
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Country");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltCountry.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PostalCode
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Postal Code");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPostalCode.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Voice Folder

		private void renderVoiceFolder(HtmlTextWriter output)
		{
			//
			// Render Voice Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Voice");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render HomePhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Home Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltHomePhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render WorkPhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Work Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltWorkPhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Fax
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Fax");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltFax.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Pager
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Pager");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPager.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MobilePhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Mobile Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMobilePhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Internet Folder

		private void renderInternetFolder(HtmlTextWriter output)
		{
			//
			// Render Internet Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Internet");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Email1
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("First Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEmail1.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Email2
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Second Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEmail2.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Url
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Url");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltUrl.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Business Folder

		private void renderBusinessFolder(HtmlTextWriter output)
		{
			//
			// Render Business Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Business Details");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render BusinessName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Business Name");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltBusinessName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render Default Folder

		private void renderDefaultFolder(HtmlTextWriter output)
		{
			//
			// Render Default Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Default");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MemoText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Memo Text");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltMemoText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render BirthDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Birth Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltBirthDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

		#region Render _system Folder

		private void render_systemFolder(HtmlTextWriter output)
		{
			//
			// Render _system Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("System Folder");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		#endregion

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
			myState[1] = greyFoxContactID;
			return myState;
		}
	}
}
