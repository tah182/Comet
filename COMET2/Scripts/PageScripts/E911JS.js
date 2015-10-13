/*@cc_on@*/

var iconURLPrefix = 'http://maps.google.com/mapfiles/ms/icons/';

var MapColors = {
    yellow: "#F2FA00",
    white: "#eee",
    black: "#111",
    blue: "#0000FF",
    purple: "#4000BF",
    orange: "#FF7700",
    darkWhite: "#ddd",
    lightBlack: "#333",
    red: "#FF0000"
}

var BoundaryType = {
    RATECENTER: "rateCenter",
    RATECENTERLABEL: "rateCenterLabel",
    PSAP: "psap",
    COUNTY: "county",
    COUNTYLABEL: "countyLabel",
    LATA: "lata"
};

var Coordinate = function(lat, lng) {
    this.Lat = lat;
    this.Lng = lng;
}

var Boundary = function(array) {
    this.Coordinate = [];
    for(var i = 0; i < array.length; i++)
        this.Coordinate.push(Coordinate.call(this, array.lat, array.lng));
}

function indexAjax(funcName, updateLocation, data, references) {
    "use strict";
    var mapControllerPath;
    var PAGE_NAME = "E911";
    var errorPanel = "#map-canvas";
    var insert = new ErrorInsert(PAGE_NAME, funcName);
    var nullData, nullContentType, nullBeforeSend;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && typeof data === "object") ? data : null

    switch (references) {
        case BoundaryType.RATECENTERLABEL:
        case BoundaryType.RATECENTER:
            mapControllerPath = "./E911/JsonRcBoundaries";
            break;
        case BoundaryType.PSAP:
            mapControllerPath = "./E911/JsonPsapBoundaries";
            break;
        case BoundaryType.COUNTYLABEL:
        case BoundaryType.COUNTY:
            mapControllerPath = "./E911/JsonCountyBoundaries";
            break;
        case BoundaryType.LATA:
            mapControllerPath = "./E911/JsonLataBoundaries";
            break;
        default:
            mapControllerPath = "";
    }
    if (updateLocation !== null && typeof updateLocation === "string") {
        errorPanel = updateLocation;
    }

    function getExpiration () {
        var date = new Date();
        date.setDate(date.getDate() + 1)
        date.setHours(8, 0, 0, 0);
        return date;
    }

    function saveToLocalStorage(key, data) {
        if (typeof key !== "string" || typeof data !== "object")
            return null;

        localStorage.setItem(key, { "data": data, "expiration": getExpiration() });
    }

    function mapFunctions(type) {
        return {
            SUCCESS: function (result) {
                // remove from swcMarkers if not in result
                removeFromMarkers(result, type);
                // add if result not in swcMarkers
                addNewMarkers(result, type);
                showMarkers(type);
            },
            FAILURE: function (request, status, error) {
                insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState === 4) insert.ErrorCode = "c-e-j-" + type + "-01";
                if (request.readyState === 2) insert.ErrorCode = "c-e-j-" + type + "-02";
                insert.logError(errorPanel);
            }
        }
    }
    
    return {
        OnMap: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath,
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                mapFunctions(references).SUCCESS,
                mapFunctions(references).FAILURE),
        LocDetails: ajaxCall(ajaxDetails().HttpType.GET,
                "./E911/LocationDetails",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                function () {
                    $(updateLocation).html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
                },
                function (result) {
                    $(updateLocation).html(result).parent().removeClass("minimized").fadeIn(500).find(".title").fadeOut(0);
                    $("#map-right").bind("mouseenter", function () {
                        $(updateLocation).parent().addClass("minimized").find(".title").css("display", "");
                        $(this).unbind("mouseenter");
                    });
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) { insert.ErrorCode = "c-e-j-ld-01"; }
                    if (request.readyState === 2) { insert.ErrorCode = "c-e-j-ld-02"; }
                    insert.logError();
                }),
        LocInfo: ajaxCall(ajaxDetails().HttpType.GET,
                "./E911/LocationDetails",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    var iWindow = references.clickMarker === true ? clickInfowindow : infowindow;

                    iWindow.close();
                    var content = "<div style='font-size: 0.9em; width: 260px; height: 300px;'><strong>" + references.name + "</strong>"
                                + "<p style='font-size: 0.83em'>" + result
                                + "</p></div>";
                    iWindow.setContent(content);
                    iWindow.open(map, references.marker);
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-e-j-ld-01";
                    if (request.readyState === 2) insert.ErrorCode = "c-e-j-ld-02";
                    insert.logError();
                }),
        Centroid: ajaxCall(ajaxDetails().HttpType.GET,
                "./E911/ProjectCentroid",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    getProjectInfo(result, references);
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-e-j-c-01";
                    if (request.readyState === 2) insert.ErrorCode = "c-e-j-c-02";
                    insert.logError();
                })
    }
}

