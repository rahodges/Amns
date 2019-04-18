//using System;
//using System.Web;
//using System.Web.UI;
//
//namespace Amns.GreyFox.Content.Support
//{
//	/// <summary>
//	/// Summary description for WebChart.
//	/// </summary>
//	public class WebChart : System.Web.UI.Control
//	{
//		int width;
//		int height;
//		int[] values;
//		
//		protected override void Render(System.Web.UI.HtmlTextWriter output)
//		{
//			output.WriteBeginFullBeginTag("style");
//			output.Write("#vertgraph { width : " + width + "px; height: " + height + "px; position: relative; " + 
//				"background: url(\"" + backgroundUrl + "\") no-repeat; }");
//			output.Write("#vertgraph ul { width: " + width + "px; height: " + height + "px; margin 0; padding 0; }");
//			output.Write("#vertgraph ul li { position: absolute; width: " + barWidth + "px; height " + maxHeight + "px; " +
//				"bottom: 34px; padding 0; margin 0; background: url (\"" + barUrl + "\") no-repeat; " +
//				"text-align: center; font-weight: bold; color: white; line-height: 2.5em; }");
//			output.Write("#vertgraph li.a { left: 24px; background-position: 0px bottom; }");
//			output.Write("#vertgraph li.b { left: 101px; background-position: 0px bottom; }");
//			output.Write("#vertgraph li.c { left: 176px; background-position: 0px bottom; }");
//			output.Write("#vertgraph li.d { left: 251px; background-position: 0px bottom; }");
//			output.Write("#vertgraph li.e { left: 327px; background-position: 0px bottom; }");
//			output.WriteEndTag("style");
//
//			output.WriteBeginTag("div");
//			output.WriteAttribute("id", "vertgraph");
//			output.WriteFullBeginTag("ul");
//			output.WriteBeginTag("li");
//			output.WriteAttribute("class", "a");
//			output.WriteAttribute("style", "height: " + pixelHeight[x] + "px;");
//			output.Write(HtmlTextWriter.TagRightChar);
//			output.Write(values[x]);
//			output.WriteEndTag("li");
//			output.WriteEndTag("ul");
//			output.WriteEndTag("div");
//		}
//
//	}
//}
