/* =================================================================== *
/* Toolbar Methods													   *
/* =================================================================== */

function gfx_setbtstyle(buttonTD,style,checkstyle) {
	if (buttonTD == null) return;
	if (buttonTD.className != checkstyle)
	buttonTD.className = style;
}

function gfx_getclasssubname(className) {
	underscore = className.indexOf("_");
	if (underscore < 0) return className;
		return className.substring(underscore+1);
}

function gfx_tbov(theTD,styleName,imageOver,imageDown) {
	gfx_setbtstyle(theTD,styleName+'_ButtonOver',null);
	if (eval(styleName+'_OverImage').src != '') theTD.background=eval(styleName+'_OverImage').src;
	if(theTD.childNodes.length && theTD.childNodes[0].tagName == "IMG" && imageOver){
		oldSrc = theTD.childNodes[0].src;
		if (oldSrc.indexOf('.over.') == -1) {
			theTD.childNodes[0].src=oldSrc.substring(0, oldSrc.length-4) + ".over.gif";
		}
	}
}

function gfx_tbot(theTD,styleName,imageOver,imageDown) {
	gfx_setbtstyle(theTD,styleName+'_ButtonNormal',null);
	document.body.style.cursor = 'default';
	theTD.background='';
	if(theTD.childNodes.length && theTD.childNodes[0].tagName == "IMG"){
		oldSrc = theTD.childNodes[0].src;
		if (oldSrc.indexOf('.over.') > 0) {
			theTD.childNodes[0].src=oldSrc.substring(0, oldSrc.length-9) + ".gif";
		}
		if (oldSrc.indexOf('.down.') > 0) {
			theTD.childNodes[0].src=oldSrc.substring(0, oldSrc.length-9) + ".gif";
		}
	}
}
function gfx_tbdn(theTD,styleName,imageOver,imageDown) {
	document.body.style.cursor = 'default';
	gfx_setbtstyle(theTD,styleName+'_ButtonDown',null);
	if (eval(styleName+'_DownImage').src != '') theTD.background=eval(styleName+'_DownImage').src;
	if(theTD.childNodes.length && theTD.childNodes[0].tagName == "IMG" && imageDown == 1){
		oldSrc = theTD.childNodes[0].src;
		if (oldSrc.indexOf('.over.') > 0) {
			theTD.childNodes[0].src=oldSrc.substring(0, oldSrc.length-9) + ".down.gif";
		}
	}
}

function gfx_tbup(theTD,styleName,imageOver,imageDown) {
	document.body.style.cursor = 'auto';
	gfx_setbtstyle(theTD,styleName+'_ButtonOver',null);
	if (eval(styleName+'_OverImage').src != '') theTD.background=eval(styleName+'_OverImage').src;
	if(theTD.childNodes.length && theTD.childNodes[0].tagName == "IMG" && imageOver == 1){
			oldSrc = theTD.childNodes[0].src;
		if (oldSrc.indexOf('.over.') == -1) {
			theTD.childNodes[0].src=oldSrc.substring(0, oldSrc.length-4) + ".over.gif";
		}
	}
}