function PageMap() {
    this.polygonMarkers = [];
    this.map;
};

PageMap.prototype = {
    clearMarkers: function(boundaryType) {
        if (this.boundaryType === null || typeof this.boundaryType === "undefined")
            return;

        var i = this.polygonMarkers[boundaryType].length - 1;
        for (; i >= 0; i--) {
            // check if the object has a label that needs to be removed as well.
            if (this.polygonMarkers[boundaryType][i].label !== null && typeof this.polygonMarkers[boundaryType][i].label !== "undefined")
                this.polygonMarkers[boundaryType][i].label.setMap(null);

            // remove the object polygon from the map
            this.polygonMarkers[boundaryType][i].polygon.setMap(null);
        }
        this.polygonMarkers[boundaryType] = [];
    },
    addMarkers: function(boundaryType, polygon, label) {
        if (this.boundaryType === null || typeof this.boundaryType === "undefined")
            return;

        if (this.polygonMarkers[boundaryType] !== null && typeof this.polygonMarkers[boundaryType] !== "undefined")
            clearMarkers(boundaryType);

        var boundaryObject = new {
            polygon: polygon,
            label: label
        }

        polygon.setMap(this.map);
        if (label !== null && typeof label !== "undefined")
            label.setMap(this.map);

        this.polygonMarkers[boundaryType].push(boundaryObject);
    }
}


var mapListener;
var localStorageAllowed;
var countyMarkers = new Array();
var rateCenterMarkers = new Array();
var psapMarkers = new Array();
var lataMarkers = new Array();
var highlight = new Array();
var marker, clickMarker;
var map;
var infowindow, clickInfowindow;
var searchBox;

var djConfig = {
    parseOnLoad: true,
    packages: [{
        name: "agsjs",
        location: 'https://gmaps-utility-gis.googlecode.com/svn/tags/agsjs/2.04/xbuild/agsjs' // for xdomain load
    }]
};

