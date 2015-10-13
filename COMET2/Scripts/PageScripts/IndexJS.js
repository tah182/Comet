/// <reference path="Global.js" />
/*@cc_on@*/
//google.load('visualization', '1', { 'packages': ['corechart', 'table', 'geomap'] });

var iconURLPrefix = 'http://maps.google.com/mapfiles/ms/icons/';
var icons = [
    "../../Images/CircleFire.png",
    "../../Images/CircleBlue.png",
    "../../Images/CircleGreen.png",
    "../../Images/CirclePurple.png",
    "../../Images/CircleRed.png",
    "../../Images/CircleYellow.png",
    "../../Images/CircleBrown.png",
    "../../Images/CircleGrey.png",
    "../../Images/CircleOrange.png",
    "../../Images/CircleForest.png",
    "../../Images/CirclePink.png",
    "../../Images/CircleBlue1.png",
    "../../Images/CircleTeal.png",
    "../../Images/CirclePurple1.png",
    "../../Images/CircleExtra.png"
];

var locationIcons = [
    "../../Images/SSDP01.png",
    "../../Images/SSDP02.png",
    "../../Images/SSDP03.png",
    "../../Images/SSDP04.png",
    "../../Images/SSDP05.png",
    "../../Images/SSDP06.png",
    "../../Images/SSDP07.png",
    "../../Images/SSDP08.png",
    "../../Images/SSDP09.png",
    "../../Images/SSDP10.png",
    "../../Images/SSDP11.png",
    "../../Images/SSDP12.png",
    "../../Images/SSDP13.png",
    "../../Images/SSDP14.png",
    "../../Images/SSDP15.png"
];

var MapColors = {
    yellow:         "#F2FA00",
    white:          "#eee",
    black:          "#111",
    purple:         "#4000BF",
    orange:         "#FF7700",
    darkWhite:      "#ddd",
    lightBlack:     "#333",
    red:            "#FF0000"
}

var LatLng = function(lat, lng) {
    var lat = lat;
    var lng = lng;
    return {
        latlng: function() {
            return new { "lat": lat, "lng": lng };
        },
        lat: function() {
            return lat;
        },
        lng: function() {
            return lng;
        }
    };
}

var PAGE = PAGE || {};
var requestInfo;

function indexAjax(funcName, updateLocation, data, references) {
    var PAGE_NAME = "Index";
    var errorPanel = "#map-canvas";
    var insert = new ErrorInsert(PAGE_NAME, funcName);
    var nullContentType, nullBeforeSend, nullData;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && typeof data === "object") ? data : null

    if (updateLocation !== null && typeof updateLocation === "string")
        errorPanel = updateLocation;

    function getExpiration() {
        var date = new Date();
        date.setDate(date.getDate() + 1);
        date.setHours(8, 0, 0, 0);
        return date;
    }

    function saveToLocalStorage(key, data) {
        if (typeof key !== "string" || typeof data !== "object")
            return null;

        localStorage.setItem(key, { "data": data, "expiration": getExpiration() });
    }
    
    return {
        Sdp: ajaxCall(ajaxDetails().HttpType.GET,
                "/Main/getSDPLocations",
                nullData,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.NULL,
                ajaxDetails().nullBeforeSend,
                function(result) {
                    saveToLocalStorage("sdpMarkers", result);
                    PlotSDPToMap(result);
                }, 
                function(request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-asm-01";
                    if (request.readyState === 0) insert.ErrorCode = "c-i-j-gi-02";
                    
                    logError(insert, errorPanel);
                }),
        PlotSdp: ajaxCall(ajaxDetails().HttpType.POST,
                "Main/LocationSDP",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function(result) {
                    infowindow.setContent(result);
                    infowindow.open(map, references);
                },
                function(request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-als-01";
                    if (request.readyState === 0) insert.ErrorCode = "c-i-j-als-02";

                    logError(insert, errorPanel);
                }),
        Network: ajaxCall(ajaxDetails().HttpType.POST,
                "Main/LocationDetails",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                function() {
                    $(updateLocation).children(".dynamic").each(function() {
                        $(this).remove();
                    });
                },
                function (result) {
                    var divs = ["lataSearch", "swcSearch", "msoSearch", "onNetSearch", "nearNetSearch"];

                    $(updateLocation).append(result);
                    for (var i = divs.length - 1; i >= 0; i--) {
                        $("#" + divs[i] + " h4").bind("click", function () {
                            if (!$(this).parent().parent().hasClass("SearchResultDivDisplay"))
                                $(this).parent().parent().addClass("SearchResultDivDisplay");
                            else
                                $(this).parent().parent().removeClass("SearchResultDivDisplay");
                        });
                    }

                    $(updateLocation).bind("mouseenter", function () {
                        for (var i = divs.length - 1; i >= 0; i--) 
                            $("#" + divs[i]).addClass("minimized").find(".title").css("display", "");

                        $(this).unbind("mouseenter");
                    });
                },
                function(request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-als-01";
                    if (request.readyState === 0) insert.ErrorCode = "c-i-j-als-02";

                    logError(insert, errorPanel);
                })
    }
}

