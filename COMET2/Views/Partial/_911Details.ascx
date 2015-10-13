<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<COMET.Model.E911.Details911>" %>

<% if (Model != null) { %>
    <% if (Model.ShowIcon) { %>
        <img src="http://maps.google.com/mapfiles/ms/icons/red-dot.png" />
    <% } %>
    <table id="detialGrid" class="grid e911Details" >
        <tr><th>County</th>
            <td class="numCell"><%= Model.County %></td>
        </tr>
        <tr><th>Postal Code</th>
            <td class="numCell"><%= Model.ZipCode %></td>
        </tr>
        <tr><th>Fips</th>
            <td class="numCell"><%= Model.Fips %></td>
        </tr>
        <tr><th>Rate Center</th>
            <td class="numCell"><%= Model.RateCenter %></td>
        </tr>
        <tr><th>FCC ID</th>
            <td class="numCell"><%= Model.FCCID %></td>
        </tr>
        <tr><th>Agency</th>
            <td class="numCell"><%= Model.Agency %></td>
        </tr>
        <tr><th>Coverage Area</th>
            <td class="numCell"><%= Model.CoverageArea %></td>
        </tr>
        <tr><th colspan="2">Comments</th>
        </tr>
        <tr><td colspan="2" rowspan="5" style="white-space: unset;"><%= Model.PsapComments %></td></tr>
    </table>
<% } %>