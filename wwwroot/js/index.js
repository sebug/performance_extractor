/*global $*/
(function ($) {
'use strict';

var maxStart = 1000; // TODO: Actually get the number of log files found with a second call

function updateBuckets(buckets, item) {
	if (!buckets || !item.totalMs) {
		return;
	}

	var maxLowerKey;
	Object.keys(buckets).forEach(function (k) {
		if (item.totalMs > Number(k)) {
			if (!maxLowerKey || maxLowerKey < Number(k)) {
				maxLowerKey = k;
			}
		}
	});

	if (!maxLowerKey) {
		maxLowerKey = "more";
	}

	if (!buckets[maxLowerKey]) {
		buckets[maxLowerKey] = 1;
	} else {
		buckets[maxLowerKey] += 1;
	}
}

function getEntries(entriesBefore, i, buckets, end, entryCallback) {
	var res;
	if (i >= end) {
		res = new $.Deferred();
		res.resolve(buckets);
		return res.promise();
	}
	return $.ajax({
		url: 'api/PerformanceCounter?start=' + i
	}).then(function (r) {
		if (r) {
			r.forEach(function (itm) {
				entriesBefore.push(itm);
				updateBuckets(buckets, itm);
			});
		}
		if (entryCallback) {
			entryCallback();
		}
		return getEntries(entriesBefore, i + 1, buckets, end, entryCallback);
	}).then(null, function () {
		console.log("Error at batch " + i);
		if (entryCallback) {
			entryCallback();
		}
		return getEntries(entriesBefore, i + 1, buckets, end, entryCallback);
	});
}

$(document).ready(function () {
	$('#progressbar').progressbar({
		value: 0
	});
	var commonList = [];
	var commonBucket = {
		"0": 0,
		"100": 0,
		"1000": 0,
		"10000": 0,
		"60000": 0,
		"120000": 0,
		"180000": 0,
		"more": 0
	};
	var invocationCount = 0;
	function progressBarCallback() {
		invocationCount += 1;
		$('#progressbar').progressbar("option","value", 100 * invocationCount / maxStart);
	}


	var promises = [];
	var i;
	for (i = 0; i < maxStart; i += 100) {
		promises.push(getEntries(commonList, i, commonBucket, i + 100, progressBarCallback));
	}
	'
	$.when.apply($, promises).then(function () {
		console.log(commonBucket);
	});
});
	
}($));