var localStorageAllowed;
var markers = [];
var clecMarkers = [];
var assetLocations = [];
var vendorFiberMarkers = [];
var twLayer = new google.maps.FusionTablesLayer({
    suppressInfoWindows: false,
    query: {
        select: 'col2',
        from: '1V_70gH0H3SaS0gRSPvf_b1C1zI6z0IQES6zyC5po'//,
        //where: 'col0 does not contain ignoring case \'level\''
    },
    options: {
        styleId: 2,
        templateId: 2
    }
});
var boundaryMarker = {
    "lata": [],
    "swc": []
};
var circle;
var map;
var infowindow, infoHover;
var infoFiber = new google.maps.InfoWindow();
var searchBox;

var djConfig = {
    parseOnLoad: true,
    packages: [{
        "name": "agsjs",
        "location": 'https://gmaps-utility-gis.googlecode.com/svn/tags/agsjs/2.04/xbuild/agsjs' // for xdomain load
    }]
};

// ***************** Initial page Load *************************** //
// Calls: loadEnd, maps.Listener.searchProviders
// Returns: 
// Sets: multiStates selected
//      multiSelect ui
$(document).ready(function() {
    "use strict";

    // check for local storage allowed in HTML5
    localStorageAllowed = supportsHtml5Storage();

    $("#slider-vertical").slider({
        orientation: "horizontal",
        range: "min",
        min: 0,
        max: 50,
        value: 2,
        slide: function(event, ui) {
            $("label[for='amount']").text("Range: " + $("#slider-vertical").slider("value") + " miles");
        }
    });

    $("#lat-input, #lng-input").keypress(function(e) {
        if (e.which === 13)
            searchProviders();
    });

    $("#MinMaxSearch").click(function() {
        minMaxParent(this);
    });
        
    requestInfo = new RequestData();
    requestInfo.lat = 38.850033;
    requestInfo.lng = -95.6500523;
    requestInfo.latLng = new LatLng(38.850033, -95.6500523);
    requestInfo.zip = "";
    requestInfo.range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
    requestInfo.addressConcat = " ||";
    requestInfo.contoller = null;

    // ----------- creates google map-----------------
    map = new google.maps.Map(document.getElementById('map-canvas'), {
        center: new google.maps.LatLng(requestInfo.latLng.lat(), requestInfo.latLng.lng()),
        zoom: 5,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_BOTTOM,
            style: google.maps.ZoomControlStyle.SMALL
        },
        panControl: false,
        mapTypeId: google.maps.MapTypeId.HYBRID  // ROADMAP
    }); // end map creation
    infowindow = new google.maps.InfoWindow();
    infoHover = new google.maps.InfoWindow();

    //var options = {
    //    //types: ['(cities)'],
    //    componentRestrictions: { country: "us" }
    //};  // end map search options restrictions

    var input = document.getElementById("pac-input");
    searchBox = new google.maps.places.Autocomplete(input);//, options);
    $("#addressSearch").click(function() {
        searchProviders();
    });

    $("#clli-input").autocomplete({
        delay: 200,
        focus: function(event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label.replace("<mark>", "").replace("</mark>", ""));
        },
        source: function(request, response) {
            var data = { "input": $("#clli-input").val() };
            $.get("/Main/JsonCLLIMatch", data
                , function(result) {
                    var results = [];
                    for (var i = 0; i < result.length; i++) {
                        results.push({
                            "label": result[i].CLLI,
                            "value": result[i],
                            "id": result[i].AddressID,
                        });
                    }
                    response(results);
                });
        },
        select: function(event, ui) {
            event.preventDefault();
            
            searchProviders(ui.item.value.FullStreet, ui.item.value.City, ui.item.value.State, ui.item.value.LatLng.Lat, ui.item.value.LatLng.Lng);
        },
        open: function() {
            $(".ui-autocomplete").css({
                //"max-width": "320px",
                "background-color": "#FFFFFF",
                "font-size": "0.7em",
                "max-height": "350px",
                "overflow-y": "scroll",
                "overflow-x": "hidden",
                "width": $(this).width() + "px"
            });
            $(".ui-autocomplete li").css({
                outline: "1px"
            });
        }
    }).data("ui-autocomplete")._renderItem = function(ul, item) {
        return $("<li title='" + item.label + "'>")
            .data("ui-autocomplete-item", item)
            .append($("<a>").html(item.label))
            .appendTo(ul);
    };

    google.maps.event.addListener(searchBox, "place_changed", function() {
        searchProviders();
    });

    google.maps.event.addListener(map, "rightclick", function (event) {
        showContextMenu(event.latLng);
    });

    google.maps.event.addListener(map, "maptypeid_changed", function() {
        var isHybridMap = this.getMapTypeId() === "hybrid";

        $("div#boundarySpan input").each(function() {
            var element = this;
            var id = $(element).prop("id");
            var type = id.slice(0, id.indexOf("ShowHide"));
            if (type !== "mso") {
                var labelStrokeColor, polyStrokeColor, polyFillColor, labelfontColor;
                if (type === "swc") {
                    polyStrokeColor = isHybridMap ? MapColors.yellow : MapColors.purple;
                    polyFillColor = isHybridMap ? MapColors.yellow : MapColors.purple;
                    labelStrokeColor = isHybridMap ? MapColors.yellow : MapColors.purple;
                    labelfontColor = isHybridMap ? MapColors.purple : MapColors.yellow;
                } else if (type === "lata") {
                    polyStrokeColor = isHybridMap ? MapColors.white : MapColors.black;
                    polyFillColor = isHybridMap ? MapColors.darkWhite : MapColors.lightBlack;
                    labelStrokeColor = isHybridMap ? MapColors.white : MapColors.black;
                    labelfontColor = isHybridMap ? MapColors.black : MapColors.white;
                }

                if ($(element).prop("checked") && typeof boundaryMarker[type] !== "undefined" && boundaryMarker[type] !== null) {
                    for (var i = boundaryMarker[type].length; i--;) {
                        boundaryMarker[type][i].polygon.setMap(null);
                        boundaryMarker[type][i].label.setMap(null);
                        // set fontColor, fillColor and strokeColor to the same colors;
                        boundaryMarker[type][i].polygon.setOptions({
                            fillColor: polyFillColor,
                            strokeColor: polyStrokeColor
                        });
                        boundaryMarker[type][i].label.setOptions({
                            strokeColor: labelStrokeColor,
                            fontColor: labelfontColor
                        });
                        boundaryMarker[type][i].polygon.setMap(map);
                        boundaryMarker[type][i].label.setMap(type === "mso" ? null : map);
                    }
                }
            }
        });
    });

    google.maps.event.addListener(map, "idle", function () {
        var mapMoveTimer;
        var moveDetect = google.maps.event.addListener(map, "bounds_changed", function () {
            clearTimeout(mapMoveTimer);
        });
        
        mapMoveTimer = setTimeout(function () {
            var bounds = map.getBounds();
            if (map.getZoom() >= 9) {
                $("#swcShowHideMap").prop("disabled", false);
                if ($("#swcShowHideMap").prop("checked"))
                    getBoundary("swc");
            } else {
                $("#swcShowHideMap").prop("disabled", true);
                removeFromMap("swc");
            }

            if ($("#lataShowHideMap").prop("checked"))
                getBoundary("lata");
            if ($("#msoShowHideMap").prop("checked"))
                getBoundary("mso");

            $("#fiberSpan input[type='checkbox']:checked").each(function () {
                var id = $(this).prop("id");
                if (id in vendorFiberMarkers) {
                    for (var i = vendorFiberMarkers[id].length; i--;)
                        vendorFiberMarkers[id][i].setMap(null);
                }

                getFiber($(this).prop("id"));
            });
            searchBox.setBounds(bounds);

            google.maps.event.removeListener(moveDetect);
        }, 1000);
    });

    // Add Click Bind to MSO Legend Div
    $("#cableLegend").bind("click", function () {
        if ($(this).hasClass("SearchResultDivDisplay"))
            $(this).removeClass("SearchResultDivDisplay");
        else
            $(this).addClass("SearchResultDivDisplay");
    });

    addSDPToMap();
});


