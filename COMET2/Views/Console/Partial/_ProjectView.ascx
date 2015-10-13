<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<COMET.Model.Console.Domain.View.ProjectView>" %>
<% string closedDate = Model.ClosedDate == null ? "N/A" : Model.ClosedDate.Value.ToString("MM/dd/yyyy"); %>
<div class="fullWidth solidBottom">
    <label for="title" style="font-size: 1.3em;" class="inline"><%= Model.Status.Text %> - <%= Model.Summary %></label>
    <% if ((bool)ViewData["isAdmin"]) { %>
        <span class="align-right pointer link" onclick="location.href='/console/project/<%= Model.ID %>'")">Edit</span>
    <% } %>
</div>
<div id="detailContainer" class="fullWidth">
    <div id="primaryDetailContainer">
        <table>
            <tr>
                <th><label for="assignedTo">Assigned To:</label></th>
                <td id="assignedTo" onmouseover="showUserDetails(this)"><%= Model.AssignedTo.EnglishName %><br />
                    <span id="assignedToUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", Model.AssignedTo); %></span>
                </td>
            </tr>
            <tr>
                <th><label for="requestedBy">Requested By:</label></th>
                <td id="requestedBy" onmouseover="showUserDetails(this)"><%= Model.RequestedBy.EnglishName %><br />
                    <span id="requestedByUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", Model.RequestedBy); %></span>
                </td>
            </tr>
            <tr>
                <td colspan ="2" />
            </tr>
            <tr>
                <th><label for="supportArea">Support Area:</label></th>
                <td id="supportArea"><%= Model.SupportArea.Text %></td>
            </tr>
            <tr>
                <th><label for="type">Request Type:</label></th>
                <td id="type"><%= Model.CType.Text %></td>
            </tr>
            <tr>
                <th><label for="program">Program:</label></th>
                <td id="program"><%= Model.Program == null ? "" : Model.Program.Text %></td>
            </tr>
        </table>
    </div>
    <div id="datesContainer">
        <table>
            <tr>
                <th><label for="lastUpdatedDate">Last Updated:</label></th>
                <td id="lastUpdatedDate" class="rightAlign"><%= Model.LastUpdated.ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="submitDate">Submitted:</label></th>
                <td id="submitDate" class="rightAlign"><%= Model.OpenDate.ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="desiredDate">Requested Due Date:</label></th>
                <td id="desiredDate" class="rightAlign"><%= Model.RequestedDueDate == null ? "" : ((DateTime)Model.RequestedDueDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="desiredDate">Started:</label></th>
                <td id="startDate" class="rightAlign"><%= Model.StartDate == null ? "" : ((DateTime)Model.StartDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="estimateddate">Estimated Due Date:</label></th>
                <td id="estimateddate" class="rightAlign"><%= Model.EstimatedDueDate == null ? "" : ((DateTime)Model.RequestedDueDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="holdDate">Hold:</label></th>
                <td id="holdDate" class="rightAlign"><%= Model.HoldDate == null ? "" : ((DateTime)Model.HoldDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="resumeDate">Resume:</label></th>
                <td id="resumeDate" class="rightAlign"><%= Model.ResumeDate == null ? "" : ((DateTime)Model.ResumeDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="managerQueueDate">Manager Queue:</label></th>
                <td id="managerQueueDate" class="rightAlign"><%= Model.ManagerQueueDate == null ? "" : ((DateTime)Model.ManagerQueueDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="approvedDate">Manager Approved:</label></th>
                <td id="approvedDate" class="rightAlign"><%= Model.ManagerApprovedDate == null ? "" : ((DateTime)Model.ManagerApprovedDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
                <th><label for="closedDate">Closed:</label></th>
                <td id="closedDate" class="rightAlign"><%= Model.ClosedDate == null ? "" : ((DateTime)Model.ClosedDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <tr>
        </table>
    </div>
    <%--<div id="textContainer">--%>
        <table>
            <tr>
                <td rowspan="2">
                    <table class="lightBorder" style="min-width:225px">
                        <tr>
                            <td></td>
                            <th class="rightAlign">Estimated</th>
                            <th class="rightAlign">Actual</th>
                        </tr>
                        <tr>
                            <th>Hours</th>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.EstimatedHours ?? 0) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.Hours) %></td>
                        </tr>
                        <tr>
                            <th>Cost</th>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.EstimatedCost ?? 0) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.ActualCost ?? 0) %></td>
                        </tr>
                        <tr>
                            <th>Value / Cost</th>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.EstimatedCost == 0 ? 0 : (Model.Value / Model.EstimatedCost)) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.ActualCost == 0 ? 0 : (Model.Value / Model.ActualCost)) %></td>
                        </tr>
                        <tr>
                            <th>Complete</th>
                            <td class="rightAlign"></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00%}", Model.PercentComplete / 100) %></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <label for="valueDriver" class="inline">Value Driver: </label>&nbsp;<%= Model.ValueDriver.Text %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="valueDescription" class="header">Value Description</label><p class="header"><%= Model.ValueReason %></p>
                </td>
            </tr>
        </table>
        <br />
        <label for="description" class="header">Description</label><br /><p class="header"><%= Model.Description %></p>
        <br />
        <label for="requests" style="font-size: 1.2em">Requests:</label>
        <table id="requests" style="font-size: 0.83em;" class="report">
            <tr>
                <th>Assigned To</th>
                <th>Status</th>
                <th class="rightAlign">Hours</th>
                <th class="rightAlign">Complete</th>
                <th>Summary</th>
                <th class="rightAlign">Assigned</th>
                <th class="rightAlign">Closed</th>
            </tr>
            <% foreach(RequestView request in Model.RequestList) { %>
            <tr>
                <td><%= request.AssignedTo.EnglishName %></td>
                <td><%= request.Status.Text %></td>
                <td class="rightAlign"><%= request.Hours %></td>
                <td class="rightAlign"><%= string.Format("{0: 0.00%}", request.PercentComplete / 100) %></td>
                <td class="link ellipse" onclick="openItem('PartialRequest/<%= request.ID %>')" title="<%= request.Summary %>"><%= request.Summary %></td>
                <td class="rightAlign"><%= request.OpenDate.ToString("MM/dd/yyyy") %></td>
                <td class="rightAlign"><%= request.ClosedDate == null ? "" : ((DateTime)request.ClosedDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <% } %>
        </table>
    <%--</div>--%>
</div>