<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DashboardGrid>" %>
<a href="#" onclick="rerunGrid(true)">Clear all</a>
<table class="fulWidth report">
    <tr>
        <th>ID</th>
        <th>Requestor</th>
        <th>Assigned</th>
        <th>Category</th>
        <th>Type</th>
        <th>Area</th>
        <th>Summary</th>
        <th>Submitted</th>
        <th>Due Date</th>
        <th>Status</th>
        <th>Closed</th>
        <th>Hours</th>
        <th>Parent</th>
    </tr>
    <tr>
        <th class="squeeze"><%= Html.TextBoxFor(x => x.SelectedId, new { Class="filter report", type = "number" }) %></th>
        <th class="squeeze"><%= Html.ListBoxFor(x => x.SelectedRequestors, new MultiSelectList(Model.RequestorList, "EmployeeID", "EnglishName", Model.SelectedRequestors), new { Class = "invisible noheight" }) %></th>
        <th class="squeeze"><%= Html.ListBoxFor(x => x.SelectedAssignee, new MultiSelectList(Model.AssignedList, "EmployeeID", "EnglishName", Model.SelectedAssignee), new { Class = "invisible noheight" }) %></th>
        <th class="squeeze"><%= Html.ListBoxFor(x => x.SelectedCategory, new MultiSelectList(Model.CategoryList, "ID", "Text", Model.SelectedCategory), new { Class = "invisible noheight" }) %></th>
        <th class="squeeze"><%= Html.ListBoxFor(x => x.SelectedType, new MultiSelectList(Model.TypeList, "ID", "Text", Model.SelectedType), new { Class = "invisible noheight" }) %></th>
        <th class="squeeze"><%= Html.ListBoxFor(x => x.SelectedArea, new MultiSelectList(Model.AreaList, "ID", "Text", Model.SelectedArea), new { Class = "invisible noheight" }) %></th>
        <th class="squeeze"><%= Html.TextBoxFor(x => x.EnteredSummary, new { Class="filter report", type = "text" }) %></th>
        <th class="squeeze"><%= Html.TextBoxFor(x => x.SubmittedRange, new { Class="dateRange report", type = "text" }) %></th>
        <th class="squeeze"><%= Html.TextBoxFor(x => x.DueDateRange, new { Class="dateRange report", type = "text" }) %></th>
        <th class="squeeze"><%= Html.ListBoxFor(x => x.SelectedStatus, new SelectList(Model.StatusList, "ID", "Text", Model.SelectedStatus), new { Class = "invisible noheight" }) %></th>
        <th class="squeeze"><%= Html.TextBoxFor(x => x.ClosedRange, new { Class="dateRange report", type = "text" }) %></th>
        <th></th>
        <th></th>
    </tr>
    <% int i = 0; %>
    <% foreach (RequestView request in Model.Data) { %>
    <tr>
        <td class="link <%= i % 2 == 0 ? "alt" : "" %>" onclick="openItem('PartialRequest/<%= request.ID %>')"><%= request.ID %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.RequestedBy.EnglishName %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.AssignedTo.EnglishName %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.RequestCategory.Text %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.CType.Text %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.SupportArea == null ? "" : request.SupportArea.Text %></td>
        <td class="ellipse <%= i % 2 == 0 ? "alt" : "" %>" title="<%= request.Summary %>"><%= request.Summary %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.OpenDate.ToString("MM/dd/yyyy") %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= ((DateTime)request.RequestedDueDate).ToString("MM/dd/yyyy") %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.Status.Text %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.ClosedDate == null ? "" : ((DateTime)request.ClosedDate).ToString("MM/dd/yyyy") %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.Hours %></td>
        <td class="<%= i % 2 == 0 ? "alt" : "" %>"><%= request.Parent == null ? "" : request.Parent.ID.ToString() %></td>
    </tr>
    <% i++; } %>
</table>