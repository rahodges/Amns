using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for IFrameWindow.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:IFrameWindow runat=server></{0}:IFrameWindow>")]
	public class IFrameWindow : System.Web.UI.Control
	{
		#region Window Properties

		private string headerText;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")]
		public string Text
		{
			get
			{
				return headerText;
			}
			set
			{
				headerText = value;
			}
		}

		#endregion

		#region Appearance Properties

		private string cssClass;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")]
		public string CssClass
		{
			get
			{
				return cssClass;
			}
			set
			{
				cssClass = value;
			}
		}

		private string headerCssClass;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")]
		public string HeaderCssClass
		{
			get
			{
				return headerCssClass;
			}
			set
			{
				headerCssClass = value;
			}
		}

		private string contentCssClass;
		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public string ContentCssClass
		{
			get
			{
				return contentCssClass;
			}
			set
			{
				contentCssClass = value;
			}
		}

		private string subHeaderCssClass;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")]
		public string SubHeaderCssClass
		{
			get
			{
				return subHeaderCssClass;
			}
			set
			{
				subHeaderCssClass = value;
			}
		}

		private Unit cellPadding;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit CellPadding
		{
			get
			{
				return cellPadding;
			}
			set
			{
				cellPadding = value;
			}
		}

		private Unit cellSpacing;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit CellSpacing
		{
			get
			{
				return cellSpacing;
			}
			set
			{
				cellSpacing = value;
			}
		}

		private Unit borderWidth;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit BorderWidth
		{
			get
			{
				return borderWidth;
			}
			set
			{
				borderWidth = value;
			}
		}

		private Unit width;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		private Unit height;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public Unit Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		#endregion

		#region IFrame Properties

		private string _IFrameId;
		[Bindable(true), Category("IFrame"), DefaultValue("")] 
		public string IFrameId
		{
			get
			{
				return _IFrameId;
			}
			set
			{
				_IFrameId = value;
			}
		}

		private string _SourceUrl;
		[Bindable(true), Category("IFrame"), DefaultValue("")] 
		public string SourceUrl
		{
			get
			{
				return _SourceUrl;
			}
			set
			{
				_SourceUrl = value;
			}
		}

		//private Unit _MarginHeight;
		//private Unit _MarginWidth;

		private string _Scrolling = "auto";
		[Bindable(true), Category("IFrame"), DefaultValue("auto")] 
		public string Scrolling
		{
			get
			{
				return _Scrolling;
			}
			set
			{
				_Scrolling = value;
			}
		}

		private bool _FrameBorder = true;
		[Bindable(true), Category("IFrame"), DefaultValue(true)] 
		public bool FrameBorder
		{
			get
			{
				return _FrameBorder;
			}
			set
			{
				_FrameBorder = value;
			}
		}

		#endregion

		#region Behavior Properties

		private bool _DragEnabled = true;
		[Bindable(true), Category("Window"), DefaultValue(true)]
		public bool DragEnabled
		{
			get
			{
				return _DragEnabled;
			}
			set
			{
				_DragEnabled = value;
			}
		}

		private bool _ScrollAdjustmentEnabled;
		public bool ScrollAdjustmentEnabled
		{
			get
			{
				return _ScrollAdjustmentEnabled;
			}
			set
			{
				_ScrollAdjustmentEnabled = value;
			}
		}

		#endregion

		protected override void OnLoad(System.EventArgs e)
		{
			ControlExtender.RegisterDraggableLayerScript(this.Page);
		}

//		protected virtual void RenderHeader(HtmlTextWriter output)
//		{
//			output.WriteBeginTag("table");
//			if(!cellPadding.IsEmpty)
//				output.WriteAttribute("cellPadding", cellPadding.ToString());
//			if(!cellSpacing.IsEmpty)
//				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
//			if(!width.IsEmpty)
//				output.WriteAttribute("width", SubWidth.ToString());
//			if(headerCssClass != string.Empty)
//				output.WriteAttribute("class", headerCssClass);
//			output.WriteLine(HtmlTextWriter.TagRightChar);
//			
//			output.Indent++;
//			output.WriteFullBeginTag("tr");
//			output.WriteLine();
//					
//			output.Indent++;
//			output.WriteFullBeginTag("th");
//			output.Write(headerText);
//			output.WriteEndTag("th");
//			output.WriteLine();
//					
//			output.Indent--;
//			output.WriteEndTag("tr");
//			output.WriteLine();
//			
//			output.Indent--;
//			output.WriteEndTag("table");
//		}

		protected virtual void RenderContent(HtmlTextWriter output)
		{
			output.WriteBeginTag("div");
			output.WriteAttribute("id", UniqueID + "_content");
			output.WriteAttribute("name", UniqueID + "_content");
			output.WriteAttribute("style", "height:100%;");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteBeginTag("iframe");
			output.WriteAttribute("width", "100%");
			output.WriteAttribute("height", "100%");
			output.WriteAttribute("name", this._IFrameId);
			output.WriteAttribute("src", this._SourceUrl);
			if(this._Scrolling != "auto")
                output.WriteAttribute("scrolling", this._Scrolling);
			if(!this._FrameBorder)
				output.WriteAttribute("frameborder", "0");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write("Your browser does not support IFRAME tags.");
			output.WriteEndTag("iframe");
			output.WriteEndTag("div");
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			// START RENDERING ========================================
			output.WriteLine();

			output.WriteBeginTag("div");
			output.WriteAttribute("id", UniqueID);
			output.WriteAttribute("style", "position:absolute;left:100;top:100;visibility:visible;" +
				"width:" + this.width.ToString() + ";");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();

			output.Indent++;

			// TABLE DEFINE TAG =======================================
			output.WriteBeginTag("table");
			output.WriteAttribute("id", UniqueID + "_table");
			if(cssClass != "")
				output.WriteAttribute("class", cssClass);
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(!borderWidth.IsEmpty)
				output.WriteAttribute("border", borderWidth.ToString());
			if(!width.IsEmpty)
				output.WriteAttribute("width", width.ToString());
			if(!height.IsEmpty)
				output.WriteAttribute("height", height.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();

			// Indent to begin table rows
			output.Indent++;

			// HEADER ROW ==============================================
			output.WriteFullBeginTag("tr");
			output.WriteLine();

			output.Indent++;
			output.WriteBeginTag("th");
			output.WriteAttribute("width", "100%");
			output.Write(HtmlTextWriter.TagRightChar);	
			output.WriteLine();

			output.Indent++;
			output.WriteBeginTag("div");
			output.WriteAttribute("id", UniqueID + "_header");
			output.WriteAttribute("style", "cursor:move;width:100%;");
			output.WriteAttribute("onMousedown", "initializedrag(event, '" + UniqueID + "', '" + UniqueID + "_content')");
//			output.WriteAttribute("onMouseout", "stopdrag('" + UniqueID + "', '" + UniqueID + "_content')");
			output.WriteAttribute("onMouseup", "stopdrag('" + UniqueID + "', '" + UniqueID + "_content')");
			output.WriteAttribute("onSelectStart", "return false");
			output.WriteLine(HtmlTextWriter.TagRightChar);			
			
			output.Indent++;
			output.WriteLine(headerText);
			
			output.Indent--;
			output.WriteEndTag("div");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("th");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();
			
			// RENDER CONTENTS OF TABLE ================================
			output.WriteFullBeginTag("tr");
			output.WriteLine();
			
			output.Indent++;
			output.WriteBeginTag("td");
			if(!height.IsEmpty)
			{
//				output.WriteAttribute("height", "100%");
				output.WriteAttribute("valign", "top");		// must be top aligned for windows
			}
			output.Write(HtmlTextWriter.TagRightChar);		// with custom heights.
			output.WriteLine();								
			
			output.Indent++;
			RenderContent(output);
			
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();

			// TABLE END TAG ===========================================
			output.Indent--;
			output.WriteEndTag("table");
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("div");
			output.WriteLine();
		}

		#region Table Rendering Helper Methods

		protected void renderTableBeginTag(HtmlTextWriter output, string id,
			Unit cellPadding, Unit cellSpacing, Unit width, Unit height, string cssClass)
		{
			output.WriteBeginTag("table");
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(cssClass != null && cssClass != string.Empty)
				output.WriteAttribute("class", cssClass);
			if(!width.IsEmpty)
				output.WriteAttribute("width", width.ToString());
			if(!height.IsEmpty)
				output.WriteAttribute("height", height.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
		}

		#endregion

		#region Cell Rendering Helper Methods

		protected void RenderCell(HtmlTextWriter output, string s)
		{
			output.Indent++;
			output.WriteBeginTag("td");			
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
			output.Write(s);
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
		}

		protected void RenderCell(HtmlTextWriter output, params Control[] controls)
		{
			output.Indent++;
			output.WriteBeginTag("td");			
			output.WriteLine(HtmlTextWriter.TagRightChar);

			output.Indent++;
			foreach(Control c in controls)
					c.RenderControl(output);
			output.WriteLine();

			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
		}

		protected void RenderCell(HtmlTextWriter output, Control control, string attributes)
		{
			output.Indent++;
			output.WriteBeginTag("td");
			output.Write(" ");
			output.Write(attributes);
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
			control.RenderControl(output);
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
		}

		protected void RenderCell(HtmlTextWriter output, string s, string attributes)
		{
			output.Indent++;
			output.WriteBeginTag("td");
			output.Write(" ");
			output.Write(attributes);
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
			output.Write(s);
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
		}

		protected void RenderCell(HtmlTextWriter output, string s, string align, string colspan)
		{
			output.Indent++;
			output.WriteBeginTag("td");
			output.WriteAttribute("align", align);
			output.WriteAttribute("colspan", colspan);
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
			output.Write(s);
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
		}

		protected void RenderCell(HtmlTextWriter output, string s, string align, string colspan, string rowspan)
		{
			output.Indent++;
			output.WriteBeginTag("td");
			output.WriteAttribute("align", align);
			output.WriteAttribute("colspan", colspan);
			output.WriteAttribute("rowspan", rowspan);
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Indent++;
			output.Write(s);
			output.WriteLine();
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			output.Indent--;
		}

		#endregion
	}
}
