<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<Note>>" %>
<% foreach(Note note in Model.OrderByDescending(x => x.Date)) { %>
    <div class="fullWidth lightBorder focusLift">
        <table class="fullWidth noBorder" style="border:none; margin-right: 5px; cursor: default">
            <tr>
                <td style="text-align: left;" colspan="3">
                    <b>Added: </b><%= note.Date %>
                </td>
                <td style="text-align: left;" colspan="4">
                    <b>By:</b> <%= note.UpdatedBy.EnglishName %>
                </td>
            </tr>
            <tr>
                <td colspan="7"><%= note.Text %></td>
            </tr>
        </table>
    </div>
<% } %>
