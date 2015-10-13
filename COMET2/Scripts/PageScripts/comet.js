///#source 1 1 /Scripts/PageScripts/Activity.js
google.load("visualization", "1.0", { 'packages': ['corechart'] });
//google.setOnLoadCallback(getUsageByWeeksJson);

$(document).ready(function () {
    getUsageByWeeksJson();
 
    loadEnd();
});



function openMenu(option) {
    
}



// ***************** Creates Graph ************************ //
// Calls: AJAX(getUsageByWeeks)
// Returns: 
// Sets: 
function CreateUsageGraph(usageArray) {
    var colorsArray = ["#acacac"
            , "#34C6CD"
            , "#FFB473"
            , "#2A4380"
            , "#FFD173"
            , "#0199AB"
            , "#FF7600"
            , "#123EAB"
            , "#FFAB00"
            , "#1A7074"
            , "#A64D00"
            , "#466FD5"
            , "#BE008A"
            , "#B4F200"
            , "#DF38B1"
            , "#EF002A"
            ];
    var data = new google.visualization.DataTable();
    
    data.addColumn("string", "Week Start");
    var frequency = 2 * Math.PI / usageArray[0].length;
    // create the headings columns and colors array dynamically
    for (var i = 1; i < usageArray[0].length; i++) {
        var red = Math.sin(i * frequency + 0) * 127 + 128;
        var green = Math.sin(i * frequency + 2) * 127 + 128;
        var blue = Math.sin(i * frequency + 4) * 127 + 128;
        data.addColumn("number", usageArray[0][i]);
        colorsArray.push(RGB2Color(red, green, blue));
    }

    // add the data
    //data.addColumn({ type: "number", role: "annotation" });
    for (var i = 1; i < usageArray.length; i++) 
        data.addRows([usageArray[i]]);

    var options = {
        backgroundColor: "#efeeef",
        title: "Distinct Logins a day by group by week",
        titlePosition: "out",
        colors: colorsArray,
        lineWidth: 3,
        width: "100%",
        height: "100%",
        curveType: "function",
        explorer: {},
        hAxis: {
            textPosition: "none",
            slantedText: true,
            slantedTextAngle: 60
            //gridlines: {
            //    color: "#111",
            //    count: 5//data.getNumberOfRows()
            //}
        },
        legend: {
            position: 'top',
            textStyle: { fontSize: 13 }
        },
        chartArea: {
            left: 60,
            top: 70,
            width: "100%",
            height: "90%"
        },
        animation: {
            duration: 1000,
            easing: "out"
        },
        //selectionMode: "multiple",
        vAxis: {
            title: "Distinct Users"
        }
    };
    
    var chart = new google.visualization.LineChart(document.getElementById("usageByTime"));
    //google.visualization.events.addListener(chart, "select", function () {
    //    var arr = new Array();
    //    $(".grid").find("td").each(function () {
    //        arr.push($(this).attr("id"));
    //        $(this).removeClass("graphRowHover");
    //    });
    //    alert(arr);
    //    var selectedItem = chart.getSelection()[0];
    //    //alert("#cell" + selectedItem.column + "," + (selectedItem.row + 1));
    //    $("#cell" + selectedItem.column + "," + (selectedItem.row + 1)).addClass("graphRowHover");
    //});
    chart.draw(data, options);

    // create Data Table
    var table = document.createElement("table");
    $(table).addClass("grid");
    for (var i = 0; i < usageArray[1].length; i++) {
        var row = document.createElement("tr");
        for (var j = 0; j < usageArray.length; j++) {
            var cell = document.createElement("td");
            if (i == 0)
                cell = document.createElement("th");

                

            // start Formatting
            $(cell).addClass("graphCellHover");
            $(cell).hover(function () {
                $(this).closest("tr").addClass("graphRowHover");
            }, function () {
                $(this).closest("tr").removeClass("graphRowHover");
            });
            // end formatting

            cell.appendChild(document.createTextNode(usageArray[j][i]));
            // if heading, allow filter
            if (j === 0) {
                var userList = UsersByGroup(usageArray[j][i]);
                $(cell).css("width", "100px");
                
                if (i !== 0) $(cell).addClass("showing").addClass("rowHeader").attr("id", "t" + i).attr("title", "Hide/Show on graph \n\n" + 
                        UsersByGroup(usageArray[j][i])
                    );
                (function (d, opt, n) {                     // the onclick function
                    $(cell).click(function () {
                        view = new google.visualization.DataView(d);
                        if ($(this).hasClass("showing")) {
                            $(this).closest("tr").find("td").each(function () {
                                $(this).removeClass("showing").removeClass("graphCellHover").addClass("grapCellhHoverNotShow");
                            });
                        } else {
                            $(this).closest("tr").find("td").each(function () {
                                $(this).removeClass("grapCellhHoverNotShow").addClass("showing").addClass("graphCellHover");
                            });
                        }

                        var showColumnsArray = new Array();
                        // toggle
                        $(".rowHeader").each(function () {
                            if (!$(this).hasClass("showing"))
                                showColumnsArray.push(parseInt($(this).attr("id").substr(1, $(this).attr("id").length - 1)));
                            //view.hideRows([parseInt($(this).attr("id").substr(1, $(this).attr("id").length - 1))]);
                        });

                        view.hideColumns(showColumnsArray);
                        chart.draw(view, opt);
                    });
                })(data, options, i);
            } else {
                $(cell).addClass("cellNumberTight");
                if (i > 0) {
                    $(cell).attr("id", "cell" + i + "," + j);
                    $(cell).mouseover(function () {
                        var selectItem = chart.series;
                        var split = $(this).attr("id").substr(4, $(this).attr("id").length - 4).split(",");
                    });
                }
            }
            row.appendChild(cell);
        }
        table.appendChild(row);
    }
    $("#graphDetails").html(table);
        
    // show the graph in full-screen mode
    $("#showFullScreen").on("click", function (e) {
        $("#usageByTime").removeClass("graph").css({
                "position": "fixed",
                "left": 0,
                "top": 0,
                "z-index": 5,
                "width": "100%",
                "height": "100%",
                "margin": 0
        });

        $(this).css("z-index", 1);
        
        // create the Div for closing the full-screen
        var closeDiv = document.createElement("div");
        $(closeDiv).addClass("float-right-absolute").click(function () { 
            $("#usageByTime").css({
                "position": "",
                "left": "",
                "top": "",
                "z-index": "",
                "width": "",
                "height": "",
                "margin": ""
            }).addClass("graph");
            $("#closeButtonDiv").remove();
            chart.draw(data, options);
            $("#showFullScreen").css("z-index", 10);
        }).css({
            "min-width": "20px",
            "min-height": "20px",
            "background-image": "url(../Images/x-circle.png)",
            "background-size": "cover",
            "cursor": "pointer"
        }).attr("id", "closeButtonDiv");

        closeDiv.appendChild(document.createTextNode("  "));
        document.body.appendChild(closeDiv);
        chart.draw(data, options);
    });
}