// ***************** Initial page Load *************************** //
// Calls: loadEnd, maps.Listener.searchProviders
// Returns: 
// Sets: multiStates selected
//      multiSelect ui
$(document).ready(function () {
    "use strict";
    var ver = getInternetExplorerVersion();
    if (ver > -1 && ver < 10.0)
        alert("Your are using an outdated version of IE. Comet recommends that you use IE10, Chrome, or Firefox.");

    // check for local storage allowed in HTML5
    localStorageAllowed = supportsHtml5Storage();
    
    $("#lat-input, #lng-input").keypress(function (e) {
        if (e.which == 13)
            searchLatLng();
    });

    $("#MinMaxSearch").click(function () {
        minMaxParent(this);
    });

    // ----------- creates google map-----------------
    map = new google.maps.Map(document.getElementById('map-canvas'), {
        center: new google.maps.LatLng(38.850033, -95.6500523),
        zoom: 5,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_BOTTOM,
            style: google.maps.ZoomControlStyle.SMALL
        },
        panControl: false,
        scaleControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP  // HYBRID
    }); // end map creation
    infowindow = new google.maps.InfoWindow();
    clickInfowindow = new google.maps.InfoWindow();

    var input = document.getElementById("pac-input");
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    searchBox = new google.maps.places.Autocomplete(input);//, options);

    google.maps.event.addListener(searchBox, "place_changed", function () {
        searchProviders();
    });
    
    $("#project-input").autocomplete({
        delay: 500,
        focus: function (event, ui) {
            event.preventDefault();
            var split = ui.item.label.split(": ");
            $(this).val(split[1].replace("<mark>", "").replace("</mark>", ""));
        },
        source: function (request, response) {
            var data = { "search": $("#project-input").val() };
            $.get("./E911/ShowProject", data
                , function (result) {
                    var results = [];
                    for (var i = 0; i < result.length; i++) {
                        results.push({
                            "label": result[i][0],
                            "value": result[i][1],
                            "id": i,
                        });
                    }
                    response(results);
                });
        },
        select: function (event, ui) {
            event.preventDefault();
            var split = ui.item.label.split(": ");
            var type = split[0].replace("<b>", "").replace("</b>", "").replace("<u>", "").replace("</u>",  "");
            var search = split[1].replace("<mark>", "").replace("</mark>", "");
            $(this).val(search);
            searchProject(type, ui.item.value);
        },
        open: function () {
            $(".ui-autocomplete").css({
                //"max-width": "320px",
                "background-color": "#FFFFFF",
                "font-size": "0.7em",
                "max-height": "350px",
                "overflow-y": "scroll",
                "overflow-x": "hidden",
                "width": ($(this).width() + 5) + "px"
            });
            $(".ui-autocomplete li").css({
                outline: "1px"
            });
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        return $("<li title='" + item.title + "'>")
            .data("ui-autocomplete-item", item)
            .append($("<a>").html(item.label))
            .appendTo(ul);
    };

    google.maps.event.addListener(map, "idle", function () {
        var mapMoveTimer;
        var moveDetect = google.maps.event.addListener(map, "bounds_changed", function () {
            clearTimeout(mapMoveTimer);
        });

        mapMoveTimer = setTimeout(function () {
            if (map.getZoom() >= 10) {
                $("#CountyPolyShowHideMap").prop("disabled", false);
                if ($("#CountyPolyShowHideMap").prop("checked"))
                    getCounty($("#CountyPolyShowHideMap"));

                $("#CountyLabelShowHideMap").prop("disabled", false);

                $("#RateCenterPolyShowHideMap").prop("disabled", false);
                if ($("#RateCenterPolyShowHideMap").prop("checked"))
                    getRateCenter($("#RateCenterPolyShowHideMap"));

                $("#RateCenterLabelShowHideMap").prop("disabled", false);

                $("#PsapPolyShowHideMap").prop("disabled", false);
                if ($("#PsapPolyShowHideMap").prop("checked"))
                    getPsap($("#PsapPolyShowHideMap"));

                $("#LataPolyShowHideMap").prop("disabled", false);
                if ($("#LataPolyShowHideMap").prop("checked"))
                    getLata($("#LataPolyShowHideMap"));

                if (mapListener === null || typeof mapListener === "undefined")
                    mapListener = google.maps.event.addListener(map, "click", function (event) {
                        getClickDetails(event);
                    });
            } else {
                $("#CountyPolyShowHideMap").prop("disabled", true);
                $("#CountyLabelShowHideMap").prop("disabled", true);
                for (var i = 0; i < countyMarkers.length; i++) {
                    countyMarkers[i].polygon.setMap(null);
                    countyMarkers[i].label.setMap(null);
                }

                $("#RateCenterPolyShowHideMap").prop("disabled", true);
                $("#RateCenterLabelShowHideMap").prop("disabled", true);
                for (var i = 0; i < rateCenterMarkers.length; i++) {
                    rateCenterMarkers[i].polygon.setMap(null);
                    rateCenterMarkers[i].label.setMap(null);
                }

                $("#PsapShowHideMap").prop("disabled", true);
                for (var i = 0; i < psapMarkers.length; i++)
                    psapMarkers[i].polygon.setMap(null);

                $("#LataShowHideMap").prop("disabled", true);
                for (var i = 0; i < lataMarkers.length; i++)
                    lataMarkers[i].polygon.setMap(null);

                google.maps.event.clearListeners(map, "click");
                
                google.maps.event.removeListener(moveDetect);
                searchBox.setBounds(map.getBounds());
            }
        }, 500);
    }); // end bounds_changed eventlistener


    // bind a click event that allows the details div to stay up
    $("#selectDetail").bind("click", function () {
        if ($(this).hasClass("SearchResultDivDisplay"))
            $(this).removeClass("SearchResultDivDisplay");
        else
            $(this).addClass("SearchResultDivDisplay");
    });
});



// ***************** Search Project method ************************ //
// Calls: 
// Returns: 
// Sets: 
function searchProject(type, search) {
    var funcName = trimFuncName(arguments.callee.toString());
    var data = {
        "searchType": type,
        "searchString": search
    };
    runAjax(indexAjax(funcName, "#map-canvas", data, type).Centroid);
}


// ***************** Search LatLng method ************************ //
// Calls: 
// Returns: 
// Sets: 
function searchLatLng() {
    var lat = $("#lat-input").val();
    var lng = $("#lng-input").val();

    // This costs geoCoding passes
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': new google.maps.LatLng(lat, lng) }, function (results, status) {
        var zip, name;
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[2]) 
                name = results[2].formatted_address;
            for (var i = 0; i < results.length; i++) {
                for (var k = 0; k < results[i].address_components.length; k++) {
                    if (results[i].address_components[k].types[0] === "postal_code")
                        zip = results[i].address_components[0].short_name;
                }
            }
        } else {
            console.log(status);
        }

        var place = {
            "name": ("Lat:" + lat + " - Lng:" + lng),
            "address_components": [{
                "short_name": zip,
                "types": [
                    "postal_code"
                ]
            }]
        }
        getDetails(place, lat, lng, true);
    });
}


