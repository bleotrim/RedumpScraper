function changeDiscStatus(id) {
	ajaxRequest('http://redump.org/disc/'+id+'/mydisc/', '', changeDiscStatusCallback);
}

function changeDiscStatusCallback(ajax) {
	if (ajax.responseXML.getElementsByTagName('response')[0].getElementsByTagName('status')[0].lastChild.nodeValue == 0) {
		var data = ajax.responseXML.getElementsByTagName('response')[0].getElementsByTagName('data')[0];
		document.getElementById('ad'+data.getElementsByTagName('id')[0].lastChild.nodeValue).innerHTML = (data.getElementsByTagName('text')[0].lastChild.nodeValue == 'have') ? 'Remove from my discs' : 'Add to my discs';
	}
}
