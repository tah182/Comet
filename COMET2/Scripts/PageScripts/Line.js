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
                    vendor: result[i].Name,
                    color: result[i].Color,
                    line: zipLine
                });
            }
            postMessage(polyArray);
        }
    }

    var queryString = '?';
    for (var key in e.data) 
        queryString += key + '=' + e.data[key] + '&';
    queryString = queryString.slice(0, queryString.length - 1);

    //console.log(queryString);
    xmlhttp.open("GET", "/Rest/FiberPoints" + queryString, true);
    xmlhttp.setRequestHeader("Content-type", "application/json");
    xmlhttp.send();
}