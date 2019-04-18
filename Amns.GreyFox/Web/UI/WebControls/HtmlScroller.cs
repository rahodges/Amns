using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for HtmlScroller.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:HtmlScroller runat=server></{0}:HtmlScroller>")]
	public class HtmlScroller : System.Web.UI.WebControls.WebControl
	{
		private string text;
		[Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public string Text 
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
			}
		}

		private Int16 scrollSpeed = 2;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue(2)] 
		public Int16 ScrollSpeed
		{
			get
			{
				return scrollSpeed;
			}

			set
			{
				scrollSpeed = value;
			}
		}

		public Int16 CellPadding;
		public Int16 CellSpacing;
		public Int16 Border;

		private Int16 InnerWidth
		{
			get
			{
				return (Int16) (Width.Value - (double) (CellPadding + CellSpacing));
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.WriteBeginTag("table");
			output.WriteAttribute("cellpadding", "5");
			output.WriteAttribute("cellspacing", "0");
			output.WriteAttribute("border", "0");
			output.WriteAttribute("align", "center");
			output.WriteAttribute("width", Width.Value.ToString());
			output.Write(HtmlTextWriter.TagRightChar);

			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("bgcolor", BackColor.ToString());
			output.Write(HtmlTextWriter.TagRightChar);

			output.WriteBeginTag("span");
			output.WriteAttribute("style", "borderWidth:0; borderColor:#ffffff; width:" + InnerWidth.ToString() + "; height:" + Height.Value.ToString() + ";\">");
            output.Write("<ilayer width=" + InnerWidth.ToString() + " height=" + Height.ToString() + " name=\"slider1\" bgcolor=\"" + BackColor.ToString() + " visibility=hide>");
            output.Write("<layer name=\"slider2\" onMouseOver=\"sspeed=0;\" onMouseOut=\"sspeed=" + scrollSpeed.ToString() + "\"></layer>");
            output.Write("</ilayer>");
						
			output.Write("<script language=\"JavaScript\">");
			output.Write("if (document.all){");
			output.Write("document.writeln('<marquee id=\"ieslider\" scrollAmount=" + scrollSpeed.ToString() + " width=" + InnerWidth.ToString() + " height=" + Height.ToString() + " direction=up style=\"border:0 solid grey;background-color:#ffffff\">')");
            output.Write("document.writeln(mymessage)");
			output.Write("ieslider.onmouseover=new Function(\"ieslider.scrollAmount=0\")");
			output.Write("ieslider.onmouseout=new Function(\"if (document.readyState=='complete') ieslider.scrollAmount=" + scrollSpeed.ToString() + "\")");
			output.Write("document.write('</marquee>')");
			output.Write("}");
			output.Write("if (document.getElementById&&!document.all){ ");
			output.Write("document.write('<div style=\"position:relative;overflow:hidden;width:" + InnerWidth.ToString()+ ";height:" + Height.ToString() + ";clip:rect(0 302 102 0); background-color:#ffffff;border:0px solid white;\" onMouseover=\"sspeed=0;\" onMouseout=\"sspeed=" + scrollSpeed.ToString() + "\">')");
            output.Write("document.write('<div id=\"slider\" style=\"position:relative;width:&{swidth};\">')");
            output.Write("document.write('</div></div>')");
			output.Write("}" );
			output.Write("</script>");

			output.WriteEndTag("span");
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			output.WriteEndTag("table");
		}

		protected void RegisterScript()
		{
			string script = "<script language=\"JavaScript\"> \n" +
				"<!--//hide script \n" +
				"<!-- This script and many more are available free online at ---> \n" +
				"<!-- The JavaScript Source!! http://javascript.internet.com ---> \n" +
				"<!--Begin \n" + 
				"var swidth=" + Width.Value + " \n" +					// Scroller Width
				"var sheight=" + Height.Value + " \n" +					// Scroller Height
				"var sspeed=" + ScrollSpeed + " \n" +					// Scroller Speed
				"var mymessage='' \n" +									// Message
				"mymessage='<div align=\"left\">" + text + "</DIV>' \n" +
				"function start(){ \n" +
				"if (document.all) return \n" +
				"if (document.getElementById){ \n" +
				"document.getElementById(\"slider\").style.visibility=\"show\" \n" +
				"ns6marquee(document.getElementById('slider')) \n" +
				"} \n" +
				"else if(document.layers){ \n" +
				"document.slider1.visibility=\"show\" \n" +
				"ns4marquee(document.slider1.document.slider2) \n" +
				"} \n" +
				"} \n" +
				"function ns4marquee(whichlayer){ \n" +
				"ns4layer=eval(whichlayer) \n" +
				"ns4layer.document.write(mymessage) \n" +
				"ns4layer.document.close() \n" +
				"sizeup=ns4layer.document.height \n" +
				"ns4layer.top-=sizeup \n" +
				"ns4slide() \n" +
				"} \n" +
				"function ns4slide(){ \n" +
				"if (ns4layer.top>=sizeup*(-1)){ \n" +
				"ns4layer.top-=sspeed \n" +
				"setTimeout(\"ns4slide()\",100) \n" +
				"}" +
				"else{ \n" +
				"ns4layer.top=sheight \n" +
				"ns4slide() \n" +
				"} \n" +
				"} \n" +
				"function ns6marquee(whichdiv){ \n" +
				"ns6div=eval(whichdiv) \n" +
				"ns6div.innerHTML=mymessage \n" +
				"ns6div.style.top=sheight \n" +
				"sizeup=sheight \n" +
				"ns6slide() \n" +
				"} \n" +
				"function ns6slide(){ \n" +
				"if (parseInt(ns6div.style.top)>=sizeup*(-1)){ \n" +
				"ns6div.style.top=parseInt(ns6div.style.top)-sspeed \n" +
				"setTimeout(\"ns6slide()\",100) \n" +
				"} \n" +
				"else{ \n" +
				"ns6div.style.top=sheight \n" +
				"ns6slide() \n" +
				"} \n" +
				"} \n" +
				"// End ---> \n" +
				"function MM_jumpMenu(targ,selObj,restore){ //v3.0 \n" +
				"eval(targ+\".location='\"+selObj.options[selObj.selectedIndex].value+\"'\"); \n" +
				"if (restore) selObj.selectedIndex=0; \n" +
				"} \n" +
				"//stop hiding script ---> \n" +
				"</script> \n";

			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "HtmlScroller", script);
		}
	}
}
