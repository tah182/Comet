<%@ Import Namespace="COMET.Model.Console.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IRequestView>" %>
<asp:Content ID="DashboardTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/Content/Console.css") %>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <script>
        var canImpersonate = <%= ((IUser)ViewData["user"]).canImpersonate().ToString().ToLower() %>;
        $(document).ready(function () {
            <% if ((EOpenType)ViewData["type"] != EOpenType.Element) { %>
            $("#EstimatedDueDate").val("<%= ((AProjectView)Model).EstimatedDueDate == null ? "" : ((DateTime)((AProjectView)Model).EstimatedDueDate).ToString("yyyy-MM-dd") %>");
            <% } %>
            (function ($) {
                $.extend({
                    APP: {
                        title: document.title,
                        formatTimer: function (a) {
                            if (a < 10) {
                                a = '0' + a;
                            }
                            return a;
                        },

                        startTimer: function (dir) {
                            var a;

                            // save type
                            $.APP.dir = dir;
                            // get current date
                            $.APP.d1 = new Date();
                            switch ($.APP.state) {
                                case 'alive':
                                    // save timestamp of pause
                                    $.APP.dp = new Date();
                                    $.APP.tp = $.APP.dp.getTime();

                                    // save elapsed time (until pause)
                                    $.APP.td = $.APP.tp - $.APP.t1;

                                    // change button value
                                    $('#' + $.APP.dir + '_start').val('Resume');

                                    // set state
                                    $.APP.state = 'pause';
                                    document.title = $.APP.title + "(Paused) " + $("#timeDisplay").html();
                                    break;
                                case 'pause':
                                    // resume timer
                                    // get current timestamp (for calculations) and
                                    // substract time difference between pause and now
                                    $.APP.t1 = $.APP.d1.getTime() - $.APP.td;

                                    $("#" + $.APP.dir + '_start').val("Pause");
                                    // reset state
                                    $.APP.state = 'alive';
                                    // start loop
                                    $.APP.loopTimer();
                                    break;
                                default:
                                    $.APP.state = "alive";
                                    $("#" + $.APP.dir + '_start').val("Pause");
                                    // get current timestamp (for calculations)
                                    $.APP.t1 = $.APP.d1.getTime();
                                    // start loop
                                    $.APP.loopTimer();
                                    break;
                            }
                        },
                        
                        resetTimer: function () {

                            // reset display
                            $('#timeDisplay').html('00:00:00');

                            // change button value
                            $('#' + $.APP.dir + '_start').val('Work');

                            // set state
                            $.APP.state = 'reset';
                            document.title = $.APP.title;
                        },

                        endTimer: function (callback) {

                            // change button value
                            $('#' + $.APP.dir + '_start').val('Restart');

                            // set state
                            $.APP.state = 'end';

                            // invoke callback
                            if (typeof callback === 'function') {
                                callback();
                            }

                        },

                        loopTimer: function () {
                            var td;
                            var d2, t2;

                            var ms = 0;
                            var s = 0;
                            var m = 0;
                            var h = 0;

                            if ($.APP.state === 'alive') {

                                // get current date and convert it into 
                                // timestamp for calculations
                                d2 = new Date();
                                t2 = d2.getTime();

                                // calculate time difference between
                                // initial and current timestamp
                                if ($.APP.dir === 'sw') {
                                    td = t2 - $.APP.t1;
                                    // reversed if countdown
                                } else {
                                    td = $.APP.t1 - t2;
                                    if (td <= 0) {
                                        // if time difference is 0 end countdown
                                        $.APP.endTimer(function () {
                                            $.APP.resetTimer();
                                            $('#' + $.APP.dir + '_status').html('Ended & Reset');
                                        });
                                    }
                                }

                                // calculate milliseconds
                                ms = td % 1000;
                                if (ms < 1) {
                                    ms = 0;
                                } else {
                                    // calculate seconds
                                    s = (td - ms) / 1000;
                                    if (s < 1) {
                                        s = 0;
                                    } else {
                                        // calculate minutes   
                                        var m = (s - (s % 60)) / 60;
                                        if (m < 1) {
                                            m = 0;
                                        } else {
                                            // calculate hours
                                            var h = (m - (m % 60)) / 60;
                                            if (h < 1) {
                                                h = 0;
                                            }
                                        }
                                    }
                                }

                                // substract elapsed minutes & hours
                                ms = Math.round(ms / 100);
                                s = s - (m * 60);
                                m = m - (h * 60);

                                // update display
                                var time = $.APP.formatTimer(h) + ":" + $.APP.formatTimer(m) + ":" + $.APP.formatTimer(s)
                                $("#timeDisplay").html(time);
                                var status = $.APP.state === "alive" ? "(Working)" : $.APP.state === "pause" ? "(Paused)" : "(Stopped)";
                                document.title = $.APP.title + status + " " + time;

                                // loop
                                $.APP.t = setTimeout($.APP.loopTimer, 1);

                            } else {

                                // kill loop
                                clearTimeout($.APP.t);
                                return true;

                            }
                        },
                        
                        addToHours: function () {
                            var timeSplit = $("#timeDisplay").html().split(":");
                            var hours = parseFloat(timeSplit[0]);
                            var min = parseFloat(timeSplit[1] * 1.0);
                            var sec = parseFloat(timeSplit[2] * 1.0);
                            $("#Hours").val(Math.floor((parseFloat($("#Hours").val()) + hours + (min / 60.0)) * 100000) / 100000);
                        }
                    }
                });

                var status = $("#Status_ID option:selected").text().toLowerCase();
                if (!canImpersonate 
                        || status === "moved to project" 
                        || status === "complete" 
                        || status === "cancelled" 
                        || status === "rejected"
                        || status === "out of scope") 
                    disableObjects();
                                
                $('#sw_start').click(function () {
                    $.APP.startTimer('sw');
                });

                $('#sw_reset').click(function () {
                    $.APP.resetTimer();
                });

                $('#sw_add').click(function () {
                    $.APP.addToHours();
                });

                $("#addNote").keyup(function(event) {
                    if (event.keyCode == 13)
                        addToNotes();
                    event.preventDefault();
                });
                        
            })(jQuery);


            // Attach form validation 
            $("form").bind("submit", function(event) {
                if ($("#addNote").is(":focus"))
                    event.preventDefault();
                else
                    validateStatus(event);
            });

            //$("#Status_ID, #PercentComplete").change(function () { validateStatus(); } );

            loadEnd();
        });

        function validateStatus(event) {
            if (($("#Status_ID option:selected").text() === "Complete" || $("#PercentComplete").val() == 100) && !($("#Status_ID option:selected").text() === "Complete" && $("#PercentComplete").val() == 100)) {
                if (confirm("You are marking this as complete. All related status' will be updated (Percent Complete, Elements, etc).")) {
                    $("#Status_ID > option").each(function () {
                        if ($(this).text() === "Complete") {
                            $("#Status_ID").val($(this).val());
                            $("form :input[type='submit']").prop("disabled", true);
                            return;
                        }
                    });
                    if ($("#PercentComplete").length)
                        $("#PercentComplete").val(100);
                    $("form :input[type='submit']").prop("disabled", true);
                    return true;
                }
                event.preventDefault();
            }
            return true;
        }

        function gotoElement() {
            window.location.replace("/console/CreateElement/<%= Model.ID %>");
        }

        //function promote() {
        //    $("form").attr("action", "/console/CreateProject").submit();
        //}

        function addToNotes() {
            if ($("#addNote").val().length < 10) 
                alert("Let's try to be a little more descriptive, shall we?");
            else if ($("#addNote").val().length > 500)
                alert("That's a little too descriptive, please keep under 500 characters. Currently at " + $("#addNote").val().length + " characters.");
            else {
                runAjax(ajax("addNote", "notes", { "text": $("#addNote").val(), "elementId" : <%= Model.ID %> }, null).AddNote);
                $("#addNote").val("");
            }
        }

        function disableObjects() {
            $("input").each(function() {
                $(this).prop("disabled", "disabled");
            });
            $("textarea").each(function() {
                $(this).prop("disabled", "disabled");
            });
            $("select").each(function() {
                $(this).prop("disabled", "disabled");
            });
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
                date.setDate(date.getDate() + 1)
                date.setHours(8, 0, 0, 0);
                return date;
            }

            function saveToLocalStorage(key, data) {
                if (typeof key !== "string" || typeof data !== "object")
                    return null;

                localStorage.setItem(key, { "data": data, "expiration": getExpiration() });
            }

            return {
                AddNote: 
                    new ajaxCall(ajaxDetails().HttpType.POST,
                        "/console/AddNote",
                        data,
                        ajaxDetails().Synchronous.TRUE,
                        ajaxDetails().ContentType.DEFAULT,
                        function () {
                            $("#" + updateLocation).animate({ opacity: 0 }, 300);
                        },
                        function (result) {
                            $("#" + updateLocation).html(result).removeClass("centerContents").removeClass("hidden").animate({ opacity: 100 }, 150);
                        },
                        function (request, status, error) {
                            $("#" + updateLocation).html("Unable to load due to " + error);
                        })
            }
        }

    </script>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <div id="page">
            <div id="message" class="<%= (bool)ViewData["isValidated"] ? "valid" : "invalid" %>"><%= ViewData["error"] %></div>
            <% using(Html.BeginForm((Model.isNew ? "/create" : "/update") + ((EOpenType)ViewData["type"]).ToString(), "Console")) { %>
            <div id="detailHeader" class="solidBottom">
                <% if (!Model.isNew) { %>
                    <h3><%= Model.Summary %></h3>
                <% } else { %>
                    <label for="Summary">Summary: </label><%= Html.TextBoxFor(x => x.Summary, new { @style = "width: 400px" })  %>
                <% } %>
                    <% if ((EOpenType)ViewData["type"] == EOpenType.Element) { %>
                     <span class="align-right pointer link" onclick="location.href='/Console/Request/<%= ((ElementView)Model).Parent.ID %>'">Parent Request</span>
                    <% } else if ((EOpenType)ViewData["type"] == EOpenType.Request && ((RequestView)Model).Parent != null) { %>
                        <span class="align-right pointer link" onclick="location.href='/Console/Project/<%= ((RequestView)Model).Parent.ID %>'">Parent Project</span>
                    <% } %>
                <%= Html.HiddenFor(x => x.ID) %>
                <table class="fullWidth">
                    <tr>
                        <th class="leftAlign"><label for="status">Status: </label></th>
                        <td class="leftAlign">
                            <% if (((IUser)ViewData["user"]).canImpersonate()) { %>
                                <%= Html.DropDownListFor(m => m.Status.ID, new SelectList((IList<LookupSorted>)ViewData["statusList"], "ID", "Text"))  %>
                            <% } else { %>
                                <%= Model.Status.Text %>
                            <% } %>
                        </td>
                    </tr>
                    <tr>
                        <th><label for="openDate">Opened: </label></th>
                        <td class="leftAlign"><%= Model.OpenDate.ToString("MM/dd/yyyy") %></td>
                        <th><label for="closedDate">Closed: </label></th>
                        <td class="leftAlign"><%= Model.ClosedDate == null ? "" : ((DateTime)Model.ClosedDate).ToString("MM/dd/yyyy") %></td>
                        <th><label for="lastUpdated">Last Updated: </label></th>
                        <td class="leftAlign"><%= Model.LastUpdated %></td>
                    </tr>
                </table>
                <%= Html.HiddenFor(x => x.OpenDate) %> <%= Html.HiddenFor(x => x.ClosedDate) %><%= Html.HiddenFor(x => x.Summary) %>
            </div>
                <% if ((EOpenType)ViewData["type"] == EOpenType.Request || (EOpenType)ViewData["type"] == EOpenType.Project) { %>
                    <%= Html.HiddenFor(x => ((AProjectView)x).InternalHoursMultiplier) %><%= Html.HiddenFor(x => ((AProjectView)x).ExternalHoursMultiplier) %>
                <br />
                <div id="request_project" class="borderBottom">
                    <table>
                        <tr>
                            <th><label for="RequestedBy">Requested By: </label></th>
                            <td id="RequestedBy" class="leftAlign userDetailHover"><%= ((AProjectView)Model).RequestedBy.EnglishName %>
                                <span id="RequestedByUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", ((AProjectView)Model).RequestedBy); %></span><%= Html.HiddenFor(x => ((AProjectView)x).RequestedBy.EmployeeID) %></td>
                            <th><label for="SubmittedBy">Submitted By: </label></th>
                            <td id="SubmittedBy" class="leftAlign userDetailHover"><%= ((AProjectView)Model).SubmittedBy.EnglishName %>
                                <span id="SubmittedByUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", ((AProjectView)Model).SubmittedBy); %></span><%= Html.HiddenFor(x => ((AProjectView)x).SubmittedBy.EmployeeID) %></td>
                        </tr>
                        <tr>
                            <th><label for="Program">Program: </label></th>
                            <td><%= Html.DropDownListFor(m => ((AProjectView)m).Program.ID, new SelectList((IList<ALookup>)ViewData["programList"], "ID", "Text", ((AProjectView)Model).Program == null ? 0 : ((AProjectView)Model).Program.ID), "-- Select Program --") %></td>
                        </tr>
                        <tr>
                            <th><label for="ProjectType">Type: </label></th>
                            <td><%= Html.DropDownListFor(m => ((AProjectView)m).CType.ID, new SelectList((IList<LookupActive>)ViewData["typeList"], "ID", "Text", ((AProjectView)Model).CType.ID, ((IEnumerable<LookupActive>)ViewData["typeList"]).Where(x => !x.Active).Select(y => y.ID))) %></td>
                        </tr>
                        <tr>
                            <th><label for="SupportArea">Support Area: </label></th>
                            <td><%= Html.DropDownListFor(m => ((AProjectView)m).SupportArea.ID, new SelectList((IList<SupportArea>)ViewData["supportArea"], "ID", "Text", ((AProjectView)Model).SupportArea.ID)) %></td>
                        </tr>
                        <tr>
                            <th><label for="ValueDriver">Value Driver: </label></th>
                            <td><%= Html.DropDownListFor(m => ((AProjectView)m).ValueDriver.ID, new SelectList((IList<ALookup>)ViewData["valueDriver"], "ID", "Text", ((AProjectView)Model).ValueDriver.ID)) %></td>
                        </tr>
                        <tr>
                            <th><label for="Value">Value: </label></th>
                            <td colspan="3" style="max-width: 950px"><%= ((AProjectView)Model).Value %> 
                                <br />
                                <%= ((AProjectView)Model).ValueReason %> <%= Html.HiddenFor(x => ((AProjectView)x).ValueReason) %></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table class="lightBorder">
                                    <tr>
                                        <th class="centerAlign noPadding"><label for="RequestedDueDate">Requested Completion</label></th>
                                        <th class="centerAlign noPadding"><label for="EstimatedDueDate">Estimated Completion</label></th>
                                        <th class="centerAlign noPadding"><label for="HoldDate">Hold</label></th>
                                        <th class="centerAlign noPadding"><label for="ResumeDate">Resume</label></th>
                                        <th class="centerAlign noPadding"><label for="ManagerQueueDate">Manager Queue</label></th>
                                        <th class="centerAlign noPadding"><label for="ManagerApproveDate">Manager Approved</label></th>
                                    </tr>
                                    <tr>
                                        <td class="centerAlign noPadding"><%= ((DateTime)((AProjectView)Model).RequestedDueDate).ToString("MM/dd/yyyy") %> <%= Html.HiddenFor(x => ((AProjectView)x).RequestedDueDate) %></td>
                                        <td class="centerAlign noPadding"><%= Html.TextBoxFor(x => ((AProjectView)x).EstimatedDueDate, new { @type = "date" })%></td>
                                        <td class="centerAlign noPadding"><%= ((AProjectView)Model).HoldDate == null ? "" : ((DateTime)((AProjectView)Model).HoldDate).ToString("MM/dd/yyyy") %> <% Html.HiddenFor(x => ((AProjectView)x).HoldDate); %></td>
                                        <td class="centerAlign noPadding"><%= ((AProjectView)Model).ResumeDate == null ? "" : ((DateTime)((AProjectView)Model).ResumeDate).ToString("MM/dd/yyyy") %> <%= Html.HiddenFor(x => ((AProjectView)x).ResumeDate) %></td>
                                        <td class="centerAlign noPadding"><%= ((AProjectView)Model).ManagerQueueDate == null ? "" : ((DateTime)((AProjectView)Model).ManagerQueueDate).ToString("MM/dd/yyyy") %> <%= Html.HiddenFor(x => ((AProjectView)x).ManagerQueueDate) %></td>
                                        <td class="centerAlign noPadding"><%= ((AProjectView)Model).ManagerApprovedDate == null ? "" : ((DateTime)((AProjectView)Model).ManagerApprovedDate).ToString("MM/dd/yyyy") %> <%= Html.HiddenFor(x => ((AProjectView)x).ManagerApprovedDate) %></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <th><label for="EstimatedHours">Estimated Hours: </label></th>
                            <td><%= Html.TextBoxFor(x => ((AProjectView)x).EstimatedHours, new { @type = "number", @step="any" })%></td>
                            <th><label for="EstimatedCost">Estimated Cost: </label></th>
                            <td><%= ((AProjectView)Model).EstimatedCost %><%= Html.HiddenFor(x => ((AProjectView)x).EstimatedCost) %></td>
                        </tr>
                        <tr>
                            <th><label for="Description">Description: </label></th>
                            <td colspan="4"><%= ((AProjectView)Model).Description %> <%= Html.HiddenFor(x => ((AProjectView)x).Description) %></td>
                        </tr>
                        <tr>
                            <th><label for="ManagerNote">Manager Note: </label></th>
                            <td colspan="4"><%= ((AProjectView)Model).ManagerNote %> <%= Html.HiddenFor(x => ((AProjectView)x).ManagerNote) %></td>
                        </tr>
                    </table>                
                </div>
                    <% if ((EOpenType)ViewData["type"] == EOpenType.Request) { %>
                    <div id="request">
                        <table class="fullWidth">
                            <tr>
                                <td style="width: 40%">
                                    <table>
                                        <tr>
                                            <th><label for="AssignedTo">Assigned To:</label></th>
                                            <td><%= Html.DropDownListFor(m => ((RequestView)m).AssignedTo.EmployeeID, new SelectList((IList<IEmployee>)ViewData["assignedTo"], "User.EmployeeID", "User.EnglishName", ((RequestView)Model).AssignedTo.EmployeeID, ((IList<IEmployee>)ViewData["assignedTo"]).Where(x => !x.Active).Select(y => y.ID))) %></td>
                                        </tr>
                                        <tr>
                                            <th><label for="RequesetCategory">Request Category: </label></th>
                                            <td><%= Html.DropDownListFor(m => ((RequestView)m).RequestCategory.ID, new SelectList((IList<LookupActive>)ViewData["requestCategory"], "ID", "Text", ((RequestView)Model).RequestCategory.ID, ((IList<LookupActive>)ViewData["requestCategory"]).Where(x => !x.Active).Select(y => y.ID))) %></td>
                                        </tr>
                                        <tr>
                                            <th><label for="Project">Project: </label></th>
                                            <td><%= Html.DropDownListFor(m => ((RequestView)m).Parent.ID, new SelectList((IList<ProjectView>)ViewData["project"], "ID", "Summary", ((RequestView)Model).Parent == null ? 0 : ((RequestView)Model).Parent.ID, ((IList<ProjectView>)ViewData["project"]).Where(x => x.ClosedDate != null).Select(y => y.ID)), "-- Select Project --") %></td>
                                        </tr>
                                        <tr>
                                            <th><label for="ITFeatures">IT Features: </label></th>
                                            <td><%= Html.EditorFor(x => ((RequestView)Model).ITFeatures) %></td>
                                        </tr>
                                        <tr>
                                            <th><label for="TopOffnetAttributeNumbr">TOA Number: </label></th>
                                            <td><%= Html.EditorFor(x => ((RequestView)Model).TopOffnetAttributeNumber) %></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top">
                                    <label for="elements">Elements</label>
                                    <div id="requestElements" class="fullWidth"><% Html.RenderPartial("Partial/_ARequestList", ((RequestView)Model).ElementList.Cast<ARequestView>().ToList()); %></div>
                                    <input type="button" class="float-right" onclick="gotoElement()" value="New Element"/>
                                </td>
                            </tr>
                        </table>
                        <label for="Resolution">Resolution: </label><%= Html.TextAreaFor(x => ((RequestView)Model).Resolution) %>
                    </div>
                    <% } else { // Projects %>
                    <div id="project" class="fullWidth">
                        <label for="requests">Requests:</label>
                        <div id="requests" class="fullWidth multiDisplay"><% Html.RenderPartial("Partial/_ARequestList", ((ProjectView)Model).RequestList.Cast<ARequestView>().ToList()); %>
                        </div>
                        <input type="button" class="float-right" onclick="location.href='/console/newrequest'" value="Add Request"/>
                    </div>
                    <% } %>
                <% } else { //Elements %>
                <div id="element" class="fullWidth">
                    <%= Html.HiddenFor(x => ((ElementView)x).Parent.ID) %>
                    <table class="fullWidth">
                        <tr>
                            <td style="vertical-align: top; min-width: 350px;">
                                <table>
                                    <tr>
                                        <th style="width:150px"><label for="AssignedTo">Assigned To: </label></th>
                                        <td><%= Html.DropDownListFor(m => ((ElementView)m).AssignedTo.EmployeeID, new SelectList((IList<IEmployee>)ViewData["assignedTo"], "User.EmployeeID", "User.EnglishName", ((ElementView)Model).AssignedTo.EmployeeID)) %></td>
                                    </tr>
                                    <tr>
                                        <th style="width:150px"><label for="PercentComplete">Percent Complete: </label></th>
                                        <td style="width:150px">
                                            <% if(((IUser)ViewData["user"]).canImpersonate()) { %>
                                                <%= Html.TextBoxFor(x => ((ElementView)x).PercentComplete, new { @type = "number", @class="percentage", @min=0, @max=100, @step="any" }) %>
                                            <% } else { %>
                                                <%= ((ElementView)Model).PercentComplete %> <%= Html.HiddenFor(x => ((ElementView)x).PercentComplete) %>
                                            <% } %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:150px"><label for="Hours">Hours : </label></th>
                                        <td style="width:150px">
                                            <% if(((IUser)ViewData["user"]).canImpersonate()) { %>
                                                <%= Html.TextBoxFor(x => ((ElementView)x).Hours, new { @type = "number", @min=0, @step="any" })%>
                                            <% } else { %>
                                                <%= ((ElementView)Model).Hours %><%= Html.HiddenFor(x => ((ElementView)x).Hours) %>
                                            <% } %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input type="button" value="Work" id="sw_start" />
                                        </td>
                                        <td>
                                            <span id="timeDisplay">00:00:00</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><input type="button" value="Reset" id="sw_reset" /><input type="button" value="Add to Hours" id="sw_add" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <% if (!Model.isNew) { %>
                                    <label for="notes">Notes:</label>
                                    <div id="notes" class="fullWidth multiDisplay"><% Html.RenderPartial("Partial/_NoteList", ((ElementView)Model).Note); %>
                                    </div>
                                    <input type="text" id="addNote" style="width: 700px;"/><input type="button" onclick="addToNotes()" value="Add Note"/>
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <label for="Resolution">Resolution: </label>
                        <% if(((IUser)ViewData["user"]).canImpersonate()) { %>
                        <%= Html.TextAreaFor(x => ((ElementView)x).Resolution, new { @type = "text", @class="fullWidth" })%>
                        <% } else { %>
                        <%= ((ElementView)Model).Resolution %><%= Html.HiddenFor(x => ((ElementView)Model).Resolution) %>
                        <% } %>
                </div>
            <% } %>
            <input type="submit" value="<%= Model.isNew ? "Save" : "Update" %>" />
            <%--<% if ((EOpenType)ViewData["type"] == EOpenType.Request && ((RequestView)Model).Parent == null) { %>
                <input type="button" value="Promote to Project" onclick="promote()"/>
            <% } %>--%>
        <% } %>
            </div>
    </div>  
</asp:Content>