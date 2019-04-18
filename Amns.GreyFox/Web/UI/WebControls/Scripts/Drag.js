/* =================================================================== *
/* Drag API															   *
/* =================================================================== */

var dragObj = new Object();
dragObj.zIndex = 0;

function gfx_dragStart(event, id) {
	var el, x, y;
	if(id)
		dragObj.elNode = document.getElementById(id);
	else {
		if(ie4)
			dragObj.elNode = window.event.srcElement;
		if(ns4|ns6)
			dragObj.elNode = event.target;
		if(dragObj.elNode.nodeType == 3)
			dragObj.elNode = dragObj.elNode.parentNode;		
	}
	if(ie4) {
		x = window.event.clientX + document.documentElement.scrollLeft +
			document.body.scrollLeft;
		y = window.event.clientY + document.documentElement.scrollTop +
			document.body.scrollTop;
	}
	if(ns4|ns6) {
		x = event.clientX + window.scrollX;
		y = event.clientY + window.scrollY;
	}
	
	dragObj.cursorStartX	= x;
	dragObj.cursorStartY	= y;
	dragObj.elStartLeft		= parseInt(dragObj.elNode.style.left, 10);
	dragObj.elStartTop		= parseInt(dragObj.elNode.style.top, 10);
	
	if(isNaN(dragObj.elStartLeft)) dragObj.elStartLeft = 0;
	if(isNaN(dragObj.elStartTop))  dragObj.elStartTop  = 0;
	
	dragObj.elNode.style.zIndex = ++dragObj.zIndex;
	
	if(ie4) {
		document.attachEvent("onmousedown", dragGo);
		document.attachEvent("onmouseup",	dragStop);
		windows.event.cancelBubble = true;
		windows.event.returnValue = false;
	}
	if(ns4|ns6) {
		document.addEventListener("onmousemove", dragGo,   true);
		document.addEventListener("mouseup",	 dragStop, true);
		event.preventDefault();
	}
}

function dragGo(event) {
  var x, y;
  
  if(ie4) {
    x = window.event.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
    y = window.event.clientY + document.documentElement.scrollTop + document.body.scrollTop;
  }
  
  if(ns4|ns6) {
    x = event.clientX + window.scrollX;
    y = event.clientY + window.scrollY;
  }
  
  dragObj.elNode.style.left = (dragObj.elStartLeft + x - dragObj.cursorStartX) + "px";
  dragObj.elNode.style.top  = (dragObj.elStartTop  + y - dragObj.cursorStartY) + "px";
  
  if(ie4) {
    window.event.cancelBubble = true;
    window.event.returnValue = false;
  }
  if(ns4|ns6)
    event.preventDefault();
}

function dragStop(event) {
  if(ie4) {
    document.detachEvent("onmousemove", dragGo);
    document.detachEvent("onmouseup",   dragStop);
  }
  if(ns4|ns6) {
    document.removeEventListener("mousemove", dragGo,   true);
    document.removeEventListener("mouseup",   dragStop, true);
  }
}
