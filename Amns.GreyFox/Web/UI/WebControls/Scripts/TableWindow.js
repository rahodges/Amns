/* =================================================================== *
/* Amns.GreyFox Support Scripts										   *
/* Copyright (C) 2004-2006 Roy A.E. Hodges. All Rights Reserved		   *
/* Unauthorized duplication not permitted.							   *
/* =================================================================== */

/* =================================================================== *
/* TableWindow Scroller												   *
/* =================================================================== */

function gfx_ToggleView(scrollerid, height) { 
	var scrolldiv = new getObj(scrollerid + "_ContentDiv").obj; 
	var scrolltable = new getObj(scrollerid).obj; 
	if(scrolldiv.style.overflow == 'scroll') { 
		scrolltable.height = ''; 
		scrolltable.style.height = ''; 
		scrolldiv.style.overflow = 'visible'; 
		new getObj("__" + scrollerid + "ScrollState").obj.value = 'False'; 
	} else { 
		scrolltable.height = height; 
		scrolltable.style.height = height; 
		scrolldiv.style.overflow = 'scroll'; 
		scrolldiv.scrollTop = new getObj("__" + scrollerid +"ScrollTop").obj.value; 
		scrolldiv.scrollLeft = new getObj("__" + scrollerid +"ScrollLeft").obj.value; 
		new getObj("__" + scrollerid + "ScrollState").obj.value = 'True'; 
	} 
}

function gfx_clipcopy(source) {
	var text = document.getElementById(source).innerHTML;
	if (window.clipboardData) {
		window.clipboardData.setData("Text", text);
	}
	else if (window.netscape) { 
		netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');
		var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
		if (!clip) return;
		var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
		if (!trans) return;
		trans.addDataFlavor('text/unicode');
		var str = new Object();
		var len = new Object();   
		var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
		str.data=text;
		trans.setTransferData("text/unicode",str,copytext.length*2);
		var clipid=Components.interfaces.nsIClipboard;
		if (!clip) return false;
		clip.setData(trans,null,clipid.kGlobalClipboard);
	}	
	return false;
}

/* =================================================================== *
/* Dynamic IFRAME/LAYER Swap										   *
/* =================================================================== */

function gfx_loadSource(url) {
	document.getElementById('gfx_bufferFrame').src = url;
}

function gfx_loadSourceFinish(id) {
	document.getElementById(id).innerHTML = window.frames["gfx_bufferFrame"].document.body.innerHTML;
}

/* =================================================================== *
/* TableWindow Methods												   *
/* November 21, 2004 (Utilizes DHTML micro API and Event Handler)	   *
/* =================================================================== *
/* Be sure to set gridObject.selectCssClass = 'selectclass'			   *
/* =================================================================== */

function gfx_TableGridObject(
    tableId,
    contentWidth,
    clientSelectEnabled,
    viewPaneEnabled,
    scrollEnabled) {       
		
	// Properties
	this.id	= tableId;
	
	this.contentPane = new getObj(tableId + '_ContentDiv').obj;
	this.toolbarPane = new getObj(tableId + '_ToolbarDiv').obj;
	this.contentWidth = contentWidth;
	this.printCssClass = '';
	this.callBackControl = null;
	this.executeCallBack = gfxtbo_executeCallBack;
	
	this.clientSelectEnabled = clientSelectEnabled;
	this.viewPaneEnabled = viewPaneEnabled;
	this.scrollEnabled = scrollEnabled;
	
	this.headerLockEnabled = false;
	this.headerSortEnabled = false;
	this.columnLockEnabled = false;
	
	// Methods
	this.bind = gfxtbo_bind;
	this.hidePanes = gfxtbo_hidePanes;
	this.showPanes = gfxtbo_showPanes;
	this.print = gfxtbo_print;
	
	if(clientSelectEnabled) {
		this.dataTable = new getObj(tableId + '_datatable').obj;
		this.dataRows = this.dataTable.rows;
		this.selectField = new getObj('__' + tableId + '_ID').obj;
		this.selectId = this.selectField.value;	
		this.selectRow = null;
		this.priorCellClasses = null;
		this.selectCssClass = '';
		this.highlightRow = gfxtbo_hrow;
	}
	
	if(viewPaneEnabled) {
		// Properties
		this.contentPaneField = new getObj('__' + tableId + '_CP').obj;
		this.viewPaneField = new getObj('__' + tableId + '_VP').obj;
		this.viewPaneMessage = "<h3>Loading...</h3>";
		this.viewPane = new getObj('__gfxViewPane_' + this.id).obj;
		// Methods
		this.toggleViewPane = gfxtbo_tvp;
		this.refreshPane = gfxtbo_rp;
		// Init
	}
	
	if(scrollEnabled) {
		// Properties
		this.headerRow = null;
		//this.headerCells = null;
		this.scrollTD = new getObj(tableId + '_ScrollTD').obj;
		this.scrollTopField = new getObj('__' + tableId + 'ScrollTop').obj;
		this.scrollLeftField = new getObj('__' + tableId + 'ScrollLeft').obj;
		// Methods
		this.scroll = gfxtbo_scroll;
		this.applyHeaderLock = gfxtbo_ahl;
		this.applyColumnLock = gfxtbo_acl;
	}
}