function showContextMenu(latlng) {
    var projection = map.getProjection();
    $('.contextmenu').remove();
    var contextmenuDir = $("<div>");
    contextmenuDir.addClass("contextmenu");
    var contextMenu1 = $("<a>");
    contextMenu1.prop("id", "menu1");
    contextMenu1.html("What\'s Nearby?")
    contextMenu1.bind("click", function () {
        searchTableSelect(document.getElementById("latLng"));
        
        setTimeout(function () {
            $("#lat-input").val(Math.round(latlng.lat() * 1000000000) / 1000000000);
            $("#lng-input").val(Math.round(latlng.lng() * 1000000000) / 1000000000);
            $("#addressSearch").click();
        }, 550);
        $(".contextmenu").remove();
    });
    contextmenuDir.html(contextMenu1);

    $("#map-canvas").append(contextmenuDir);

    setMenuXY(latlng);

    var contextRemove = google.maps.event.addListener(map, "click", function () {
        contextmenuDir.remove();
        google.maps.event.removeListener(contextRemove);
    });
}

function getCanvasXY(latlng) {
    var scale = Math.pow(2, map.getZoom());
    var nw = new google.maps.LatLng(
        map.getBounds().getNorthEast().lat(),
        map.getBounds().getSouthWest().lng()
    );

    var worldCoordNW = map.getProjection().fromLatLngToPoint(nw);
    var worldCoord = map.getProjection().fromLatLngToPoint(latlng);
    var currentLatLngOffset = new google.maps.Point(
        Math.floor((worldCoord.x - worldCoordNW.x) * scale),
        Math.floor((worldCoord.y - worldCoordNW.y) * scale)
        );

    return currentLatLngOffset;
}

