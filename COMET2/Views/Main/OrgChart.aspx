<%@Import Namespace="COMET.Model.Domain" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="ToolTitle" ContentPlaceHolderID="TitleContent" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
   &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/Content/OrgChart.css") %>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ['orgchart'] });
        google.setOnLoadCallback(function () {
            drawChart('<%= ((IUser)ViewData["person"]).EmployeeID %>');
        });

        function drawChart(userID) {
            setTimeout(function () {
                runAjax(ajax("post", "drawChart", "chart_div", { "id": userID }, userID).UserHierarchy);
            }, 301);
        }

        function search(userID, name) {
            runAjax(ajax("post", "userDetails", "details_div", { "id": userID }, null).UserDetails);
            drawChart(userID);

            history.pushState({ "id": userID, "name": name }, document.title, "/OrgChart/Employee/" + userID);
            if (name !== null && typeof name !== "undefined")
                document.title = "Employee Search - " + name;
            else
                document.title = "Employee Search";
        }

        $(document).ready(function () {
            $("#searchField").autocomplete({
                delay: 500,
                focus: function (event) {
                    event.preventDefault();
                },
                source: function (request, response) {
                    var data = { "input": $("#searchField").val() };
                    $.get("/OrgChart/PeopleList", data
                        , function (result) {
                            var results = [];
                            for (var i = 0; i < result.length; i++) {
                                results.push({
                                    "label": result[i][1],
                                    "value": result[i][0],
                                    "id": result[i][0]
                                });
                            }
                            response(results);
                        });
                },
                select: function (event, ui) {
                    event.preventDefault();
                    var name = ui.item.label.replace("<mark>", "").replace("</mark>", "");
                    search(ui.item.value, name);
                    $("#searchField").val("").prop("placeholder", name);
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
                return $("<li>")
                    .data("ui-autocomplete-item", item)
                    .append($("<a>").html(item.label))
                    .appendTo(ul);
            };
            
            window.addEventListener("popstate", function (event) {
                var userID = "";
                if (event.state !== null && typeof event.state !== "undefined") {
                    userID = event.state.id;
                    document.title = "Employee Search - " + event.state.name;
                } else {
                    userID = "<%= ((IUser)ViewData["person"]).EmployeeID %>";
                    document.title = "Employee Search";
                }

                runAjax(ajax("post", "userDetails", "details_div", { "id": userID }, null).UserDetails);
                drawChart(userID);
            });
        });

        function ajax(type, funcName, updateLocation, data, references) {
            "use strict";
            var nullData, nullContentType, nullBeforeSend;
            nullData = nullContentType = nullBeforeSend = null;
            data = (data !== null && (typeof data === "object" || typeof data === "string")) ? data : null;

            if (updateLocation !== null && typeof updateLocation === "string")
                var errorPanel = updateLocation;

            var httpType = ajaxDetails().HttpType.POST;
            switch (type) {
                case "get":
                    httpType = ajaxDetails().HttpType.GET;
                    break;
            };

            return {
                UserDetails:
                    new ajaxCall(httpType,
                        "/OrgChart/GetUser",
                        data,
                        ajaxDetails().Synchronous.TRUE,
                        ajaxDetails().ContentType.DEFAULT,
                        function () {
                            $("#" + updateLocation).addClass("centerContents").html("<img src='/Images/loading.gif' style='margin: auto; position: absolute; top: 50%;' />");
                        },
                        function (result) {
                            $("#" + updateLocation).removeClass("centerContents").html(result);
                        },
                        function (request, status, error) {
                            $("#" + errorPanel).html(request + " " + status + " " + error);
                            //var errorInsert = new ErrorInsert();
                            //errorInsert.PageName = "OrgChart";
                            //errorInsert.StepName = "UserDetails";
                            //errorInsert.ErrorDetails = request.responseText + " -- " + error;
                            //if (request.readyState == 4) {
                            //    errorInsert.ErrorCode = "c-i-j-ud-01";
                            //    logError(errorInsert, "#details_div");
                            //}   // end if readyState 4
                            //if (request.readyState == 0) {
                            //    errorInsert.ErrorCode = "c-i-j-ud-02";
                            //    logError(errorInsert, "#details_div");
                            //}   // end if readyState 0
                        }),
                UserHierarchy:
                    new ajaxCall(httpType,
                        "/OrgChart/Hierarchy",
                        data,
                        ajaxDetails().Synchronous.TRUE,
                        ajaxDetails().ContentType.DEFAULT,
                        function () {
                            $("#" + updateLocation).addClass("centerContents").html("<img src='/Images/loading.gif' style='margin: auto; position: absolute; top: 50%;'/>");
                        },
                        function (employees) {
                            var userRow = 0;
                            var termedEmployees = [];
                            var data = new google.visualization.DataTable();
                            data.addColumn("string", "Name");
                            data.addColumn("string", "Manager");
                            data.addColumn("string", "ToolTip");

                            var userArray = [];
                            for (var i = 0; i < employees.length; i++) {
                                var userBox = "<div class='userBox'>" + employees[i].Title + "</div>";

                                userArray.push([{ v: employees[i].EmployeeID.toString(), f: employees[i].Name + userBox },
                                    employees[i].ManagerID === null ? "" : employees[i].ManagerID.toString(), employees[i].Name]);

                                if (employees[i].EmployeeID === parseInt(references)) {
                                    userRow = i;
                                    $("#searchField").attr("placeholder", employees[i].Name);
                                }
                                if (employees[i].TermDate !== null) termedEmployees.push(i);
                            }
                            data.addRows(userArray);

                            // color the background of termed employees differently
                            for (var j = 0; j < termedEmployees.length; j++) {
                                if (termedEmployees[j] === references) {
                                    data.setRowProperty(termedEmployees[j], 'style', 'background-color: rgba(255, 28, 28, 0.2); background-image:none; border-color: red;');
                                } else
                                    data.setRowProperty(termedEmployees[j], 'style', 'background-color: rgba(255, 28, 28, 0.2); background-image:none;');
                            }
                            // color the selected employee border
                            data.setRowProperty(userRow, 'style', 'border-color: red;');

                            var chart = new google.visualization.OrgChart(document.getElementById("chart_div"));
                            google.visualization.events.addListener(chart, "select", function () {
                                var selection = chart.getSelection();
                                for (var i = 0; i < selection.length; i++) {
                                    if (selection[i].row !== null && selection[i].column !== null)
                                        search(data.getValue(selection[i].row, selection[i].column), data.getValue(selection[i].row, 2));
                                    else if (selection[i].row !== null)
                                        search(data.getValue(selection[i].row, 0), data.getValue(selection[i].row, 2));
                                    else if (selection[i].column !== null)
                                        search(data.getValue(0, selection[i].column), data.getValue(0, 2));
                                }
                            });
                            $("#" + updateLocation).removeClass("centerContents");
                            chart.draw(data, { allowHtml: true });
                        },
                        function (request, status, error) {
                            $("#" + errorPanel).html(error);
                            //var errorInsert = new ErrorInsert();
                            //errorInsert.PageName = "OrgChart";
                            //errorInsert.StepName = "UserDetails";
                            //errorInsert.ErrorDetails = request.responseText + " -- " + error;
                            //if (request.readyState == 4) {
                            //    errorInsert.ErrorCode = "c-i-j-ud-01";
                            //    logError(errorInsert, "#details_div");
                            //}   // end if readyState 4
                            //if (request.readyState == 0) {
                            //    errorInsert.ErrorCode = "c-i-j-ud-02";
                            //    logError(errorInsert, "#details_div");
                            //}   // end if readyState 0
                        })
            };
        }
    </script>
</asp:Content>

<asp:Content ID="OrgContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="search">
        <label for="searchField">Search Employee:</label>
        <input type="text" id="searchField" />
    </div>
    <div id="chart_div"></div>
    <div id="details_div"><% Html.RenderPartial("~/Views/Partial/_EmployeeDetails.ascx", (IUser)ViewData["person"]); %></div>
</asp:Content>