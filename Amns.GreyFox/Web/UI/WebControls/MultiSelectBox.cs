using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Specialized;
using Amns.GreyFox.Web.Handlers;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// A selection list control for dynamically selecting the type of selection list to render.
	/// Warning! ViewState must be enabled if clearing checkboxes does not fire OnSelectionChanged Event!
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:MultiSelectBox runat=server></{0}:MultiSelectBox>")]
	public class MultiSelectBox : System.Web.UI.WebControls.ListControl, INamingContainer, IPostBackDataHandler
	{
		string	_clientScriptDirectory				= "~/amns_greyfox_client/1_75_2088/";
		bool	_autoSort							= true;
		int		_selectBoxRowCount					= 4;
		char	_delimeter							= '|';
		Unit	_selectWidth						= Unit.Pixel(150);
		Unit	_scrollHeight						= Unit.Empty;

		bool	_isChanged							= false;

		private MultiSelectBoxMode _mode			= MultiSelectBoxMode.DualSelect;
		private RepeatLayout _repeatLayout			= RepeatLayout.Table;
//		private RepeatDirection _repeatDirection	= RepeatDirection.Horizontal;
		private int _repeatColumns					= 0;
        
		#region Public Properties

		public string ClientScriptDirectory
		{
			get { return _clientScriptDirectory; }
			set { _clientScriptDirectory = value; }
		}

		public bool AutoSort
		{
			get { return _autoSort; }
			set { _autoSort = value; }
		}

		public int SelectBoxRowCount
		{
			get { return _selectBoxRowCount; }
			set { _selectBoxRowCount = value; }
		}

		public char Delimeter
		{
			get { return _delimeter; }
			set { _delimeter = value; }
		}

		/// <summary>
		/// The mode to display the selections in. See <seealso cref="MultiSelectBoxMode"/>.
		/// </summary>
		public MultiSelectBoxMode Mode
		{
			get { return _mode; }
			set { _mode = value; }
		}

		/// <summary>
		/// Returns true if the control's selections have changed since last post back. Will return
		/// false on initial post. Useful to ignore database updates when no changes have been made
		/// to selections in a control.
		/// </summary>
		public bool IsChanged
		{
			get { return _isChanged; }
		}

		/// <summary>
		/// Specifies the selection box width for dual selects.
		/// </summary>
		public Unit SelectWidth
		{
			get { return _selectWidth; }
			set { _selectWidth = value; }
		}

//		public RepeatDirection RepeatDirection
//		{
//			get { return _repeatDirection; }
//			set { _repeatDirection = value; }
//		}

		[DefaultValue(RepeatLayout.Table)]
		public RepeatLayout RepeatLayout
		{
			get { return _repeatLayout; }
			set { _repeatLayout = value; }
		}

		public int RepeatColumns
		{
			get { return _repeatColumns; }
			set { _repeatColumns = value; }
		}

		/// <summary>
		/// Specifies the scroller height for Checkbox Lists and Radiobox Lists.
		/// </summary>
		public Unit ScrollHeight
		{
			get { return _scrollHeight; }
			set { _scrollHeight = value; }
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
		}

		#region IPostBackDataHandler Methods

		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			ListItem	i;
			bool		isFound;
			string		selectData;
			string[]	selectValues;

			isFound = false;
			_isChanged = false;
			
			// BUG FIX FOR LoadPostData not firing on no selection!
			if(_mode == MultiSelectBoxMode.CheckBoxList)
				selectData = postCollection[postDataKey + "_CB"];
			else
				selectData = postCollection[postDataKey];

			if(selectData == null)
				selectValues = new string[0];
			else if(_mode == MultiSelectBoxMode.DualSelect)
				selectValues = selectData.Split(_delimeter);
			else
				selectValues = selectData.Split(',');

			// Loop through items list
			for(int itemIndex = 0; itemIndex < Items.Count; itemIndex++)
			{
				isFound = false;
				i = Items[itemIndex];

				// Loop through the postback list to check if the item is in the postback list
				// If it is in the postback list, select the item, otherwise deselect it.
				for(int selectIndex = 0; selectIndex < selectValues.Length; selectIndex++)
				{					
					if(i.Value == selectValues[selectIndex])
					{
						if(!i.Selected)
						{
							i.Selected = true;
							_isChanged = true;
						}
						isFound = true;
					}
				}

				// If the item was not found in the postback list, deselect it.
				if(!isFound & i.Selected)
				{
					i.Selected = false;
					_isChanged = true;
				}
			}

			// Bug Fix - IsChanged is not firing on no selection.
			return _isChanged;
		}

		public virtual void RaisePostDataChangedEvent()
		{
			OnSelectionChanged(EventArgs.Empty);
		}

		public event EventHandler SelectionChanged;

		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if(SelectionChanged != null)
				SelectionChanged(this, e);
		}

		#endregion

		#region PreRender

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			if(_mode == MultiSelectBoxMode.DualSelect)
			{
				AssemblyResourceHandler.RegisterScript(this.Page, "SelectBox.js");
				AssemblyResourceHandler.RegisterScript(this.Page, "OptionTransfer.js");

				Page.ClientScript.RegisterStartupScript(this.GetType(), "gfx_ms_" + ClientID,
					"<script language=\"javascript\"> \r\n" +
					"var " + ClientID + "_ot = new OptionTransfer(\"" + ClientID + "_a\", \"" + ClientID + "_b\"); \r\n" +
					ClientID + "_ot.setAutoSort(" + _autoSort.ToString().ToLower() + "); \r\n" +
					ClientID + "_ot.setDelimiter(\"" + _delimeter + "\"); \r\n" +
					ClientID + "_ot.saveNewRightOptions(\"" + UniqueID + "\"); \r\n" +
					ClientID + "_ot.init(document.forms[0]); \r\n" +
					"</script>");

				Page.ClientScript.RegisterHiddenField(UniqueID, string.Empty);
			}	
					
			if(_mode == MultiSelectBoxMode.CheckBoxList)
			{
				// BUG FIX FOR LoadPostData not firing on no selection!
				Page.ClientScript.RegisterHiddenField(UniqueID, "1");

//				System.Text.StringBuilder s = new System.Text.StringBuilder();
//				for(int x = 0; x < Items.Count - 1; x++)
//				{
//					s.Append(Items[x].Value);
//					s.Append(";");
//				}
//				s.Append(Items[Items.Count - 1]);
//				Page.ClientScript.RegisterHiddenField(UniqueID + "_values", s.ToString());
			}
		}

		#endregion

		#region Render

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			switch(_mode)
			{
				case MultiSelectBoxMode.DualSelect:
					renderDualSelectMode(output);
					break;
				case MultiSelectBoxMode.CheckBoxList:
					renderScrollStart(output);
					renderCheckBoxList(output);
					renderScrollEnd(output);					
					break;
				case MultiSelectBoxMode.RadioBoxList:
					renderScrollStart(output);
					renderRadioBoxList(output);
					renderScrollEnd(output);
					break;
				case MultiSelectBoxMode.DropDownList:
					renderDropDownList(output);
					break;
				case MultiSelectBoxMode.ListBox:
					renderListBox(output);
					break;
				case MultiSelectBoxMode.SearchableDropDownList:
					renderSearchableDropDownList(output);
					break;
			}
		}

		/// <summary>
		/// Renders the start DIV tag for scrollable checkbox/radiobox lists.
		/// </summary>
		/// <param name="output">HtmlTextWriter reference from the ASP.net renderer.</param>
		private void renderScrollStart(HtmlTextWriter output)
		{
			if(_scrollHeight == Unit.Empty)
				return;

			output.WriteBeginTag("div");
			output.WriteAttribute("style", "border-style:solid;background-color:white;border-width:thin;overflow:auto;height:" +
				_scrollHeight.ToString() + ";");
			output.Write(HtmlTextWriter.TagRightChar);
		}

		/// <summary>
		/// Renders the end DIV tag for scrollable checkbox/radiobox lists.
		/// </summary>
		/// <param name="output">HtmlTextWriter reference from the ASP.net renderer.</param>
		private void renderScrollEnd(HtmlTextWriter output)
		{
			if(_scrollHeight == Unit.Empty)
				return;

			output.WriteEndTag("div");
		}

		#region Check Box List

		private void renderCheckBoxList(HtmlTextWriter output)
		{
			ListItem i = null;
			int x = 0;

			if(RepeatLayout == RepeatLayout.Table)
			{
				output.WriteBeginTag("table");
				output.WriteAttribute("id", ClientID + "_table");
				output.WriteAttribute("name", ClientID + "_table");
				output.WriteAttribute("border", "0");
				output.WriteAttribute("cellspacing", "0");
				output.WriteAttribute("cellpadding", "0");
				if(!Width.IsEmpty)
					output.WriteAttribute("width", Width.ToString());
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent++;

				while(x < Items.Count)
				{
					output.WriteFullBeginTag("tr");
					output.WriteLine();
					output.Indent++;

					for(int c = 0; c <= RepeatColumns; c++)
					{
						// Write Dummy Columns
						if(x == Items.Count)
						{
							output.WriteFullBeginTag("td");
							output.WriteEndTag("td");
							output.WriteLine();
							continue;
						}

						i = Items[x];

						output.WriteFullBeginTag("td");
						output.WriteLine();
						output.Indent++;
						output.WriteBeginTag("input");
						output.WriteAttribute("type", "checkbox");
						output.WriteAttribute("name", UniqueID + "_CB");	// BUGFIXED FOR CHECKBOX LIST!
						output.WriteAttribute("value", i.Value);
						if(i.Selected)
							output.WriteAttribute("checked", "true");
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write(i.Text);
						output.WriteLine();
				
						output.Indent--;
						output.WriteEndTag("td");
						output.WriteLine();

						x++;
					}
			
					output.Indent--;
					output.WriteEndTag("tr");
				}
			
				output.WriteLine();
				output.Indent--;
				output.WriteEndTag("table");			
				output.WriteLine();
			}
			else
			{
				while(x < Items.Count)
				{
					i = Items[x];

					output.WriteBeginTag("input");
					output.WriteAttribute("type", "checkbox");
					output.WriteAttribute("name", UniqueID + "_CB");	// BUGFIXED FOR CHECKBOX LIST!
					output.WriteAttribute("value", i.Value);
					if(i.Selected)
						output.WriteAttribute("checked", "true");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(i.Text);
					output.WriteLine("<br />");

					x++;
				}
			}
		}

		#endregion

		#region Radio Box List

		private void renderRadioBoxList(HtmlTextWriter output)
		{
			ListItem i = null;
			int x = 0;

			if(RepeatLayout == RepeatLayout.Table)
			{
				output.WriteBeginTag("table");
				output.WriteAttribute("id", ClientID + "_table");
				output.WriteAttribute("name", ClientID + "_table");
				output.WriteAttribute("border", "0");
				output.WriteAttribute("cellspacing", "0");
				output.WriteAttribute("cellpadding", "0");
				if(!Width.IsEmpty)
					output.WriteAttribute("width", Width.ToString());
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.Indent++;
			

				while(x < Items.Count)
				{   
					output.WriteFullBeginTag("tr");
					output.WriteLine();
					output.Indent++;

					for(int c = 0; c <= RepeatColumns; c++)
					{
						if(x == Items.Count)
						{
							output.WriteFullBeginTag("td");
							output.WriteEndTag("td");
							output.WriteLine();
							continue;
						}

						i = Items[x];

						output.WriteFullBeginTag("td");
						output.WriteLine();
						output.Indent++;
						output.WriteBeginTag("input");
						output.WriteAttribute("type", "radio");
						output.WriteAttribute("name", UniqueID);
						output.WriteAttribute("value", i.Value);
						if(i.Selected)
							output.WriteAttribute("checked", "true");
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write(i.Text);
						output.WriteLine();
				
						output.Indent--;
						output.WriteEndTag("td");
						output.WriteLine();

						x++;
					}
			
					output.Indent--;
					output.WriteEndTag("tr");
				}
			
				output.WriteLine();
				output.Indent--;
				output.WriteEndTag("table");			
				output.WriteLine();
			}
			else
			{
				while(x < Items.Count)
				{
					i = Items[x];

					output.WriteBeginTag("input");
					output.WriteAttribute("type", "radio");
					output.WriteAttribute("name", UniqueID);
					output.WriteAttribute("value", i.Value);
					if(i.Selected)
						output.WriteAttribute("checked", "true");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(i.Text);
					output.WriteLine("<br />");

					x++;
				}
			}
		}

		#endregion

		#region Render Dual Select Mode

		private void renderDualSelectMode(HtmlTextWriter output)
		{
			ListItem i = null;

			output.WriteBeginTag("table");
			output.WriteAttribute("id", ClientID + "_table");
			output.WriteAttribute("name", ClientID + "_table");
			output.WriteAttribute("border", "0");
			output.WriteAttribute("cellspacing", "0");
			output.WriteAttribute("cellpadding", "0");
			if(!Width.IsEmpty)
				output.WriteAttribute("width", Width.ToString());
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;

			output.WriteLine("<tr><td width=\"33%\">Unselected:</td><td width=\"34%\">&nbsp;</td><td width=\"33%\">Selected:</td></tr>");

			output.WriteFullBeginTag("tr");
			output.WriteLine();
			output.Indent++;

			// RENDER LIST DROP DOWN BOX
			output.WriteBeginTag("td");
			output.WriteAttribute("width", "33%");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;
			output.WriteBeginTag("select");
			output.WriteAttribute("name", ClientID + "_a");
			output.WriteAttribute("size", _selectBoxRowCount.ToString());
			output.WriteAttribute("multiple", "multiple");
			if(_selectWidth != Unit.Empty)
				output.WriteAttribute("style", "width:" + _selectWidth.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;

			// Use a regular 'for' loop, it's faster than foreach
			for(int x = 0; x < Items.Count; x++)
			{
				i = Items[x];

				// Skip items already selected
				if(i.Selected)
					continue;
				
				output.WriteBeginTag("option");
				output.WriteAttribute("value", i.Value);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(i.Text);
				output.WriteEndTag("option");	
				output.WriteLine();
			}

			output.Indent--;
			output.WriteEndTag("select");
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();			

			// RENDER LIST BUTTONS
			output.WriteBeginTag("td");
			output.WriteAttribute("width", "34%");
			output.WriteAttribute("align", "center");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteBeginTag("p");
			output.WriteAttribute("align", "center");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteBeginTag("input");						// Add Button
			output.WriteAttribute("type", "button");
			output.WriteAttribute("name", ClientID + "_ad");
			output.WriteAttribute("value", ">>");
			output.WriteAttribute("onclick", ClientID + "_ot.transferRight();");
			output.WriteAttribute("style", "width:25px");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteEndTag("p");
			output.WriteBeginTag("p");
			output.WriteAttribute("align", "center");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteBeginTag("input");						// Remove Button
			output.WriteAttribute("type", "button");
			output.WriteAttribute("name", ClientID + "_rm");
			output.WriteAttribute("value", "<<");
			output.WriteAttribute("onclick", ClientID + "_ot.transferLeft();");
			output.WriteAttribute("style", "width:25px");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteEndTag("p");
			output.WriteEndTag("td");

			// RENDER LIST BOX
			output.WriteBeginTag("td");
			output.WriteAttribute("width", "33%");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;

			output.WriteBeginTag("select");
			output.WriteAttribute("id", ClientID + "_b");
			output.WriteAttribute("name", ClientID + "_b");
			output.WriteAttribute("size", _selectBoxRowCount.ToString());
			output.WriteAttribute("multiple", "multiple");
			if(_selectWidth != Unit.Empty)
				output.WriteAttribute("style", "width:" + _selectWidth.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;

			// Use a regular 'for' loop, it's faster than foreach
			for(int x = 0; x < Items.Count; x++)
			{
				i = Items[x];

				// Skip items not selected
				if(!i.Selected)
					continue;

				output.WriteBeginTag("option");
				output.WriteAttribute("value", i.Value);
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(i.Text);
				output.WriteEndTag("option");	
				output.WriteLine();
			}

			output.Indent--;
			output.WriteEndTag("select");
			output.WriteLine();
			output.Indent--;			
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("table");			
			output.WriteLine();
		}

		#endregion

		#region DropDownList

		private void renderDropDownList(HtmlTextWriter output)
		{
			ListItem i;

			output.WriteBeginTag("select");
			output.WriteAttribute("name", UniqueID);
			if(Width != Unit.Empty)
				output.WriteAttribute("style", "width:" + Width.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;

			// Use a regular 'for' loop, it's faster than foreach
			for(int x = 0; x < Items.Count; x++)
			{
				i = Items[x];

				output.WriteBeginTag("option");
				output.WriteAttribute("value", i.Value);
				if(i.Selected)
					output.WriteAttribute("selected", "true");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(i.Text);
				output.WriteEndTag("option");	
				output.WriteLine();
			}

			output.Indent--;
			output.WriteEndTag("select");
			output.WriteLine();
			output.Indent--;
		}

		#endregion

		#region ListBox

		private void renderListBox(HtmlTextWriter output)
		{
			ListItem i;

			output.WriteBeginTag("select");
			output.WriteAttribute("name", UniqueID);
			output.WriteAttribute("size", _selectBoxRowCount.ToString());
			output.WriteAttribute("multiple", "multiple");
			if(Width != Unit.Empty)
				output.WriteAttribute("style", "width:" + Width.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();
			output.Indent++;

			// Use a regular 'for' loop, it's faster than foreach
			for(int x = 0; x < Items.Count; x++)
			{
				i = Items[x];
				
				output.WriteBeginTag("option");
				output.WriteAttribute("value", i.Value);
				if(i.Selected)
					output.WriteAttribute("selected", "true");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(i.Text);
				output.WriteEndTag("option");	
				output.WriteLine();
			}

			output.Indent--;
			output.WriteEndTag("select");
			output.WriteLine();
			output.Indent--;
		}

        #endregion

		#region Searchable Drop Down List

		public void renderSearchableDropDownList(HtmlTextWriter output)
		{
			// TODO: Implement searchable drop down list
			throw(new NotImplementedException("Searchable Drop Down List not implemented in this version."));
		}

		#endregion

		#endregion
	}
}