function setMenuXY(latlng) {
    var mapWidth = $("#map-canvas").width();
    var mapHeight = $("#map-canvas").height();
    var menuWidth = $(".contextmenu").width();
    var menuHeight = $(".contextmenu").height();
    var clickedPosition = getCanvasXY(latlng);
    var x = clickedPosition.x;
    var y = clickedPosition.y;

    if ((mapWidth - x) < menuWidth) // decrease x position
        x = x - menuWidth;
    if ((mapHeight - y) < menuHeight) // decrease y position
        y = y - menuHeight;

    $(".contextmenu").css({
        "left": x,
        "top": y });
}


// ***************** Add Static Locations To Map ************************ //
// Calls: 
// Returns: 
// Sets: 
// ** makes call to gather all Service Devlivery Point Locations   ** //
function addSDPToMap() {
    assetLocations = new Array();

    //  checks to see if "sdpMarkers" exists in localStorage on this computer.
    //  if it does, checks to see if it was loaded today, will re-use if all is true
    if (localStorageAllowed
        && localStorage.getItem("sdpMarkers") !== null
        && localStorage.getItem("sdpMarkers").expiration >= Date.now()) {
            PlotSDPToMap(JSON.parse(localStorage.getItem("sdpMarkers").data));
            return;
    }

    var funcName = trimFuncName(arguments.callee.toString());
    runAjax(indexAjax(funcName, "#map-canvas", null, "").Sdp);
}


function PlotSDPToMap(result) {
    // Add the markers and infowindows to the map
    for (var i = 0; i < result.length; i++) {
        var marker = createMarker(result[i].AddressID,
                                  result[i].LatLng.Lat,
                                  result[i].LatLng.Lng,
                                  locationIcons[result[i].Services - 1],
                                  2,
                                  false);

        infoHover = new google.maps.InfoWindow();

        function closeInfoHover() {
            var map = infoHover.getMap();
            if (map !== null && typeof map !== "undefined")
                infoHover.close();
        }

        google.maps.event.addListener(marker, "click", (function(marker, i) {
            return function() {
                closeInfoHover();
                var funcName = trimFuncName(arguments.callee.toString());
                var data = { "addressID": marker.id };
                runAjax(indexAjax(funcName, "#map-canvas", data, marker).PlotSdp);
            };   // end return 
        })(marker, i)); // end eventListener

        google.maps.event.addListener(marker, "mouseover", (function(assets, marker) {
            return function() {
                if (infowindow.getMap() === null || typeof infowindow.getMap() === "undefined") {
                    infoHover.setContent((assets >> 3 & 1 === 1 ? "<img src='../../Images/star.png'>" : "")
                                    + (assets >> 2 & 1 === 1 ? "<img src='../../Images/pentagon.png'>" : "")
                                    + (assets >> 1 & 1 === 1 ? "<img src='../../Images/triangle.png'>" : "")
                                    + (assets & 1 === 1 ? "<img src='../../Images/square.png'>" : ""));
                    infoHover.open(map, marker);
                }
                google.maps.event.addListener(map, "mousemove", function() {
                    infoHover.close();
                });
            };
        })(result[i].Services, marker));


        var asset = new assetLocation(marker, result[i].Services, result[i].AddressID);
        assetLocations.push(asset);
    }   // end for
    showMarkers();
}


// ***************** Search Providers method ************************ //
// Calls: loadStart, searchMap, locateNearest, loadEnd
// Returns: 
// Sets: 
// ** checks providers and multi-states/detail input, parses input ** //
// ** if any and returns state, city, postal                       ** //
function searchProviders(street, city, state, latitude, longitude, vendor) {
    var search = searchSettings();

    if (search.SafeToSearch === true) {
        var zips, cities, states, streetNumber, route;
        zips = cities = states = streetNumber = route = "";

        var lat, lng;
        lat = lng = "";

        var vendorName = null;

        // if Searchy by address is selected
        if (search.AddressSearch === true) {
            var place = searchBox.getPlace();
            if (place.geometry !== null || typeof place.geometry !== 'undefined') {
                var decimalPlaces = 1000000;
                lat = Math.round(place.geometry.location.lat() * decimalPlaces) / decimalPlaces;
                lng = Math.round(place.geometry.location.lng() * decimalPlaces) / decimalPlaces;
                // --------------- get zip codes for the addresses -------------------
                for (var i = place.address_components.length; i--;)
                    for (var j = place.address_components[i].types.length; j--;) {
                        if (place.address_components[i].types[j] === "postal_code")
                            zips = place.address_components[i].long_name;
                        else if (place.address_components[i].types[j] === "administrative_area_level_1")
                            states = place.address_components[i].short_name;
                        else if (place.address_components[i].types[j] === "locality")
                            cities = place.address_components[i].long_name;
                        else if (place.address_components[i].types[j] === "street_number")
                            streetNumber = place.address_components[i].long_name;
                        else if (place.address_components[i].types[j] === "route")
                            route = place.address_components[i].long_name;
                    }   // end j for // end for i
                
                locateNearest(cities, zips, states, lat, lng, streetNumber, route);
                street = place.name;
            }
        } else if ($("#clli").hasClass("selected")) {   // end address if
            states = state;
            cities = city;
            streetNumber = route = street;
            lat = latitude;
            lng = longitude;
            vendorName = vendor;
            locateNearest(city, null, state, lat, lng, street, route);
        } else if ($("#latLng").hasClass("selected")) {   // end CLLI search if
            lat = $("#lat-input").val();
            lng = $("#lng-input").val();
            locateNearest(city, null, state, lat, lng, street, route);
        }

        /// create the search drop pin to show
        var bounds = new google.maps.LatLngBounds();
        var marker = createMarker("searchMarker", lat, lng, null, 2, false);
        
        createSearchCircle(marker);

        google.maps.event.addListener(marker, "click", (function(marker) {
            return function() {
                infowindow.setContent(getInfoWindow("pin",
                                        vendorName,
                                        street,
                                        lat,
                                        lng));
                infowindow.open(map, marker);
            };   // end return
        })(marker));    // end evenListener
        bounds.extend(new google.maps.LatLng(lat, lng));
        markers.push(marker);

        map.fitBounds(bounds);
        setTimeout(function() {
            var listener = google.maps.event.addListener(map, "idle", function() {
                if (map.getZoom() > 14)
                    map.setZoom(14);
                google.maps.event.removeListener(listener);
            }); // end listener
        }, 15); // end setTimeout
    }
}


