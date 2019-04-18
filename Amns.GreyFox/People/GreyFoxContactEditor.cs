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
	[ToolboxData("<{0}:GreyFoxContactEditor runat=server></{0}:GreyFoxContactEditor>")]
	public class GreyFoxContactEditor : TableWindow, INamingContainer
	{
		private int greyFoxContactID;
		private string greyFoxContactTable = "sysGlobal_Contacts";
		private GreyFoxContact obj;
		private bool loadFlag = false;
		private bool resetOnAdd;
		private bool editOnAdd;

		#region Private Control Fields for Name Folder

		private TextBox tbDisplayName = new TextBox();
		private TextBox tbPrefix = new TextBox();
		private TextBox tbFirstName = new TextBox();
		private TextBox tbMiddleName = new TextBox();
		private TextBox tbLastName = new TextBox();
		private CheckBox cbSuffixCommaEnabled = new CheckBox();
		private TextBox tbSuffix = new TextBox();
		private TextBox tbTitle = new TextBox();
		private TextBox tbValidationFlags = new TextBox();
		private TextBox tbValidationMemo = new TextBox();

		#endregion

		#region Private Control Fields for Address Folder

		private TextBox tbAddress1 = new TextBox();
		private TextBox tbAddress2 = new TextBox();
		private TextBox tbCity = new TextBox();
		private TextBox tbStateProvince = new TextBox();
		private TextBox tbCountry = new TextBox();
		private TextBox tbPostalCode = new TextBox();

		#endregion

		#region Private Control Fields for Voice Folder

		private TextBox tbHomePhone = new TextBox();
		private TextBox tbWorkPhone = new TextBox();
		private TextBox tbFax = new TextBox();
		private TextBox tbPager = new TextBox();
		private TextBox tbMobilePhone = new TextBox();

		#endregion

		#region Private Control Fields for Internet Folder

		private TextBox tbEmail1 = new TextBox();
		private TextBox tbEmail2 = new TextBox();
		private TextBox tbUrl = new TextBox();

		#endregion

		#region Private Control Fields for Business Folder

		private TextBox tbBusinessName = new TextBox();

		#endregion

		#region Private Control Fields for Default Folder

		private TextBox tbMemoText = new TextBox();
		private TextBox tbBirthDate = new TextBox();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btCancel = new Button();
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
				loadFlag = true;
				greyFoxContactID = value;
			}
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool ResetOnAdd
		{
			get
			{
				return resetOnAdd;
			}
			set
			{
				resetOnAdd = value;
			}
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool EditOnAdd
		{
			get
			{
				return editOnAdd;
			}
			set
			{
				editOnAdd = value;
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
			bindDropDownLists();

			#region Child Controls for Name Folder

			tbDisplayName.EnableViewState = false;
			Controls.Add(tbDisplayName);

			tbPrefix.EnableViewState = false;
			Controls.Add(tbPrefix);

			tbFirstName.EnableViewState = false;
			Controls.Add(tbFirstName);

			tbMiddleName.EnableViewState = false;
			Controls.Add(tbMiddleName);

			tbLastName.EnableViewState = false;
			Controls.Add(tbLastName);

			cbSuffixCommaEnabled.EnableViewState = false;
			Controls.Add(cbSuffixCommaEnabled);

			tbSuffix.EnableViewState = false;
			Controls.Add(tbSuffix);

			tbTitle.EnableViewState = false;
			Controls.Add(tbTitle);

			tbValidationFlags.EnableViewState = false;
			Controls.Add(tbValidationFlags);

			tbValidationMemo.EnableViewState = false;
			Controls.Add(tbValidationMemo);

			#endregion

			#region Child Controls for Address Folder

			tbAddress1.EnableViewState = false;
			Controls.Add(tbAddress1);

			tbAddress2.EnableViewState = false;
			Controls.Add(tbAddress2);

			tbCity.EnableViewState = false;
			Controls.Add(tbCity);

			tbStateProvince.EnableViewState = false;
			Controls.Add(tbStateProvince);

			tbCountry.EnableViewState = false;
			Controls.Add(tbCountry);

			tbPostalCode.EnableViewState = false;
			Controls.Add(tbPostalCode);

			#endregion

			#region Child Controls for Voice Folder

			tbHomePhone.EnableViewState = false;
			Controls.Add(tbHomePhone);

			tbWorkPhone.EnableViewState = false;
			Controls.Add(tbWorkPhone);

			tbFax.EnableViewState = false;
			Controls.Add(tbFax);

			tbPager.EnableViewState = false;
			Controls.Add(tbPager);

			tbMobilePhone.EnableViewState = false;
			Controls.Add(tbMobilePhone);

			#endregion

			#region Child Controls for Internet Folder

			tbEmail1.EnableViewState = false;
			Controls.Add(tbEmail1);

			tbEmail2.EnableViewState = false;
			Controls.Add(tbEmail2);

			tbUrl.EnableViewState = false;
			Controls.Add(tbUrl);

			#endregion

			#region Child Controls for Business Folder

			tbBusinessName.EnableViewState = false;
			Controls.Add(tbBusinessName);

			#endregion

			#region Child Controls for Default Folder

			tbMemoText.EnableViewState = false;
			Controls.Add(tbMemoText);

			tbBirthDate.EnableViewState = false;
			Controls.Add(tbBirthDate);

			#endregion

			btOk.Text = "OK";
			btOk.Width = Unit.Pixel(72);
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(ok_Click);
			Controls.Add(btOk);

			btCancel.Text = "Cancel";
			btCancel.Width = Unit.Pixel(72);
			btCancel.EnableViewState = false;
			btCancel.CausesValidation = false;
			btCancel.Click += new EventHandler(cancel_Click);
			Controls.Add(btCancel);

			btDelete.Text = "Delete";
			btDelete.Width = Unit.Pixel(72);
			btDelete.EnableViewState = false;
			btDelete.Click += new EventHandler(delete_Click);
			Controls.Add(btDelete);

			ChildControlsCreated = true;
		}

		private void bindDropDownLists()
		{
		}

		#region ok_Click Save and Update Method

		protected void ok_Click(object sender, EventArgs e)
		{
			if(greyFoxContactID == 0)
				obj = new GreyFoxContact(greyFoxContactTable);
			else
				obj = new GreyFoxContact(greyFoxContactTable, greyFoxContactID);

			obj.DisplayName = tbDisplayName.Text;
			obj.Prefix = tbPrefix.Text;
			obj.FirstName = tbFirstName.Text;
			obj.MiddleName = tbMiddleName.Text;
			obj.LastName = tbLastName.Text;
			obj.SuffixCommaEnabled = cbSuffixCommaEnabled.Checked;
			obj.Suffix = tbSuffix.Text;
			obj.Title = tbTitle.Text;
			obj.ValidationFlags = byte.Parse(tbValidationFlags.Text);
			obj.ValidationMemo = tbValidationMemo.Text;
			obj.Address1 = tbAddress1.Text;
			obj.Address2 = tbAddress2.Text;
			obj.City = tbCity.Text;
			obj.StateProvince = tbStateProvince.Text;
			obj.Country = tbCountry.Text;
			obj.PostalCode = tbPostalCode.Text;
			obj.HomePhone = tbHomePhone.Text;
			obj.WorkPhone = tbWorkPhone.Text;
			obj.Fax = tbFax.Text;
			obj.Pager = tbPager.Text;
			obj.MobilePhone = tbMobilePhone.Text;
			obj.Email1 = tbEmail1.Text;
			obj.Email2 = tbEmail2.Text;
			obj.Url = tbUrl.Text;
			obj.BusinessName = tbBusinessName.Text;
			obj.MemoText = tbMemoText.Text;
			obj.BirthDate = DateTime.Parse(tbBirthDate.Text);

			if(editOnAdd)
				greyFoxContactID = obj.Save();
			else
				obj.Save();

			if(resetOnAdd)
			{
				tbDisplayName.Text = string.Empty;
				tbPrefix.Text = string.Empty;
				tbFirstName.Text = string.Empty;
				tbMiddleName.Text = string.Empty;
				tbLastName.Text = string.Empty;
				cbSuffixCommaEnabled.Checked = false;
				tbSuffix.Text = string.Empty;
				tbTitle.Text = string.Empty;
				tbValidationFlags.Text = string.Empty;
				tbValidationMemo.Text = string.Empty;
				tbAddress1.Text = string.Empty;
				tbAddress2.Text = string.Empty;
				tbCity.Text = string.Empty;
				tbStateProvince.Text = string.Empty;
				tbCountry.Text = string.Empty;
				tbPostalCode.Text = string.Empty;
				tbHomePhone.Text = string.Empty;
				tbWorkPhone.Text = string.Empty;
				tbFax.Text = string.Empty;
				tbPager.Text = string.Empty;
				tbMobilePhone.Text = string.Empty;
				tbEmail1.Text = string.Empty;
				tbEmail2.Text = string.Empty;
				tbUrl.Text = string.Empty;
				tbBusinessName.Text = string.Empty;
				tbMemoText.Text = string.Empty;
				tbBirthDate.Text = DateTime.Now.ToString();
			}

			OnUpdated(EventArgs.Empty);
		}

		#endregion

		protected void cancel_Click(object sender, EventArgs e)
		{
			this.OnCancelled(EventArgs.Empty);
		}

		protected void delete_Click(object sender, EventArgs e)
		{
			this.OnDeleteClicked(EventArgs.Empty);
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

		public event EventHandler DeleteClicked;
		protected virtual void OnDeleteClicked(EventArgs e)
		{
			if(DeleteClicked != null)
			DeleteClicked(this, e);
		}

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation;
			components = TableWindowComponents.Tabs;
			tabStrip = new TabStrip();
			tabStrip.Tabs = new TabList();

			Tab NameTab = new Tab("Name");
			NameTab.Visible = true;
			NameTab.RenderDiv += new TabRenderHandler(renderNameFolder);
			NameTab.Visible = true;
			tabStrip.Tabs.Add(NameTab);

			Tab AddressTab = new Tab("Address");
			AddressTab.RenderDiv += new TabRenderHandler(renderAddressFolder);
			tabStrip.Tabs.Add(AddressTab);

			Tab VoiceTab = new Tab("Voice");
			VoiceTab.RenderDiv += new TabRenderHandler(renderVoiceFolder);
			tabStrip.Tabs.Add(VoiceTab);

			Tab InternetTab = new Tab("Internet");
			InternetTab.RenderDiv += new TabRenderHandler(renderInternetFolder);
			tabStrip.Tabs.Add(InternetTab);

			Tab BusinessTab = new Tab("Business");
			BusinessTab.RenderDiv += new TabRenderHandler(renderBusinessFolder);
			tabStrip.Tabs.Add(BusinessTab);

			Tab DefaultTab = new Tab("Default");
			DefaultTab.RenderDiv += new TabRenderHandler(renderDefaultFolder);
			tabStrip.Tabs.Add(DefaultTab);

		}

		protected override void OnPreRender(EventArgs e)
		{
			if(loadFlag)
			{
				if(greyFoxContactID > 0)
				{
					obj = new GreyFoxContact(greyFoxContactTable, greyFoxContactID);
					text = "Edit  - " + obj.ToString();
				}
				else if(greyFoxContactID <= 0)
				{
					obj = new GreyFoxContact(greyFoxContactTable);
					text = "Add ";
				}

				//
				// Set Field Entries
				//
				tbDisplayName.Text = obj.DisplayName;
				tbPrefix.Text = obj.Prefix;
				tbFirstName.Text = obj.FirstName;
				tbMiddleName.Text = obj.MiddleName;
				tbLastName.Text = obj.LastName;
				cbSuffixCommaEnabled.Checked = obj.SuffixCommaEnabled;
				tbSuffix.Text = obj.Suffix;
				tbTitle.Text = obj.Title;
				tbValidationFlags.Text = obj.ValidationFlags.ToString();
				tbValidationMemo.Text = obj.ValidationMemo;
				tbAddress1.Text = obj.Address1;
				tbAddress2.Text = obj.Address2;
				tbCity.Text = obj.City;
				tbStateProvince.Text = obj.StateProvince;
				tbCountry.Text = obj.Country;
				tbPostalCode.Text = obj.PostalCode;
				tbHomePhone.Text = obj.HomePhone;
				tbWorkPhone.Text = obj.WorkPhone;
				tbFax.Text = obj.Fax;
				tbPager.Text = obj.Pager;
				tbMobilePhone.Text = obj.MobilePhone;
				tbEmail1.Text = obj.Email1;
				tbEmail2.Text = obj.Email2;
				tbUrl.Text = obj.Url;
				tbBusinessName.Text = obj.BusinessName;
				tbMemoText.Text = obj.MemoText;
				tbBirthDate.Text = obj.BirthDate.ToString();

			}
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			RenderTabPanels(output);
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
			if(DeleteClicked != null)
			{
				output.Write(" ");
				btDelete.RenderControl(output);
			}
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
		}

		private void renderNameFolder(HtmlTextWriter output)
		{
			//
			// Render DisplayName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("DisplayName");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDisplayName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Prefix
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Prefix");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPrefix.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render FirstName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("First");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbFirstName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MiddleName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Middle");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMiddleName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LastName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Last");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbLastName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render SuffixCommaEnabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Enable comma before suffix.");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			cbSuffixCommaEnabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Suffix
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Suffix");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbSuffix.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Title
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Title");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbTitle.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ValidationFlags
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ValidationFlags");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbValidationFlags.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ValidationMemo
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ValidationMemo");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbValidationMemo.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderAddressFolder(HtmlTextWriter output)
		{
			//
			// Render Address1
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address Line 1");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbAddress1.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Address2
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Address Line 2");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbAddress2.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render City
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("City");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbCity.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render StateProvince
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("State / Province");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbStateProvince.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Country
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Country");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbCountry.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PostalCode
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Postal Code");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPostalCode.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderVoiceFolder(HtmlTextWriter output)
		{
			//
			// Render HomePhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Home Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbHomePhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render WorkPhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Work Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbWorkPhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Fax
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Fax");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbFax.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Pager
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Pager");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbPager.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render MobilePhone
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Mobile Phone");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMobilePhone.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderInternetFolder(HtmlTextWriter output)
		{
			//
			// Render Email1
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("First Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEmail1.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Email2
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Second Email");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbEmail2.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Url
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Url");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbUrl.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderBusinessFolder(HtmlTextWriter output)
		{
			//
			// Render BusinessName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Business Name");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbBusinessName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void renderDefaultFolder(HtmlTextWriter output)
		{
			//
			// Render MemoText
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Memo Text");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbMemoText.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render BirthDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Birth Date");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.Write(HtmlTextWriter.TagRightChar);
			tbBirthDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

		}

		private void render_systemFolder(HtmlTextWriter output)
		{
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
			myState[1] = greyFoxContactID;
			return myState;
		}
	}
}

