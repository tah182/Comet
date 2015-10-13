<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<NNI>>" %>

<label id="nniLabel" for="nni" class="label">NNI Details</label>
<% if (Model != null && Model.Count > 0) { %>
    <table id="nniGrid" class="grid">
        <tr>
            <th>NNI Trail</th>
            <th>RFAID</th>
            <th>NNI Vendor</th>
            <th>NNI Fac Ecckt</th>
            <th>Address</th>
            <th title="in Mbs">Used</th>
            <th title="in Mbs">Total</th>
            <th>Util %</th>
        </tr>
    <% foreach (var nni in Model) { %>
        <tr>
            <td class="textCell"><%: nni.TrailName %></td>
            <td class="textCell"><%: nni.RFAID %></td>
            <td class="textCell"><%: nni.Vendor %></td>
            <td class="textCell"><%: nni.ECCKT %></td>
            <td class="textCell"><%: nni.FullStreet %><br /><%: nni.City %>, <%: nni.State %> <%: nni.PostalCode %></td>
            <td class="numCell"><%: string.Format("{0: #,##0}", nni.UsedMbps) %></td>
            <td class="numCell"><%: string.Format("{0: #,##0}", nni.TotalMbps) %></td>
            <td class="numCell"><%: string.Format("{0: 0.00%}", nni.Utilization) %></td>
        </tr>
    <% } %>
    </table>
<% } else { %>
    No NNI's found within the range selected. Please try widening the range.
<% } %>