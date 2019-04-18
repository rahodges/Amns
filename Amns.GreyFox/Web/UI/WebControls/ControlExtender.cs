using System;
using System.Text;
using Amns.GreyFox.Web.Handlers;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for PageScript.
	/// </summary>
	public class ControlExtender
	{
		public ControlExtender()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void SetSelectionByValue(System.Web.UI.WebControls.RadioButtonList radioButtonList, string value)
		{
			SetSelectionByValue(radioButtonList.Items, value);
		}

		public static void SetSelectionByValue(System.Web.UI.WebControls.DropDownList dropDownList, string value)
		{
			SetSelectionByValue(dropDownList.Items, value);
		}

		public static void SetSelectionByValue(System.Web.UI.WebControls.ListItemCollection listItemCollection,
			string value)
		{
			for(int i = 0; i < listItemCollection.Count; i++)
			{
				listItemCollection[i].Selected = listItemCollection[i].Value == value;
			}
		}

		public static void SetSelectionByText(System.Web.UI.WebControls.ListItemCollection listItemCollection,
			string text)
		{
			for(int i = 0; i < listItemCollection.Count; i++)
			{
				listItemCollection[i].Selected = listItemCollection[i].Text == text;
			}
		}

		public static string GetTooltipStartReference(string text, string color, int pixelWidth)
		{
			if(pixelWidth == 0)
				return "gfx_starttip('" + text + "', '" + color + "')";
			else
				return "gfx_starttip('" + text + "', '" + color + "', " + pixelWidth + ")";
		}

		public static string GetTooltipEndReference()
		{
			return "gfx_endtip()";
		}

		public static void RegisterDisableOnSubmitAttribute(System.Web.UI.WebControls.Button button)
		{
//			string functionName = "DisableButtonOnSubmit_onclick";

			string script = "if (typeof(Page_ClientValidate) == 'function') { " +
				"if (Page_ClientValidate() == false) { return false; }} " +
				"this.value = 'Please wait...';" +
				"this.disabled = true;" +
				button.Page.ClientScript.GetPostBackEventReference(button, string.Empty) + ";";

			button.Attributes.Add("onclick", script);
		}

		public static void RegisterKeySortAttribute(System.Web.UI.WebControls.DropDownList dropDownList, bool caseSensitive)
		{
			string functionName = "KeySortDropDownList_onkeypress";

			string script = "\r\n<script language=\"javascript\" type=\"text/javascript\">" +
				"<!--\r\n"+ 
				"function " + functionName + " (dropdownlist,caseSensitive) {\r\n" +
				"  // check the keypressBuffer attribute is defined on the dropdownlist\r\n" +
				"  var undefined; \r\n" +
				"  if (dropdownlist.keypressBuffer == undefined) { \r\n" +
				"    dropdownlist.keypressBuffer = ''; \r\n" +
				"  } \r\n" +
				"  // get the key that was pressed \r\n" +
				"  var key = String.fromCharCode(window.event.keyCode); \r\n" +
				"  dropdownlist.keypressBuffer += key;\r\n" +
				"  if (!caseSensitive) {\r\n" +
				"  // convert buffer to lowercase\r\n" +
				"    dropdownlist.keypressBuffer = dropdownlist.keypressBuffer.toLowerCase();\r\n" +
				"  }\r\n" +
				"  // find if it is the start of any of the options \r\n" +
				"  var optionsLength = dropdownlist.options.length; \r\n" +
				"  for (var n=0; n < optionsLength; n++) { \r\n" +
				"    var optionText = dropdownlist.options[n].text; \r\n" +
				"    if (!caseSensitive) {\r\n" +
				"      optionText = optionText.toLowerCase();\r\n" +
				"    }\r\n" +
				"    if (optionText.indexOf(dropdownlist.keypressBuffer,0) == 0) { \r\n" +
				"      dropdownlist.selectedIndex = n; \r\n" +
				"      return false; // cancel the default behavior since \r\n" +
				"                    // we have selected our own value \r\n" +
				"    } \r\n" +
				"  } \r\n" +
				"  // reset initial key to be inline with default behavior \r\n" +
				"  dropdownlist.keypressBuffer = key; \r\n" +
				"  return true; // give default behavior \r\n" +
				"} \r\n" +
				"// -->" +
				"</script>\r\n";

			dropDownList.Page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), functionName, script);

			dropDownList.Attributes.Add("onkeypress", 
				"return " + functionName + "(this, " + caseSensitive.ToString().ToLower() + ")");
		}

		public static void RegisterTooltipScript(System.Web.UI.Page page, int borderWidth,
			string borderColor, int padding, string backgroundColor)
		{
			if(page.Page.ClientScript.IsClientScriptBlockRegistered("gfx_tooltip"))
				return;

			AssemblyResourceHandler.RegisterScript(page, "Tooltip.js");

            page.Page.ClientScript.RegisterStartupScript(typeof(ControlExtender), "gfx_tooltip_start", 
				"<style type=\"text/css\">\r\n" +
				"#dhtmltooltip{ position: absolute; width: 150px; " +
				"border: " + borderWidth.ToString() + "px solid " + borderColor + @";" +
				"padding: " + padding.ToString() + @"px;" +
				"background-color: " + backgroundColor + @";" +
				"visibility: hidden;" +
				"z-index: 200;" +	/*Remove below line to remove shadow. Below line should always appear last within this CSS*/
				"filter: progid:DXImageTransform.Microsoft.dropShadow(Color=999999,offX=2,offY=2,positive=true);" +
				"} </style>\r\n" +
				"<script language=\"javascript\">\r\n" +
				"if (ie4 || ns6)\r\n" +
				"var tipobj = document.all ? document.all[\"dhtmltooltip\"] : document.getElementById ? document.getElementById(\"dhtmltooltip\") : \"\";" +
				"</script>");

            page.Page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), "gfx_tooltip_div", "<div id=\"dhtmltooltip\"></div>");
		}

		/// <summary>
		/// Maps a text box to a default button when the enter key is pressed. Registers a 
		/// small client script block in JavaScript to allow the functionality.
		/// </summary>
		/// <param name="page">The page the control resides in.</param>
		/// <param name="defaultButton">The button to associate the text box to.</param>
		/// <param name="textBoxArray">The array of text box controls to associate the button to.</param>
		public static void MapDefaultButton(System.Web.UI.WebControls.Button defaultButton,
			params System.Web.UI.WebControls.WebControl[] controls)
		{
			string sScript = "\n<SCRIPT language=\"javascript\">\n" +
				"function fnTrapKD(btn){\n" +
				" if (document.all){\n" +
				"   if (event.keyCode == 13)\n" +
				"{ \n" +
				"     event.returnValue=false;\n" +
				"     event.cancel = true;\n" +
				"     btn.click();\n" +
				"   }\n" +
				" }\n" +
				"}\n" +
				"</SCRIPT>\n";

			foreach(System.Web.UI.WebControls.WebControl control in controls)
			{
				control.Attributes.Add("onkeydown", "fnTrapKD(document.all." + defaultButton.ClientID + ")");
				if(!control.Page.ClientScript.IsClientScriptBlockRegistered("ForceDefaultToScript"))
					control.Page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), "ForceDefaultToScript", sScript);
			}
		}

		public static void RegisterPopupScript(System.Web.UI.Page page)
		{
			string script = "\n<script Language=\"JavaScript\">\n" +
				"<!--\n" +
				"function popup(url, name, width, height)\n" +
				"{\n" +
				"settings=\n" +
				"\"toolbar=no,location=no,directories=no,\"+\n" +
				"\"status=no,menubar=no,scrollbars=yes,\"+\n" +
				"\"resizable=yes,width=\"+width+\",height=\"+height;\n" +
				//"MyNewWindow=window.open(\"http://\"+url,name,settings);\n" +
				"MyNewWindow=window.open(url,name,settings);\n" +
				"}\n" +
				"//-->\n" +
				"</script>\n";

			page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), "AmnsPopupJS", script);
		}

		public static string RenderPopupLink(System.Web.UI.Page page,
			string text, string url, string name, int width, int height)
		{
			if(!page.ClientScript.IsClientScriptBlockRegistered("AmnsPopupJS"))
				RegisterPopupScript(page);

			return("<a href=\"javascript:popup('" +
				url + "', '" + name + "', " + width.ToString() + ", " + height.ToString() + ")\">"+ text + "</a>");
		}

		public static void RegisterBrowserDetector(System.Web.UI.Page page)
		{
			page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), "GreyFoxBrowserDetector",
				"<script language=\"javascript\"> \n" +
				"\t<!-- \n" +
				"\t\tvar NS4 = (navigator.appName == \"Netscape\" && parseInt(navigator.appVersion) < 5); \n" +
				"\t\tvar NSX = (navigator.appName == \"Netscape\"); \n" +
				"\t\tvar IE4 = (document.all) ? true : false; \n" +
				"\t\tvar isIE = document.all; \n" +
				"\t\tvar isNN = !document.all&&document.getElementById; \n" +
				"\t\tvar isN4 = document.layers; \n" +
				"\t//--> \n" +
				"</script> \n");
		}

		public static void RegisterDraggableLayerScript(System.Web.UI.Page page)
		{
			page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), "GreyFoxDraggableLayerScript",
				"<script language=\"javascript\"> \n" +				
				"\t<!-- \n" +
				//				"//DHTML Window script- Copyright Dynamic Drive (http://www.dynamicdrive.com) \n" +
				//              "//For full source code, documentation, and terms of usage, \n" +
				//				"//Visit http://www.dynamicdrive.com/dynamicindex9/dhtmlwindow.htm \n" +
				"var dragapproved=false \n" +
				"var minrestore=0 \n" +
				"var initialwidth,initialheight \n" +
				"var ie5=document.all&&document.getElementById \n" +
				"var ns6=document.getElementById&&!document.all \n" +
				"var cwindowID='' \n" +
				"function iecompattest(){ \n" +
				"return (document.compatMode!=\"BackCompat\")? document.documentElement : document.body \n" +
				"} \n" +
				"function drag_drop(e){ \n" +
				"if (cwindowID=='') \n" +
				"	return; \n" +
				"if (ie5&&dragapproved&&event.button==1){ \n" +
				"document.getElementById(cwindowID).style.left=tempx+event.clientX-offsetx+\"px\" \n" +
				"document.getElementById(cwindowID).style.top=tempy+event.clientY-offsety+\"px\" \n" +
				"} \n" +
				"else if (ns6&&dragapproved){ \n" +
				"document.getElementById(cwindowID).style.left=tempx+e.clientX-offsetx+\"px\" \n" +
				"document.getElementById(cwindowID).style.top=tempy+e.clientY-offsety+\"px\" \n" +
				"} \n" +
				"} \n" +
				"function initializedrag(e, windowID, windowContentID){ \n" +
				"cwindowID = windowID \n" +
				"offsetx=ie5? event.clientX : e.clientX \n" +
				"offsety=ie5? event.clientY : e.clientY \n" +
				"document.getElementById(windowContentID).style.display=\"none\" //extra \n" +
				"tempx=parseInt(document.getElementById(windowID).style.left) \n" +
				"tempy=parseInt(document.getElementById(windowID).style.top) \n" +
				"dragapproved=true \n" +
				"document.getElementById(windowID).onmousemove=drag_drop \n" +
				"} \n" +
				"function loadwindow(windowID,url,width,height){ \n" +
				"if (!ie5&&!ns6) \n" +
				"window.open(url,\"\",\"width=width,height=height,scrollbars=1\") \n" +
				"else{ \n" +
				"document.getElementById(windowID).style.display='' \n" +
				"document.getElementById(windowID).style.width=initialwidth=width+\"px\" \n" +
				"document.getElementById(windowID).style.height=initialheight=height+\"px\" \n" +
				"document.getElementById(windowID).style.left=\"30px\" \n" +
				"document.getElementById(windowID).style.top=ns6? window.pageYOffset*1+30+\"px\" : iecompattest().scrollTop*1+30+\"px\" \n" +
				"document.getElementById(\"cframe\").src=url \n" +
				"}\n" +
				"}\n" +
				"function maximize(windowID){ \n" +
				"if (minrestore==0){ \n" +
				"minrestore=1 //maximize window \n" +
				"document.getElementById(\"maxname\").setAttribute(\"src\",\"restore.gif\") \n" +
				"document.getElementById(windowID).style.width=ns6? window.innerWidth-20+\"px\" : iecompattest().clientWidth+\"px\" \n" +
				"document.getElementById(windowID).style.height=ns6? window.innerHeight-20+\"px\" : iecompattest().clientHeight+\"px\" \n" +
				"}\n" +
				"else{\n"+
				"minrestore=0 //restore window\n"+
				"document.getElementById(\"maxname\").setAttribute(\"src\",\"max.gif\") \n" +
				"document.getElementById(windowID).style.width=initialwidth \n" +
				"document.getElementById(windowID).style.height=initialheight \n" +
				"}\n" +
				"document.getElementById(windowID).style.left=ns6? window.pageXOffset+\"px\" : iecompattest().scrollLeft+\"px\" \n" +
				"document.getElementById(windowID).style.top=ns6? window.pageYOffset+\"px\" : iecompattest().scrollTop+\"px\" \n" +
				"}\n" +
				"function closeit(windowID){ \n" +
				"document.getElementById(windowID).style.display=\"none\" \n" +
				"}\n" +
				"function stopdrag(windowID, windowContentID){ \n" +
				"dragapproved=false; \n" +
				"document.getElementById(windowID).onmousemove=null; \n" +
				"document.getElementById(windowContentID).style.display=\"\" //extra \n" +
				"}\n" +
				"\t//--> \n" +
				"</script> \n");
		}

		public static void AssociateListBoxes(System.Web.UI.WebControls.ListBox sourceListBox,
			System.Web.UI.WebControls.ListBox targetListBox,
			System.Web.UI.HtmlControls.HtmlInputButton addButton,
			System.Web.UI.HtmlControls.HtmlInputButton removeButton
			)
		{
			// Test for validity.
			if(sourceListBox.Page == null || targetListBox.Page == null)
				throw(new ArgumentException("The control must be added to a page before " +
					"you can set the associations."));

			RegisterBrowserDetector(sourceListBox.Page);

			string sourceId = sourceListBox.ClientID;
			string targetId = targetListBox.ClientID;
			string funcPrefix = "lba_" + sourceId + targetId;

			addButton.Attributes.Add("onClick", funcPrefix + "_Add();");
			removeButton.Attributes.Add("onClick", funcPrefix + "_Remove();");
			
			sourceListBox.Page.ClientScript.RegisterClientScriptBlock(typeof(ControlExtender), funcPrefix, 
				"<script language=\"javascript\"> \n" +
				"<!-- \n" +
				"function " + funcPrefix + "_Add() \n" +
				"{ \n" +
				"	var f = document.forms[0]; \n" +
				"	var idx = f." + sourceId + ".selectedIndex; \n" +
				"	if(idx==-1) \n" +
				"		return; \n" +
				"	if (NSX) \n" +
				"	{ \n" +
				"		f." + targetId + ".options[f." + targetId + ".length] = f." + sourceId + ".options[idx].text; \n" +
				"		f." + targetId + ".options[idx] = null; \n" +
				"	} \n" +
				"	else if (IE4) \n" +
				"	{ \n" +
				"		var newOpt = document.createElement(\"OPTION\"); \n" +
				"		newOpt.text = f." + sourceId + ".options[idx].text; \n" +
				"		newOpt.value = f." + sourceId + ".options[idx].value; \n" +
				"		f." + targetId + ".add(newOpt); \n" +
				"	} \n" +
				"	f." + sourceId +".options.remove(idx); \n" +
				"} \n" +
				"function " + funcPrefix + "_Remove() \n" +
				"{ \n" +
				"	var f = document.forms[0]; \n" +
				"	var idx = f." + targetId + ".selectedIndex; \n" +
				"	if(idx==-1) \n" +
				"		return; \n" +
				"	if (NSX) \n" +
				"	{ \n" +
				"		f." + sourceId + ".options[f." + sourceId + ".length] = f." + targetId + ".options[idx].text; \n" +
				"		f." + sourceId + ".options[idx] = null; \n" +
				"	} \n" +
				"	else if (IE4) \n" +
				"	{ \n" +
				"		var newOpt = document.createElement(\"OPTION\"); \n" +
				"		newOpt.text = f." + targetId + ".options[idx].text; \n" +
				"		newOpt.value = f." + targetId + ".options[idx].value; \n" +
				"		f." + sourceId + ".add(newOpt); \n" +
				"	} \n" +
				"	f." + targetId + ".options.remove(idx); \n" +
				"} \n" +
				"//--> \n" +
				"</script> \n");
		}

		public static void TimedScript(string script, int milliseconds, System.Web.UI.Page page)
		{
			page.ClientScript.RegisterStartupScript(typeof(ControlExtender), "TimedScript",
				"<script language=\"JavaScript\">\n" +
				"<!-- \n" +
				"var redirectTimerID = -1;\n" +
				"function doRedirect() {\n" +
				"	if(redirectTimerID != -1)\n" +
				"		clearTimeout(redirectTimerID);\n" +
				"	redirectTimerID = setTimeout(\"" + script + "\", " + milliseconds.ToString() + ");\n" +
				"}\n" +
				"document.onmousemove = doRedirect;\n" +
				"document.onkeypress = doRedirect;\n" +
				"doRedirect();\n" +
				"//->\n" +
				"</script>\n");
		}

		public static void TimedRedirect(string redirectUrl, int milliseconds, System.Web.UI.Page page)
		{
			TimedRedirect(redirectUrl, milliseconds, true, page);
		}

		public static void TimedRedirect(string redirectUrl, int milliseconds, bool slidingEnabled, System.Web.UI.Page page)
		{
			string script = "<script language=\"JavaScript\">\n" +
				"<!-- \n" +
				"var sTargetURL = \"" + redirectUrl + "\";\n" +
				"var redirectTimerID = -1;\n" +
				"function doRedirect() {\n" +
				"	if(redirectTimerID != -1)\n" +
				"		clearTimeout(redirectTimerID);\n" +
				"	redirectTimerID = setTimeout(\"window.location.href=sTargetURL\", " + milliseconds.ToString() + ");\n" +
				"}\n";
			if(slidingEnabled)
				script += "document.onmousemove = doRedirect;\n" +
				"document.onkeypress = doRedirect;\n";
			script += "doRedirect();\n" +
				"//-->\n" +
				"</script>\n";

			page.ClientScript.RegisterStartupScript(typeof(ControlExtender), "TimedRedirect", script);
		}

		/// <summary>
		/// Registers a script that will scroll the page to a specific bookmark on
		/// the page.
		/// </summary>
		/// <param name="page">The page to scroll.</param>
		/// <param name="bookmark">The bookmark to scroll to.</param>
		public static void ScrollToBookmark(System.Web.UI.Page page, string bookmark)
		{
			string key		= "ScrollToBookmark";
			string script	= "<script runat=\"server\">\r\n" +
				"location.href=\"#" + bookmark + "\";\r\n" +
				"</script>";

			page.ClientScript.RegisterStartupScript(typeof(ControlExtender), key, script);
		}

		public static void SetInitialFocus(System.Web.UI.Control control) 
		{
			string scriptKey = "InitialFocus";

			if (control.Page == null) 
			{
				throw new ArgumentException(
					"The Control must be added to a Page before you can set the IntialFocus to it.");
			}
			if (control.Page.Request.Browser.EcmaScriptVersion.Major >= 1)
			{
				if(control.Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
					throw(new InvalidOperationException("There is already a control set with the " +
						"initial focus."));

				// Create JavaScript
				StringBuilder s = new StringBuilder();
				s.Append("\n<SCRIPT LANGUAGE='JavaScript'>\n");
				s.Append("<!--\n");
				s.Append("function SetInitialFocus() {\n");
				s.Append("   if(document.getElementById) {\n");
				s.Append("	     document.getElementById('");

				// s.Append(control.UniqueID);
				
//				if(control.HasControls())
//					s.Append(control.Controls[0].UniqueID);
//				else
					s.Append(control.UniqueID);

				// Set Focus on the selected item of a RadioButtonList
				if(control is System.Web.UI.WebControls.RadioButtonList)
				{
					System.Web.UI.WebControls.RadioButtonList rbl = 
						(System.Web.UI.WebControls.RadioButtonList) control;

					string suffix = "_0";
					int t = 0;
					foreach (System.Web.UI.WebControls.ListItem li in rbl.Items) 
					{
						if (li.Selected) 
						{
							suffix = "_" + t.ToString();
							break;
						}
						t++;
					}
					s.Append(suffix);
				}

				// Set Focus on the first item of a CheckBoxList
				if (control is System.Web.UI.WebControls.CheckBoxList) 
				{
					s.Append("_0");
				}

				s.Append("').focus();\n");
				s.Append("    }\n");
				s.Append("}\n");

				s.Append("window.onload = SetInitialFocus;\n");

				s.Append("// -->\n");
				s.Append("</SCRIPT>");

				// Register Client Script
				control.Page.ClientScript.RegisterStartupScript(typeof(ControlExtender), "InitialFocus", s.ToString());
			}
		}
	}
}
