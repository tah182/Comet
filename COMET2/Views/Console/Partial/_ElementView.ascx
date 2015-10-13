<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<COMET.Model.Console.Domain.View.ElementView>" %>
<% string closedDate = Model.ClosedDate == null ? "N/A" : Model.ClosedDate.Value.ToString("MM/dd/yyyy"); %>
<div class="fullWidth solidBottom">
    <label for="title" style="font-size: 1.3em;" class="inline"><%= Model.Status.Text %> - <%= Model.Summary %></label>
        <%= Html.Hidden("hasParent", "PartialRequest/" + Model.Parent.ID) %>
        <%= Html.Hidden("parentName", "Request: " + Model.Parent.Summary) %>
    <% if ((bool)ViewData["isAdmin"]) { %>
        <span class="align-right pointer link" onclick="location.href='/console/element/<%= Model.ID %>'")">Edit</span>
    <% } %>
</div>
<div id="detailContainer" class="fullWidth partialElement">
    <div id="primaryDetailContainer">
        <table>
            <tr>
                <th><label for="assignedTo">Assigned To:</label></th>
                <td id="assignedTo" onmouseover="showUserDetails(this)"><%= Model.AssignedTo.EnglishName %><br />
                    <span id="assignedToUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", Model.AssignedTo); %></span>
                </td>
            </tr>
            <tr>
                <td colspan ="2" />
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
                            <th class="rightAlign">Hours</th>
                            <th class="rightAlign">Complete</th>
                        </tr>
                        <tr>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.Hours) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0}", Model.PercentComplete) %>%</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <label for="requests" style="font-size: 1.2em">Notes:</label>
        <table id="requests" style="font-size: 0.83em;" class="report">
            <tr>
                <th>Added by</th>
                <th class="rightAlign">Date</th>
                <th>Details</th>
            </tr>
            <% foreach(Note note in Model.Note) { %>
            <tr>
                <td><%= note.UpdatedBy.EnglishName %></td>
                <td class="rightAlign"><%= note.Date.ToString("MM/dd/yyyy") %></td>
                <td><%= note.Text %></td>
            </tr>
            <% } %>
        </table>
    <%--</div>--%>
</div>