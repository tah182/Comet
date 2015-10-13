<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<ARequestView>>" %>
<% foreach(ARequestView request in Model) { %>
    <div class="focusLift fullWidth lightBorder" onclick="location.href='/console/<%= (request.GetType() == typeof (ElementView)) ? "element" : "request" %>/<%= request.ID %>'">
        <table class="fullWidth noBorder">
            <tr>
                <td style="text-align: left;" colspan="3">
                    <b>Status: </b><%= request.Status.Text %>
                </td>
                <td style="text-align: right;" colspan="3">
                    <b>Updated:</b> <%= request.LastUpdated %>
                </td>
            </tr>
            <tr>
                <td id="elementSummary" colspan="7"><b>Summary: </b><%= request.Summary %></td>
            </tr>
        </table>
    </div>
<% } %>