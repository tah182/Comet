'use strict';

onmessage = function (e) {
    var xmlhttp = new XMLHttpRequest();

    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var result = JSON.parse(xmlhttp.responseText);
            
            postMessage(result);
        }
    }

    var queryString = '?';
    for (var key in e.data)
        if (key !== "controller")
            queryString += key + '=' + e.data[key] + '&';
    queryString = queryString.slice(0, queryString.length - 1);

    //console.log(queryString);
    xmlhttp.open("GET", e.data.controller + queryString, true);
    xmlhttp.setRequestHeader("Content-type", "application/json");
    xmlhttp.send();
}