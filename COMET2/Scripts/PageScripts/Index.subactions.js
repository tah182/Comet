/// <reference path="Global.js" />
/// <reference path="Index.js" />
google.load("visualization", "1.0", { 'packages': ['corechart'] });

function fillContactInfo(data) {
    $("#subcontent").empty();
    if (data === null || typeof data === "undefined") {
        $("#subcontent").html("Please select a vendor on the right");
        return;
    }

    raiseDetails(true, "cog");
    var div = document.createElement("div");
    div.id = "contact";
    $(div).css("padding-bottom", "10px");
    $("#subcontent").append(div);
    div = document.createElement("div");
    div.id = "quote";
    $("#subcontent").append(div);
    data = { vendorName: data };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Main/MSOContactList",
        data: JSON.stringify(data),
        async: true,
        beforeSend: function () {
            $("#contact").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
        },
        success: function (data) {
            $("#contact").html(data);
        },  // end success
        error: function (request, status, error) {
            var ErrorInsert = new ErrorInsert();
            ErrorInsert.PageName = "Index";
            ErrorInsert.StepName = "DrawDetailsUp";
            ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState === 4) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                logError(ErrorInsert, "#Contact");
            }   // end if readyState 4
            if (request.readyState === 0) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                logError(ErrorInsert, "#Contact");
            }   // end if readyState 0
        }   // end error
    }); // end ajax

    var quoteData = {};
    quoteData.vendorName = data.vendorName;
    quoteData.lat = requestInfo.lat;
    quoteData.lng = requestInfo.lng;
    quoteData.range = requestInfo.range;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Main/QuoteDetails",
        data: JSON.stringify(quoteData),
        async: true,
        beforeSend: function () {
            $("#quote").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
        },
        success: function (data) {
            $("#quote").html(data);
        },  // end success
        error: function (request, status, error) {
            var ErrorInsert = new ErrorInsert();
            ErrorInsert.PageName = "Index";
            ErrorInsert.StepName = "DrawDetailsUp";
            ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState === 4) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                logError(ErrorInsert, "#Contact");
            }   // end if readyState 4
            if (request.readyState === 0) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                logError(ErrorInsert, "#Contact");
            }   // end if readyState 0
        }   // end error
    })
}

function fillNNIInfo(addressID) {
    var data = {
        lat: requestInfo.lat,
        lng: requestInfo.lng,
        range: requestInfo.range
    };

    if (addressID != null && typeof addressID !== "undefined") {
        data.addressID = addressID;
        raiseDetails(true, "nni");
    }
    
    
    $("#subcontent").empty();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Main/NNIInRange",
        data: JSON.stringify(data),
        async: true,
        beforeSend: function () {
            $("#subcontent").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
        },
        success: function (data) {
            $("#subcontent").html(data);
        },  // end success
        error: function (request, status, error) {
            var ErrorInsert = new ErrorInsert();
            ErrorInsert.PageName = "Index";
            ErrorInsert.StepName = "DrawDetailsUp";
            ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState === 4) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                logError(ErrorInsert, "#nni");
            }   // end if readyState 4
            if (request.readyState === 0) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                logError(ErrorInsert, "#nni");
            }   // end if readyState 0
        }   // end error
    }); // end ajax
}

function fillQuoteInfo() {
    $("#subcontent").empty();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Main/QuoteDetails",
        data: JSON.stringify(requestInfo),
        async: true,
        beforeSend: function() {
            $("#subcontent").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
        },
        success: function (data) {
            $("#subcontent").html(data);
            //setTimeout(function () { showHideDetails(true, "cog"); }, 100);
        },  // end success
        error: function (request, status, error) {
            var ErrorInsert = new ErrorInsert();
            ErrorInsert.PageName = "Index";
            ErrorInsert.StepName = "DrawDetailsUp";
            ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState === 4) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                logError(ErrorInsert, "#quoteLookup");
            }   // end if readyState 4
            if (request.readyState === 0) {
                ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                logError(ErrorInsert, "#quoteLookup");
            }   // end if readyState 0
        }   // end error
    }); // end ajax
}

function fillEfInfo(ordering, addressID, isHighSpeed) {
    if (ordering === null || typeof ordering === "undefined")
        odering = 1;

    var data = {
        lat: requestInfo.lat,
        lng: requestInfo.lng,
        range: requestInfo.range,
        ordering: ordering
    };

    if (addressID != null && typeof addressID !== "undefined") {
        data.addressID = addressID;
        data.isHighSpeed = isHighSpeed === "true";
        raiseDetails(true, "ef");
    }

    $.ajax({
        type: "POST",
        url: "/Main/EFInRange",
        data: data,
        async: true,
        beforeSend: function () {
            $("#subcontent").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
        },
        success: function (data) {
            $("#subcontent").html(data);
        },  // end success
        error: function (request, status, error) {
            var Insert = new ErrorInsert();
            Insert.PageName = "Index";
            Insert.StepName = "ShowEfDetails";
            Insert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState === 4) {
                Insert.ErrorCode = "c-i-j-sed-01";
                logError(Insert, "#ef");
            }   // end if readyState 4
            if (request.readyState === 0) {
                Insert.ErrorCode = "c-i-j-sed-02";
                logError(Insert, "#ef");
            }   // end if readyState 0
        }   // end error
    }); // end ajax
}


