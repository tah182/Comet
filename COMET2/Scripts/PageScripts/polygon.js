'use strict';

onmessage = function (e) {
    var xmlhttp = new XMLHttpRequest();

    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var result = JSON.parse(xmlhttp.responseText);
            var polyArray = [];
            for (var i = 0; i < result.length; i++) {
                var zipLine = [];

                for (var j = 0; j < result[i].Coordinates.length; j++) {
                    zipLine.push({
                        lat: result[i].Coordinates[j].Lat,
                        lng: result[i].Coordinates[j].Lng
                    });
                }

                polyArray.push({
                    name: result[i].Name,
                    color: result[i].Color,
                    line: zipLine,
                    centroid: { lat: result[i].Centroid.Lat, lng: result[i].Centroid.Lng }
                });
            }
            postMessage(polyArray);
        }
    }

    var queryString = '?';
    for (var key in e.data.data)
        queryString += key + '=' + e.data.data[key] + '&';
    queryString = queryString.slice(0, queryString.length - 1);

    //console.log(queryString);
    
    xmlhttp.open("GET", "/Rest/Json" + e.data.type.charAt(0).toUpperCase() + e.data.type.slice(1) + queryString, true);
    xmlhttp.setRequestHeader("Content-type", "application/json");
    xmlhttp.send();
}