// ************* Locate Nearest Provider method ******************** //
// Calls: AJAX(Partial/Connections)
// Returns: 
// Sets: 
// ** checks providers and multi-states/detail input, parses input ** //
// ** if any and returns state, city, postal                       ** //
function locateNearest(city, zipString, states, lat, lng, streetNumber, route) {
    requestInfo = new RequestData();
    requestInfo.lat = lat;
    requestInfo.lng = lng;
    requestInfo.zip = zipString;
    requestInfo.range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
    requestInfo.addressConcat = streetNumber + " " + route + "|" + city + "|" + states;
    requestInfo.controller = "/Main/LocationCLEC";

    // web worker
    var worker = new Worker("Scripts/PageScripts/Point.js");
    worker.onmessage = function(e) {
        var shape = {
            coord: [0, 0, 30],
            type: "circle"
        };
        var bounds = new google.maps.LatLngBounds();
        var index = e.data.length - 1;
        var process = function() {
            for (; index >= 0; index--) {
                if (index + 1 < e.data.length && index % 500 === 0)
                    setTimeout(process, 5);

                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(e.data[index].LatLng.Lat, e.data[index].LatLng.Lng),
                    icon: e.data[index].Vendor === "Level3"
                        ? icons[0]
                        : e.data[index].Vendor.toLowerCase() === "level3_tw"
                            ? icons[12]
                            : icons[1],
                    shape: shape,
                    flat: true,
                    id: e.data[index].CLLI,
                    zIndex: 10,
                    origin: new google.maps.Point(0, 0),
                    scaledSize: new google.maps.Size(5, 5),
                    map: $("#CLECShowHideMap").prop("checked") ? map : null
                });
                
                clecMarkers.push(marker);
                google.maps.event.addListener(marker, "click", (function(marker, index) {
                    return function() {
                        var vendorName = e.data[index].Vendor + "</b> (" + e.data[index].VendorType + ") ";
                            vendorName += e.data[index].CLLI === null ? "" : "- " + e.data[index].CLLI;
                        var vendorAddress = "<b>"+e.data[index].FullStreet + "</b><br />" +
                                            e.data[index].City + ", " + e.data[index].State;
                        infowindow.setContent(getInfoWindow("default", vendorName, vendorAddress, e.data[index].LatLng.Lat, e.data[index].LatLng.Lng));
                        infowindow.open(map, marker);
                    }
                })(marker, index));
                bounds.extend(marker.position);
            }
        };
        process();
        worker.terminate();
    };

    worker.postMessage(requestInfo);

    var funcName = trimFuncName(arguments.callee.toString());
    runAjax(indexAjax(funcName, "#map-right", requestInfo, null).Network);
}