// Initializes the TableObject
function gfxtbo_bind() {

	if(this.clientSelectEnabled) {
	
		// Check to make sure selected row is visible
		var isRowFound = false;
		
		// Setup row selection events, cursor and highlight the selected row
		// if there is a selection.
		for(var x = 0; x < this.dataRows.length; x++) {		
			if(this.dataRows[x].getAttribute('i') != null) {
//				addEvent(this.dataRows[x], 'click', gfxtbo_select, false);
				this.dataRows[x].onclick = gfxtbo_select;				
				this.dataRows[x].style.cursor = 'hand';
				if(this.dataRows[x].getAttribute('i') == this.selectId) {
					this.highlightRow(this.dataRows[x]);
					isRowFound = true;
				}
			}
		}
		
		// Highlight the default row
		if(!isRowFound) {
			this.selectId = -1;
			this.selectField.value = '-1';
			for(var x = 0; x < this.dataRows.length; x++) {
				if(this.dataRows[x].getAttribute('i') != null) {
					this.selectId = this.dataRows[x].getAttribute('i');
					this.selectField.value = this.selectId;
					this.highlightRow(this.dataRows[x]);
					break;
				}
			}				
		}
		
		// Refresh Pane
		if(this.viewPaneEnabled && this.selectId != -1) {
			this.refreshPane();
			setElementVisibility(this.contentPane, this.contentPaneField.value == "True");
			setElementVisibility(this.viewPane, this.viewPaneField.value == "True");
		}
		
		if(this.callBackControl != null && this.selectId != -1) {
	        this.executeCallBack();
	    }
	}
	
	if(this.scrollEnabled) {		
		// Temporarily turn of Header Lock Enabled to prevent nasty script crash.
		var tempLock = this.headerLockEnabled;
		this.headerLockEnabled = false;
		this.contentPane.scrollTop = this.scrollTopField.value;
		this.contentPane.scrollLeft = this.scrollLeftField.value;
		this.headerLockEnabled = tempLock;
		
		// Apply relative position and preapply header lock		
		if(this.headerLockEnabled) {
			this.headerRow = this.dataRows[0];
			this.headerRow.style.position = 'relative';
			this.headerRow.style.zIndex = 25;
			this.applyHeaderLock();
		}
		
		// Apply sort events
		if(this.headerSortEnabled) {
			for(var x = 0; x < this.dataRows[0].cells.length; x++) {
				//Event.observe(this.dataRows[0].cells[x], 'click', sort);
				this.dataRows[0].cells[x].innerHTML = 
					"<span onclick='sort(this)' style='cursor:hand;'>" + 
					this.dataRows[0].cells[x].innerHTML +
					"</span>";
			}
		}
		
		// Apply relative position and preapply column lock
		if(this.columnLockEnabled) {
			for(var x = 0; x < this.dataRows.length; x++) {
				this.dataRows[x].cells[0].style.position = 'relative';
				this.dataRows[x].cells[0].style.zIndex = 20;
			}
			this.applyColumnLock();
		}
	}
}

function setElementVisibility(element, visible)
{
	if(visible)
		element.style.visibility = 'visible';
	else
		element.style.visibility = 'hidden';
}

// Saves the scroll position
function gfxtbo_scroll() {
	if(this.headerLockEnabled && this.scrollTopField.value != this.contentPane.scrollTop)
		this.applyHeaderLock();
	if(this.columnLockEnabled && this.scrollLeftField.value != this.contentPane.scrollLeft)
		this.applyColumnLock();
	this.scrollTopField.value = this.contentPane.scrollTop;
	this.scrollLeftField.value = this.contentPane.scrollLeft;
}

// Updates the header row position
function gfxtbo_ahl() {
	this.headerRow.style.top = this.contentPane.scrollTop - 2;
}

// Updates left column position
function gfxtbo_acl() {
	if(this.columnLockEnabled) {
		for(var x = 0; x < this.dataRows.length; x++) {
			this.dataRows[x].cells[0].style.left = this.contentPane.scrollLeft;
		}
	}
	else
	{
		for(var x = 0; x < this.dataRows.length; x++) {
			this.dataRows[x].cells[0].style.removeExpression("left");
		}
	}
}