// ***************** Draw Details Up ************************ //
// Calls: ShowEF(ordering)
// Returns: 
// Sets: 
function showHideDetails(group) {
    switch (group) {
        case "ef":
            fillEfInfo(1);
            break;
        case "cog":
            fillContactInfo();
            break;
        case "nni":
            fillNNIInfo();
            break;
        case "quote":
            fillQuoteInfo();
            break;
    }

    raiseDetails(true, group);
}


function raiseDetails(show, group) {
    if(show) {
        $("#detailsLayer").animate({
            height: "630px"
        }, 300).addClass("show");

        $("#header").children().find("li").each(function () {
            if ($(this).text().toLowerCase() === group)
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
        });

        return;
    }

    $("#detailsLayer").animate({
        height: "24px"
    }, 300, function () {
        $(this).removeClass("show")
    });
}



// ***************** addContact  ************************ //
// Calls: 
// Returns: 
// Sets: 
function addContact(parentVendor) {
    var temp = $("tr").remove("#addcontact");
    var rep = $("#contact").html();
    $("#contact").children().each(function() {
        $(this).remove();
    });

    $("#contact").append($("<form id='addContactForm' action='/Rest/RemoveContact' method='post'>"));
    $("#addContactForm").append(rep);
    $("#contactsGrid tbody").append("<tr id='newContact'>" +
                    "<td><input data-val='true' style='width:100%' id='company' name='vendor' value='" + parentVendor + "' /></td>" +
                    "<td><input data-val='true' style='width:100%' id='name' name='contactName' /></td>" +
                    "<td><input data-val='true' style='width:100%' id='office' name='officePhone' /></td>" +
                    "<td><input data-val='true' style='width:100%' id='mobile' name='mobilePhone' /></td>" +
                    "<td><input data-val='true' style='width:100%' id='email' name='email' /></td>" +
                    "<td id='addContact' class='cursor'><img src='../../images/upload.png' style='height: 12px;' /></td></tr>");
    
    $("#addContact").bind("click", function () {
        $("#addContactForm").submit();
    });
}


