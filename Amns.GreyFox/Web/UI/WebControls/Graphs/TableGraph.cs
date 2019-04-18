using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for TableGraph.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:TableGraph runat=server></{0}:TableGraph>")]
	public class TableGraph : System.Web.UI.Control
	{
		private string title;
		private string xAxisTitle;
		private int userWidth = 100;
		private Unit _width = Unit.Empty;

		private string[] yAxisItems;
		private int[] yAxisValues;
		
        [Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public string Title 
		{
			get
			{
				return title;
			}

			set
			{
				title = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public string XAxisTitle 
		{
			get
			{
				return xAxisTitle;
			}

			set
			{
				xAxisTitle = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue(100)] 
		public int UserWidth 
		{
			get
			{
				return userWidth;
			}

			set
			{
				userWidth = value;
			}
		}

		[Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Browsable(false)] 
		public string[] YAxisItems 
		{
			get
			{
				return yAxisItems;
			}

			set
			{
				yAxisItems = value;
			}
		}

		[Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Browsable(false)] 
		public int[] YAxisValues 
		{
			get
			{
				return yAxisValues;
			}

			set
			{
				yAxisValues = value;
			}
		}

		public Unit Width
		{
			get { return _width; }
			set { _width = value; }
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.WriteFullBeginTag("table");
			
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("tr");
			output.WriteAttribute("align", "center");
			if(!_width.IsEmpty)
				output.WriteAttribute("width", _width.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(title);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");

			output.WriteFullBeginTag("tr");
			output.WriteFullBeginTag("td");

			output.WriteBeginTag("table");
			output.WriteAttribute("border", "1");
			output.WriteAttribute("bordercolor", "#777777");
			output.WriteAttribute("cellspacing", "0");
			output.WriteAttribute("cellpadding", "0");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteFullBeginTag("td");
			output.WriteFullBeginTag("table");

			// As long as we have values to display, do so
			if (YAxisValues != null) 
			{
            
				// Color array
				String [] sColor = new String[9];
				sColor[0] = "red";
				sColor[1] = "lightblue";
				sColor[2] = "green";
				sColor[3] = "orange";
				sColor[4] = "yellow";
				sColor[5] = "blue";
				sColor[6] = "lightgrey";
				sColor[7] = "pink";
				sColor[8] = "purple";

				// Initialize the color category
				Int32 iColor = 0;

				// Get the largest value from the available items
				Int32 iMaxVal = 0;            
				for (int i = 0; i < YAxisValues.Length; i++) 
				{
					if (YAxisValues[i] > iMaxVal)
						iMaxVal = YAxisValues[i];
				}

				// Take the user-provided maximum width of the chart, and divide it by the
				// largest value in our valueset to obtain the modifier
				Int32 iMod;
				
				if(iMaxVal == 0)
					iMod = Math.Abs(userWidth/100);
				else
					iMod = Math.Abs(userWidth/iMaxVal);

				// This will be the string holder for our actual bars.
				System.Text.StringBuilder sOut = new System.Text.StringBuilder();
            
				// Render a bar for each item
				for (int i = 0; i < YAxisValues.Length; i++) 
				{

					// Only display this item if we have a value to display
					if (YAxisValues[i] > 0) 
					{

						sOut.Append("<tr><td align=right>" + YAxisItems[i] + "</td>");
						sOut.Append("<td>" + renderItem(YAxisValues[i], iMod, sColor[iColor]) + "</td></tr>");
						iColor++;
                    
						// If we have reached the end of our color array, start over
						if (iColor > 8) iColor = 0;
					}
				}

				output.Write(sOut);
			}


			// End the chart

			output.WriteEndTag("table");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			output.WriteEndTag("table");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("colspan","2");
			output.WriteAttribute("align", "center");
			output.Write(HtmlTextWriter.TagRightChar);
			output.Write(xAxisTitle);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			output.WriteEndTag("table");
		}

		private String renderItem (int iVal, int iMod, string sColor) 
		{
			System.Text.StringBuilder sRet = new System.Text.StringBuilder();
			sRet.Append("<table border=0 bgcolor=" + sColor + " cellpadding=0 cellspacing=0><tr>");
			sRet.Append("<td align=center width=" + (iVal * iMod) + " nobr nowrap>");
			sRet.Append("<b>" + iVal + "</b>");
			sRet.Append("</tr><td></table>");
			return sRet.ToString();
		}

	}
}
