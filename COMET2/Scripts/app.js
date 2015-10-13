;google.load("visualization", "1.0", { 'packages': ['corechart'] });
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
;/// <reference path="Global.js" />
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
            "type"              : ($("#SelectedType").size() === 1 && $("#SelectedType").val() === null
                                            ? null : $("#SelectedType").val()),
            "area"              : ($("#SelectedArea").size() === 1 && $("#SelectedArea").val() === null
                                            ? null : $("#SelectedArea").val()),
            "summary"           : $("#EnteredSummary").val(),
            "submittedRange"    : $("#SubmittedRange").val(),
            "dueDateRange"      : $("#DueDateRange").val(),
            "status"            : ($("#SelectedStatus").size() === 1 && $("#SelectedStatus").val() === null ? null : $("#SelectedStatus").val()),
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
    loadEnd();
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
;var ilecMarkers = new Array();
var nniMarkers = new Array();
var highMarkers = new Array();
var lowMarkers = new Array();

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
    "../../Images/SDP01.png",
    "../../Images/SDP02.png",
    "../../Images/SDP03.png",
    "../../Images/SDP04.png",
    "../../Images/SDP05.png",
    "../../Images/SDP06.png",
    "../../Images/SDP07.png",
    "../../Images/SDP08.png",
    "../../Images/SDP09.png",
    "../../Images/SDP10.png",
    "../../Images/SDP11.png",
    "../../Images/SDP12.png",
    "../../Images/SDP13.png",
    "../../Images/SDP14.png",
    "../../Images/SDP15.png"
];

var slocationIcons = [
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
;/*@cc_on@*/
var MapColors = {
    yellow: "#F2FA00",
    white: "#eee",
    black: "#111",
    blue: "#0000FF",
    purple: "#4000BF",
    pink: "#FF00FF",
    orange: "#FF7700",
    darkWhite: "#ddd",
    lightBlack: "#333",
    red: "#FF0000"
};

var BoundaryType = {
    RATECENTER: "rateCenter",
    RATECENTERLABEL: "rateCenterLabel",
    PSAP: "psap",
    COUNTY: "county",
    COUNTYLABEL: "countyLabel",
    LATA: "lata"
};

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
                    $(updateLocation).html(result);
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) { insert.ErrorCode = "c-e-j-ld-01"; }
                    if (request.readyState === 2) { insert.ErrorCode = "c-e-j-ld-02"; }
                    insert.logError(errorPanel);
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
                    insert.logError(errorPanel);
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
                    insert.logError(errorPanel);
                })
    }
}