// Toggle View Pane
// Toggles content and preview pane to on-on (default), on-off, off-on.
// Saves this state.
function gfxtbo_tvp() {		
	if(this.contentPane.style.visibility == 'hidden' & this.viewPane.style.visibility == 'visible') {		
		this.contentPane.style.width = this.contentWidth;
		this.contentPane.style.visibility = 'visible';
		this.contentPaneField.value = 'True';
		this.viewPane.style.visibility = 'visible';
		this.viewPaneField.value = 'True';
	}
	else if(this.contentPane.style.visibility == 'visible' & this.viewPane.style.visibility == 'visible') {
		this.contentPane.style.width = '100%';
		this.viewPane.style.visibility = 'hidden';
		this.viewPaneField.value = 'False';
	}
	else {
		this.contentPane.style.width = '0px';
		this.contentPane.style.visibility = 'hidden';
		this.contentPaneField.value = 'False';
		this.viewPane.style.visibility = 'visible';
		this.viewPaneField.value = 'True';
	}
	this.scroll();
}

function gfxtbo_hrow(highlightRow) 
{
	if(this.selectRow != null) {
		var restoreCells = this.selectRow.childNodes;
		for(var i = 0; i < restoreCells.length; i++)
			if(restoreCells[i].nodeType == 1 && restoreCells[i].tagName.toLowerCase() == 'td') {
				restoreCells[i].className = this.priorCellClasses[i];
			}
	}	
	var highlightCells = highlightRow.childNodes;
	this.priorCellClasses = new Array(highlightCells.length);
	for(var i = 0; i < highlightCells.length; i++)
		if(highlightCells[i].nodeType == 1 && highlightCells[i].tagName.toLowerCase() == 'td') {
			this.priorCellClasses[i] = highlightCells[i].className;
			highlightCells[i].className = this.selectCssClass;
		}
	this.selectRow = highlightRow;
}

function gfxtbo_select(e) {

	// Find the TableGrid ID
	var targ;
	if (!e) var e = window.event;
	if (e.target) targ = e.target;
	else if (e.srcElement) targ = e.srcElement;
	if (targ.nodeType == 3) // defeat Safari bug
		targ = targ.parentNode;
	
	// Get the parent TableGridObject
	var tableGridObj = gfxtbo_getObject(targ);
	var row = getParent(targ, 'TR');
	tableGridObj.highlightRow(row);
	tableGridObj.selectId = row.getAttribute('i');
	
	tableGridObj.selectField.value = tableGridObj.selectId;
	
	if(tableGridObj.viewPaneEnabled)
	{
		tableGridObj.refreshPane();
    }
    
    tableGridObj.executeCallBack();
}

function gfxtbo_rp()
{
	var vp = this.viewPane;
	vp.innerHTML = this.viewPaneMessage;
	var myAjax = new AjaxRequest.getRequest(
		{
			'url': '?' + '__gfxViewPane_' + this.id + '=' + this.selectId,
			'onSuccess': function(req) { vp.innerHTML = req.responseText; },
			'onError': function(req) { vp.innerHTML = 'Error.'; },
			'timeout': 10000,
			'onTimeout': function(req){ alert('The request timed out.'); }
		}
	);
}

function gfxtbo_executeCallBack()
{
    if(this.callBackControl != null)
    {
        this.callBackControl.callback(this.selectId);
    }
}

function gfxtbo_getObject(element)
{
	var table = getParent(element, 'TABLE');
	while(table.getAttribute('isRootTableWindow') == null)
		table = getParent(table.parentNode, 'TABLE');
	return eval('tableGrid' + table.id);
}

function gfxtbo_hidePanes() {
	if(this.contentPane != null)
		this.contentPane.style.visibility = 'hidden';
	if(this.viewPane != null)
		this.viewPane.style.visibility = 'hidden';
}

function gfxtbo_showPanes() {
	if(this.contentPane != null)
		this.contentPane.style.visibility = 'visible';
	if(this.viewPane != null)
		this.viewPane.style.visibility = 'visible';
}

function gfxtbo_print() {
	wprintframe.document.body.innerHTML = new getObj(this.id + '_Window').obj.innerHTML;	
	
	var pContent = wprintframe.document.getElementById(this.id + '_ContentDiv');
	var pToolbar = wprintframe.document.getElementById(this.id + '_ToolbarDIV');
	var pViewPane = wprintframe.document.getElementById('__gfxViewPane_' + this.id);	
	
	if(this.printCssClass != '') {
		wprintframe.document.createStyleSheet(this.printCssClass);
	}
	
	if(pViewPane != null && pViewPane.style.visibility == 'visible') {
		pViewPane.style.overflow = 'visible';
		wprintframe.document.body.innerHTML = pViewPane.innerHTML;
	}
	else
	{
		if(pContent != null) {
			pContent.style.overflow = 'visible'; 
		}
		if(pToolbar != null) {
			var toolbarRow = pToolbar.parentNode.parentNode;
			toolbarRow.parentNode.removeChild(toolbarRow);
		}
	}
	
	wprintframe.focus(); 
	wprintframe.print(); 	
}