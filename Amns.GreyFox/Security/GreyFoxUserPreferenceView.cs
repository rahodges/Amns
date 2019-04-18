using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxUserPreference.
	/// </summary>
	[ToolboxData("<GreyFoxUserPreference:GreyFoxUserPreferenceView runat=server></{0}:GreyFoxUserPreferenceView>")]
	public class GreyFoxUserPreferenceView : TableWindow, INamingContainer
	{
		private int greyFoxUserPreferenceID;
		private GreyFoxUserPreference greyFoxUserPreference;

		#region Private Control Fields for _system Folder


		#endregion

		#region Private Control Fields for New Folder Folder

		private Literal ltName = new Literal();
		private Literal ltPrefValue = new Literal();
		private Literal ltUser = new Literal();

		#endregion

		private Button btOk = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxUserPreferenceID
		{
			get
			{
				return greyFoxUserPreferenceID;
			}
			set
			{
				greyFoxUserPreferenceID = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for New Folder Folder

			ltUser.EnableViewState = false;
			Controls.Add(ltUser);

			ltName.EnableViewState = false;
			Controls.Add(ltName);

			ltPrefValue.EnableViewState = false;
			Controls.Add(ltPrefValue);

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
			if(greyFoxUserPreferenceID != 0)
			{
				greyFoxUserPreference = new GreyFoxUserPreference(greyFoxUserPreferenceID);

				#region Bind _system Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//


				#endregion

				#region Bind New Folder Folder

				//
				// Set Field Entries
				//

				ltName.Text = greyFoxUserPreference.Name.ToString();
				ltPrefValue.Text = greyFoxUserPreference.PrefValue.ToString();

				//
				// Set Children Selections
				//

				// User

				if(greyFoxUserPreference.User != null)
					ltUser.Text = greyFoxUserPreference.User.ToString();
				else
					ltUser.Text = string.Empty;


				#endregion

				text = "View  - " + greyFoxUserPreference.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "GreyFoxUserPreference ID", greyFoxUserPreferenceID.ToString());
			output.WriteEndTag("tr");

			render_systemFolder(output);

			renderNew_FolderFolder(output);

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

		#region Render New Folder Folder

		private void renderNew_FolderFolder(HtmlTextWriter output)
		{
			//
			// Render New Folder Folder
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", SubHeaderCssClass);
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("New Folder");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render User
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("User");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltUser.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Name
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Name");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltName.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render PrefValue
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("PrefValue");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltPrefValue.RenderControl(output);
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
					greyFoxUserPreferenceID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxUserPreferenceID;
			return myState;
		}
	}
}
