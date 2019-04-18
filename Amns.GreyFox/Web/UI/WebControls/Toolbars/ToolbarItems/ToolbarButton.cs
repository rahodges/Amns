using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ToolbarItem.
	/// </summary>
	public class ToolbarButton : ToolbarItem
	{
		private string _href			= string.Empty;
		private string _target			= string.Empty;
		private bool _showTitle			= true;

		#region Public Properties

		/// <summary>
		/// Used if a button does not execute a function
		/// </summary>
		public string Href 
		{
			get { return _href; }
			set { _href = value; }
		}	

		/// <summary>
		/// The target for Href property
		/// </summary>
		public string Target 
		{
			get { return _target; }
			set { _target = value; }
		}

		public bool ShowTitle
		{
			get { return _showTitle; }
			set { _showTitle = value; }
		}

		#endregion

		public ToolbarButton()
		{
		}

		public ToolbarButton(string name, string command, string text)
		{
			this.Name = name;
			this.Command = command;
			this.Text = text;
			this.Icon = command;
		}

		public ToolbarButton(string name, string command, string text, bool showTitle)
		{
			this.Name = name;
			this.Command = command;
			this.Text = text;
			this.Icon = command;
			this.ShowTitle = showTitle;
		}

		public override void Render(ToolbarRenderer r)
		{
			if(!Visible)
				return;

			HtmlTextWriter output = r.Output;

			Unit height;
			Unit width;

			bool showTitle = _showTitle & Text.Length > 0;
			
			if(r.Style.ButtonStyle.Height.Value > 0)
				height = r.Style.ButtonStyle.Height;
			else
				height = Height;

			if(r.Style.ButtonStyle.Width.Value > 0)
				width = r.Style.ButtonStyle.Width;
			else
				width = Width;

			if(r.Style.ButtonType == ToolbarButtonType.FormButton)
			{
				output.WriteFullBeginTag("td");
				output.WriteLine();

				output.Indent++;
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "button");
				if(Function != string.Empty)
					output.WriteAttribute("onclick", Function);
				else if(Command != string.Empty)
					output.WriteAttribute("onclick", "javascript:" +
                        r.ParentControl.Page.ClientScript.GetPostBackEventReference(r.ParentControl, Command) + ";");
				output.WriteAttribute("unselectable", "on");
				output.WriteAttribute("value", Text);
				output.WriteLine(HtmlTextWriter.TagRightChar);

				output.Indent--;
				output.WriteEndTag("td");
			}
			else
			{
				output.WriteBeginTag("td");
				output.WriteAttribute("nowrap", "true");
				output.WriteAttribute("class", r.Style.Name + "_ButtonNormal");

				// if browser is IE 4+ then render a filter for disabled buttons
				if(!Enabled)
					output.WriteAttribute("style",
                        "FILTER: progid:DXImageTransform.Microsoft.BasicImage( Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=1,Opacity=0.50);");

				if(this.Enabled)
				{
					if(Function != string.Empty)
						output.WriteAttribute("onclick", Function);
					else if(Command != string.Empty)
					{
						output.Write(" onClick=\"javascript:");
						output.Write(r.ParentControl.Page.ClientScript.GetPostBackEventReference(r.ParentControl, Command));
						output.Write(";\"");
					}
				
					output.Write(" onMouseOver=\"gfx_tbov(this, '");
					output.Write(r.Style.Name);
					output.Write("', ");
					output.Write(boolToNumber(r.Style.ButtonOverImage));
					output.Write(", ");
					output.Write(boolToNumber(r.Style.ButtonDownImage));
					output.Write(");\"");

					output.Write(" onMouseOut=\"gfx_tbot(this, '");
					output.Write(r.Style.Name);
					output.Write("', ");
					output.Write(boolToNumber(r.Style.ButtonOverImage));
					output.Write(", ");
					output.Write(boolToNumber(r.Style.ButtonDownImage));
					output.Write(");\"");

					output.Write(" onMouseDown=\"gfx_tbdn(this, '");
					output.Write(r.Style.Name);
					output.Write("', ");
					output.Write(boolToNumber(r.Style.ButtonOverImage));
					output.Write(", ");
					output.Write(boolToNumber(r.Style.ButtonDownImage));
					output.Write(");\"");

					output.Write(" onMouseUp=\"gfx_tbup(this, '");
					output.Write(r.Style.Name);
					output.Write("', ");
					output.Write(boolToNumber(r.Style.ButtonOverImage));
					output.Write(", ");
					output.Write(boolToNumber(r.Style.ButtonDownImage));
					output.Write(");\"");
				}

				output.WriteLine(HtmlTextWriter.TagRightChar);

				output.Indent++;
				output.Write("<img unselectable=\"on\" src=\"");
				output.Write(r.ParentControl.ResolveUrl(r.Style.ButtonPath));
				output.Write(Icon.ToLower());
				output.Write(".");
				output.Write(r.Style.ButtonExtension);
				output.Write("\" alt=\"");
				output.Write(ToolTip);
				output.Write("\" width=\"");
				output.Write(width.ToString());
				output.Write("\" height=\"");
				output.Write(height.ToString());
				output.Write("\" align=\"absmiddle\">");				
				
				if(showTitle)
				{
					output.WriteBeginTag("span");
					output.WriteAttribute("unselectable", "on");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(Text);
					output.WriteEndTag("span");
					output.Write("&nbsp;");
				}

				output.WriteLine();

				output.Indent--;
				output.WriteEndTag("td");
				output.WriteLine();
			}
		}

		private string boolToNumber(bool value)
		{
			return value == true ? "1" : "0";
		}
	}
}