// ***************** submitContact  ************************ //
// Calls: Partial.InsertContact(cableContact), Partial.CableContactList(ContactRequest)
// Returns: ContactPartialView
// Sets: 
function submitContact() {
    var insert = true;
    $("#contactUpdateContent label").each(function () {
        if ($(this).hasClass("important")) {
            insert = false;
            alert("Please check input. All values must be filled and phone numbers should be in xxx-xxx-xxxx format.");
            return false;
        }
    });
    $("#contactUpdateContent input[type=text]").each(function () {
        if ($(this).val().length < 1) {
            insert = false;
            alert("Please check input. All values must be filled and phone numbers should be in xxx-xxx-xxxx format.");
            return false;
        }
    });
    if (insert) {
        var contact = new cableContact();
        contact.vendorNormName = $("#CompanyInput").val();
        contact.parentVendor = $("#CompanyInput").val();
        contact.contactName = $("#ContactInput").val();
        contact.contactOffice = $("#OfficeInput").val();
        contact.contactMobile = $("#MobileInput").val();
        contact.contactEmail = $("#EmailInput").val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/InsertContact",
            data: JSON.stringify(contact),
            async: true,
            success: function (data) {
                $(".loading").css("z-index", "-10");
                $(".loading").animate({
                    opacity: 0
                }, 500);
                $("#contactUpdatePanel").fadeOut(500);
                // Update the Table;
                setTimeout(function () {
                    $("#Contact").empty();
                    var vendor = $("#hiddenCompany").val();
                    fillContactInfo({ "vendorName": vendor });
                }, 10); // end setTimeout
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "submitContact";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState === 4) {
                    ErrorInsert.ErrorCode = "c-i-j-sc-01";
                    logError(ErrorInsert, "#Contact");
                }   // end if readyState 4
                if (request.readyState === 0) {
                    ErrorInsert.ErrorCode = "c-i-j-sc-02";
                    logError(ErrorInsert, "#Contact");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    } // end if insert
}




// ***************** deleteContact ************************ //
// Calls: yesNoDialog
// Returns: 
// Sets: 
function deleteContact(id, vendor) {
    yesNoDialog("confirm", "Don't delete", "Delete", "Are you sure you want to delete this contact?", 0, id, vendor);
}


// ***************** confirmReturn ************************ //
// Calls: DeleteConfirmed(id)
// Returns: 
// Sets: 
function confirmReturn(switchClause, confirm, id, details) {
    switch (switchClause) {
        case 0:
            DeleteConfirmed(confirm, id, details);
            break;
        default:
            break;
    }
}



// ***************** Draw Details Up ************************ //
// Calls: AJAX(Partial/removeContact)
// Returns: 
// Sets: 
function DeleteConfirmed(deleteConfirmed, id, vendor) {
    if (deleteConfirmed) {
        var contact = new cableContact();
        contact.id = id;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Main/RemoveContact",
            data: JSON.stringify(contact),
            async: true,
            success: function (data) {
                var vendor = vendor;
                fillContactInfo({ "vendorName": vendor });
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "deleteContact";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState === 4) {
                    ErrorInsert.ErrorCode = "c-i-j-dc-01";
                    logError(ErrorInsert, "#Contact");
                }   // end if readyState 4
                if (request.readyState === 0) {
                    ErrorInsert.ErrorCode = "c-i-j-dc-02";
                    logError(ErrorInsert, "#Contact");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    } // end if confirmDelete
}


// Export to Excel
function ExportToQuotesToExcel() {
    var place = searchBox.getPlace();
    setTimeout(function () {
        var requestInfo = new requestData();
        requestInfo.lat = place.geometry.location.lat();
        requestInfo.lon = place.geometry.location.lng();
        requestInfo.range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/ExportQuotes",
            data: JSON.stringify(requestInfo),
            async: true,
            success: function (data) {
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "ExportQuotes";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState === 4) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                    logError(ErrorInsert, "#quote");
                }   // end if readyState 4
                if (request.readyState === 0) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                    logError(ErrorInsert, "#quote");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 50); // end setTimeout
}


function plotLatLon(trailName, lat, lon) {
    raiseDetails(false);
    var bounds = map.getBounds();
    var iconURLPrefix = 'http://maps.google.com/mapfiles/ms/icons/';
    var image = {
        url: iconURLPrefix + "green-dot.png",
        origin: new google.maps.Point(0, 0)
    };  // end image
    var marker = new google.maps.Marker({
        map: map,
        icon: image,
        title: trailName,
        position: new google.maps.LatLng(lat, lon),
        Zindex: 4
    }); // end marker
    infowindow.setContent("<div style='font-size: 0.9em'><strong>" + trailName + "</strong>" +
                "<p style='font-size: 0.8em'><b>Latitude: </b>" + lat + "<br />" +
                "<b>Longitude: </b>" + lon + "</p></div>");
    google.maps.event.addListener(marker, "click", (function (marker) {
        return function () {
            infowindow.open(map, marker);
        };   // end return
    })(marker));    // end evenListener
    bounds.extend(new google.maps.LatLng(lat, lon));
    infowindow.open(map, marker);
    markers.push(marker);
    map.fitBounds(bounds);
}


function getTrend(addressId, type) {
    var data = new google.visualization.DataTable();
    data.addColumn("string", "Week");
    // create the headings columns and colors array dynamically
    data.addColumn("number", "Utilization");

    var options = {
        backgroundColor: "#fff",
        titlePosition: "none",
        lineWidth: 3,
        width: "100%",
        height: "90%",
        legend: {
            position: 'none'
        },
        chartArea: {
            left: 0,
            top: 0,
            width: "100%",
            height: "90%"
        },
        animation: {
            duration: 1000,
            easing: "out"
        },
        hAxis: {
            textPosition: "none",
            gridlines: { color: "#fff" },
            minorGridlines: { color: "#fff", count: 0 }
        },
        vAxis: {
            textPosition: "in",
            maxValue: 100,
            minValue: 0,
            viewWindowMode: "maximized", 
            gridlines: { color: "#fff" },
            minorGridlines: { color: "#fff" }
        }
    };

    $.ajax({
        type: "GET",
        url: "Rest/SDPTrend",
        async: true,
        data: { "addressID": addressId, "type": type },
        success: function (result) {
            for (var i = 0; i < result.length; i++)
                data.addRow([(8 - i) + (i === result.length - 1 ? " week ago" : " weeks ago"), Math.round(result[i] * 10000) / 100]);

            var chart = new google.visualization.LineChart(document.getElementById("graph"));
            chart.draw(data, options);
            $("#trendGraph").css({
                "opacity": 1,
                "z-index": 3
            });
        },  // end success
        error: function (request, status, error) {
            var Insert = new ErrorInsert();
            Insert.PageName = "Index";
            Insert.StepName = "ajaxgetTrend";
            Insert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState === 4) {
                Insert.ErrorCode = "c-i-j-agt-01";
                return Insert;
            }   // end if readyState 4
            if (request.readyState === 0) {
                Insert.ErrorCode = "c-i-j-agt-02";
                return Insert;
            }   // end if readyState 0
        }   // end error
    }); // end Ajax
}

function hideTrend() {
    $("#trendGraph").css({
        "opacity": 0,
        "z-index": -10
    });
}

// ********************** Classes *******************
function cableContact() {
    var vendorNormName;
    var parentVendor;
    var contactName;
    var contactOffice;
    var contactMobile;
    var contactEmail;
}

function ErrorInsert() {
    var PageName;
    var StepName;
    var ErrorCode;
    var ErrorDetails;
}