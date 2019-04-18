using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.WebControls;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Default web editor for Amns.GreyFoxKeyword.
	/// </summary>
	[DefaultProperty("ConnectionString"), 
		ToolboxData("<{0}:GreyFoxKeywordEditor runat=server></{0}:GreyFoxKeywordEditor>")]
	public class Amns.GreyFoxKeywordEditor : TableWindow, INamingContainer
	{
		private int Amns.GreyFoxKeywordID;
		private Amns.GreyFoxKeyword editGreyFoxKeyword;
		private string connectionString;
		private bool resetOnAdd;
		private bool editOnAdd;

		private TextBox tbKeyword = new TextBox();
		private TextBox tbDefinition = new TextBox();
		private TextBox tbCulture = new TextBox();

		private Button btOk = new Button();
		private Button btCancel = new Button();

		[Bindable(true), Category("Data"), DefaultValue(0)]
		public int Amns.GreyFoxKeywordID
		{
			get
			{
				return Amns.GreyFoxKeywordID;
			}
			set
			{
				greyFoxKeywordID = value;
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

		[Bindable(true), Category("Data"), DefaultValue(0)]
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

		protected override void CreateChildControls()
		{
			Controls.Clear();
			bindDropDownLists();

			tbKeyword.Width = Unit.Pixel(175);
			tbKeyword.EnableViewState = false;
			Controls.Add(tbKeyword);

			tbDefinition.Width = Unit.Pixel(175);
			tbDefinition.EnableViewState = false;
			Controls.Add(tbDefinition);

			tbCulture.Width = Unit.Pixel(175);
			tbCulture.EnableViewState = false;
			Controls.Add(tbCulture);

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
			if(greyFoxKeywordID == 0)
				editGreyFoxKeyword = new Amns.GreyFoxKeyword(connectionString);
			else
				editGreyFoxKeyword = new Amns.GreyFoxKeyword(connectionString, Amns.GreyFoxKeywordID);

			editGreyFoxKeyword.Keyword = tbKeyword.Text;
			editGreyFoxKeyword.Definition = tbDefinition.Text;
			editGreyFoxKeyword.Culture = tbCulture.Text;

			if(editOnAdd)
				greyFoxKeywordID = editGreyFoxKeyword.Save();
			else
				editGreyFoxKeyword.Save();

			if(resetOnAdd)
			{
				tbKeyword.Text = string.Empty;
				tbDefinition.Text = string.Empty;
				tbCulture.Text = string.Empty;
			}

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
			features = TableWindowFeatures.DisableContentSeparation;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if(greyFoxKeywordID != 0)
			{
				editGreyFoxKeyword = new Amns.GreyFoxKeyword(connectionString, Amns.GreyFoxKeywordID);

				//
				// Set Field Entries
				//
				tbKeyword.Text = editGreyFoxKeyword.Keyword.ToString();
				tbDefinition.Text = editGreyFoxKeyword.Definition.ToString();
				tbCulture.Text = editGreyFoxKeyword.Culture.ToString();

				//
				// Set Children Selections
				//
				if(editGreyFoxKeyword.Synonyms != null)
					foreach(ListItem item in ddSynonyms.Items)
						item.Selected = editGreyFoxKeyword.Synonyms.ID.ToString() == item.Value;

				if(editGreyFoxKeyword.Antonyms != null)
					foreach(ListItem item in ddAntonyms.Items)
						item.Selected = editGreyFoxKeyword.Antonyms.ID.ToString() == item.Value;

				if(editGreyFoxKeyword.References != null)
					foreach(ListItem item in ddReferences.Items)
						item.Selected = editGreyFoxKeyword.References.ID.ToString() == item.Value;

				headerText = "Edit  - " + editGreyFoxKeyword.Name;
			}
			else
				headerText = "Add ";
		}

		protected override void RenderContent(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("tr");
			renderCell(output, "GreyFoxKeyword ID", "class=\"row1\"");
			renderCell(output, Amns.GreyFoxKeywordID.ToString(), "class=\"row1\"");
			output.WriteEndTag("tr");

			//
			// Render Keyword
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Keyword");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbKeyword.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Definition
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Definition");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbDefinition.RenderControl(output);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			//
			// Render Culture
			//
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row1");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Culture");
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", "row2");
			output.Write(HtmlTextWriter.TagRightChar);
			tbCulture.RenderControl(output);
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
					greyFoxKeywordID = (int) myState[1];
			}
		}

		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = Amns.GreyFoxKeywordID;
			return myState;
		}
	}
}