// ***************** Search Providers method ************************ //
// Calls: 
// Returns: 
// Sets: 
function searchProviders() {
    var place = searchBox.getPlace();
    getDetails(place, place.geometry.location.lat(), place.geometry.location.lng(), true);
}

function setMapNull() {
    if (marker !== null && typeof marker !== "undefined")
        marker.setMap(null);
    for (i = 0; i < highlight.length; i++)
        highlight[i].setMap(null);
    highlight = new Array();
}

function getClickDetails(event) {
    if (clickMarker !== null && typeof clickMarker !== "undefined")
        clickMarker.setMap(null);
    
    var lat = event.latLng.lat();
    var lng = event.latLng.lng();
    var funcName = trimFuncName(arguments.callee.toString());
    clickMarker = createMarker("Lat:" + lat + " Lng:" + lng, lat, lng, iconURLPrefix + "blue-dot.png", 2, false);

    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': new google.maps.LatLng(lat, lng) }, function (results, status) {
        var name, zip;
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[2]) 
                name = results[2].formatted_address;
            for (var i = 0; i < results.length; i++) {
                for (var k = 0; k < results[i].address_components.length; k++) {
                    if (results[i].address_components[k].types[0] === "postal_code")
                        zip = results[i].address_components[0].short_name;
                }
            }
        } else 
            console.log(status);

        var data = {
            "lat": lat,
            "lng": lng,
            "zip": zip,
            "showIcon": false
        };
        var reference = {
            "name": name,
            "marker": clickMarker,
            "clickMarker": true
        }
        runAjax(indexAjax(funcName, null, data, reference).LocInfo);
    });
}
 