// ***************** getLevel3Fiber Method ************************ //
// Calls: Home/Level3Map
// Returns: 
// Sets: 
function getFiber(company) {
    if (company === "tw") {
        showMarkers();
        return;
    } 
    
    if ($("input[id='" + company + "']").prop("checked")) {
        var bounds = map.getBounds();
        var data = {
            "vendorName": company,
            "bottomLeftLat": bounds.getSouthWest().lat(), "bottomLeftLng": bounds.getSouthWest().lng(),
            "topRightLat": bounds.getNorthEast().lat(), "topRightLng": bounds.getNorthEast().lng(),
            "zoom": map.getZoom()
        };

        // web worker
        var worker = new Worker("Scripts/PageScripts/Line.js");
        worker.onmessage = function(e) {
            addFiberMap(e.data);
            worker.terminate();
        };
        worker.postMessage(data);

        return;
    } else
        removeFiber(company);
}
function removeFiber(vendor) {
    if (vendorFiberMarkers[vendor] !== null && typeof vendorFiberMarkers[vendor] !== "undefined") {
        var index = vendorFiberMarkers[vendor].length - 1;
        var removeFiberProcess = function() {
            for (;index >= 0; index--) {
                if (index + 1 < vendorFiberMarkers[vendor].length && index % 500 === 0)
                    setTimeout(removeFiberProcess, 5);
                vendorFiberMarkers[vendor][index].setMap(null);
            }
        };
        removeFiberProcess();
    }
}

function getBoundary(type) {
    if ($("#" + type + "ShowHideMap").prop("checked")) {
        if (type === null || typeof type === "undefined")
            return;

        var bounds = map.getBounds();
        var data = {
            type: type,
            data: {
                "bottomLeftLat": bounds.getSouthWest().lat(), "bottomLeftLng": bounds.getSouthWest().lng(),
                "topRightLat": bounds.getNorthEast().lat(), "topRightLng": bounds.getNorthEast().lng(),
                zoom: map.getZoom()
            }
        };

        var worker = new Worker("Scripts/PageScripts/Polygon.js");
        worker.onmessage = function(e) {
            addToMap(type, e.data);
            worker.terminate();
        };

        worker.postMessage(data);

        return;

    } else
        removeFromMap(type);
}
function removeFromMap(type) {
    if (typeof boundaryMarker[type] !== "undefined") {
        var index = boundaryMarker[type].length - 1;
        var removeMapProcess = function() {
            for (;index >= 0; index--) {
                if (index + 1 < boundaryMarker[type].length && index % 500 === 0)
                    setTimeout(removeMapProcess, 5);
                
                boundaryMarker[type][index].polygon.setMap(null);
                boundaryMarker[type][index].label.setMap(null);
            }
        };
        removeMapProcess();
    }
}
function addToMap(type, result) {
    if (typeof boundaryMarker[type] !== "undefined" && boundaryMarker[type] !== null)
        if (boundaryMarker[type].length > 0)
            removeFromMap(type);
    
    boundaryMarker[type] = [];
    var index = result.length - 1;
    var process = function() {
        for (;index >= 0; index--) {
            if (typeof result[index] !== "undefined") {
                if (index + 1 < result.length && index % 500 === 0)
                    setTimeout(process, 5);

                var bounds = result[index].line;
                var labelStrokeColor, polyStrokeColor, polyFillColor, labelfontColor;

                var isHybridMap = map.getMapTypeId() === "hybrid";
                if (type === "swc") {
                    polyStrokeColor = isHybridMap ? MapColors.yellow : MapColors.purple;
                    polyFillColor = isHybridMap ? MapColors.yellow : MapColors.purple;
                    labelStrokeColor = isHybridMap ? MapColors.yellow : MapColors.purple;
                    labelfontColor = isHybridMap ? MapColors.purple : MapColors.yellow;
                } else if (type === "lata") {
                    polyStrokeColor = isHybridMap ? MapColors.white : MapColors.black;
                    polyFillColor = isHybridMap ? MapColors.darkWhite : MapColors.lightBlack;
                    labelStrokeColor = isHybridMap ? MapColors.white : MapColors.black;
                    labelfontColor = isHybridMap ? MapColors.black : MapColors.white;
                }

                var _strokeOpacity, _fillOpacity;
                _strokeOpacity = type === "mso" ? 0.8 : 1;
                _fillOpacity = type === "mso" ? 0.2 : 0.1;
                var poly = {
                    polygon: new google.maps.Polygon({
                        paths: bounds,
                        strokeColor: type === "mso" ? result[index].color : polyStrokeColor,
                        fillColor: type === "mso" ? result[index].color : polyFillColor,
                        strokeOpacity: _strokeOpacity,
                        fillOpacity: _fillOpacity,
                        strokeWeight: 0.8,
                        //icons: [{
                        //    icon: {
                        //        path: 'M 0, -1, 0, 1',
                        //        strokeOpacity: 1,
                        //        strokeWeight: 1.5,
                        //        scale: 3
                        //    },
                        //    offset: "100%",
                        //    repeat: "10px"
                        //}],
                        map: map,
                        zIndex: 0
                    }),
                    label: new MapLabel({
                        text: result[index].name,
                        position: new google.maps.LatLng(result[index].centroid.lat, result[index].centroid.lng),
                        fontSize: 10,
                        fontColor: labelfontColor,
                        labelInBackground: true,
                        strokeColor: labelStrokeColor,
                        map: type === "mso" ? null : map
                    })
                };
                google.maps.event.addListener(poly.polygon, "mouseover", function() {
                    this.setOptions({ fillOpacity: _fillOpacity / 3 });
                });
                google.maps.event.addListener(poly.polygon, "mouseout", function() {
                    this.setOptions({ fillOpacity: _fillOpacity });
                });
                boundaryMarker[type].push(poly);
            }
        }
    };
    process();
}

