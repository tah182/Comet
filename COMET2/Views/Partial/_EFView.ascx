<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<EntranceFacility>>" %>

<% if (Model.Count() > 0) { %>
<%--<% using(Html.BeginForm("ExportEF", "Partial", new {lat = Model.lat, lon = Model.lon, range = Model.range, ordering = Model.ordering}, FormMethod.Post, new { enctype = "multipart/form-data" })) { %>--%>
    <label id="efLabelWithData" for="ef" class="label" style="display: inline-block;">EF Details</label>
    <input type="submit" name="Export" id="Export" value="Export" /> Show: 
    <%--<label for="az0" class="inline"><input id="az0" type="radio" name="az" value="0" onchange="ShowEF($(this).val())" <%= Model != null ? Model[0].AOrdering ? "checked" : "" : "" %>/>A End</label>
    <label for="az1" class="inline"><input id="az1" type="radio" name="az" value="1" onchange="ShowEF($(this).val())" <%= Model != null ? Model[0].AOrdering ? "checked" : "" : "" %> />Z End</label>--%>
    
    <table id="efGrid" class="grid">
        <tr>
            <th>Legacy</th>
            <th>Trail</th>
            <th>RFAID</th>
            <th>Vendor</th>
            <th>Ecckt</th>
            <th>Address</th>
            <th>Speed</th>
            <th title="in Mbs">Used</th>
            <th title="in Mbs">Total</th>
            <th>Util %</th>
            <th>Map</th>
        </tr>
    <% foreach (var ef in Model)
    { %>
        <tr>
            <td class="textCell" style="max-width: 250px"><%: ef.Legacy %></td>
            <td title="<%= ef.TrailName %>"class="textCell" style="max-width: 250px"><%: ef.TrailName %></td>
            <td class="textCell"><%: ef.RFAID %></td>
            <td class="textCell"><%: ef.Vendor %></td>
            <td title="<%= ef.ECCKT %>"class="textCell" style="max-width: 250px"><%: ef.ECCKT %></td>
            <td class="textCell"><%: ef.Address.FullStreet %><br /><%: ef.Address.City %>, <%: ef.Address.State %> <%: ef.Address.PostalCode %></td>
            <td class="textCell"><%: ef.HighSpeed ? "High Speed" : "Low Speed"  %></td>
            <td class="numCell"><%: ef.UsedSlots %></td>
            <td class="numCell"><%: ef.TotalSlots %></td>
            <td class="numCell"><%: string.Format("{0: 0.00%}", ef.Utilization ) %></td>
            <td class="textCell"><span class="link" onclick="plotLatLon('<%: ef.TrailName %>', '<%: ef.Address.LatLng.Lat %>', '<%: ef.Address.LatLng.Lng %>')">Show</span>
        </tr>
    <% } %>
    </table>
<% } else { %>
    <label id="efLabelNoData" for="ef" class="label">EF Details</label>
    No Entrance Facilities found nearby. Increase range and try again.
<% } %>