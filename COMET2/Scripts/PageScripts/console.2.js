/// <reference path="Global.js" />

var slickGrid;
var requestApp = angular.module("requestAng", ["ngRoute"]);

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

function bindModel(data) {
    if (data.requestID !== null && data.requestID !== typeof "undefined")
        alert("request");
    if (data.projectID !== null && data.projectID !== typeof "undefined")
        alert("project");
    if (data.elementID !== null && data.elementID !== typeof "undefined")
        alert("element");
}

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

                    bindModel(data);
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