function getDetails(place, lat, lng, zoom) {
    setMapNull();

    var funcName = trimFuncName(arguments.callee.toString());
    marker = createMarker("Searched Location", lat, lng, null, 2, false);
    
    var zip;
    for (var i = 0; i < place.address_components.length; i++) {
        if (place.address_components[i].types[0] === "postal_code")
            zip = place.address_components[i].short_name;
    }

    google.maps.event.addListener(marker, "click", (function (marker) {
        return function () {
            var data = {
                "lat": lat,
                "lng": lng,
                "zip": zip,
                "showIcon": false
            };
            var reference = {
                "name": place.name,
                "marker": marker,
                "clickMarker": false
            }
            runAjax(indexAjax(funcName, null, data, reference).LocInfo);
        }   // end return
    })(marker));    // end evenListener

    var bounds = new google.maps.LatLngBounds();
    bounds.extend(new google.maps.LatLng(lat, lng));
    if (zoom) {
        map.fitBounds(bounds);
        setTimeout(function () {
            var listener = google.maps.event.addListener(map, "idle", function () {
                if (map.getZoom() > 14)
                    map.setZoom(14);
            }); // end listener
            google.maps.event.removeListener(listener);
        }, 15); // end setTimeout
    }

    var data = {
        "lat": lat,
        "lng": lng,
        "zip": zip,
        "showIcon": true
    };
    runAjax(indexAjax(funcName, $("#selectDetail .content"), data, null).LocDetails);
}


// ***************** getProjectInfo Method ************************ //
// Calls: 
// Returns: 
// Sets: 
function getProjectInfo(result, references) {
    setMapNull();

    if (result.length > 0) {
        var color = MapColors.white;
        switch (references.toLowerCase()) {
            case "fips":
            case "counties":
                color = MapColors.black;
                break;
            case "rate center":
                color = MapColors.red;
                break;
            case "fcc id":
            case "psap agency":
                color = MapColors.blue;
                break;
        }
        var boundary = new google.maps.LatLngBounds();
        for (var i = 0; i < result.length; i++) {
            for (var j = 0; j < result[i].Polygon.length; j++) {
                var bounds = new Array();
                for (var l = 0; l < result[i].Polygon[j].Coordinates.length; l++) {
                    var latLng = new google.maps.LatLng(result[i].Polygon[j].Coordinates[l].Lat,
                                                       result[i].Polygon[j].Coordinates[l].Lng);
                    bounds.push(latLng);
                    boundary.extend(latLng);
                }
                var polygon = new google.maps.Polygon({
                    path: bounds,
                    strokeColor: color,
                    fillColor: color,
                    strokeWeight: 2,
                    strokeOpacity: 0.8,
                    fillOpacity: 0.2,
                    zIndex: 0,
                    map: map
                });
                google.maps.event.addListener(polygon, "click", function (event) {
                    if (!event.alreadyCalled_)
                        getClickDetails(event);
                });
                highlight.push(polygon);
            }
        }
        map.fitBounds(boundary);
        if (map.getZoom() > 10) {
            $("#selectDetail .content")
                .html("Please click on map to get full details.")
                .parent()
                .removeClass("minimized")
                .fadeIn(500)
                .find(".title")
                .fadeOut(0);
            $("#map-right").bind("mouseenter", function () {
                $("#selectDetail").addClass("minimized").find(".title").css("display", "");
                $(this).unbind("mouseenter");
            });
        } else {
            $("#selectDetail .content")
                .html("Please zoom in and click on map to get full details")
                .parent()
                .removeClass("minimized")
                .fadeIn(500)
                .find(".title")
                .fadeOut(0);
            $("#map-right").bind("mouseenter", function () {
                $("#selectDetail").addClass("minimized").find(".title").css("display", "");
                $(this).unbind("mouseenter");
            });
        }
    } else
        alert("We're sorry, but we were unable to locate this project on a map.");
}



