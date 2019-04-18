using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.People;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Security.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxUser.
	/// </summary>
	[ToolboxData("<GreyFoxUser:GreyFoxUserView runat=server></{0}:GreyFoxUserView>")]
	public class GreyFoxUserView : TableWindow, INamingContainer
	{
		private int greyFoxUserID;
		private GreyFoxUser greyFoxUser;

		#region Private Control Fields for Default Folder

		private Literal ltUserName = new Literal();
		private Literal ltIsDisabled = new Literal();
		private Literal ltLoginDate = new Literal();
		private Literal ltLoginCount = new Literal();
		private Literal ltLoginPassword = new Literal();
		private Literal ltActivationID = new Literal();
		private Literal ltContact = new Literal();
		private Literal ltRoles = new Literal();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxUserID
		{
			get
			{
				return greyFoxUserID;
			}
			set
			{
				greyFoxUserID = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for Default Folder

			ltUserName.EnableViewState = false;
			Controls.Add(ltUserName);

			ltIsDisabled.EnableViewState = false;
			Controls.Add(ltIsDisabled);

			ltLoginDate.EnableViewState = false;
			Controls.Add(ltLoginDate);

			ltLoginCount.EnableViewState = false;
			Controls.Add(ltLoginCount);

			ltLoginPassword.EnableViewState = false;
			Controls.Add(ltLoginPassword);

			ltContact.EnableViewState = false;
			Controls.Add(ltContact);

			ltRoles.EnableViewState = false;
			Controls.Add(ltRoles);

			ltActivationID.EnableViewState = false;
			Controls.Add(ltActivationID);

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
			if(greyFoxUserID != 0)
			{
				greyFoxUser = new GreyFoxUser(greyFoxUserID);

				#region Bind Default Folder

				//
				// Set Field Entries
				//

				ltUserName.Text = greyFoxUser.UserName.ToString();
				ltIsDisabled.Text = greyFoxUser.IsDisabled.ToString();
				ltLoginDate.Text = greyFoxUser.LoginDate.ToString();
				ltLoginCount.Text = greyFoxUser.LoginCount.ToString();
				ltLoginPassword.Text = greyFoxUser.LoginPassword.ToString();
				ltActivationID.Text = greyFoxUser.ActivationID.ToString();

				//
				// Set Children Selections
				//

				// Contact

				if(greyFoxUser.Contact != null)
					ltContact.Text = greyFoxUser.Contact.ToString();
				else
					ltContact.Text = string.Empty;

				// Roles

				if(greyFoxUser.Roles != null)
					ltRoles.Text = greyFoxUser.Roles.ToString();
				else
					ltRoles.Text = string.Empty;


				#endregion

				#region Bind _system Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//


				#endregion

				text = "View  - " + greyFoxUser.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "GreyFoxUser ID", greyFoxUserID.ToString());
			output.WriteEndTag("tr");

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
			// Render UserName
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Username");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltUserName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render IsDisabled
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Disable user account.");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltIsDisabled.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LoginDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("LoginDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltLoginDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LoginCount
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("LoginCount");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltLoginCount.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render LoginPassword
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("LoginPassword");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltLoginPassword.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Contact
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Contact");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltContact.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Roles
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Roles");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltRoles.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render ActivationID
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("ActivationID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltActivationID.RenderControl(output);
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
					greyFoxUserID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxUserID;
			return myState;
		}
	}
}
