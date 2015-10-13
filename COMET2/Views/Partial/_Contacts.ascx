<%@ Import Namespace="COMET.Server.Domain" %>
<%@ Import Namespace="COMET.Model.Business.Factory" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IDictionary<string, IList<LKP_VENDOR_LOCKED>>>" %>

<label id="ContactLabel" for="Contact" class="label">Account Rep</label>
<table id="contactsGrid" class="grid">
    <tr>
        <th>Cable Provider</th>
        <th>Contact Name</th>
        <th>&#9742; Office</th>
        <th>&#9742; Mobile</th>
        <th>&#9993; Email</th>
        <th></th>
    </tr>
<% foreach (LKP_VENDOR_LOCKED trans in Model[Model.Keys.First()]) { %>
    <tr>
        <td class="textCell"><%: MainFactory.formatProvider(trans.XREF_PARENT_VENDOR) %></td>
        <td class="textCell"><%: trans.CONTACT_NAME %></td>
        <td class="textCell"><%: trans.CONTACT_OFFICE_PHONE %></td>
        <td class="textCell"><%: trans.CONTACT_MOBLE_PHONE %></td>
        <td class="textCell"><a href="mailto:<%: trans.CONTACT_EMAIL %>"><%: trans.CONTACT_EMAIL %></a></td>
        <td style="text-align: center; cursor: pointer;" onclick="deleteContact('<%: trans.ID %>', '<%= trans.XREF_PARENT_VENDOR.Substring(0, 3) %>')"><img src="../../Images/trash.png" style="height: 12px;"/></td>
    </tr>
<% } %>
    <tr id="addcontact">
        <td class="textCell" colspan="6"><span class="link" onclick="addContact('<%= Model.Keys.First() %>')">+ Add a contact</span></td>
    </tr>
</table>