var mapListener;
var mapMoveListener;
var localStorageAllowed;
var countyMarkers = new Array();
var rateCenterMarkers = new Array();
var psapMarkers = new Array();
var lataMarkers = new Array();
var swcMarkers = new Array();
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
            position: google.maps.ControlPosition.LEFT_BOTTOM
        },
        mapTypeId: google.maps.MapTypeId.ROADMAP  // ROADMAP
    }); // end map creation
    infowindow = new google.maps.InfoWindow();
    clickInfowindow = new google.maps.InfoWindow();

    var input = document.getElementById("pac-input");
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    $("#searchCriteria").push(input);
    $(input).css("font-size", "0.8em");                                             // override default font-size of google
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

    google.maps.event.addListener(map, "drag", function () {
        clearTimeout(mapMoveListener);
    });

    google.maps.event.addListener(map, "idle", function () {
        mapMoveListener = setTimeout(function () {
            if (map.getZoom() >= 10) {
                $("#CountyPolyShowHideMap").prop("disabled", false);
                if ($("#CountyPolyShowHideMap").prop("checked"))
                    getCounty($("#CountyPolyShowHideMap"));

                $("#CountyLabelShowHideMap").prop("disabled", false);

                $("#RateCenterPolyShowHideMap").prop("disabled", false);
                if ($("#RateCenterPolyShowHideMap").prop("checked"))
                    getRateCenter($("#RateCenterPolyShowHideMap"));

                $("#RateCenterLabelShowHideMap").prop("disabled", false);

                $("#PsapShowHideMap").prop("disabled", false);
                if ($("#PsapShowHideMap").prop("checked"))
                    getPsap($("#PsapShowHideMap"));

                $("#LataShowHideMap").prop("disabled", false);
                if ($("#LataShowHideMap").prop("checked"))
                    getLata($("#LataShowHideMap"));

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
                mapListener = null;
            }
        }, 500);
        searchBox.setBounds(map.getBounds());
    }); // end bounds_changed eventlistener

    loadEnd();
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
    runAjax(indexAjax(funcName, $("#selectDetail"), data, null).LocDetails);
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
        if (map.getZoom() > 10)
            $("#selectDetail").html("Please click on map to get full details.");
        else
            $("#selectDetail").html("Please zoom in and click on map to get full details");
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
    if (type === BoundaryType.PSAP || type === BoundaryType.LATA)
        var polygon = {
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
        var polygon = {
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
                id = "#PsapShowHideMap";
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
                id = "#LataShowHideMap";
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



// ***************** shows/hides searchable fields ************************ //
// Calls: ***DEPRECATED***
// Returns: 
// Sets: 
function searchTableSelect(field, nofade) {
    if (!(nofade === null || typeof nofade === "undefined") && nofade) {
        $("#searchCriteria").children("span").each(function () {
            $(this).attr("onclick", "");
        });
        $("#searchMethods").children().each(function () {
            $(this).css("display", "none");
        });

        $("#" + field.id + "SearchDiv").css("display", "block");

        $("#searchCriteria").children("span").each(function () {
            if (this.id == field.id)
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
            $(this).attr("onclick", "searchTableSelect(this)");
        });
    } else {
        $("#searchCriteria").children("span").each(function () {
            $(this).attr("onclick", "");
        });
        $("#searchMethods").children().each(function () {
            $(this).fadeOut(500);
        });
        setTimeout(function () {
            $("#" + field.id + "SearchDiv").fadeIn(800);
        }, 550);
        setTimeout(function () {
            $("#searchCriteria").children("span").each(function () {
                if (this.id == field.id)
                    $(this).addClass("selected");
                else
                    $(this).removeClass("selected");
                $(this).attr("onclick", "searchTableSelect(this)");
            });
        }, 600);
    }
}

function ShowLocationSwitch(field) {
    $("#location").children("span").each(function () {
        $(this).attr("onclick", "");
    });
    $("#location").children("div").each(function () {
        $(this).fadeTo(500, 0, null);
        $(this).css("z-index", -1);
    });
    setTimeout(function () {
        $("#" + field.id + "Div").fadeTo(800, 1, null);
        $("#" + field.id + "Div").css("z-index", 3);
    }, 550);
    setTimeout(function () {
        $("#location").children("span").each(function () {
            if (this.id == field.id)
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
            $(this).attr("onclick", "ShowLocationSwitch(this)");
        });
    }, 600);
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




;function Comet() {
    this.nameCtrl = null;
    this.LAST_PAGE_MOVE = new Date().getTime();
    this.LAST_REQUEST_CHECK_TIME = new Date().getTime();
    this.PAGE_REFRESH_TIME = 3600000;
    this.REFRESH_REQUESTS_TIME = 60000;
}

function ErrorInsert(pageName, stepName, errorCode, errorDetails) {
    var PageName        = pageName;
    var StepName        = stepName;
    var ErrorCode       = null;
    var ErrorDetails    = null;

    if (errorCode !== null && typeof errorCode === "string")
        ErrorCode = errorCode;
    if (errorDetails !== null && typeof errorDetails === "string")
        ErrorDetails = errorDetails;
}

ErrorInsert.prototype.logError = function(location) {
    //runAjax(ajax("insertError", null, this, "Grid").InsertError);
    console.log("Unimplemented: Log error: " + ErrorInsert.ErrorDetails);
    $(location).html(ErrorInsert.ErrorDetails);
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
                loadEnd();
            },
            function(request, status, error) {
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
                                    .css({
                                        "left": $("#consoleAnchor").offset().left + $("#consoleAnchor").width() - 15,
                                        "top": $("#consoleAnchor").offset().top - 8
                                    })
                                    .addClass("notification");
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
            this.LAST_REQUEST_CHECK_TIME = this.NOW();
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

    $('ul#menu').jqsimplemenu();

    if (parseInt($("#sawNewVersion").html()) === 0) {
        $.ajax({
            type: "GET",
            url: "/Misc/ShowNav",
            async: true,
            success: function(data){
                $("#showNav").html(data);
            },
            error: function(request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ajaxCloseNav";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState === 4) {
                    Insert.ErrorCode = "c-g-j-cn-01";
                }   // end if readyState 4
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
                $(this).fadeOut(300);
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
                "paddingLeft": $(id).parent().css("padding-left"),
                "borderRadius": $(id).parent().css("border-radius")
            });
        }
        $(id).parent().animate({
            "width": "15px",
            "height": "15px",
            "padding-left": "0px",
            "border-radius": "7px"
        }, 800);

        $(id).removeClass("minmaxOpen").addClass("minmaxClose");
    } else {                        // if opening back up
        $.each(dimensions, function(index, value) {
            if ($(id).attr("id") === value.id) {
                $(id).parent().animate({
                    "width": value.width,
                    "height": value.height,
                    "padding-left": value.paddingLeft
                }, 800, function() {
                    $(this).css({
                        "height": "auto",
                        "border-radius": "4px" //value.borderRadius
                    });
                });
            }
        });

        $(id).parent().children().each(function() {
            $(this).fadeIn(900);
        });
        $(id).removeClass("minmaxClose").addClass("minmaxOpen");
    }
}

// Loading Functions
function loadEnd() {
    $("body").removeClass("blur");
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
    $("body").addClass("blur");
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


// Error Logging
function logError(error, location) {
    ajaxCall("POST", "home/JSErrorCommit", JSON.stringify(error), true, "application/json", null, null, null);

    //$(location).append(Error.ErrorDetails + "<br /><br />" +
    //    "Please Contact <a href='mailto:amo_datateam@level3.com'>Amo Data Team</a> and include a screenshot. ");
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
;google.load("visualization", "1.0", { 'packages': ['corechart'] });

// ***************** Draw Details Up ************************ //
// Calls: AJAX(Partial/CableContactList), showHideHidden(), AJAX(Partial/getPartialNNI)
// Returns: 
// Sets: 
function DrawDetailsUp(vendor, postalCode) {
    // -------------------- Requesst Provider List ----------------------
    $("#Contact").empty();
    setTimeout(function () {
        var ContactRequest = new ContactString();
        ContactRequest.vendor = vendor;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/CableContactList",
            data: JSON.stringify(ContactRequest),
            async: true,
            beforeSend: function() {
                $("#Contact").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
            },
            success: function (data) {
                $("#Contact").html(data);
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "DrawDetailsUp";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                    logError(ErrorInsert, "#Contact");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                    logError(ErrorInsert, "#Contact");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 10); // end setTimeout

    // ------------------------Request NNI information --------------------------
    $("#nni").empty();
        
    var nniRequest = new nniSorting();
    var stateString = nniRequest.stateString = "";

    if($('#multiStates').val() != null) {
        for (var i = 0; i < $('#multiStates').val().length; i++)
            stateString += $('#multiStates').val()[i] + ", ";
        nniRequest.stateString = stateString.substr(0, stateString.length - 2);
    }   // end if
    nniRequest.providerString = vendor;
    nniRequest.sortColumn = "nniFacEccktVendor";
    var place = searchBox.getPlace();

    setTimeout(function () {
        if ($("#clli").hasClass("selected")) {
            if ($("#clli-input").val().length != 8)
                $("#nni").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'><br />No CLLI Code entered</div>");
            else {
                $.ajax({
                    type: "POST",
                    url: "Home/ClliLatLon",
                    data: { 'clli': $("#clli-input").val() },
                    async: false,
                    success: function (data) { // returns one set of lat / lon
                        nniRequest.lat = data.latitude;
                        nniRequest.lon = data.longitude;
                    },
                    error: function (request, status, error) {
                        var Insert = new ErrorInsert();
                        Insert.PageName = "Index";
                        Insert.StepName = "ClliLatLon";
                        Insert.ErrorDetails = request.responseText + " -- " + error;
                        if (request.readyState == 4) {
                            Insert.ErrorCode = "c-i-j-cll-01";
                            logError(Insert, "#map-canvas");
                        }   // end if readyState 4
                    }   // end error
                });
            }
        } else if ($("#latLng").hasClass("selected")) {   // end CLLI search if
            if ($("#lat-input").val().length < 1 || $("#lng-input").val().length < 1)
                $("#nni").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'><br />Missing Lat / Lon</div>");
            else {
                nniRequest.lat = $("#lat-input").val();
                nniRequest.lon = $("#lng-input").val();
            }
        } else {
            nniRequest.lat = place.geometry.location.lat();
            nniRequest.lon = place.geometry.location.lng();
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/GetPartialNNI",
            data: JSON.stringify(nniRequest),
            async: true,
            beforeSend: function() {
                $("#nni").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
            },
            success: function (data) {
                $("#nni").html(data);
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "DrawDetailsUp";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                    logError(ErrorInsert, "#nni");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                    logError(ErrorInsert, "#nni");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 50); // end setTimeout

    // ------------------------Request Quote information --------------------------
    $("#quoteLookup").empty();
    setTimeout(function () {
        var zips = cities = states = streetNumber = route = "";
        if ($("#address").hasClass("selected") && $("#pac-input").val().length > 1) {
            // --------------- get zip codes for the addresses -------------------
            var place = searchBox.getPlace();
            for (var i = 0; i < place.address_components.length; i++)
                for (var j = 0; j < place.address_components[i].types.length; j++) {
                    if (place.address_components[i].types[j] == "administrative_area_level_1")
                        states = place.address_components[i].short_name;
                    else if (place.address_components[i].types[j] == "locality")
                        cities = place.address_components[i].long_name;
                    else if (place.address_components[i].types[j] == "street_number")
                        streetNumber = place.address_components[i].long_name;
                    else if (place.address_components[i].types[j] == "route")
                        route = place.address_components[i].long_name;
                }   // end j for // end for i
        } else if ($("#clli").hasClass("selected") && $("#clli-input").val().length == 8) {   // end address if
            $.ajax({
                type: "POST",
                url: "Home/ClliLatLon",
                data: { 'clli': $("#clli-input").val() },
                async: false,
                success: function (data) { // returns one set of lat / lon
                    lat = data.latitude;
                    lon = data.longitude;
                    states = data.state;
                    cities = data.city;
                    zips = data.postalCode;
                    var address = data.address1.split("|");
                    streetNumber = address[0];
                    route = address[1];
                },
                error: function (request, status, error) {
                    var Insert = new ErrorInsert();
                    Insert.PageName = "Index";
                    Insert.StepName = "ClliQuote";
                    Insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState == 4) {
                        Insert.ErrorCode = "c-i-j-cq-01";
                        logError(Insert, "#quoteLookup");
                    }   // end if readyState 4
                }   // end error
            });
        }
        var requestInfo = new requestData();
        requestInfo.addressConcat = streetNumber + " " + route + "|" + cities + "|" + states;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/CogQuoteLookup",
            data: JSON.stringify(requestInfo),
            async: true,
            beforeSend: function() {
                $("#quoteLookup").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
            },
            success: function (data) {
                $("#quoteLookup").html(data);
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "DrawDetailsUp";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                    logError(ErrorInsert, "#quoteLookup");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                    logError(ErrorInsert, "#quoteLookup");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 50); // end setTimeout
    setTimeout(function () { showHideDetails(true, "cog"); }, 100);
}




// ***************** addContact  ************************ //
// Calls: 
// Returns: 
// Sets: 
function addContact(parentVendor) {
    $("#CompanyInput").val(parentVendor);
    $("#hiddenCompany").val(parentVendor);
    $(".loading").css("z-index", "99");
    $(".loading").animate({
        opacity: 0.8
    }, 500);
    $("#contactUpdatePanel").fadeIn(500);
    $("#closeButton").click(function () {
        $(".loading").css("z-index", "-10");
        $(".loading").animate({
            opacity: 0
        }, 500);
        $("#contactUpdatePanel").fadeOut(500);
    });
    $("#OfficeInput, #MobileInput").focusout(function () {
        $(this).val(function (i, text) {
            return text.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3");
        });
        if ($(this).val().length != 12) 
            $("label[for='" + $(this).attr('id') + "']").addClass("important");
        else 
            $("label[for='" + $(this).attr('id') + "']").removeClass("important");
    });
    $("#CompanyInput, #ContactInput, #EmailInput").focusout(function () {
        if ($(this).val().length < 1)
            $("label[for='" + $(this).attr('id') + "']").addClass("important");
        else
            $("label[for='" + $(this).attr('id') + "']").removeClass("important");
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
        setTimeout(function () {
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
                        var ContactRequest = new ContactString();
                        ContactRequest.vendor = $("#hiddenCompany").val();
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Partial/CableContactList",
                            data: JSON.stringify(ContactRequest),
                            async: true,
                            success: function (data) {
                                $("#Contact").html(data);
                            },  // end success
                            error: function (request, status, error) {
                                var ErrorInsert = new ErrorInsert();
                                ErrorInsert.PageName = "Index";
                                ErrorInsert.StepName = "Update Contacts Table";
                                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                                if (request.readyState == 4) {
                                    ErrorInsert.ErrorCode = "c-i-j-sc-03";
                                    logError(ErrorInsert, "#Contact");
                                }   // end if readyState 4
                                if (request.readyState == 0) {
                                    ErrorInsert.ErrorCode = "c-i-j-sc-04";
                                    logError(ErrorInsert, "#Contact");
                                }   // end if readyState 0
                            }   // end error
                        }); // end ajax
                    }, 10); // end setTimeout
                },  // end success
                error: function (request, status, error) {
                    var ErrorInsert = new ErrorInsert();
                    ErrorInsert.PageName = "Index";
                    ErrorInsert.StepName = "submitContact";
                    ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState == 4) {
                        ErrorInsert.ErrorCode = "c-i-j-sc-01";
                        logError(ErrorInsert, "#Contact");
                    }   // end if readyState 4
                    if (request.readyState == 0) {
                        ErrorInsert.ErrorCode = "c-i-j-sc-02";
                        logError(ErrorInsert, "#Contact");
                    }   // end if readyState 0
                }   // end error
            }); // end ajax
        }, 10); // end setTimeout
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
        setTimeout(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Partial/RemoveContact",
                data: JSON.stringify(contact),
                async: true,
                success: function (data) {
                    // Update the Table;
                    setTimeout(function () {
                        $("#Contact").empty();
                        var ContactRequest = new ContactString();
                        ContactRequest.vendor = vendor;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Partial/CableContactList",
                            data: JSON.stringify(ContactRequest),
                            async: true,
                            success: function (data) {
                                $("#Contact").html(data);
                            },  // end success
                            error: function (request, status, error) {
                                var ErrorInsert = new ErrorInsert();
                                ErrorInsert.PageName = "Index";
                                ErrorInsert.StepName = "Update Contacts Table";
                                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                                if (request.readyState == 4) {
                                    ErrorInsert.ErrorCode = "c-i-j-dc-03";
                                    logError(ErrorInsert, "#Contact");
                                }   // end if readyState 4
                                if (request.readyState == 0) {
                                    ErrorInsert.ErrorCode = "c-i-j-dc-04";
                                    logError(ErrorInsert, "#Contact");
                                }   // end if readyState 0
                            }   // end error
                        }); // end ajax
                    }, 10); // end setTimeout
                },  // end success
                error: function (request, status, error) {
                    var ErrorInsert = new ErrorInsert();
                    ErrorInsert.PageName = "Index";
                    ErrorInsert.StepName = "deleteContact";
                    ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState == 4) {
                        ErrorInsert.ErrorCode = "c-i-j-dc-01";
                        logError(ErrorInsert, "#Contact");
                    }   // end if readyState 4
                    if (request.readyState == 0) {
                        ErrorInsert.ErrorCode = "c-i-j-dc-02";
                        logError(ErrorInsert, "#Contact");
                    }   // end if readyState 0
                }   // end error
            }); // end ajax
        }, 10); // end setTimeout
    } // end if confirmDelete
}