// ***************** getCounty Method ************************ //
// Calls: 
// Returns: 
// Sets: 
function getCounty(id) {
    var type = $(id).attr("id") === "CountyPolyShowHideMap" ? BoundaryType.COUNTY : BoundaryType.COUNTYLABEL;
    if ($(id).prop("checked")) {
        if (type === BoundaryType.COUNTYLABEL)
            $("#CountyPolyShowHideMap").prop("checked", true);
        getBoundary(type);
    } else {
        if (type === BoundaryType.COUNTY)
            $("#CountyLabelShowHideMap").prop("checked", false);
        showMarkers(type);
    }
}



// ***************** getRateCenter Method ************************ //
// Calls: 
// Returns: 
// Sets: 
function getRateCenter(id) {
    var type = $(id).attr("id") === "RateCenterPolyShowHideMap" ? BoundaryType.RATECENTER : BoundaryType.RATECENTERLABEL;
    if ($(id).prop("checked")) {
        if (type === BoundaryType.RATECENTERLABEL)
            $("#RateCenterPolyShowHideMap").prop("checked", true);
        getBoundary(type);
    } else {
        if (type === BoundaryType.RATECENTER)
            $("#RateCenterLabelShowHideMap").prop("checked", false);
        showMarkers(type);
    }
}


// ***************** getLata Method ************************ //
// Calls: 
// Returns: 
// Sets: 
function getLata(id) {
    if ($(id).prop("checked"))
        getBoundary(BoundaryType.LATA);
    else showMarkers(BoundaryType.LATA);
}



// ***************** getPsap Method ************************ //
// Calls: 
// Returns: 
// Sets: 
function getPsap(id) {
    if ($(id).prop("checked"))
        getBoundary(BoundaryType.PSAP);
    else showMarkers(BoundaryType.PSAP);
}

function getBoundary(boundary) {
    if (boundary !== null && typeof boundary === "string") {
        var bounds = map.getBounds();

        var funcName = trimFuncName(arguments.callee.toString());
        var data = {
            "bottomLeftLat": bounds.getSouthWest().lat().toFixed(6), "bottomLeftLng": bounds.getSouthWest().lng().toFixed(6),
            "topRightLat": bounds.getNorthEast().lat().toFixed(6), "topRightLng": bounds.getNorthEast().lng().toFixed(6)
        }
        runAjax(indexAjax(funcName, "#map-canvas", data, boundary).OnMap);
    }
}

