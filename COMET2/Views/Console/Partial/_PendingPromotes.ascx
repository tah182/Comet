<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<RequestView>>" %>
<% foreach(RequestView request in Model) { %>
    <div class="focusLift lightBorder" onclick="openItem('PartialRequest/<%= request.ID %>')">
        <label for="requestedBy" class="inline">Requested By:</label>
            <%= request.RequestedBy.EnglishName %>
        <br />
        <label for="assignedTo" class="inline">Assigned To:</label>
        <%= request.AssignedTo.EnglishName %>
        <br />
        <label for="summary" class="inline">Summary:</label>
        <%= request.Summary %>
        <br />
        <label for="value" class="inline">Value:</label>
        <%= request.Value %>
        <br />
        <label for="estimatedCost" class="inline">Estimated Cost:</label>
        <%= request.EstimatedCost %>
        <br />
        <label for="valueDescription" class="inline">Value Description:</label>
        <%= request.ValueReason %>
        <br />
        <label for="description" class="inline">Description:</label>
        <%= request.Description %>
    </div>
<% } %>