/* ********************************************************** *
 * AMNS Yari Media Keyword Delete Dialog                      *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using Amns.GreyFox.People;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Yari.WebControls
{
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:YariMediaRecordDeleteDialog runat=server></{0}:YariMediaRecordDeleteDialog>")]
	public class YariMediaRecordDeleteDialog : TableWindow
	{
		private int yariMediaRecordID;
		private YariMediaRecord editYariMediaRecord;
		private string connectionString;
		
		private Button btOk = new Button();
		private Button btCancel = new Button();

		#region Public Properties
		
		[Bindable(true),
		Category("Behavior"),
		DefaultValue(0)]
		public int YariMediaRecordID
		{
			get
			{
				return yariMediaRecordID;
			}
			set
			{
				yariMediaRecordID = value;
			}
		}

		[Bindable(true),
		Category("Data"),
		DefaultValue("")]
		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				connectionString = value;
			}
		}

		#endregion

		// Organization activity cannot be set here, it is set through organization interractions,
		// however it is displayed with a javascript selector. A drop down list has the organizations
		// available for the student to be part of, and the student's status in the organization.
		// On promotion scans, organization status codes for students can be required.

		protected override void CreateChildControls()
		{
			Controls.Clear();

			btOk.Text = "OK";
			btOk.EnableViewState = false;
			btOk.Click += new EventHandler(btOk_Click);
			Controls.Add(btOk);

			btCancel.Text = "Cancel";
			btCancel.EnableViewState = false;
			btCancel.Click += new EventHandler(btCancel_Click);
			Controls.Add(btCancel);

			ChildControlsCreated = true;
		}
		protected void btOk_Click(object sender, EventArgs e)
		{
			editYariMediaRecord = new YariMediaRecord(yariMediaRecordID);
			editYariMediaRecord.Delete();

			OnDeleted(EventArgs.Empty);
		}		

		protected void btCancel_Click(object sender, EventArgs e)
		{
			this.OnCancelled(EventArgs.Empty);
		}

		#region Public Control Events

		public event EventHandler Cancelled;
		protected virtual void OnCancelled(EventArgs e)
		{
			if(Cancelled != null)
				Cancelled(this, e);
		}

		public event EventHandler Deleted;
		protected virtual void OnDeleted(EventArgs e)
		{
			if(Deleted != null)
				Deleted(this, e);
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			columnCount = 2;
			features = TableWindowFeatures.DisableContentSeparation;
		}

		protected override void OnPreRender(EventArgs e)
		{
            base.OnPreRender(e);

			if(yariMediaRecordID != 0)
			{
				editYariMediaRecord = new YariMediaRecord(yariMediaRecordID);
				Text = "Delete - " + editYariMediaRecord.Title;
			}
			else
			{
				Text = "Delete Record";
			}		
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			if(yariMediaRecordID == 0)
			{
				output.WriteFullBeginTag("tr");
				output.WriteBeginTag("td");
				output.WriteAttribute("class", "row1");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);

				output.Write("The class selected does not exist.");

				output.WriteEndTag("td");
				output.WriteEndTag("tr");
				return;
			}

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.WriteAttribute("colspan", "2");
			output.Write(HtmlTextWriter.TagRightChar);

			output.Write("<strong>Warning:</strong> This action deletes the selected class and all " +
				"attendance related to the class. This action should only be " +
				"used to remove duplicate or erroneous records from the database, use the archive utility to archive old " +
				"classes. <em>Use with caution, this option cannot be undone.</em>");
			
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");			
			output.WriteAttribute("nowrap", "true");
			output.WriteAttribute("valign", "top");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Media Record to Delete: ");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.WriteAttribute("width", "100%");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(editYariMediaRecord.Title);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			
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

		#region ViewState Methods

		protected override void LoadViewState(object savedState) 
		{
			if(savedState != null)
			{
				// Load State from the array of objects that was saved at ;
				// SavedViewState.
				object[] myState = (object[])savedState;
				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
					yariMediaRecordID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{			
			object baseState = base.SaveViewState();
			object[] myState = new object[5];
			myState[0] = baseState;
			myState[1] = yariMediaRecordID;
			return myState;
		}

		#endregion

	}
}