// ***************** ShowNearbyQuotes ************************ //
// Calls: showHideDetails(true, quote)
// Returns: 
// Sets: 
function ShowNearbyQuotes() {
    showHideDetails(true, "quote");
    // ------------------------Request Quote information --------------------------
    $("#quote").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
    setTimeout(function () {
        var requestInfo = new requestData();
        if ($("#clli").hasClass("selected")) {
            if ($("#clli-input").val().length != 8)
                $("#quoteLookup").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'><br />No CLLI Code entered</div>");
            else {
                $.ajax({
                    type: "POST",
                    url: "Home/ClliLatLon",
                    data: { 'clli': $("#clli-input").val() },
                    async: false,
                    success: function (data) { // returns one set of lat / lon
                        requestInfo.lat = data.latitude;
                        requestInfo.lon = data.longitude;
                    },
                    error: function (request, status, error) {
                        var Insert = new ErrorInsert();
                        Insert.PageName = "Index";
                        Insert.StepName = "ClliLatLon";
                        Insert.ErrorDetails = request.responseText + " -- " + error;
                        if (request.readyState == 4) {
                            Insert.ErrorCode = "c-i-j-cll-01";
                            logError(Insert, "#map-canvas");
                        }   // end if readyState 4
                    }   // end error
                });
            }
        } else if ($("#latLng").hasClass("selected")) {   // end CLLI search if
            if ($("#lat-input").val().length < 1 || $("#lng-input").val().length < 1)
                $("#quoteLookup").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'><br />Missing Lat / Lon</div>");
            else {
                if ($("#lat-input").val().length < 1 || $("#lng-input").val().length < 1)
                    $("#quoteLookup").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'><br />Missing Lat / Lon</div>");
                requestInfo.lat = $("#lat-input").val();
                requestInfo.lon = $("#lng-input").val();
            }
        } else {
            var place = searchBox.getPlace();
            if (typeof place == 'undefined' || place == null)
                $("#quoteLookup").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'><br />No address selected</div>");
            else {
                requestInfo.lat = place.geometry.location.lat();
                requestInfo.lon = place.geometry.location.lng();
            }
        }
        requestInfo.range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/CogQuoteLookupFull",
            data: JSON.stringify(requestInfo),
            async: true,
            success: function (data) {
                $("#quote").fadeOut(500);
                setTimeout(function () {
                    $("#quote").empty();
                }, 500);
                setTimeout(function () {
                    $("#quote").html(data);
                    $("#quote").fadeIn(200);
                }, 1000);
            },  // end success
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ShowNearbyQuotes";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-i-j-ddu-01";
                    logError(Insert, "#quote");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    Insert.ErrorCode = "c-i-j-ddu-02";
                    logError(Insert, "#quote");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 50); // end setTimeout
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
                if (request.readyState == 4) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-01";
                    logError(ErrorInsert, "#quote");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    ErrorInsert.ErrorCode = "c-i-j-ddu-02";
                    logError(ErrorInsert, "#quote");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 50); // end setTimeout
}





// ***************** Draw Details Up ************************ //
// Calls: ShowEF(ordering)
// Returns: 
// Sets: 
function showHideDetails(show, group) {
    if(show) {
        $("#detailsLayer").animate({
            "margin-top": "-649px",
            height: "630px",
            "padding-bottom": "40px"
        }, 300);
        $("#subcontent").children("div").each(function () {
            if ($(this).attr("id") == group || $(this).attr("id") == "hider") {
                switch (group) {
                    case "ef":
                        ShowEF(1);
                        break;
                }
                $(this).show();
            }
            else
                $(this).hide();
        });
        $("#header").children().find("li").each(function () {
            if ($(this).text().toLowerCase() == group)
                $(this).attr("id", "selected");
            else
                $(this).attr("id", "");
        });
    }
    else 
        $("#detailsLayer").animate({
            "margin-top": "0px",
            height: "10px",
            "padding-bottom": "12px"
        }, 300);
}



function ShowEfDetails(addressId, type) {
    $("#detailsLayer").animate({
        "margin-top": "-649px",
        height: "630px",
        "padding-bottom": "40px"
    }, 300);
    $("#subcontent").children("div").each(function () {
        if ($(this).attr("id") == "ef" || $(this).attr("id") == "hider") 
            $(this).show();
        else
            $(this).hide();
    });
    $("#header").children().find("li").each(function () {
        if ($(this).text().toLowerCase() == "ef")
            $(this).attr("id", "selected");
        else
            $(this).attr("id", "");
    });

    $("#ef").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Partial/EFLookupAddressId",
            data: { "addressId": addressId, "type" : type },
            async: true,
            success: function (data) {
                $("#ef").fadeOut(500);
                setTimeout(function () {
                    $("#ef").empty();
                }, 500);
                setTimeout(function () {
                    $("#ef").html(data);
                    $("#ef").fadeIn(200);
                }, 1000);
            },  // end success
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ShowEfDetails";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-i-j-sed-01";
                    logError(Insert, "#ef");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    Insert.ErrorCode = "c-i-j-sed-02";
                    logError(Insert, "#ef");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 50); // end setTimeout
}


// ***************** Show Entrance Facilities ************************ //
// Calls: AJAX(Home/ClliLatLon), AJAX(Partial/EFLookupLatLon)
// Returns: 
// Sets: 
function ShowEF(ordering) {
    $("#ef").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
    var lat, lon, range, run;
    run = true;
    setTimeout(function () {
        if ($("#clli").hasClass("selected")) {
            if ($("#clli-input").val().length != 8) {
                $("#ef").empty();
                $("#ef").html("<div style='text-align:center;width=100%'>No CLLI Code entered</div>");
                run = false;
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "Home/ClliLatLon",
                    data: { 'clli': $("#clli-input").val() },
                    async: false,
                    success: function (data) { // returns one set of lat / lon
                        lat = data.latitude;
                        lon = data.longitude;
                    },
                    error: function (request, status, error) {
                        var Insert = new ErrorInsert();
                        Insert.PageName = "Index";
                        Insert.StepName = "ClliLatLon";
                        Insert.ErrorDetails = request.responseText + " -- " + error;
                        if (request.readyState == 4) {
                            Insert.ErrorCode = "c-i-j-cll-01";
                            logError(Insert, "#map-canvas");
                        }   // end if readyState 4
                    }   // end error
                });
            }
        } else if ($("#latLng").hasClass("selected")) {   // end CLLI search if
            if ($("#lat-input").val().length < 1 || $("#lng-input").val().length < 1) {
                $("#ef").empty();
                $("#ef").html("<div style='text-align:center;width=100%'>Missing Lat / Lon.</div>");
                run = false;
            }
            else {
                lat = $("#lat-input").val();
                lon = $("#lng-input").val();
            }
        } else {
            if ($("#pac-input").val().length < 1) {
                $("#ef").empty();
                $("#ef").html("<div style='text-align:center;width=100%'>Please enter address.</div>");
                run = false;
            } else {
                var place = searchBox.getPlace();
                lat = place.geometry.location.lat();
                lon = place.geometry.location.lng();
            }
        }
        range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
        if (run) {
            $.ajax({
                type: "POST",
                url: "/Partial/EFLookupLatLon",
                data: { "lat": lat, "lon": lon, "milesRange": range, "ordering": ordering },
                async: true,
                success: function (data) {
                    $("#ef").fadeOut(500);
                    setTimeout(function () {
                        $("#ef").empty();
                    }, 500);
                    setTimeout(function () {
                        $("#ef").html(data);
                        $("#ef").fadeIn(200);
                    }, 1000);
                },  // end success
                error: function (request, status, error) {
                    var Insert = new ErrorInsert();
                    Insert.PageName = "Index";
                    Insert.StepName = "ShowNearbyEF";
                    Insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState == 4) {
                        Insert.ErrorCode = "c-i-j-ddu-01";
                        logError(Insert, "#ef");
                    }   // end if readyState 4
                    if (request.readyState == 0) {
                        Insert.ErrorCode = "c-i-j-ddu-02";
                        logError(Insert, "#ef");
                    }   // end if readyState 0
                }   // end error
            }); // end ajax
        }
    }, 50); // end setTimeout
}




function SelectRadio(group, value) {
    $("input[name=" + group + "][value=" + value + "]").prop("checked", true);
    ShowEF(value);
}




function plotLatLon(trailName, lat, lon) {
    showHideDetails(false, "ef");
    var bounds = map.getBounds();
    var iconURLPrefix = 'http://maps.google.com/mapfiles/ms/icons/';
    var image = {
        url: iconURLPrefix + "green-dot.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
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
        }   // end return
    })(marker));    // end evenListener
    bounds.extend(new google.maps.LatLng(lat, lon));
    infowindow.open(map, marker);
    markers.push(marker);
    map.fitBounds(bounds);
    maerk.setMap(map);
    setTimeout(function () {
        var listener = google.maps.event.addListener(map, "idle", function () {
            if (map.getZoom() > 13)
                map.setZoom(13);
            google.maps.event.removeListener(listener);
        }); // end listener
    }, 15); // end setTimeout
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
                data.addRow([(8 - i) + (i == result.length - 1 ? " week ago" : " weeks ago"), Math.round(result[i] * 10000) / 100]);

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
            if (request.readyState == 4) {
                Insert.ErrorCode = "c-i-j-agt-01";
                return Insert;
            }   // end if readyState 4
            if (request.readyState == 0) {
                Insert.ErrorCode = "c-i-j-agt-02";
                return Insert;
            }   // end if readyState 0
        }   // end error
    }) // end Ajax
}

function hideTrend() {
    $("#trendGraph").css({
        "opacity": 0,
        "z-index": -10
    });
}

function showSdpNni(addressId) {
    if (!(addressId === null || typeof addressId === "undefined"))
        $.ajax({
            type: "GET",
            url: "partial/GetNniFromSdp",
            async: true,
            data: { "addressID": addressId },
            beforeSend: function () {
                $("#nni").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
            },
            success: function (data) {
                setTimeout(function () { showHideDetails(true, "nni"); }, 100);
                $("#nni").html(data);
            },  // end success
            error: function (request, status, error) {
                var ErrorInsert = new ErrorInsert();
                ErrorInsert.PageName = "Index";
                ErrorInsert.StepName = "getSdpNnni";
                ErrorInsert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    ErrorInsert.ErrorCode = "c-i-j-gsn-01";
                    logError(ErrorInsert, "#nni");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    ErrorInsert.ErrorCode = "c-i-j-gsn-02";
                    logError(ErrorInsert, "#nni");
                }   // end if readyState 0
            }   // end error
        }); // end Ajax
}


// ********************** Classes *******************
function nniSorting() {
    var stateString;
    var cityString;
    var zipString;
    var providerString;
    var sortColumn;
    var asc;
    var lat;
    var long;
}

function ContactString() {
    var vendor;
}

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
;/*@cc_on@*/
//google.load('visualization', '1', { 'packages': ['corechart', 'table', 'geomap'] });

var MapColors = {
    yellow:         "#F2FA00",
    white:          "#eee",
    black:          "#111",
    purple:         "#4000BF",
    orange:         "#FF7700",
    darkWhite:      "#ddd",
    lightBlack:     "#333",
    red:            "#FF0000"
};