// ***************** returns object of ActivityByWeeks ************************ //
// Calls: AJAX(getUsageByWeeks)
// Returns: 
// Sets: 
function getUsageByWeeksJson() {
    var usageWeeks = new Array();
    var usageData = new Array();
    var usageArray = new Array();
    setTimeout(function () {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "GetUsageByWeeks",
            async: false,
            success: function (data) {
                /*
	                column 1 = Week Start Date
	                column 2-n  = Department Name
                */
                var weekList = new Array();
                var deptArray = new Array();
                var usageArray = new Array();

                deptArray.push("Week Start");
                // create an Array of weeks to check off of
                for (var i = 0; i < data.length; i++) {
                    if (jQuery.inArray(data[i].WeekStart, weekList) === -1)
                        weekList.push(data[i].WeekStart);
                    if (jQuery.inArray(data[i].Group, deptArray) === -1)
                        deptArray.push(data[i].Group);
                }
                
                var newDeptArray = deptArray.slice(0);
                usageArray.push(newDeptArray);
                deptArray.shift();
                for (var i = 0; i < weekList.length; i++) {
                    var tempArray = new Array();
                    tempArray.push(weekList[i]);
                    for (var j = 0; j < deptArray.length; j++) 
                        tempArray.push(data[i * deptArray.length + j].DistinctLogins);
                    usageArray.push(tempArray);
                }
                
                CreateUsageGraph(usageArray);
            },
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ajaxGetUsageByWeeks";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-g-j-ageubw-01";
                }   // end if readyState 4
            }   // end error
        });
    }, 5);
}




// ***************** returns string of Users ************************ //
// Calls: AJAX(UsersInGroup)
// Returns: 
// Sets: 
function UsersByGroup(group) {
    var users = ""

    $.ajax({
        type: "POST",
        data: { "userGroup": group },
        url: "GetUsersInGroup",
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++)
                users += data[i] + "\n";
        },
        error: function (request, status, error) {
            var Insert = new ErrorInsert();
            Insert.PageName = "Index";
            Insert.StepName = "ajaxGetUsageByWeeks";
            Insert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState == 4) {
                Insert.ErrorCode = "c-g-j-ageubw-01";
            }   // end if readyState 4
        }   // end error
    });

    return users;
}






// ***************** creates dynamic color scheme for legend ************************ //
// Calls: 
// Returns: 
// Sets: 
function RGB2Color(r, g, b) {
    return '#' + byte2Hex(r) + byte2Hex(g) + byte2Hex(b);
}
function byte2Hex(n) {
    var nybHexString = "0123456789ABCDEF";
    return String(nybHexString.substr((n >> 4) & 0x0F, 1)) + nybHexString.substr(n & 0x0F, 1);
}