// ************* addFiberMap method ****************** //
// Calls: 
// Returns: 
// Sets: 
function addFiberMap(results) {
    if (results !== null && results.length > 0) {
        removeFiber(results[0].vendor);
        vendorFiberMarkers[results[0].vendor] = [];

        if (infoFiber.getMap() !== null || typeof infoFiber.getMap() !== "undefined")
            infoFiber.close();
        
        var index = results.length - 1;
        var addProcess = function() {
            for (; index >= 0; index--) {
                if (index + 1 < results.length && index % 500 === 0)
                    setTimeout(addProcess, 5);

                var mapLine = new google.maps.Polyline({
                    path: results[index].line,
                    geodesic: true,
                    strokeColor: results[index].color,
                    strokeOpacity: 1,
                    strokeWeight: 3,
                    zIndex: 0,
                    map: map,
                    message: results[index].vendor + " Fiber"
                });
                google.maps.event.addListener(mapLine, "click", function(event) {
                    infoFiber.setContent(this.message);
                    infoFiber.setPosition(event.latLng);
                    infoFiber.open(map);
                }); // end eventListener
                vendorFiberMarkers[results[0].vendor].push(mapLine);
            }
        };
        addProcess();
        showMarkers();
    }
}



// ************* show on Map method ****************** //
// Calls: 
// Returns: 
// Sets: 
function showMarkers(e) {
    if (e === null || typeof e === "undefined") {
        $("#legend table tr input:checkbox").each(function() {
            var id = $(this).attr("id");
            var checked = $(this).prop("checked");
            switch (id) {
                case "tw":
                    twLayer.setMap(checked ? map : null);
                    break;
                case "CLECShowHideMap":
                    for (var i = clecMarkers.length - 1; i >= 0; i--)
                        clecMarkers[i].setMap(checked ? map : null);
                    break;
                default:
                    break;
            }
        });
    } else {
        var id = $(e).attr("id");
        var removal = 0;
        switch (id) {
            case "NNIShowHideMap":
                removal += 4;                       // equals 8 when cascaded
            case "ILECShowHideMap":
                removal += 2;                       // equals 4 when cascaded
            case "LOWShowHideMap":
                removal += 1;                       // equals 2 when cascaded
            case "HIGHShowHideMap":
                removal += 1;
                for (var i = assetLocations.length - 1; i >= 0; i--) {
                    assetLocations[i].marker.setMap(null);
                    assetLocations[i].attributes = (assetLocations[i].attributes ^ removal) & assetLocations[i].modAttributes;  // XOR the attribute EX. 1011 XOR 1000 = 0011
                    assetLocations[i].marker.icon = locationIcons[assetLocations[i].attributes - 1];
                    assetLocations[i].marker.setMap(assetLocations[i].attributes <= 0 ? null : map);
                }
                break;
        }
    }
}


// ***************** creates expand and collapse on pivot ************************ //
// Calls: 
// Returns: 
// Sets: 
function expandCollapse(group, focus, vendor) {
    if (focus.src.match("expand")) {
        focus.src = "../../Images/collapse.gif";
        $("#" + group + " tr." + vendor).each(function() {
            $(this).show();
        }); // end foreach
    } else {
        focus.src = "../../Images/expand.gif";
        $("#" + group + " tr." + vendor).each(function() {
            $(this).hide();
        }); // end foreachr
    }   // end if
}

// ***************** shows/hides searchable fields ************************ //
// Calls: 
// Returns: 
// Sets: 
function searchTableSelect(field) {
    var fadein = function(element, field) {
        that = $("#" + field.id + "SearchDiv");
        $(element).css("display", "none");
        that.css("display", "block");

        setTimeout(function() {
            that.animate({ "opacity": 1 }, 500).addClass("selected");
        }, 1);

        $("#searchCriteria").children("span").each(function() {
            $(this).attr("onclick", "searchTableSelect(this)");
        });
    };

    // add the class to the correct tab
    $(".tab").each(function () {
        $(this).removeClass("selected");
    });
    $("#" + field.id).addClass("selected");

    // remove click events until done.
    $("#searchCriteria").children("span").each(function() {
        $(this).attr("onclick", "");
    });

    // animate the fade in of the selected tab
    $("#searchMethods").children().each(function() {
        if ($(this).hasClass("selected"))
            $(this).animate({
                "opacity": 0}, 500, null, fadein(this, field)).removeClass("selected");
    });
}

function ShowLocationSwitch(field) {
    $("#location").children("span").each(function() {
        $(this).attr("onclick", "");
    });
    $("#location").children("div").each(function() {
        $(this).fadeTo(500, 0, null);
        $(this).css("z-index", -1);
    });
    setTimeout(function() {
        $("#" + field.id + "Div").fadeTo(800, 1, null);
        $("#" + field.id + "Div").css("z-index", 3);
    }, 550);
    setTimeout(function() {
        $("#location").children("span").each(function() {
            if (this.id === field.id)
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
            $(this).attr("onclick", "ShowLocationSwitch(this)");
        });
    }, 600);
}


