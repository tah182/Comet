<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Import Namespace="COMET.Model.Business.Factory" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Quote>>" %>

<%--<% using(Html.BeginForm("ExportQuotes", "Partial", new {lat = Model[0].LatLng.Lat, lon = Model[0].LatLng.Lng, range = Model.range}, FormMethod.Post, new { enctype = "multipart/form-data" })) { %>--%>
<label id="lookupLabel1" for="lookupGrid" class="label" style="display: inline-block;">IFO Quotes</label>
<input type="submit" name="Export" id="Export" value="Export" /> 
<%--    <table id="lookupGrid" class="grid">
        <tr>
            <th>Vendor</th>
            <th>CLLI Code</th>
            <th>Quote Created</th>
            <th style="text-align:right">Bandwidth</th>
            <th style="text-align:right">MRC</th>
            <th style="text-align:right">Incremental NRC</th>
        </tr>
    <% if (Model.Count < 1) { %>
        <tr>
            <td colspan="6">
                No quotes within range. Try increasing range
            </td>
        </tr>
        <% } else { 
           foreach (var quote in Model) { %>
        <tr>
            <td class="textCell"><%: quote.Vendor %></td>
            <td class="textCell"><%: quote.AddressClli %></td>
            <td class="textCell"><%: quote.QuoteCreateDate %></td>
            <td class="numCell"><%: string.Format("{0: #,##0}", quote.BandwidthCapacity) %></td>
            <td class="numCell">$<%: string.Format("{0: #,##0.00}", quote.mrcUSD) %></td>
            <td class="numCell">$<%: string.Format("{0: #,##0.00}", quote.IncrementalNrCost) %></td>
        </tr>
        <% } 
    } %>
        <tr>
            <td colspan="6">
                <span class="link" onclick="showHideDetail('quote')">Show all quotes within range</span>
            </td>
        </tr>
    </table>   
<% } else if (Model.quoteList.Count() > 0) { %>--%>
    <table id="quoteGrid" class="grid">
        <tr>
            <th style="text-align:right;">Entity ID</th>
            <th>Vendor</th>
            <th>Description</th>
            <th>Quote Won</th>
            <th title="CLLI Code associated directly to Address">CLLI Code</th>
            <th title="CLLI Code for the Serving wire center">SWC Code</th>
            <th>Quote Created</th>
            <th style="text-align:right">Price Term ID</th>
            <th title="Bandwidth capacity" style="text-align:right">Bandwidth</th>
            <th title="Ethernet Bandwidth" style="text-align:right">Ethernet</th>
            <th title="Monthly Rate Cost" style="text-align:right">MRC</th>
            <th style="text-align:right">Incremental MRC</th>
            <th style="text-align:right">Incremental NRC</th>
            <th title="Distance (in miles) from the Address searched" style="text-align:right">Dist. (mi)</th>
            <th>Address</th>
        </tr>
    <% foreach (var quote in Model) { %>
        <tr>
            <td class="numCell"><%: quote.EntityLineItemId %></td>
            <td class="textCell"><%: MainFactory.formatProvider(quote.Vendor) %></td>
            <td class="textCell"><%: quote.BandwidthDesc %></td>
            <td class="textCell"><%: quote.IsWin ? "Yes" : "No" %></td>
            <td class="textCell"><%: quote.AddressClli %></td>
            <td class="textCell"><%: quote.SwcCLLI %></td>
            <td class="textCell"><%: quote.QuoteCreateDate.ToString("MM/dd/yyyy") %></td>
            <td class="numCell"><%: quote.PriceTermId %></td>
            <td class="numCell"><%: string.Format("{0: #,##0}", quote.BandwidthCapacity) %></td>
            <td class="numCell"><%: string.Format("{0: #,##0}", quote.BandwidthEthernet) %></td>
            <td class="numCell">$<%: string.Format("{0: #,##0.00}", quote.mrcUSD) %></td>
            <td class="numCell">$<%: string.Format("{0: #,##0.00}", quote.IncrementalMrCost) %></td>
            <td class="numCell">$<%: string.Format("{0: #,##0.00}", quote.IncrementalNrCost) %></td>
            <td class="numCell"><%: quote.distance %></td>
            <td class="textCell"><%: quote.FullStreet + " " + quote.City + ", " + quote.State %></td>
        </tr>
    <% } %>
    </table>