// ***************** creates expand and collapse on pivot ************************ //
// Calls: 
// Returns: 
// Sets: 
function expandCollapse(group, focus, vendor) {
    setTimeout(loadStart(), 500);
    setTimeout(function () {
        if (focus.src.match("expand")) {
            focus.src = "../../Images/collapse.gif";
            $("#" + group + " > tbody > tr").each(function () {
                if ($(this).attr('id') === vendor)
                    $(this).show();
            }); // end foreach
        } else {
            focus.src = "../../Images/expand.gif";
            $("#" + group + " > tbody > tr").each(function () {
                if ($(this).attr('id') === vendor)
                    $(this).hide();
            }); // end foreach
        }   // end if
    }, 1000);
    setTimeout(loadEnd(), 10000);
}
///#source 1 1 /Scripts/PageScripts/console.2.js
/// <reference path="Global.js" />
var slickGrid;
function openItem(link) {
    var item;
    var isPartial = link.toLowerCase().indexOf("partial") > -1;
    if (isPartial) {
        var reverse = link.split("").reverse().join("");
        item = parseFloat(reverse.substring(0, reverse.indexOf("/")).split("").reverse().join(""));
        reverse = reverse.substring(reverse.indexOf("/") + 1);
        link = reverse.split("").reverse().join("");
        var type = link.replace("Partial", "");

        history.pushState({ "item": item, "link": link },
                            "Dashboard - " + type + "#" + item,
                            parseUrl() + "/" + type + "/" + item);
        var data = type === "Project"
            ? { "projectID": item }
                : type === "Request"
                ? { "requestID" : item }
                    : { "elementID" : item };
        runAjax(ajax("ready", "displayDiv", isPartial ? data : null, link).OpenItem);
    } else {
        location.href = "console/" + link;
    }
}

function openMenu(element, option) {
    var title = ""
    switch (option) {
        case "PendingPromotes":
            title = "Manager - Pending";
            runAjax(ajax("openMenu", "displayDiv", null, "PendingRequest").Pending);
            break;
        default :
    }
    history.pushState(null, title, parseUrl() + "/" + option);
    $("#menu ul li").each(function() {
        $(this).removeClass("selected");
    });
    $(element).addClass("selected");
}

function parseUrl() {
    var url = window.location.href
                    .replace("/R", "/r")
                    .replace("/E", "/e")
                    .replace("/P", "/p")
                    .replace("/A", "/s")
                    .replace("/S", "/s")
                    .replace("/ManageGroups", "/managegroups")
                    .replace("#", "");
    var href = url.split("/r");
    if (href.length <= 1)
        href = url.split("/e");
    if (href.length <= 1)
        href = url.split("/p");
    if (href.length <= 1)
        href = url.split("/managegrou")
    if (href.length <= 1)
        href = url.split("/a");
    if (href.length <= 1)
        href = url.split("/s");
    return href[0];
}

function getGrid() {
    history.pushState(null, "Dashboard", parseUrl());
    //runAjax(ajax("ready", "displayDiv", null, "GridAsJson").SlickGrid);
    runAjax(ajax("ready", "displayDiv", null, "Grid").Grid);
}

function showParent() {
    $(".tree").unbind("click");
    if ($("#hasParent").length > 0) {
        if ($(".tree").hasClass("invisible")) {
            $(".tree").css("opacity", 0);
            $(".tree").removeClass("invisible");
            setTimeout(function() { $(".tree").animate({ opacity : 1 }, 300); }, 300);
        }
        $(".tree").attr("tooltip", $("#parentName").val());
        $(".tree").bind("click", function() { openItem($("#hasParent").val()); });
    } else {
        $(".tree").animate({ opacity : 0 }, 200);
        $(".tree").addClass("invisible");
    }
}

function bindInputs() {
    //date range pickers
    var datePickerConfig = {
        format      : "MM/DD/YYYY",
        separator   : "-"
    };

    $("div.date-picker-wrapper").each(function() {
        $(this).remove();
    });

    $("input.dateRange").each(function() {
        $(this).dateRangePicker(datePickerConfig)
            .keyup(function(event) {
                if (event.keyCode === 13)
                    rerunGrid();
            })
            .bind("datepicker-closed", function() {
                if ($(this).val().indexOf("-") >= 10)
                    rerunGrid();
            });
    });

    // multi-selects
    $("select[multiple]")
        .multiselect({
            show    : ["blind", 100],
            hide    : ["blind", 400]
        })
        .multiselectfilter({
            label       : "",
            autoReset   : true
        });

    $(".filter")
        .focusout(function() { rerunGrid(); })
        .keyup(function (event) {
            if (event.keyCode === 13)
                rerunGrid();
        });
}