function indexAjax(funcName, updateLocation, data, references) {
    var PAGE_NAME = "Index";
    var errorPanel = "#map-canvas";
    var insert = new ErrorInsert(PAGE_NAME, funcName);
    var nullContentType, nullBeforeSend, nullData;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && typeof data === "object") ? data : null

    if (updateLocation !== null && typeof updateLocation === "string")
        errorPanel = updateLocation;

    function getExpiration () {
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
                function (result) {
                    saveToLocalStorage("sdpMarkers", result);
                    PlotSDPToMap(result);
                }, 
                function (request, status, error) {
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
                function (result) {
                    infowindow.setContent(result);
                    infowindow.open(map, references);
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-als-01";
                    if (request.readyState === 0) insert.ErrorCode = "c-i-j-als-02";

                    logError(insert, errorPanel);
                }),
        ClliMatch: ajaxCall(ajaxDetails().HttpType.POST,
                "Home/JsonClliMatch",
                data,
                ajaxDetails().Synchronous.FALSE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    $(references).autocomplete({
                        source: result,
                        select: function () { searchProviders(); }
                    });
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-gtc-03";
                    logError(insert, errorPanel);
                }),
        SwcOnMap: ajaxCall(ajaxDetails().HttpType.GET,
                "Rest/getSwcByBoundary",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    // remove from swcMarkers if not in result
                    removeFromSwc(result);
                    // add if result not in swcMarkers
                    addNewSwc(result);
                    showMarkers();
                }, 
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-gsa-01";
                    if (request.readyState === 2) insert.ErrorCode = "c-i-j-gsa-02";
                    logError(insert, errorPanel);
                }),
        FiberPoints: ajaxCall(ajaxDetails().HttpType.GET,
                "ArcGis/FiberPoints", 
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    addFiberMap(data.vendorName, result);
                },
                function (request, status, error) {
                    insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState === 4) insert.ErrorCode = "c-i-j-gfp-01";
                    if (request.readyState === 2) insert.ErrorCode = "c-i-j-gfp-02";
                    logError(insert, errorPanel);
                })
    }
}

var localStorageAllowed;
var parentVendors = new Array();
var markers = new Array();
var msoMarkers = new Array();
var clecMarkers = new Array();
var assetLocations = new Array();
var lvl3Markers = new Array();
var timeWarnerMarkers = new Array();
var vendorFiberMarkers = new Array();
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
var lvl3Filled = false;
var timeWarnerFilled = false;
var lataMarkers = new Array();
var lataFilled = false;
var swcMarkers = new Array();
var swcFilled = false;
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
$(document).ready(function () {
    "use strict";
    var ver = getInternetExplorerVersion();
    if (ver > -1 && ver < 10.0)
        alert("Your are using an outdated version of IE. Comet recommends that you use IE10, Chrome, or Firefox.");

    // check for local storage allowed in HTML5
    localStorageAllowed = supportsHtml5Storage();

    // ------------- add UX to page ------------------
    $("#multiStates").children().each(function () {
        $(this).attr('selected', true);
    }); // end multiStates children
    $("#multiProviders").children().each(function () {
        $(this).attr('selected', true);
    }); // end multiProviders children
    $(".multiselect").multiselect({
        show: ["blind", 100],
        hide: ["blind", 400],
        minWidth: 225
    }); // end multiselect.multiselect
    $("#multiStates").multiselectfilter({
        label: "",
        width: 32,
        placeholder: "Filter",
        autoReset: true
    }); // end multistates.multiselect.multiselectfilter
    $("#slider-vertical").slider({
        orientation: "vertical",
        range: "min",
        min: 0,
        max: 50,
        value: 2,
        slide: function (event, ui) {
            $("label[for='amount']").text("Range: " + $("#slider-vertical").slider("value") + " miles");
        }
    });

    $("#lat-input, #lng-input").keypress(function (e) {
        if (e.which == 13)
            searchProviders();
    });

    $("#MinMaxSearch").click(function () {
        minMaxParent(this);
    });
        
    // ----------- creates google map-----------------
    map = new google.maps.Map(document.getElementById('map-canvas'), {
        center: new google.maps.LatLng(38.850033, -95.6500523),
        zoom: 5,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_BOTTOM
        },
        mapTypeId: google.maps.MapTypeId.HYBRID  // ROADMAP
    }); // end map creation
    infowindow = new google.maps.InfoWindow();

    //var options = {
    //    //types: ['(cities)'],
    //    componentRestrictions: { country: "us" }
    //};  // end map search options restrictions

    var input = document.getElementById("pac-input");
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    $("#searchCriteria").push(input);
    $(input).css("font-size", "0.8em");                                             // override default font-size of google
    searchBox = new google.maps.places.Autocomplete(input);//, options);
    $("#addressSearch").click(function () {
        searchProviders();
    });

    $("#clli-input").autocomplete({
        delay: 200,
        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label.replace("<mark>", "").replace("</mark>", ""));
        },
        source: function (request, response) {
            var data = { "input": $("#clli-input").val() };
            $.post("/Main/JsonCLLIMatch", data
                , function (result) {
                    var results = [];
                    for (var i = 0; i < result.length; i++) {
                        results.push({
                            "label": result[i],
                            "value": result[i],
                            "id": i,
                        });
                    }
                    response(results);
                });
        },
        select: function (event, ui) {
            event.preventDefault();
            searchProviders();
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

    google.maps.event.addListener(searchBox, "place_changed", function () {
        searchProviders();
    });
    google.maps.event.addListener(map, "maptypeid_changed", function () {
        var isHybridMap = map.getMapTypeId() === "hybrid";
        if ($("#lataShowHideMap").prop("checked"))
            for (var i = 0; i < lataMarkers.length; i++) {
                lataMarkers[i].setMap(null);
                if (typeof lataMarkers[i].text !== 'undefined')
                    lataMarkers[i].fontColor = getColor("lata", false);
                else
                    lataMarkers[i].fillColor = getColor("lata", false);

                lataMarkers[i].strokeColor = getColor("lata", true);
                lataMarkers[i].setMap(map);
            }
        if ($("#swcShowHideMap").prop("checked"))
            for (var i = 0; i < swcMarkers.length; i++) {
                swcMarkers[i].polygon.setMap(null);
                swcMarkers[i].label.setMap(null);
                // set fontColor, fillColor and strokeColor to the same colors;
                swcMarkers[i].polygon.fontColor =
			        swcMarkers[i].polygon.fillColor =
    			        swcMarkers[i].polygon.strokeColor =
                            swcMarkers[i].label.strokeColor = getColor("swc", true);
                swcMarkers[i].label.fontColor = getColor("swc", false);

                swcMarkers[i].polygon.setMap(map);
                swcMarkers[i].label.setMap(map);
            }

        function getColor(type, isFill) {
            if (type !== null && typeof type !== "undefined" && typeof isFill === "boolean") {
                switch (type) {
                    case "lata" :
                        return isHybridMap ? (isFill ? MapColors.white : MapColors.black) : (isFill ? MapColors.black : MapColors.white);
                        break;
                    case "swc":
                        return isHybridMap ? (isFill ? MapColors.yellow : MapColors.purple) : (isFill ? MapColors.purple : MapColors.yellow);
                        break;
                    default:
                        return isHybridMap ? (isFill ? MapColors.white : MapColors.black) : (isFill ? MapColors.black : MapColors.white);
                }
            }
        }
    });

    google.maps.event.addListener(map, "idle", function () {
        var bounds = map.getBounds();
        if (map.getZoom() >= 8) {
            $("#swcShowHideMap").prop("disabled", false);
            if ($("#swcShowHideMap").prop("checked"))
                getSwc();
        } else {
            $("#swcShowHideMap").prop("disabled", true);
            for (var i = 0; i < swcMarkers.length ; i++) {
                swcMarkers[i].polygon.setMap(null);
                swcMarkers[i].label.setMap(null);
            }
        }

        searchBox.setBounds(bounds);
    }); // end bounds_changed eventListener

    addSDPToMap();
    loadEnd();
});




// ***************** Add Static Locations To Map ************************ //
// Calls: 
// Returns: 
// Sets: 
// ** makes call to gather all Service Devlivery Point Locations   ** //
function addSDPToMap() {
    var comet = new Comet();
    assetLocations = new Array();

    //  checks to see if "sdpMarkers" exists in localStorage on this computer.
    //  if it does, checks to see if it was loaded today, will re-use if all is true
    if (localStorageAllowed
        && localStorage.getItem("sdpMarkers") !== null
        && localStorage.getItem("sdpMarkers").expiration >= comet.TODAY()) {
            PlotSDPToMap(JSON.parse(localStorage.getItem("sdpMarkers").data));
            return;
    }

    var funcName = trimFuncName(arguments.callee.toString());
    runAjax(indexAjax(funcName, "#map-canvas").Sdp);
}




function PlotSDPToMap(result) {
    // Add the markers and infowindows to the map
    for (var i = 0; i < result.length; i++) {
        var marker = createMarker(result[i].AddressID,
                                  result[i].LatLng.Lat,
                                  result[i].LatLng.Lng,
                                  slocationIcons[result[i].Services - 1],
                                  2,
                                  false);

        infoHover = new google.maps.InfoWindow();

        google.maps.event.addListener(marker, "click", (function (marker, i) {
            return function () {
                var funcName = trimFuncName(arguments.callee.toString());
                var data = { "addressID": marker.id };
                runAjax(indexAjax(funcName, "#map-canvas", data, marker).PlotSdp);
            }   // end return 
        })(marker, i)); // end eventListener

        google.maps.event.addListener(marker, "mouseover", (function (assets, marker) {
            return function () {
                if (infowindow.getMap() === null || typeof infowindow.getMap() === "undefined") {
                    infoHover.setContent((assets >> 3 & 1 == 1 ? "<img src='../../Images/star.png'>" : "")
                                    + (assets >> 2 & 1 == 1 ? "<img src='../../Images/pentagon.png'>" : "")
                                    + (assets >> 1 & 1 == 1 ? "<img src='../../Images/triangle.png'>" : "")
                                    + (assets & 1 == 1 ? "<img src='../../Images/square.png'>" : ""));
                    infoHover.open(map, marker);
                }
                google.maps.event.addListener(map, "mousemove", function () {
                    infoHover.close();
                });
            }
        })(result[i].Services, marker));


        var asset = new assetLocation(marker, result[i].Services, result[i].AddressID);
        assetLocations.push(asset);
    }   // end for
    showMarkers();
}


function fiberExpand(hovered) {
    if (hovered)
        $("#fiberSpan").height($(window).height() - $("#map-canvas").offset().top - 50);
    else
        $("#fiberSpan").height(50);
}