function removeFromMarkers(result, type) {
    switch (type) {
        case BoundaryType.COUNTYLABEL:
        case BoundaryType.COUNTY :
            for (var i = 0; i < countyMarkers.length; i++) {
                var found = $.grep(result, function (a) { return a.ObjectID === countyMarkers[i].text; });
                if (found.length === 0) {
                    countyMarkers[i].polygon.setMap(null);
                    countyMarkers[i].label.setMap(null);
                    countyMarkers[i] = null;
                    countyMarkers.splice(i, 1);
                }
            }
            break;
        case BoundaryType.RATECENTERLABEL:
        case BoundaryType.RATECENTER :
            for (var i = 0; i < rateCenterMarkers.length; i++) {
                var found = $.grep(result, function (a) { return a.ID === rateCenterMarkers[i].text; });
                if (found.length === 0) {
                    rateCenterMarkers[i].polygon.setMap(null);
                    rateCenterMarkers[i].label.setMap(null);
                    rateCenterMarkers[i] = null;
                    rateCenterMarkers.splice(i, 1);
                }
            }
            break;
        case BoundaryType.PSAP :
            for (var i = 0; i < psapMarkers.length; i++) {
                var found = $.grep(result, function (a) { return a.ObjectID === psapMarkers[i].text; });
                if (found.length === 0) {
                    psapMarkers[i].polygon.setMap(null);
                    psapMarkers[i] = null;
                    psapMarkers.splice(i, 1);
                }
            }
            break;
        case BoundaryType.LATA:
            for (var i = 0; i < lataMarkers.length; i++) {
                var found = $.grep(result, function (a) { return a.ObjectID === lataMarkers[i].text; });
                if (found.length === 0) {
                    lataMarkers[i].polygon.setMap(null);
                    lataMarkers[i] = null;
                    lataMarkers.splice(i, 1);
                }
            }
            break;
    }
    
}
function addNewMarkers(result, type) {
    switch (type) {
        case BoundaryType.COUNTYLABEL:
        case BoundaryType.COUNTY:
            for (var i = 0; i < result.length; i++) {
                var found = $.grep(countyMarkers, function (a) { return a.text === result[i].ObjectID; });
                if (found.length === 0) {
                    for (var k = 0; k < result[i].Polygon.length; k++)
                        countyMarkers.push(createShape(result[i].Polygon[k], type, result[i].ObjectID, result[i].Name));
                }
            }
            break;
        case BoundaryType.PSAP:
            for (var i = 0; i < result.length; i++) {
                var found = $.grep(psapMarkers, function (a) { return a.text === result[i].ObjectID; });
                if (found.length === 0) {
                    for (var k = 0; k < result[i].Polygon.length; k++)
                        psapMarkers.push(createShape(result[i].Polygon[k], type, result[i].ObjectID, null));
                }
            }
            break;
        case BoundaryType.RATECENTERLABEL:
        case BoundaryType.RATECENTER:
            for (var i = 0; i < result.length; i++) {
                var found = $.grep(rateCenterMarkers, function (a) { return a.text === result[i].ID; });
                if (found.length === 0) {
                    for (var k = 0; k < result[i].Polygon.length; k++)
                        rateCenterMarkers.push(createShape(result[i].Polygon[k], type, result[i].ID, result[i].Name));
                }
            }
            break;
        case BoundaryType.LATA:
            for (var i = 0; i < result.length; i++) {
                var found = $.grep(lataMarkers, function (a) { return a.text === result[i].ObjectID; });
                if (found.length === 0) {
                    for (var k = 0; k < result[i].Polygon.length; k++)
                        lataMarkers.push(createShape(result[i].Polygon[k], type, null));
                }
            }
            break;
    }
}

function createShape(result, type, id, labelName) {
    var color = MapColors.white;
    switch (type) {
        case BoundaryType.COUNTYLABEL:
        case BoundaryType.COUNTY:
            color = MapColors.black;
            break;
        case BoundaryType.RATECENTERLABEL:
        case BoundaryType.RATECENTER:
            color = MapColors.red;
            break;
        case BoundaryType.PSAP:
            color = MapColors.blue;
            break;
        case BoundaryType.LATA:
            color = MapColors.pink;
            break;
    }

    var bounds = new Array();
    for (var l = 0; l < result.Coordinates.length; l++)
        bounds.push(new google.maps.LatLng(result.Coordinates[l].Lat,
                                                result.Coordinates[l].Lng));

    var polygon;
    if (type === BoundaryType.PSAP || type === BoundaryType.LATA)
        polygon = {
            polygon: new google.maps.Polyline({
                path: bounds,
                strokeColor: color,
                fillColor: color,
                strokeWeight: 2,
                strokeOpacity: 0.8,
                fillOpacity: 0.8,
                zIndex: 1
            }),
            text: id
        };
    else
        polygon = {
            polygon: new google.maps.Polyline({
                path: bounds,
                strokeColor: color,
                fillColor: color,
                strokeWeight: 2,
                strokeOpacity: 0.8,
                fillOpacity: 0.8,
                zIndex: 1
            }),
            text: id,
            label: new MapLabel({
                text: labelName.toUpperCase(),
                position: new google.maps.LatLng(result.Centroid.Lat, result.Centroid.Lng),
                fontSize: 10,
                fontColor: color,
                labelInBackground: true,
                strokeColor: color,
                strokeWeight: 0.4
            })
        };
    return polygon;
}