function rerunGrid(clearParam) {
    var data = null;
    if (!clearParam) {
        data = {
            "ID": $("#SelectedId").val(),
            "requestorIdList"   : ($("#SelectedRequestors").size() === 1 && $("#SelectedRequestors").val() === null
                                            ? null : $("#SelectedRequestors").val()),
            "assignedIdList"    : ($("#SelectedAssignee").size() === 1 && $("#SelectedAssignee").val() === null
                                            ? null : $("#SelectedAssignee").val()),
            "category"          : ($("#SelectedCategory").size() === 1 && $("#SelectedCategory").val() === null
                                            ? null : $("#SelectedCategory").val()),
            "requestType"       : ($("#SelectedType").size() === 1 && $("#SelectedType").val() === null
                                            ? null : $("#SelectedType").val()),
            "area"              : ($("#SelectedArea").size() === 1 && $("#SelectedArea").val() === null
                                            ? null : $("#SelectedArea").val()),
            "summary"           : $("#EnteredSummary").val(),
            "submittedRange"    : $("#SubmittedRange").val(),
            "dueDateRange"      : $("#DueDateRange").val(),
            "status"            : ($("#SelectedStatus").size() === 1 && $("#SelectedStatus").val() === null
                                            ? null : $("#SelectedStatus").val()),
            "closedRange"       : $("#ClosedRange").val()
        };
    }
    //runAjax(ajax("ready", "displayDiv", data, "GridAsJson").SlickGrid);
    runAjax(ajax("ready", "displayDiv", data, "Grid").Grid);
    return false;
}

function evalHref() {
    var href = window.location.href;
    var stripoutString = "manager/dashboard/";
    if (href.toLowerCase().indexOf(stripoutString) > 1) {
        href = href.substring(href.toLowerCase().indexOf(stripoutString) + stripoutString.length);
        if (href.length > 0 && href.indexOf("/") < 0)
            $("#" + href).addClass("selected");
    }
}

function promote(id) {
    if ($("#startDate").val() == null || $("#startDate").val().length !== 10) {
        alert("Please enter Start Date.");
        return;
    }

    var form = document.createElement("form");
    form.method = "POST";
    form.action = "/console/manager/PromoteToProject";

    $(form).append(createInput("number", "requestID", id));
    $(form).append($("#startDate"));
    console.log(form);

    $("#body").css("display", "none").append(form); 
    form.submit();

    function createInput(type, name, value) {
        return $("<input>").attr("type", type).attr("name", name).val(value);
    };
}