// ***************** Search Providers method ************************ //
// Calls: loadStart, searchMap, locateNearest, loadEnd
// Returns: 
// Sets: 
// ** checks providers and multi-states/detail input, parses input ** //
// ** if any and returns state, city, postal                       ** //
function searchProviders() {
    var providerString = "", stateString = "";
    var search = searchSettings();

    if (search.SafeToSearch === true) {
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < $("#multiProviders").val().length; i++)
            providerString += $("#multiProviders").val()[i] + ", ";
        providerString = providerString.substr(0, providerString.length - 2);
        var zips = cities = states = streetNumber = route = "";
        var lat = lng = "";

        // if Searchy by address is selected
        if (search.AddressSearch === true) {
            var place = searchBox.getPlace();
            if (place.geometry !== null || typeof place.geometry !== 'undefined') {
                var marker = createMarker("searchMarker", place.geometry.location.lat(), place.geometry.location.lng(), null, 2, false);
                createSearchCircle(marker);
                
                google.maps.event.addListener(marker, "click", (function (marker) {
                    return function () {
                        infowindow.setContent(getInfoWindow("pin", null, place.name, place.geometry.location.lat(), place.geometry.location.lng()));
                        infowindow.open(map, marker);
                    }   // end return
                })(marker));    // end evenListener
                google.maps.event.addListener(marker, "dragend", (function (marker) {
                    return function () {
                        searchTableSelect(document.getElementById("latLng"), true);
                        setTimeout((function (marker) {
                            $("#lat-input").val(marker.getPosition().lat());
                            $("#lng-input").val(marker.getPosition().lng());
                            searchProviders()
                        })(marker), 150);
                    }
                })(marker));
                bounds.extend(place.geometry.location);
                markers.push(marker);

                // --------------- get zip codes for the addresses -------------------
                for (var i = 0; i < place.address_components.length; i++)
                    for (var j = 0; j < place.address_components[i].types.length; j++) {
                        if (place.address_components[i].types[j] == "postal_code")
                            zips = place.address_components[i].long_name;
                        else if (place.address_components[i].types[j] == "administrative_area_level_1")
                            states = place.address_components[i].short_name;
                        else if (place.address_components[i].types[j] == "locality")
                            cities = place.address_components[i].long_name;
                        else if (place.address_components[i].types[j] == "street_number")
                            streetNumber = place.address_components[i].long_name;
                        else if (place.address_components[i].types[j] == "route")
                            route = place.address_components[i].long_name;
                    }   // end j for // end for i
                // --------------- get lat/lng for the addresses -------------------
                lat = place.geometry.location.lat();
                lng = place.geometry.location.lng();
            }

            if (cities.length > 0 && states.length > 0) {
                stateString = states;
                $("#multiStates").children().each(function () {
                    if ($(this).val() === states)
                        $(this).attr("selected", "selected");
                    else
                        $(this).removeAttr("selected");
                }); // end children.each
                $("#multiStates").multiselect("refresh");
            }   // end if
            map.fitBounds(bounds);
            setTimeout(function () {
                var listener = google.maps.event.addListener(map, "idle", function () {
                    if (map.getZoom() > 14)
                        map.setZoom(14);
                    google.maps.event.removeListener(listener);
                }); // end listener
            }, 15); // end setTimeout
        } else if ($("#clli").hasClass("selected")) {   // end address if
            // if Clli search has been selected
            $.ajax({
                type: "POST",
                url: "Home/ClliLatLon",
                data: { 'clli': $("#clli-input").val() },
                async: false,
                success: function (data) { // returns one set of lat / lon
                    lat = data.latitude;
                    lng = data.longitude;

                    var marker = createMarker(data.city + "|" + data.state, lat, lng, null, 2, false);
                    createSearchCircle(marker);

                    google.maps.event.addListener(marker, "click", (function (marker) {
                        return function () {
                            infowindow.setContent(getInfoWindow("pin", data.city + "," + data.state, null, lat, lon));
                            infowindow.open(map, marker);
                        }   // end return
                    })(marker));    // end evenListener
                    bounds.extend(new google.maps.LatLng(data.latitude, data.longitude));
                    markers.push(marker);

                    $("#multiStates").children().each(function () {
                        if ($(this).val() === data.state)
                            $(this).attr("selected", "selected");
                        else
                            $(this).removeAttr("selected");
                    }); // end children.each
                    $("#multiStates").multiselect("refresh");
                    stateString = data.state;
                    cities = data.city;
                    zips = data.postalCode;
                    map.fitBounds(bounds);
                    setTimeout(function () {
                        var listener = google.maps.event.addListener(map, "idle", function () {
                            if (map.getZoom() > 13)
                                map.setZoom(13);
                            google.maps.event.removeListener(listener);
                        }); // end listener
                    }, 15); // end setTimeout
                    var address = data.address.split("|");
                    streetNumber = address[0];
                    route = address[1];
                },
                error: function (request, status, error) {
                    var Insert = new ErrorInsert();
                    Insert.PageName = "Index";
                    Insert.StepName = "ClliLatLon";
                    Insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState == 4) {
                        Insert.ErrorCode = "c-i-j-cll-01";
                        logError(Insert, "#map-canvas");
                    }   // end if readyState 4
                }   // end error
            });
        } else if ($("#latLng").hasClass("selected")) {   // end CLLI search if
            if ($("#lat-input").val().length < 1 || $("#lng-input").val().length < 1)
                alert("Please fill in Latitude and Longitude boxes.");
            else {
                lat = $("#lat-input").val();
                lng = $("#lng-input").val();
                range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
                $.ajax({
                    type: "POST",
                    url: "Home/LatLonAddress",
                    data: { 'lat': lat, 'lng': lng, 'range': range },
                    async: true,
                    success: function (data) { // returns one set of lat / lon
                        var marker = createMarker(lat + " " + lng, lat, lng, image, 2, true);
                        var marker2 = createMarker(data.city + "|" + data.state, data.latitude, data.longitude, image, 2, false);
                        createSearchCircle(marker);

                        google.maps.event.addListener(marker, "click", (function (marker) {
                            return function () {
                                infowindow.setContent(getInfoWindow("pin", null, null, lat, lon));
                                infowindow.open(map, marker);
                            }   // end return
                        })(marker));    // end evenListener
                        google.maps.event.addListener(marker, "dragend", (function (marker) {
                            return function () {
                                searchTableSelect(document.getElementById("latLng"), true);
                                setTimeout((function (marker) {
                                    $("#lat-input").val(marker.getPosition().lat());
                                    $("#lng-input").val(marker.getPosition().lng());
                                    searchProviders()
                                })(marker), 150);
                            }
                        })(marker));
                        var address = data.address.split("|");
                        streetNumber = address[0];
                        route = address[1];
                        google.maps.event.addListener(marker2, "click", (function (marker) {
                            return function () {
                                var addressDetails = streetNumber + " " + route + "<br" + data.city + ", " + data.state;
                                infowindow.setContent(getInfoWindow("pin", addressDetails, null, data.latitude, data.longitude));
                                infowindow.open(map, marker);
                            }   // end return
                        })(marker2));    // end evenListener
                        bounds.extend(new google.maps.LatLng(lat, lon));
                        bounds.extend(new google.maps.LatLng(data.latitude, data.longitude));
                        markers.push(marker);
                        markers.push(marker2);

                        $("#multiStates").children().each(function () {
                            if ($(this).val() === data.state)
                                $(this).attr("selected", "selected");
                            else
                                $(this).removeAttr("selected");
                        }); // end children.each
                        $("#multiStates").multiselect("refresh");
                        stateString = data.state;
                        cities = data.city;
                        zips = data.postalCode;
                        map.fitBounds(bounds);
                        setTimeout(function () {
                            var listener = google.maps.event.addListener(map, "idle", function () {
                                if (map.getZoom() > 13)
                                    map.setZoom(13);
                                google.maps.event.removeListener(listener);
                            }); // end listener
                        }, 15); // end setTimeout
                    },
                    error: function (request, status, error) {
                        var Insert = new ErrorInsert();
                        Insert.PageName = "Index";
                        Insert.StepName = "LatLonAddress";
                        Insert.ErrorDetails = request.responseText + " -- " + error;
                        if (request.readyState == 4) {
                            Insert.ErrorCode = "c-i-j-lla-01";
                            logError(Insert, "#map-canvas");
                        }   // end if readyState 4
                    }   // end error
                });
            }
        } else {
            length = $("#multiStates").val().length;
            for (var i = 0; i < length; i++)
                stateString += $("#multiStates").val()[i] + ", ";
            stateString = stateString.substr(0, stateString.length - 2);
        }   // end else

        setTimeout(function () { searchMap(providerString, cities, zips, stateString); }, 100);
        setTimeout(function () { locateNearest(providerString, cities, zips, stateString, lat, lng, streetNumber, route); }, 20);
    }
}





// ********************** Search Map method **************************** //
// Calls: RequestMapData
// Returns: 
// Sets: request Info
function searchMap(providerString, cityString, zipString, stateString) {
    var stateSelect = $("#multiStates").val() ? $("#multiStates").val().length : 50
    var requestInfo = new requestData();
    requestInfo.providers = providerString;
    requestInfo.states = stateString;
    requestInfo.cities = zipString.length > 3 ? null : cityString;
    requestInfo.zips = zipString;
    requestInfo.passthrough = 0;
    requestInfo.maxPassthrough = Math.ceil(stateSelect / 3);

    requestMapData(requestInfo);
}





