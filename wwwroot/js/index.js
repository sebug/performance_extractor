/*global $*/
(function ($) {
'use strict';

var maxStart = 200; // TODO: Actually get the number of log files found with a second call

function getEntries(entriesBefore, i) {
	var res;
	if (i >= maxStart) {
		res = new $.Deferred();
		res.resolve(entriesBefore);
		return res.promise();
	}
	return $.ajax({
		url: 'api/PerformanceCounter?start=' + i
	}).then(function (r) {
		if (r) {
			r.forEach(function (itm) {
				entriesBefore.push(itm);
			});
		}
		$('#progressbar').progressbar("option","value", 100 * i / maxStart);
		return getEntries(entriesBefore, i + 1);
	});
}

$(document).ready(function () {
	$('#progressbar').progressbar({
		value: 0
	});
	getEntries([], 0).then(function (allEntries) {
		console.log(allEntries);
	});
});
	
}($));