$(document).ready(function () {
    evalHref();

    showParent();
    window.addEventListener("popstate", function (event) {
        if (event.state !== null && typeof event.state !== "undefined") {
            var type = event.state.link.replace("Partial", "");
            var data = type === "Project" ? { "projectID": event.state.item } : type === "Request" ? { "requestID": event.state.item } : { "elementID": event.state.item };
            runAjax(ajax("ready", "displayDiv", data, event.state.link).OpenItem);
        } else
            rerunGrid(true);

        evalHref();
    });

    var tOut = null;
    $("li.ellipse").hover(
        function () {
            var $this = $(this);
            tOut = setTimeout(function () {
                $this.children().first().addClass("showEllipse").css({
                    top: $this.offset().top - $this.parent().offset().top + $this.parent().position().top + 2
                });
            }, 500);
        }, function () {
            clearTimeout(tOut);
            $(this).children().first().removeClass("showEllipse");

        }
    );

    //bind inputs
    bindInputs();

    //modify multiselect prototype
    var oldMultiselectCloseFn = $.ech.multiselect.prototype.close;
    $.ech.multiselect.prototype.close = function () {
        var result = oldMultiselectCloseFn.apply(this, arguments);
        rerunGrid();
        return result;
    };
    
    $("#globalSearch").autocomplete({
        delay: 750,
        focus: function(event) {
            event.preventDefault();
        },
        source: function (request, response) {
            var data = { "searchContext": $("#globalSearch").val() };
            $.get("/console/SearchConsole", data
                , function (result) {
                    var results = [];
                    for (var i = 0; i < result.length; i++) {
                        results.push({
                            "label": result[i].Text,
                            "value": result[i].RelativeUrl,
                            "id": i,
                            "title": result[i].ToolTip
                        });
                    }
                    response(results);
                });
        },
        select: function (event, ui) {
            event.preventDefault();
            $("#globalSearch").text("").html("");
            openItem(ui.item.value);
        },
        open: function () {
            $(".ui-autocomplete").css({
                //"max-width": "320px",
                "background-color": "#FFFFFF",
                "font-size": "0.7em",
                "max-height": "350px",
                "overflow-y": "scroll",
                "overflow-x": "hidden"
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
});

function ajax(funcName, updateLocation, data, references) {
    "use strict";
    var nullData, nullContentType, nullBeforeSend;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && (typeof data === "object" || typeof data === "string")) ? data : null;

    if (updateLocation !== null && typeof updateLocation === "string")
        var errorPanel = updateLocation;

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
        OpenItem:
            new ajaxCall(ajaxDetails().HttpType.POST,
                "/console/" + references,
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                function () {
                    $("#" + updateLocation + " > div").addClass("centerContents").html("<img src='/Images/loading.gif' />");
                },
                function (result) {
                    $("#" + updateLocation + " > div").removeClass("centerContents").html(result);
                    showParent();
                    var type = history.state.link.replace("Partial", "");
                    document.title = type + " #" + history.state.item;
                    $(".site-title").html(document.title);

                    ///////////////////////////////// THIS NEEDS TO BE CORRECTED BEFORE FINAL PUBLICATION ////////////////////////////////////
                    //$(".editable").each(function () {
                    //    $(this).find("button").remove();
                    //    $(this).removeClass("editable");
                    //});
                    //$(".uneditable").each(function () {
                    //    $(this).removeClass("uneditable");
                    //});
                },
                function (request, status, error) {
                    $("#" + errorPanel + " > div").html("Unable to load due to " + error);
                }),
        Grid:
            new ajaxCall(ajaxDetails().HttpType.POST,
                "/console/" + references,
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                function () {
                    $("#" + updateLocation + " > div").addClass("centerContents").html("<img src='/Images/loading.gif' />");
                },
                function (result) {
                    $("#" + updateLocation + " > div").removeClass("centerContents").html(result);
                    showParent();
                    document.title = "Dashboard";
                    $(".site-title").html(document.title);
                    bindInputs();
                },
                function (request, status, error) {
                    $("#" + errorPanel + " > div").html("Unable to load due to " + error);
                }),
        SlickGrid:
            new ajaxCall(ajaxDetails().HttpType.POST,
                "/console/" + references,
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                function () {
                    $("#" + updateLocation + " > div").addClass("centerContents").html("<img src='/Images/loading.gif' />");
                },
                function (result) {
                    $("#" + updateLocation + " > div").removeClass("centerContents");
                    var columns = [];
                    for(var i = 0; i < result[0].length; i++) {
                        columns.push({ 
                            id: columns[i],
                            name: columns[i],
                            field: columns[i]
                        });
                    }

                    slickGrid = new Slick.Grid($("#" + updateLocation + " > div"), result, columns, null);
                    showParent();
                    document.title = "Dashboard";
                    $(".site-title").html(document.title);
                    alert(document.title);
                    //bindInputs();
                },
                function (request, status, error) {
                    $("#" + errorPanel + " > div").html("Unable to load due to " + error);
                }),
        Pending:
            new ajaxCall(ajaxDetails().HttpType.POST,
                "/manager/" + references,
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                function () {
                    $("#" + updateLocation + " > div").addClass("centerContents").html("<img src='/Images/loading.gif' />");
                },
                function (result) {
                    $("#" + updateLocation + " > div").removeClass("centerContents").html(result);
                    showParent();
                    document.title = "Manager - Pending";
                    $(".site-title").html(document.title);
                },
                function (request, status, error) {
                    $("#" + errorPanel + " > div").html("Unable to load due to " + error);
                })
    };
}
///#source 1 1 /Scripts/PageScripts/E911JS.js
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




///#source 1 1 /Scripts/PageScripts/Global.js
function Comet() {
    this.nameCtrl = null;
    this.LAST_PAGE_MOVE = new Date().getTime();
    this.LAST_REQUEST_CHECK_TIME = new Date().getTime();
    this.PAGE_REFRESH_TIME = 3600000;
    this.REFRESH_REQUESTS_TIME = 60000;

    updateRefreshTime = function() {
        this.LAST_PAGE_MOVE = new Date().getTime();
    };

    this.updateRequestCheckTime = function() {
        this.LAST_REQUEST_CHECK_TIME = new Date().getTime();
    };
}

function ErrorInsert(pageName, stepName, errorCode, errorDetails) {
    this.PageName       = pageName;
    this.StepName       = stepName;
    this.ErrorCode      = null;
    this.ErrorDetails   = null;

    if (errorCode !== null && typeof errorCode === "string")
        ErrorCode = errorCode;
    if (errorDetails !== null && typeof errorDetails === "string")
        ErrorDetails = errorDetails;

    this.logError = function (location) {
        runAjax(ajax("ErrorInsert", null, this, "Grid").InsertError);
        console.log("Unimplemented: Log error: " + this.ErrorDetails);
        $(location).html(ErrorInsert.ErrorDetails);
    }
}

var Global = {
    PAGE_NAME: "Global"
}

Comet.prototype = {
    setNameCtrl: function(ctrl) {
        this.nameCtrl = ctrl;
    },
    TODAY: function() {
        var today = new Date();
        today.setDate(today.getDate());
        return today;
    },
    NOW: function() {
        return new Date().getTime();
    },
    setRefresh: function() {
        var that = this;
        var method = function() { that.setRefresh(); };
        if (this.NOW() - this.LAST_PAGE_MOVE >= 21600000)
            window.location.reload(true);
        else
            setTimeout(method, this.PAGE_REFRESH_TIME);
    },
    refreshData: function() {
        runAjax(ajaxCall(
            ajaxDetails().HttpType.POST,
            "/Rest/RefreshAppData",
            null,
            ajaxDetails().Synchronous.TRUE,
            ajaxDetails().ContentType.JSON,
            function() {
                loadStart();
            },
            function(result) {
                alert(result);
                this.updateRefreshTime();
                location.reload();
            },
            function (request, status, error) {
                
            }));
    },
    getNewRequests: function() {
        var that = this;

        var method = function() { that.getNewRequests(); };
        if (this.NOW() - this.LAST_REQUEST_CHECK_TIME >= this.REFRESH_REQUESTS_TIME) {
            runAjax(ajaxCall(
                    ajaxDetails().HttpType.POST,
                    "/Rest/Requests",
                    null,
                    ajaxDetails().Synchronous.TRUE,
                    ajaxDetails().ContentType.DEFAULT,
                    ajaxDetails().nullBeforeSend,
                    function(result) {
                        if (result.length > 0) {
                            if ($("#consoleUnread").length === 0) {
                                var notification = document.createElement("div");
                                $(notification)
                                    .attr("id", "consoleUnread")
                                    .addClass("notification");
                                CorrectNotification();
                                $("#menuDiv").append(notification);
                            }
                            $("#consoleUnread").html(result.length);
                        } else {
                            if ($("#consoleUnread") !== null && typeof $("#consoleUnread") !== "undefined")
                                $("#consoleUnread").remove();
                        }
                    },
                    function(request, status, error) {
                        //alert(error);
                    })
            );
            this.updateRequestCheckTime();
        }
        setTimeout(method, this.REFRESH_REQUESTS_TIME);
    },
    CorrectNotification: function() {
        $("#consoleUnread").css({
            "left": $("#consoleAnchor").offset().left + $("#consoleAnchor").width() - 15,
            "top": $("#consoleAnchor").offset().top - 8,
        });
    }
};

var userTimer = null
function showUserDetails(element) {
    var $this = $(element);
    if ($(element).data("events") === null || typeof ($(element).data("events") === "undefined")) {
        $(element).bind({
            mouseleave: function() {
                clearTimeout(userTimer);
                $("#" + $this.prop("id") + "User").css("visibility", "hidden");
            }
        });
    }
    userTimer = setTimeout(function() {
        $("#" + $this.prop("id") + "User").css("visibility", "visible");
    }, 500);
}



/**
 * Page Refresh Embed
 */
$(document).ready(function() {
    var comet = new Comet();
    comet.setRefresh();
    comet.LAST_REQUEST_CHECK_TIME = comet.LAST_REQUEST_CHECK_TIME - comet.REFRESH_REQUESTS_TIME;
    comet.getNewRequests();
    $(window).resize(comet.CorrectNotification);

    var ver = getInternetExplorerVersion();
    if(ver > -1 && ver < 10.0)
        alert("Your are using an outdated version of IE. Comet recommends that you use IE10, Chrome, or Firefox.");

    //$('ul#menu').jqsimplemenu();

    $("div#body").scroll(function () {
        var scroll = $("div#body").scrollTop();
        if (scroll > 0) 
            $("header").addClass("scroll");
        else 
            $("header").removeClass("scroll");
    });
    
    if ($("#sawNewVersion").html().toLowerCase() === "false") {
        $.ajax({
            type: "GET",
            url: "/Rest/ShowNav",
            async: true,
            success: function(data){
                $("#showNav").html(data);
            },
            error: function(request, status, error) {
                var e = request.responseText + " --- " + error;
                var insert = new ErrorInsert(Global.PAGE_NAME, "showNav", "g-r-sn-01", e);
                insert.logError();
            }   // end error
        });
        $("#showNav").css("display", "block");
    }
    
    $("a#refresh").click(function() {
        comet.refreshData();
    });

    // attach a datepicker to date inputs 
    $("input[type='date']").each(function() {
        if (this.type !== "date") {
            $.datepicker.formatDate("MM/dd/yyyy");
            $(this).datepicker({
                buttonImage: "calendar.gif",
                buttonText: "Pick a date",
                beforeShowDay: $.datepicker.noWeekends
            });
        }
    });
});

function closeNav(neverShowAgain) {
    if (neverShowAgain) {
        $.ajax({
            type: "GET",
            url: "/Rest/MarkNewVersionAcknowledgement",
            async: true,
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ajaxCloseNav";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-g-j-cn-01";
                }   // end if readyState 4
            }   // end error
        });
    }
    $("#showNav").fadeOut("slow");
}

/**
 * Returns Capitalized names without all Upper
 */ 
function toTitleCase(str) {
    return str.replace(/\w\S*/g, function(txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
}

var dimensions = [];
function minMaxParent(id) {
    if ($(id).hasClass("minmaxOpen")) {
        $(id).parent().children().each(function() {
            if ($(this).attr("id") !== $(id).attr("id"))
                $(this).fadeOut(200);
        });

        $(id).parent().parent().children().each(function () {
            $(this).animate(
            { "height": 0 }, 300);
        });

        // check if Dimensions has been stored already
        var foundID = false;
        $.each(dimensions, function(index, value) {
            if ($(id).attr("id") === value.id)
                foundID = true;
        });
        if (!foundID) {
            dimensions.push({
                "id": $(id).attr("id"),
                "width": $(id).parent().css("width"),
                "height": $(id).parent().css("height"),
                "padding": $(id).parent().css("padding")
            });
        }
        $(id).parent().animate({
            "width": "15px",
            "height": "15px",
            "padding": "0px",
            "border-radius": "7px"
        }, 500);

        $(id).removeClass("minmaxOpen").addClass("minmaxClose");
    } else {                        // if opening back up
        $.each(dimensions, function(index, value) {
            if ($(id).attr("id") === value.id) {
                $(id).parent().animate({
                    "width": value.width,
                    "height": value.height,
                    "padding": value.padding
                }, 400, function() {
                    $(this).css({
                        "height": "auto",
                        "border-radius": "0" //value.borderRadius
                    });
                });
            }
        });

        $(id).parent().find(".content").each(function() {
            if(this.hasChildNodes())
                $(this).parent().fadeIn(900);
        });

        $(id).parent().parent().children().each(function () {
            $(this).animate(
            { "height": "auto" }, 300);
        });
        $(id).removeClass("minmaxClose").addClass("minmaxOpen");
    }
}

// Loading Functions
function loadEnd() {
    $("#loadImage").stop().animate({
        opacity: 0
    }, 1000);
    $(".loading").stop().animate({
        opacity: 0
    }, 800);
    setTimeout(function() {
        $(".loading").css("z-index", "-10");
        $("#loadImage").css("z-index", "-10");
    }, 1800);
}
function loadStart() {
    $(".loading").css("z-index", "99");
    $("#loadImage").css("z-index", "120");
    setTimeout(function() {
        $(".loading").stop().animate({
            opacity: 0.8,
            "background-color": "#000"
        }, 200);
    }, 0);
    setTimeout(function() {
        $("#loadImage").stop().animate({
            opacity: 1
        }, 200);
    }, 0);
}

function getInternetExplorerVersion() {
    // Returns the version of Internet Explorer or a -1
    // (indicating the use of another browser).

    var rv = -1; // Return value assumes failure.
    if (navigator.appName === "Microsoft Internet Explorer") {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) !== null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

function supportsHtml5Storage() {
    try {
        return 'localStorage' in window && window.localStorage !== null;
    } catch (e) {
        return false;
    }
}

function createStorageExpiration() {
    var expiration = new Date();
    expiration.setDate(expiration.getDate() + 1);
    return expiration.setHours(8, 0, 0, 0);
}

function trimFuncName(func) {
    var name = func.substr('function '.length);
    name = name.substr(0, name.indexOf('('));
    return name;
}

function ajaxDetails() {
    return {
        ContentType: {
            DEFAULT: "application/x-www-form-urlencoded; charset=UTF-8",
            NULL: null,
            JSON: "application/json; charset=utf-8",
            TEXT: "text/plain",
            HTML: "html"
        },
        HttpType: {
            GET: "GET",
            POST: "POST"
        },
        Synchronous: {
            TRUE: true,
            FALSE: false
        }
    };
}

//----------------------------------------- Refactored Ajax Call methods --------------------------------------
function ajaxCall(
            httpType,
            queryString,
            data,
            synchronous,
            contentType,
            beforeSendDelegate,
            handleSuccessDelegate,
            handleFailureDelegate) {

    if (hasValidParams === false)
        return null;

    var _queryString = queryString;
    var _data = data;
    var _httpType = httpType;
    var _contentType = contentType;
    var _beforeSendDelagate = beforeSendDelegate;
    var _handleSuccessDelegate = handleSuccessDelegate;
    var _handleFailureDelegate = handleFailureDelegate;

    var hasValidParams = function() {
        if (typeof queryString !== "string")
            return false;

        if (typeof httpType !== "string")
            return false;

        if (typeof contentType !== "string" || contentType === null)
            return false;

        if (typeof data === "object" && data !== null)
            if (Array.isArray(data) !== true)
                return false;

        if ((typeof handleSuccessDelegate === "object" && handleSuccessDelegate !== null) === false)
            return false;

        if ((typeof handleFailureDelegate === "object" && handleFailureDelegate !== null) === false)
            return false;

        if ((typeof synchronous !== "boolean"))
            return false;

        return true;
    };

    return {
        QueryString: _queryString,
        Data: _data,
        HttpType: _httpType,
        ContentType: _contentType,
        Synchronous: synchronous,
        BeforeSendDelegate: function() { return(_beforeSendDelagate === null || typeof _beforeSendDelagate === "undefined") ? null : _beforeSendDelagate(); },
        SuccessDelegate: function(data) { return _handleSuccessDelegate(data); },
        FailureDelegate: function(request, status, error) { return _handleFailureDelegate(request, status, error); }
    };
}


function runAjax(ajaxCallInstance) {
    if (typeof ajaxCallInstance === "object" && ajaxCallInstance !== null) {
        if (ajaxCallInstance.ContentType !== null && ajaxCallInstance.Data !== null)
            $.ajax({
                type: ajaxCallInstance.HttpType,
                contentType: ajaxCallInstance.ContentType,
                data: ajaxCallInstance.Data,
                url: ajaxCallInstance.QueryString,
                async: ajaxCallInstance.Synchronous,
                //xhrFields:      { widthCredentials: true },
                beforeSend: function() {
                    ajaxCallInstance.BeforeSendDelegate();
                },
                success: function(data) {
                    ajaxCallInstance.SuccessDelegate(data);
                },
                error: function(request, status, error) {
                    ajaxCallInstance.FailureDelegate(request, status, error);
                }
            });
        else
            $.ajax({
                type: ajaxCallInstance.HttpType,
                url: ajaxCallInstance.QueryString,
                async: ajaxCallInstance.Synchronous,
                beforeSend: function() {
                    ajaxCallInstance.BeforeSendDelegate();
                },
                success: function(data) {
                    ajaxCallInstance.SuccessDelegate(data);
                },
                error: function(request, status, error) {
                    ajaxCallInstance.FailureDelegate(request, status, error);
                }
            });
    }
}

function Ajax(funcName, updateLocation, data, references) {
    "use strict";
    var nullData, nullContentType, nullBeforeSend;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && (typeof data === "object" || typeof data === "string")) ? data : nullData;

    if (updateLocation !== null && typeof updateLocation === "string")
        errorPanel = updateLocation;
    return {
        NewRequests: new ajaxCall(ajaxDetails().HttpType.GET,
            "Rest/Requests",
            data,
            ajaxDetails().Synchronous.TRUE,
            ajaxDetails().ContentType.JSON,
            nullBeforeSend,
            function(result) {
                $("#" + updateLocation).html(result.length);
            },
            function(request, status, error) {
            }),
        InsertError: new ajaxCall(ajaxDetails().HttpType.POST,
            "Rest/JSError",
            data,
            ajaxDetails().Synchronous.FALSE,
            ajaxDetails().ContentType.JSON,
            nullBeforeSend,
            function() {

            },
            function(request, status, error) {
            })
    };
}
///#source 1 1 /Scripts/PageScripts/Index.subactions.js
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
///#source 1 1 /Scripts/PageScripts/IndexJS.js
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

///#source 1 1 /Scripts/PageScripts/NNI_Calc.js
$(document).ready(function () {
    $("input[type='radio']").click(function() { 
        calculateTotal();
    });
    $("input[type='number']").change(function () {
        calculateTotal();
    });
    loadEnd();
});

function select(id) {
    $("input[value=" + id + "]").prop("checked", true);
    calculateTotal();
}

function calculateTotal() {
    //alert($("input[type='radio'][name='gb']:checked").val());
    //alert($("input[type='radio'][name='gateway']:checked").val());
    var perMeg = $("input[type='radio'][name='gb']:checked").val() == "oneGB" ? 1000 : 10000;
    var nniMRC = $("input[type='radio'][name='gateway']:checked").val() == "yes" ?
        0 : (perMeg == 1000 ? 885.28 : 954);
    
    //alert("((" + $("#NNI_MRC").val().toString() + " + " + nniMRC.toString() + ") / " + perMeg.toString() + " * " + $("#EVC").val() + " * .75)");
    $("#Total").val("$" + Math.round(((Number($("#NNI_MRC").val()) + nniMRC) / perMeg * Number($("#EVC").val()) * 1.25) * 100) / 100);
}