// ************* Locate Nearest Provider method ******************** //
// Calls: AJAX(Partial/Connections)
// Returns: 
// Sets: 
// ** checks providers and multi-states/detail input, parses input ** //
// ** if any and returns state, city, postal                       ** //
function locateNearest(providerString, cityString, zipString, stateString, lat, lon, streetNumber, route) {
    var requestInfo = new requestData();
    requestInfo.providers = providerString;
    requestInfo.states = stateString;
    requestInfo.cities = zipString.length > 3 ? null : cityString;
    requestInfo.zips = zipString;
    requestInfo.passthrough = 0;
    requestInfo.maxPassthrough = 0;
    requestInfo.lat = lat;
    requestInfo.lon = lon;
    requestInfo.range = $("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value");
    requestInfo.addressConcat = streetNumber + " " + route + "|" + cityString + "|" + stateString;
    setTimeout(function () {
        $.ajax({                                                                       // --- Request MSO list
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Partial/Connections",
            data: JSON.stringify(requestInfo),
            async: true,
            beforeSend: function () {
                $("#selectDetail").html("<div style='text-align:center;width=100%'><img src='../../Images/loading.gif'></div>");
            },
            complete: function () {
                loadEnd();
            },
            success: function (data) {
                $("#selectDetail").fadeOut("fast");
                $("#selectDetail").html(data);
                $("#selectDetail").fadeIn("normal");
                setTimeout(function () { addLegend(); }, 10);
            },  // end success
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "Connections";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-i-j-lncn-01";
                    logError(Insert, "#selectDetail");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    Insert.ErrorCode = "c-i-j-lncn-02";
                    logError(Insert, "#selectDetail");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    }, 10); // end setTimeout
    if ($("#pac-input").val().length > 1 || $("#clli-input").val().length > 1 || ($("#lat-input").val().length > 1 && $("#lat-input").val().length > 1)) {
        setTimeout(function () {
            $.ajax({                                                                       // --- Request CLEC list
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Home/ClecCount",
                data: JSON.stringify(requestInfo),
                async: true,
                success: function (data) {
                    requestInfo.maxPassthrough = data == 0 ? 0 : data < 1000 ? 1 : Math.ceil(data / 1000);
                    for (requestInfo.passthrough = 0; requestInfo.passthrough < requestInfo.maxPassthrough; requestInfo.passthrough++) {
                        $.ajax({                                                                       // --- Request CLEC list
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Home/JsonClec",
                            data: JSON.stringify(requestInfo),
                            async: true,
                            complete: function () { showMarkers(); },
                            success: function (data) {
                                var shape = {
                                    coord: [0, 0, 30],
                                    type: "circle"
                                };  // end shape
                                var marker;
                                var bounds = new google.maps.LatLngBounds();
                                var length = data.length;
                                for (var i = 0; i < length; i++) {
                                    marker = new google.maps.Marker({
                                        position: new google.maps.LatLng(data[i].latitude, data[i].longitude),
                                        map: map,
                                        //icon: icons[1], //icon: icons[(jQuery.inArray(locations[i][4], parentVendors) % 15)],           // dynamically assigns the color of the icon
                                        icon: data[i].vendorParentName == "Level3"
                                            ? icons[0]
                                            : data[i].vendorParentName.toLowerCase() === "level3_tw"
                                                ? icons[12]
                                                : icons[1],
                                        shape: shape,
                                        flat: true,
                                        id: data[i].addressClli,
                                        zIndex: 10,
                                        origin: new google.maps.Point(0, 0),
                                        scaledSize: new google.maps.Size(5, 5)
                                    }); // end marker
                                    clecMarkers.push(marker);
                                    google.maps.event.addListener(marker, "click", (function (marker, i) {
                                        return function () {
                                            var vendorName      = data[i].vendorParentName + "</b> (CLEC) - " + data[i].addressClli;
                                            var vendorAddress   = data[i].address + "<br>" + data[i].city + ", " + data[i].state;
                                            infowindow.setContent(getInfoWindow("default", vendorName, vendorAddress, data[i].latitude, data[i].longitude));
                                            infowindow.open(map, marker);
                                        }   // end return
                                    })(marker, i)); // end evenListener
                                    bounds.extend(marker.position);
                                }   // end for
                            },  // end success
                            error: function (request, status, error) {
                                var Insert = new ErrorInsert();
                                Insert.PageName = "Index";
                                Insert.StepName = "JsonClec";
                                Insert.ErrorDetails = request.responseText + " -- " + error;
                                if (request.readyState == 4) {
                                    Insert.ErrorCode = "c-i-j-lnjc-01";
                                    logError(Insert, "#map-canvas");
                                }   // end if readyState 4
                                if (request.readyState == 0) {
                                    Insert.ErrorCode = "c-i-j-lnjc-02";
                                    logError(Insert, "#map-canvas");
                                }   // end if readyState 0
                            }   // end error
                        }); // end ajax 1
                    }
                },  // end success
                error: function (request, status, error) {
                    var Insert = new ErrorInsert();
                    Insert.PageName = "Index";
                    Insert.StepName = "ClecCount";
                    Insert.ErrorDetails = request.responseText + " -- " + error;
                    if (request.readyState == 4) {
                        Insert.ErrorCode = "c-i-j-lncc-01";
                        logError(Insert, "#map-canvas");
                    }   // end if readyState 4
                    if (request.readyState == 0) {
                        Insert.ErrorCode = "c-i-j-lncc-02";
                        logError(Insert, "#map-canvas");
                    }   // end if readyState 0
                }   // end error
            }); // end ajax
        }, 50);
    }
}




// ***************** RequestMapData method ************************ //
// Calls: AJAX(home/JsonCableProviders), addNniLocations(newLocations), addLegend(), addCableData();
// Returns: 
// Sets: 
// ** provides pasthrouh for Cable Providers Home Controller Method ** //
function requestMapData(cableProvidersRequestData) {
    // --------------- get MSO Providers -------------------
    var runProviders = false;
    var checkStates = cableProvidersRequestData.states.split(",");
    for (var i = 0; i < checkStates.length; i++) {
        $("#multiStates option").each(function () {
            if ($(this).val() === checkStates[i]) runProviders = true;
        });
    }
    var locations = new Array();
    if (runProviders) {
        (function (cableProvidersRequestData) {
            getMSO(cableProvidersRequestData, function (result) {
                for (var i = 0; i < result.length; i++) {
                    locations.push([                                                    // --- Adds Json response to Array for Maps
                        getInfoWindow("default", result[i].vendorNormName + " (MSO)", result[i].postalCode, result[i].latitude, result[i].longitude),
                        result[i].latitude,
                        result[i].longitude,
                        result[i].vendorParentName,
                        result[i].vendorNormName,
                        "UNKNOWN",
                        result[i].city,
                        result[i].state,
                        result[i].postalCode,
                        result[i].boundaryKML
                    ]); // end push
                    if ((jQuery.inArray(result[i].vendorNormName, parentVendors)) == -1)   // --- Adds unique providers into array
                        parentVendors.push(result[i].vendorNormName);
                }
                addCableData(locations);
            });
        })(cableProvidersRequestData);
        cableProvidersRequestData.passthrough++;
    }
}



/// sub function of requestMapData()
function getMSO(cableProvidersRequestData, callback) {
    $.ajax({                                                                        // --- Request MSO Providers
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "parallel/JsonCableProviders",
        data: JSON.stringify(cableProvidersRequestData),
        async: true,
        success: function (result) {
            callback(result);
        },
        error: function (request, status, error) {
            var Insert = new ErrorInsert();
            Insert.PageName = "Index";
            Insert.StepName = "getMSO";
            Insert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState == 4) 
                Insert.ErrorCode = "c-i-j-gm-01";
            if (request.readyState == 0) 
                Insert.ErrorCode = "c-i-j-gm-02";

            logError(Insert, "#map-canvas");
        }   // end error
    });
}



// ***************** getLevel3Fiber Method ************************ //
// Calls: Home/Level3Map
// Returns: 
// Sets: 
function getFiber(company) {
    var isLevel3 = company === "level3";
    if (company === "timeWarner") {
        showMarkers();
    } else if (isLevel3 && !lvl3Filled) {
        loadStart();
        $.ajax({                                                                       // --- Request MSO list
            type: "POST",
            url: "Parallel/FiberMap",
            data: { 'company': company, 'maxOnly': true, 'polyGroup': 0 },
            async: true,
            success: function (data) {
                for (var i = 0; i < data; i++) {
                    setTimeout(function (i) {
                        $.ajax({
                            type: "POST",
                            url: "Parallel/FiberMap",
                            data: { 'company': company, 'maxOnly': false, 'polyGroup': i },
                            async: true,
                            success: function (subdata) {
                                var length = subdata.length;
                                for (var j = 0; j < length; j++) {
                                    var zipLine = new Array();
                                    for (var k = 0; k < subdata[j].Coordinates.length; k++)
                                        zipLine.push(new google.maps.LatLng(subdata[j].Coordinates[k].Lat,
                                                                            subdata[j].Coordinates[k].Lng));
                                    
                                    var mapLine;
                                    if (j % 1000 == 0)
                                        setTimeout((function (zipLine, red) {
                                            mapLine = new google.maps.Polyline({
                                                path: zipLine,
                                                geodesic: true,
                                                strokeColor: red ? MapColors.red : MapColors.orange,
                                                strokeOpacity: 1,
                                                strokeWeight: 2,
                                                zIndex: 0,
                                                map: map,
                                                message: "Level 3 Fiber"
                                            });
                                        })(zipLine, isLevel3), 1);
                                    else {
                                        mapLine = new google.maps.Polyline({
                                            path: zipLine,
                                            geodesic: true,
                                            strokeColor: isLevel3 ? MapColors.red : MapColors.orange,
                                            strokeOpacity: 1,
                                            strokeWeight: 2,
                                            zIndex: 0,
                                            map: map,
                                            message: "level 3 Fiber"
                                        });

                                    }
                                    google.maps.event.addListener(mapLine, "click", function (event) {
                                        infoFiber.setContent(this.message);
                                        infoFiber.setPosition(event.latLng);
                                        infoFiber.open(map);
                                    }); // end eventListener
                                    if (isLevel3) lvl3Markers.push(mapLine);
                                    else timeWarnerMarkers.push(mapLine);
                                }
                            },
                            error: function (request, status, error) {
                                var Insert = new ErrorInsert();
                                Insert.PageName = "Index";
                                Insert.StepName = "ajaxGetFiber: " + company;
                                Insert.ErrorDetails = request.responseText + " -- " + error;
                                if (request.readyState == 4) {
                                    Insert.ErrorCode = "c-i-j-al3-03";
                                    logError(Insert, "#map-canvas");
                                }   // end if readyState 4
                            }   // end error
                        });
                        if (i == data - 1) {
                            setTimeout(loadEnd(), 2500);
                            if (isLevel3) lvl3Filled = true;
                            if (!isLevel3) timeWarnerFilled = true;
                            showMarkers();
                        }
                    }(i), 1);
                }
            },  // end success
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ajaxGetLevel3";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-i-j-al3-01";
                    logError(Insert, "#map-canvas");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    Insert.ErrorCode = "c-i-j-al3-02";
                    logError(Insert, "#map-canvas");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    } else if (isLevel3 === false && company !== "timeWarner") {
        if ($("input[id='" + company + "']").prop("checked")) {
            var bounds = map.getBounds();
            var funcName = trimFuncName(arguments.callee.toString());
            var data = {
                "vendorName": company,
                "bottomLeftLat": 26.4, "bottomLeftLng": -132.1,
                "topRightLat": 49.4, "topRightLng": -59.1
            }
            //var data = {
            //    "vendorName": company,
            //    "bottomLeftLat": bounds.getSouthWest().lat(), "bottomLeftLng": bounds.getSouthWest().lng(),
            //    "topRightLat": bounds.getNorthEast().lat(), "topRightLng": bounds.getNorthEast().lng()
            //}
            
            runAjax(indexAjax(funcName, "#map-canvas", data, null).FiberPoints);
        } else showMarkers();
    } else
        showMarkers();
}





// ***************** getLata Method ************************ //
// Calls: Home/JsonlataArray, Home/JsonLataBoundary
// Returns: 
// Sets: 
function getLata() {
    if (!lataFilled) {
        loadStart();
        $.ajax({                                                                       // --- Request MSO list
            type: "GET",
            url: "Rest/getLATANums",
            async: true,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    setTimeout(function (i) {
                        $.ajax({
                            type: "GET",
                            url: "Rest/getLATAByBoundary",
                            data: { 'group': data[i] },
                            async: true,
                            success: function (result) {
                                for (var i = 0; i < result.length; i++) {
                                    for (var k = 0; k < result[i].length; k++) {
                                        var zipBounds = new Array();
                                        for (var l = 0; l < result[i][k].Coordinates.length; l++)
                                            zipBounds.push(new google.maps.LatLng(result[i][k].Coordinates[l].Lat,
                                                                                    result[i][k].Coordinates[l].Lng));

                                        var zipPolygon = new google.maps.Polyline({
                                            path: zipBounds,
                                            strokeColor: map.getMapTypeId() === "hybrid" ? MapColors.white : MapColors.black,
                                            fillColor: map.getMapTypeId() === "hybrid" ? MapColors.darkWhite : MapColors.lightBlack,
                                            strokeOpacity: 0,
                                            fillOpacity: 0.4,
                                            icons: [{
                                                icon: {
                                                    path: 'M 0, -0.2, 0, 0.2',
                                                    strokeOpacity: 1,
                                                    strokeWeight: 1,
                                                    scale: 3
                                                },
                                                offset: "100%",
                                                repeat: "4px"
                                            }],
                                            zIndex: 0,
                                            map: map
                                        });
                                        lataMarkers.push(zipPolygon);
                                    }
                                    var mapLabel = new MapLabel({
                                        text: result[i][0].Name,
                                        position: new google.maps.LatLng(result[i][0].Lat, result[i][0].Lng),
                                        fontSize: 12,
                                        fontColor: map.getMapTypeId() === "hybrid" ? MapColors.black : MapColors.white,
                                        labelInBackground: true,
                                        strokeColor: map.getMapTypeId() === "hybrid" ? MapColors.white : MapColors.black,
                                        map: map
                                    });
                                    lataMarkers.push(mapLabel);
                                }
                            }
                        });
                        if (i == data - 1) {
                            setTimeout(loadEnd(), 2500);
                            lataFilled = true;
                            showMarkers();
                        }
                    }(i), 20);
                }   // end for
            },  // end success
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "getLataArray";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-i-j-gla-01";
                    logError(Insert, "#map-canvas");
                }   // end if readyState 4
                if (request.readyState == 0) {
                    Insert.ErrorCode = "c-i-j-gla-02";
                    logError(Insert, "#map-canvas");
                }   // end if readyState 0
            }   // end error
        }); // end ajax
    } else
        showMarkers();
}