// ************* show on Map method ****************** //
// Calls: 
// Returns: 
// Sets: 
function showMarkers(e) {
    if (e !== null && typeof e !== "undefined") {
        var id;
        switch (e) {
            case BoundaryType.COUNTYLABEL:
            case BoundaryType.COUNTY:
                id = "#CountyPolyShowHideMap";
                var checked = $(id).prop("checked");
                id = "#CountyLabelShowHideMap";
                var labelChecked = $(id).prop("checked");
                for (var i = 0; i < countyMarkers.length; i++) {
                    if (!((countyMarkers[i].polygon.getMap() === map && checked) || (countyMarkers[i].polygon.getMap() === null && !checked)))
                        countyMarkers[i].polygon.setMap(checked ? map : null);
                    if (!((countyMarkers[i].label.getMap() === map && labelChecked) || (countyMarkers[i].label.getMap() === null && !labelChecked)))
                        countyMarkers[i].label.setMap(labelChecked ? map : null);
                }
                break;
            case BoundaryType.PSAP:
                id = "#PsapPolyShowHideMap";
                var checked = $(id).prop("checked");
                for (var i = 0; i < psapMarkers.length; i++) {
                    if (!((psapMarkers[i].polygon.getMap() === map && checked) || (psapMarkers[i].polygon.getMap() === null && !checked))) 
                        psapMarkers[i].polygon.setMap(checked ? map : null);
                }
                break;
            case BoundaryType.RATECENTERLABEL:
            case BoundaryType.RATECENTER:
                id = "#RateCenterPolyShowHideMap";
                var checked = $(id).prop("checked");
                id = "#RateCenterLabelShowHideMap";
                var labelChecked = $(id).prop("checked");
                for (var i = 0; i < rateCenterMarkers.length; i++) {
                    if (!((rateCenterMarkers[i].polygon.getMap() === map && checked) || (rateCenterMarkers[i].polygon.getMap() === null && !checked)))
                        rateCenterMarkers[i].polygon.setMap(checked ? map : null);
                    if (!((rateCenterMarkers[i].label.getMap() === map && labelChecked) || (rateCenterMarkers[i].label.getMap() === null && !labelChecked)))
                        rateCenterMarkers[i].label.setMap(labelChecked ? map : null);
                }
                break;
            case BoundaryType.LATA:
                id = "#LataPolyShowHideMap";
                var checked = $(id).prop("checked");
                for (var i = 0; i < lataMarkers.length; i++) {
                    if (!((lataMarkers[i].polygon.getMap() === map && checked) || (lataMarkers[i].polygon.getMap() === null && !checked)))
                        lataMarkers[i].polygon.setMap(checked ? map : null);
                }
            default:
                break;
        }
    }
}



// ***************** shows/hides searchable fields ************************ //
// Calls: ***DEPRECATED***
// Returns: 
// Sets: 
function searchTableSelect(field) {
    var fadein = function (element, field) {
        that = $("#" + field.id + "SearchDiv");
        $(element).css("display", "none");
        that.css("display", "block");

        setTimeout(function () {
            that.animate({ "opacity": 1 }, 500).addClass("selected");
        }, 10);

        $("#searchCriteria").children("span").each(function () {
            $(this).attr("onclick", "searchTableSelect(this)");
        });
    };
    $(".tab").each(function () {
        $(this).removeClass("selected");
    })
    $("#" + field.id).addClass("selected");
    $("#searchCriteria").children("span").each(function () {
        $(this).attr("onclick", "");
    });
    $("#searchMethods").children().each(function () {
        if ($(this).hasClass("selected"))
            $(this).animate({
                "opacity": 0
            }, 500, null, fadein(this, field)).removeClass("selected");
    });
}


function createMarker(id, lat, lng, icon, index, draggable) {
    //var image = {
    //    url: iconURLPrefix + "red-dot.png",
    //    size: new google.maps.Size(71, 71),
    //    origin: new google.maps.Point(0, 0),
    //    anchor: new google.maps.Point(0, 34),
    //    scaledSize: new google.maps.Size(25, 25)
    //};  // end image

    return new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        map: map,
        icon: icon === null ? iconURLPrefix + "red-dot.png" : icon,
        title: id.toString(),
        id: id,
        zIndex: index,
        draggable: draggable
    }); 
}



