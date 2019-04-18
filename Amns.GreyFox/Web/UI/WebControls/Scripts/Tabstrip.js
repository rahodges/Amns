/* ==================================================================== *
/* Tab Scripts															*
/* Copyright © 2005 Roy A.E. Hodges										*
/* ==================================================================== */

function gfx_tabover() {
	document.body.style.cursor='default';
}

function gfx_tabout() {
	document.body.style.cursor='auto';
}

function gfx_sat(theTD,styleName) {
	parentTR = theTD.parentElement;
	selectedTab = 1;
	totalButtons = parentTR.cells.length-1;
	for (var i=1;i< totalButtons;i++) {
		parentTR.cells[i].className = styleName + "_TabOffRight";
		if (theTD == parentTR.cells[i]) { selectedTab = i; }
	}
	if (selectedTab==1) {
		parentTR.cells[0].className = styleName + "_StartTabOn";
	} else {
		parentTR.cells[0].className = styleName + "_StartTabOff";
		parentTR.cells[selectedTab-1].className = styleName + "_TabOffLeft";
	}
	theTD.className = styleName + "_TabOn";
}

function gfx_setTab(controlId, tabId) { 
	var t; 
	var tabs = eval(controlId + "_Tabs");
	for (var i = 0; i < tabs.length; i++) { 
		tab		= new getObj(controlId + "_Tab_" + tabs[i]).obj;
		tabPage = new getObj(controlId + "_TabPage_" + tabs[i]).obj;
		if(tabs[i] == tabId) { 
 			tabPage.style.visibility = 'visible'; 
			tabPage.style.display = 'block'; 
			tab.className = 'gfxtabon';
		} 
		else { 
			tabPage.style.visibility = 'hidden'; 
			tabPage.style.display = 'none';
			tab.className = 'gfxtaboff';
		} 
	} 
} 