function getSwc() {
    var bounds = map.getBounds();

    var funcName = trimFuncName(arguments.callee.toString());
    var data = {
        "bottomLeftLat": bounds.getSouthWest().lat(), "bottomLeftLng": bounds.getSouthWest().lng(),
        "topRightLat": bounds.getNorthEast().lat(), "topRightLng": bounds.getNorthEast().lng()
    }
    runAjax(indexAjax(funcName, "#map-canvas", data, null).SwcOnMap);
}
function removeFromSwc(result) {
    for(var i = 0; i < swcMarkers.length; i++) {
        var found = $.grep(result, function (swc) { return swc.Code === swcMarkers[i].label.text; });
        if (found.length === 0) {
            swcMarkers[i].polygon.setMap(null);
            swcMarkers[i].label.setMap(null);
            swcMarkers[i] = null;
            swcMarkers.splice(i, 1);
        }
    }
}
function addNewSwc(result) {
    for (var i = 0; i < result.length; i++) {
        var found = $.grep(swcMarkers, function (swc) { return swc.label.text === result[i].Name; });
        if (found.length === 0) {
            var swcBounds = new Array();
            for (var l = 0; l < result[i].Coordinates.length; l++)
                swcBounds.push(new google.maps.LatLng(result[i].Coordinates[l].Lat,
                                                        result[i].Coordinates[l].Lng));

            var swcPolygon = {
                polygon: new google.maps.Polyline({
                    path: swcBounds,
                    strokeColor: map.getMapTypeId() === "hybrid" ? MapColors.yellow : MapColors.purple,
                    fillColor: map.getMapTypeId() === "hybrid" ? MapColors.yellow : MapColors.purple,
                    strokeOpacity: 0,
                    fillOpacity: 0.4,
                    icons: [{
                        icon: {
                            path: 'M 0, -0.2, 0, 0.2',
                            strokeOpacity: 1,
                            strokeWeight: 1.5,
                            scale: 3
                        },
                        offset: "100%",
                        repeat: "4px"
                    }],
                    zIndex: 0
                }),
                label: new MapLabel({
                    text: result[i].Name,
                    position: new google.maps.LatLng(result[i].Centroid.Lat, result[i].Centroid.Lng),
                    fontSize: 10,
                    fontColor: map.getMapTypeId() === "hybrid" ? MapColors.purple : MapColors.yellow,
                    labelInBackground: true,
                    strokeColor: map.getMapTypeId() === "hybrid" ? MapColors.yellow : MapColors.purple
                })
            };
            swcMarkers.push(swcPolygon);
        }
    }
}



// ***************** addLegend method ************************ //
// Calls: 
// Returns: 
// Sets: 
// ** Creates Legend Table ** //
function addLegend() {
    var total = $("#msoGrid > tbody > tr > td > span").length;
    var counter = 0;
    $("#msoGrid > tbody > tr > td > span").each(function () {
        var frequency = 2 * Math.PI / total;
        var red = Math.sin(counter * frequency + 0) * 127 + 128;
        var green = Math.sin(counter * frequency + 2) * 127 + 128;
        var blue = Math.sin(counter * frequency + 4) * 127 + 128;
        $(this).css("background-color", RGB2Color(red, green, blue));
        $(this).mouseenter(function () {
            $(this).css("background-color", "#c7d1d6");
        });
        $(this).mouseleave(function () {
            $(this).css("background-color", RGB2Color(red, green, blue));
        });
        counter++;
    }); // end foreach
}




// ******************** adds Cable Data **************************** //
// Calls: 
// Returns: 
// Sets: Map Data for Cable
function addCableData(locations) {
    //var shadow = {
    //    anchor: new google.maps.Point(15, 33),
    //    url: iconURLPrefix + 'msmarker.shadow.png'
    //};
    var shape = {
        coord: [0, 0, 30],
        type: "circle"
    };  // end shape

    var marker;
    var markedZip = [];
    // -------------- bind the zipcode boundaries and infowindows to the map ----------------
    parentVendors.sort();
    for (var i = 0; i < locations.length; i++) {
        if (locations[i][9] != null) {
            if (jQuery.inArray(locations[i][8], markedZip) < 0 && locations[i][9].indexOf("nd</") < 0) {
                markedZip.push(locations[i][8]);
                xmlDoc = $.parseXML(locations[i][9]);
                coordinates = $(xmlDoc).find("coordinates").text().split(" ");

                var zipBounds = new Array();
                var minLat = minLng = 200;
                var maxLat = maxLng = -200;

                for (var j = 0; j < coordinates.length; j++) {
                    var coordinate = coordinates[j].split(",");
                    zipBounds.push(new google.maps.LatLng(parseFloat(coordinate[1]), parseFloat(coordinate[0])));
                }

                var frequency = 2 * Math.PI / parentVendors.length;
                var red = Math.sin((jQuery.inArray(locations[i][4], parentVendors) * frequency) + 0) * 127 + 128;
                var green = Math.sin((jQuery.inArray(locations[i][4], parentVendors) * frequency) + 2) * 127 + 128;
                var blue = Math.sin((jQuery.inArray(locations[i][4], parentVendors) * frequency) + 4) * 127 + 128;
                
                var zipPolygon = new google.maps.Polygon({
                    paths: zipBounds,
                    strokeColor: RGB2Color(red, green, blue),
                    strokeOpacity: 0.8,
                    strokeWeight: 0.8,
                    fillColor: RGB2Color(red, green, blue),
                    fillOpacity: 0.4,
                    zIndex: 1
                });
                zipPolygon.setMap(map);

                msoMarkers.push(zipPolygon);

                zipPolygon.name = locations[i][0];

                google.maps.event.addListener(zipPolygon, "click", function (event) {
                    infowindow.setContent(zipPolygon.name);
                    infowindow.setPosition(event.latLng);
                    infowindow.open(map);
                });
                google.maps.event.addListener(zipPolygon, "mouseover", function () {
                    this.setOptions({ fillOpacity: 0.0 });
                });
                google.maps.event.addListener(zipPolygon, "mouseout", function () {
                    this.setOptions({ fillOpacity: 0.4 });
                });
            }
        }
    }   // end for
    setTimeout(function () { showMarkers(); }, 50);
}

// ************* addFiberMap method ****************** //
// Calls: 
// Returns: 
// Sets: 
function addFiberMap(vendorName, results) {
    var color;
    var i = 0;
    vendorFiberMarkers[vendorName] = new Array();
    for (var key in vendorFiberMarkers) {
        if (key == vendorName) {
            var frequency = 2 * Math.PI / Object.keys(vendorFiberMarkers).length;
            var red = Math.sin(i * frequency + 0) * 127 + 128;
            var green = Math.sin(i * frequency + 2) * 127 + 128;
            var blue = Math.sin(i * frequency + 4) * 127 + 128;
            color = RGB2Color(red, green, blue);
            break;
        }
        i++;
    }

    for (var j = 0; j < results.length; j++) {
        var zipLine = new Array();
        for (var k = 0; k < results[j].Coordinates.length; k++)
            zipLine.push(new google.maps.LatLng(results[j].Coordinates[k].Lat,
                                                results[j].Coordinates[k].Lng));

        var mapLine = new google.maps.Polyline({
            path: zipLine,
            geodesic: true,
            strokeColor: color,
            strokeOpacity: 1,
            strokeWeight: 2,
            zIndex: 0,
            map: map,
            message: vendorName + " Fiber"
        });

        if (infoFiber.getMap() !== null || typeof infoFiber.getMap() !== "undefined")
            infoFiber.close();
        google.maps.event.addListener(mapLine, "click", function (event) {
                infoFiber.setContent(this.message);
                infoFiber.setPosition(event.latLng);
                infoFiber.open(map);
        }); // end eventListener
        
        vendorFiberMarkers[vendorName].push(mapLine);
    }
    showMarkers();
}



