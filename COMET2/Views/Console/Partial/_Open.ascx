<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Link>>" %>
<%--<table id="outstandingRequests" class="fullWidth" style="padding: 0">
<%--<% int i = 0;
    foreach(Link link in Model) { %>
    <% if (i % 2 == 0) { %> <tr> <% } %>
    <td style="text-align: center; padding: 1px 2px 1px 2px" id="link <%= i %>" class="link" onclick="openItem('<%= Url.Content(link.RelativeUrl) %>')" title="<%= link.ToolTip %>"><%= link.Text %></td>
    <% if (i % 2 != 0) { %> </tr> <% } i++; %>
<% } %>--%>
<%--  %><%  foreach(Link link in Model) { %>
    <tr>
     <td id="link<%= link.RelativeUrl%>" class="highlight ellipse" onclick="openItem('<%= Url.Content(link.RelativeUrl) %>')"><%-- tooltip="<%= link.ToolTip %>"> --%>
<%--  %>         <span>
             <%= link.Text %>
         </span>
     </td>
    </tr>
<% } %>
</table>--%>
<ul class="nomark">
    <% foreach(Link link in Model) { %>
        <li id="link<%= link.RelativeUrl %>" class="highlight ellipse" onclick="openItem('<%= Url.Content(link.RelativeUrl) %>')">
            <span class="<%= link.Class %>" tooltip="<%= link.Text %>">
                <%= link.Text %>
            </span>
        </li>
    <% } %>
</ul>