function searchSettings() {
    var safeToSearch = null;
    var addressSearch = false;
    var clliSearch = false;
    var latLngSearch = false;

    if (circle !== null && typeof circle !== "undefined")
        circle.setMap(null);

    if (infowindow.getMap() !== null || typeof infowindow.getMap() !== "undefined")
        infowindow.close();
    if (infoHover.getMap() !== null || typeof infoHover.getMap() !== "undefined")
        infoHover.close();

    for (var i = 0; i < markers.length; i++)
        markers[i].setMap(null);
    for (var i = 0; i < clecMarkers.length; i++)
        clecMarkers[i].setMap(null);
    markers = [];
    clecMarkers = [];
    locations = [];

    safeToSearch = true;

    if ($("#address").hasClass("selected") && $("#pac-input").val().length > 1)
        addressSearch = true;
    if ($("#clli").hasClass("selected"))
        clliSearch = true;
    if ($("#latLng").hasClass("selected"))
        clliSearch = true;

    return {
        SafeToSearch: safeToSearch,
        AddressSearch: addressSearch,
        ClliSearch: clliSearch,
        LatLngSearch: latLngSearch
    };
}

// ***************** creates return for standard infowindows ************************ //
function getInfoWindow(type, vendor, address, lat, lng) {
    var decimalPlaces = 6;
    var rounding = Math.pow(10, decimalPlaces);
    var returnWindow = "<div style='width: 240px; height: 110px; overflow: hidden; line-height: 1.35'>";
    var p = "<p style='font-size: 0.9em'>";
    var table = "<table class='flat' style='font-size: 0.85em'>";
    var latLngDetails = "<tr>"
						+ "<td style='vertical-align:top;'><h6>Latitude: </h6>"
                        +   "<h6>Longitude: </h6></td>"
						+ "<td  style='vertical-align:top; padding-left: 5px;' class='numCell'><div>" + Math.round(lat * rounding) / rounding + "</div>"
                        + "<div>" + Math.round(lng * rounding) / rounding + "</div></td>"
                        //+ "<td style='padding-left: 5px;'><img src='//maps.googleapis.com/maps/api/staticmap?size=120x80&zoom=13&markers=size:small|color:red|" + lat + "," + lng + "'></td>"
						+ "</tr>";
    var endTable = "</table>";
    var end = "</p></div>";

    if (type !== null && typeof type !== "undefined") {
        switch (type) {
            case "pin":
                if (address !== null && typeof address !== "undefined")
                    returnWindow += "<h6>" + address + "</h6>";

                var removePin = "<a onclick='removePin()'>Remove Pin</a>";                
                var removeCircle = "<a onclick='removeCircle()'>Remove Circle</a>";

                returnWindow += p + table
							 + latLngDetails
                             + endTable 
                             + removePin + "&nbsp;&nbsp;" + removeCircle;
							 + end;
                break;
            case "default":
                returnWindow += "<h5>" + vendor + "</h5>"
							 + p + address + "</p>"
							 + p + table
							 + latLngDetails
                             + endTable
							 + end;
                break;
            default:
                returnWindow += end;
                break;
        }
    }

    return returnWindow;
}

function removePin() {
    for (var i = markers.length - 1; i >= 0; i--)
        if (markers[i].id === "searchMarker") {
            markers[i].setMap(null);
            markers[i] = null;
            markers.splice(i, 1);
        }
    removeCircle();
}

function createMarker(id, lat, lng, icon, index, draggable) {
    var image = {
        url: iconURLPrefix + "red-dot.png",
        //size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34)//,
        //scaledSize: new google.maps.Size(25, 25)
    };  // end image

    var m = new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        map: map,
        icon: icon === null ? image : icon,
        title: id.toString(),
        id: id,
        zIndex: index,
        draggable: draggable
    });

    return m;
}

function removeCircle() {
    if (circle != null)
        circle.setMap(null);
        circle = null;
}

function createSearchCircle(bindingElement) {
    if (bindingElement !== null && typeof bindingElement !== "undefined")
        // create radius cirlcle
        circle = new google.maps.Circle({
            map: map,
            radius: ($("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value")) / 0.00062137, // miles to meters
            fillColor: "#0066FF",
            fillOpacity: 0.1,
            strokeColor: "#0066FF",
            strokeOpacity: 0.7,
            strokeWeight: 2,
            zIndex: -2
        });
    circle.bindTo('center', bindingElement, 'position');
}


// ***************** Classes ************************ //
function RequestData() {
    var lat;
    var lng;
    var zip;
    var range;
    var addressConcat;
    var controller;
}

function assetLocation(marker, attributes, addressID) {
    this.marker = marker;
    this.attributes = attributes;
    this.modAttributes = attributes;
    this.addressID = addressID;
}