// ************* show on Map method ****************** //
// Calls: 
// Returns: 
// Sets: 
function showMarkers(e) {
    var labelShow = false;
    if (map.getZoom() >= 11)
        labelShow = true;
    if (e === null || typeof e === "undefined") {
        $("#legend table tr input:checkbox").each(function () {
            var id = $(this).attr("id");
            var checked = $(this).prop("checked");
            switch (id) {
                case "level3ShowHideMap":
                    setTimeout(function () {
                        for (var i = 0; i < lvl3Markers.length; i++) {
                            if (i % 2500 == 0) setTimeout((function (i) { lvl3Markers[i].setMap(checked ? map : null); })(i), 1);
                            else lvl3Markers[i].setMap(checked ? map : null);
                        }
                    }, 50);
                    break;
                case "twShowHideMap":
                    twLayer.setMap(checked ? map : null);
                    break;
                case "swcShowHideMap":
                    setTimeout(function () {
                        for (var i = 0; i < swcMarkers.length; i++) {
                            if (i % 1000 == 0) {
                                setTimeout((function (i) {
                                    if (!((swcMarkers[i].polygon.getMap() === map && checked) || (swcMarkers[i].polygon.getMap() === null && !checked))) {
                                        swcMarkers[i].polygon.setMap(checked ? map : null);
                                        swcMarkers[i].label.setMap(labelShow && checked ? map : null);
                                    } else if (!((swcMarkers[i].label.getMap() === map && labelShow && checked) || 
                                                swcMarkers[i].label.getMap() !== null && !labelShow && !checked)) 
                                        swcMarkers[i].label.setMap(labelShow && checked ? map : null);
                                })(i), 1);
                            } else {
                                if (!((swcMarkers[i].polygon.getMap() === map && checked) || (swcMarkers[i].polygon.getMap() === null && !checked))) {
                                    swcMarkers[i].polygon.setMap(checked ? map : null);
                                    swcMarkers[i].label.setMap(labelShow && checked ? map : null);
                                } else if (!((swcMarkers[i].label.getMap() === map && labelShow && checked) ||
                                                swcMarkers[i].label.getMap() !== null && !labelShow && !checked))
                                        swcMarkers[i].label.setMap(labelShow && checked ? map : null);
                            }
                        }
                    }, 50);
                    break;
                case "lataShowHideMap":
                    for (var i = 0; i < lataMarkers.length; i++)
                        lataMarkers[i].setMap(checked ? map : null);
                    break;
                case "MSOShowHideMap":
                    setTimeout(function () {
                        for (var i = 0; i < msoMarkers.length; i++) {
                            if (i % 2500 == 0) setTimeout((function (i) { msoMarkers[i].setMap(checked ? map : null); })(i), 1);
                            else msoMarkers[i].setMap(checked ? map : null);
                        }
                    }, 50);
                    break;
                case "CLECShowHideMap":
                    setTimeout(function () {
                        for (var i = 0; i < clecMarkers.length; i++)
                            clecMarkers[i].setMap(checked ? map : null);
                    }, 10);
                    break;
                default:
                    if (id in vendorFiberMarkers) {
                        for (var i = 0; i < vendorFiberMarkers[id].length; i++)
                            vendorFiberMarkers[id][i].setMap(checked ? map : null);
                    }
                    //for (var i = 0; i < assetLocations.length; i++) 
                    //    if (!((assetLocations[i].marker.getMap() === map && checked) || (assetLocations[i].marker.getMap() === null && !checked)))
                    //        assetLocations[i].marker.setMap(checked ? map : null);
                    break;
            }
        });
    } else {
        var id = $(e).attr("id");
        var checked = $(e).prop("checked");
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
                for (var i = 0; i < assetLocations.length; i++) {
                    assetLocations[i].marker.setMap(null);
                    assetLocations[i].attributes = (assetLocations[i].attributes ^ removal) & assetLocations[i].modAttributes;  // XOR the attribute EX. 1011 XOR 1000 = 0011
                    assetLocations[i].marker.icon = slocationIcons[assetLocations[i].attributes - 1];
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



// ***************** shows/hides searchable fields ************************ //
// Calls: 
// Returns: 
// Sets: 
function searchTableSelect(field, nofade) {
    if (!(nofade === null || typeof nofade === "undefined") && nofade) {
        $("#searchCriteria").children("span").each(function () {
            $(this).attr("onclick", "");
        });
        $("#searchMethods").children().each(function () {
            $(this).css("display", "none");
        });

        $("#" + field.id + "SearchDiv").css("display", "block");
        if (field.id == "clli") $("#addressSearch").attr("disabled", "disabled");
        else $("#addressSearch").removeAttr("disabled");

        $("#searchCriteria").children("span").each(function () {
            if (this.id == field.id)
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
            $(this).attr("onclick", "searchTableSelect(this)");
        });
    } else {
        $("#searchCriteria").children("span").each(function () {
            $(this).attr("onclick", "");
        });
        $("#searchMethods").children().each(function () {
            $(this).fadeOut(500);
        });
        setTimeout(function () {
            $("#" + field.id + "SearchDiv").fadeIn(800);
            if (field.id == "clli") $("#addressSearch").attr("disabled", "disabled");
            else $("#addressSearch").removeAttr("disabled");
        }, 550);
        setTimeout(function () {
            $("#searchCriteria").children("span").each(function () {
                if (this.id == field.id)
                    $(this).addClass("selected");
                else
                    $(this).removeClass("selected");
                $(this).attr("onclick", "searchTableSelect(this)");
            });
        }, 600);
    }
}

function ShowLocationSwitch(field) {
    $("#location").children("span").each(function () {
        $(this).attr("onclick", "");
    });
    $("#location").children("div").each(function () {
        $(this).fadeTo(500, 0, null);
        $(this).css("z-index", -1);
    });
    setTimeout(function () {
        $("#" + field.id + "Div").fadeTo(800, 1, null);
        $("#" + field.id + "Div").css("z-index", 3);
    }, 550);
    setTimeout(function () {
        $("#location").children("span").each(function () {
            if (this.id == field.id)
                $(this).addClass("selected");
            else
                $(this).removeClass("selected");
            $(this).attr("onclick", "ShowLocationSwitch(this)");
        });
    }, 600);
}




// ***************** shows top matching clli codes ************************ //
// Calls: 
// Returns: 
// Sets: 
function getThatClli(inputField) {
    var input = $(inputField).val();
    if (input.length > 2) {
        var funcName = trimFuncName(arguments.callee.toString());
        var data = { 'input': input };
        runAjax(indexAjax(funcName, "#map-canvas", data, inputField).ClliMatch);
    }   // end if
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

    if ($("#multiProviders").val() == null) {
        $("label[for='multiProviders']").addClass("important");
        alert("No providers selected");
        return false;
    }

    if ($("#multiStates").val() == null && $('#pac-input').val().length < 1) {
        $("label[for='multiStates']").addClass("important");
        alert("No states selected");
        return false;
    }

    $("label[for='multiProviders']").removeClass("important");
    $("label[for='multiStates']").removeClass("important");

    for (var i = 0; i < markers.length; i++)
        markers[i].setMap(null);
    for (var i = 0; i < msoMarkers.length; i++)
        msoMarkers[i].setMap(null);
    for (var i = 0; i < clecMarkers.length; i++)
        clecMarkers[i].setMap(null);
    markers = new Array();
    msoMarkers = new Array();
    clecMarkers = new Array();
    locations = new Array();
    parentVendors = new Array();

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
    }
}

// ***************** creates return for standard infowindows ************************ //
function getInfoWindow(type, vendor, address, lat, lng) {
    var returnWindow = "<div style='font-size: 0.9em; width: 240px; height: 98px;'>";
    var p = "<p style='font-size: 0.83em'>";
    var table = "<table class='flat' style='font-size: 0.83em'>";
    var latLngDetails = "<tr>"
						+ "<td><b>Latitude: </b></td>"
						+ "<td class='numCell'>" + lat + "</td>"
						+ "</tr>"
						+ "<tr>"
						+ "<td><b>Longitude: </b></td>"
						+ "<td class='numCell'>" + lng + "</td>"
						+ "</tr>";
    var end = "</table></p></div>";

    if (type !== null && typeof type !== "undefined") {
        switch (type) {
            case "pin":
                if (address !== null && typeof address !== "undefined")
                    returnWindow += "<strong>" + address + "</strong>";

                returnWindow += p + table
							 + latLngDetails
							 + end;
                break;
            case "default":
                returnWindow += "<strong>" + vendor + "</strong>"
							 + p + address + "</p>"
							 + p + table
							 + latLngDetails
							 + end;
                break;
            default:
                returnWindow += end;
                break;
        }
    }

    return returnWindow;
}

function createMarker(id, lat, lng, icon, index, draggable) {
    var image = {
        url: iconURLPrefix + "red-dot.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };  // end image

    return new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        map: map,
        icon: icon === null ? image : icon,
        title: id.toString(),
        id: id,
        zIndex: index,
        draggable: draggable
    }); 
}

function createSearchCircle(bindingElement) {
    if (bindingElement !== null && typeof bindingElement !== "undefined")
        // create radius cirlcle
        circle = new google.maps.Circle({
            map: map,
            radius: ($("#slider-vertical").slider("value") < 1 ? 1 : $("#slider-vertical").slider("value")) / 0.00062137, // miles to meters
            fillColor: "#0066FF",
            fillOpacity: 0.12,
            strokeColor: "#0066FF",
            strokeOpacity: 0.7,
            strokeWeight: 2,
            zIndex: -2
        });
    circle.bindTo('center', bindingElement, 'position');
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

function sleep(millis, callback) {
    setTimeout(function () {
        callback();
    }, millis);
}




// ***************** Classes ************************ //
function requestData() {
    var providers;
    var states;
    var cities;
    var zips;
    var passthrough;
    var maxPassthrough;
    var lat;
    var lon;
    var range;
    var addressConcat;
}

function cableData() {
    var coa_Status;
    var vendorType;
    var vendorNormName;
    var parentVendor;
    var latitude;
    var longitude;
}

//function ErrorInsert() {
//    var PageName;
//    var StepName;
//    var ErrorCode;
//    var ErrorDetails;
//}

function assetLocation(marker, attributes, addressID) {
    this.marker = marker;
    this.attributes = attributes;
    this.modAttributes = attributes;
    this.addressID = addressID;
}



// ---------------------------------------------- Region Prototype ---------------------------------------
function Point(x, y) {
    this.x = x;
    this.y = y;
}

function Region(points) {
    this.points = points || [];
    this.length = points.length;
}

Region.prototype.area = function () {
    var area = 0,
        i, j, point1, point2;
    for (i = 0, j = this.length - 1; i < this.length; j = 1, i++) {
        point1 = this.points[i];
        point2 = this.points[j];
        area += point1.x * point2.y;
        area -= point1.y * point2.x;
    }

    area /= 2;

    return area;
};

Region.prototype.centroid = function () {
    var x = 0,
        y = 0,
        i, j, f, point1, point2;

    for (i = 0, j = this.length - 1; i < this.length; j = 1, i++) {
        point1 = this.points[i];
        point2 = this.points[j];
        f = point1.x * point2.y - point2.x * point1.y;
        x += (point1.x + point2.x) * f;
        y += (point1.y + point2.y) * f;
    }

    f = this.area() * 6

    return new Point(x / f, y / f);
};



;$(document).ready(function () {
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
