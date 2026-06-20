// Simple JavaScript for "Use My Location" button
$(function () {
    var $locationBtn = $("#locationBtn");
    var $loading = $("#loadingMessage");
    var $cityInput = $("#cityInput");

    $locationBtn.on("click", function () {
        if (!navigator.geolocation) {
            alert("Your browser does not support location services.");
            return;
        }

        $loading.show();

        navigator.geolocation.getCurrentPosition(
            function (position) {
                var lat = position.coords.latitude;
                var lon = position.coords.longitude;
                window.location.href = "/Home/Index?lat=" + lat + "&lon=" + lon;
            },
            function () {
                alert("Location access denied. Please allow location or search by city name.");
                $loading.hide();
            }
        );
    });

    $("#searchForm").on("submit", function () {
        if ($cityInput.val().trim() !== "") {
            $loading.show();
        }
    });
});
