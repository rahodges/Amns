using System;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.EventLog.Web.UI.WebControls
{
	/// <summary>
	/// Default web editor for GreyFoxEvent.
	/// </summary>
	[ToolboxData("<GreyFoxEvent:GreyFoxEventView runat=server></{0}:GreyFoxEventView>")]
	public class GreyFoxEventView : TableWindow, INamingContainer
	{
		private int greyFoxEventID;
		private string greyFoxEventTable = "sysGlobal_EventLog";
		private GreyFoxEvent greyFoxEvent;

		#region Private Control Fields for Default Folder

		private Literal ltType = new Literal();
		private Literal ltEventDate = new Literal();
		private Literal ltSource = new Literal();
		private Literal ltCategory = new Literal();
		private Literal ltDescription = new Literal();
		private Literal ltEventID = new Literal();
		private Literal ltUser = new Literal();

		#endregion

		#region Private Control Fields for _system Folder


		#endregion

		private Button btOk = new Button();
		private Button btDelete = new Button();

		#region Public Control Properties

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int GreyFoxEventID
		{
			get
			{
				return greyFoxEventID;
			}
			set
			{
				greyFoxEventID = value;
			}
		}

		[Bindable(true), Category("Data"), DefaultValue("sysGlobal_EventLog")]
		public string GreyFoxEventTable
		{
			get
			{
				return greyFoxEventTable;
			}
			set
			{
				greyFoxEventTable = value;
			}
		}

		#endregion

		protected override void CreateChildControls()
		{
			Controls.Clear();

			#region Child Controls for Default Folder

			ltType.EnableViewState = false;
			Controls.Add(ltType);

			ltEventDate.EnableViewState = false;
			Controls.Add(ltEventDate);

			ltSource.EnableViewState = false;
			Controls.Add(ltSource);

			ltCategory.EnableViewState = false;
			Controls.Add(ltCategory);

			ltDescription.EnableViewState = false;
			Controls.Add(ltDescription);

			ltEventID.EnableViewState = false;
			Controls.Add(ltEventID);

			ltUser.EnableViewState = false;
			Controls.Add(ltUser);

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
			if(greyFoxEventID != 0)
			{
				greyFoxEvent = new GreyFoxEvent(greyFoxEventTable, greyFoxEventID);

				#region Bind Default Folder

				//
				// Set Field Entries
				//

				ltType.Text = greyFoxEvent.Type.ToString();
				ltEventDate.Text = greyFoxEvent.EventDate.ToString();
				ltSource.Text = greyFoxEvent.Source.ToString();
				ltCategory.Text = greyFoxEvent.Category.ToString();
				ltDescription.Text = greyFoxEvent.Description.ToString();
				ltEventID.Text = greyFoxEvent.EventID.ToString();

				//
				// Set Children Selections
				//

				// User

				if(greyFoxEvent.User != null)
					ltUser.Text = greyFoxEvent.User.ToString();
				else
					ltUser.Text = string.Empty;


				#endregion

				#region Bind _system Folder

				//
				// Set Field Entries
				//


				//
				// Set Children Selections
				//


				#endregion

				text = "View  - " + greyFoxEvent.ToString();
			}
		}
		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			RenderRow("row1", "GreyFoxEvent ID", greyFoxEventID.ToString());
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
			// Render Type
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Type of event.");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltType.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EventDate
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EventDate");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEventDate.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Source
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Source");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltSource.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Category
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Category");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltCategory.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Description
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Description");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltDescription.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render EventID
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("valign", "top");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("EventID");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			ltEventID.RenderControl(output);
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
					greyFoxEventID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = greyFoxEventID;
			return myState;
		}
	}
}
