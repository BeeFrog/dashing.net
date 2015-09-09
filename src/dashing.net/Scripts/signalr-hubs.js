(function ($) {
	$.ajax({
		url: "/signalr/hubs",
		dataType: "script",
		async: false,
		cache: true
	});

	$.connection.hub.disconnected(function () {
		setTimeout(function () {
			$.connection.hub.start();
		}, 5000); // Restart connection after 5 seconds.
	});

	$.connection.hub.start();
}(jQuery));

