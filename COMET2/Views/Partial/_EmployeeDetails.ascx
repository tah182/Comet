<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<COMET.Model.Domain.IUser>" %>

<% if (Model != null) { %>
    <span>
        <%: Model.EnglishName %>
    </span>
    <table class="grid">
        <tr>
            <th colspan="2" style="text-align:right"><%: Model.Title %></th>
        </tr>
        <tr>
            <th>Department:</th>
            <td><%: Model.DeptName %></td>
        </tr>
        <tr>
            <th>&#9742; Office:</th>
            <td><%: Model.OfficePhone %></td>
        </tr>
        <tr>
            <th>&#9742; Mobile:</th>
            <td><%: Model.MobilePhone %></td>
        </tr>
        <tr>
            <th>&#9993; Email:</th>
            <td><a href="mailto:<%: Model.EmailAddress %>"><%: Model.EmailAddress %></a></td>
        </tr>
        <tr>
            <th>Office Location:</th>
            <td><%: Model.Building.OfficeName %></td>
        </tr>
        <tr>
            <th>Office Address:</th>
            <td><%: Model.Building.FullStreet %></td>
        </tr>
        <tr>
            <th>Office City:</th>
            <td><%: Model.Building.City %></td>
        </tr>
        <tr>
            <th>Office Postal:</th>
            <td><%: Model.Building.PostalCode %></td>
        </tr>
        <tr>
            <th>Hire Date:</th>
            <td><%: Model.HireDate.ToString("MM/dd/yyyy") %></td>
        </tr>
        <tr>
            <th>Status:</th>
            <td><%: Model.TermDate == null ? "Active" : ("In-active as of " + ((DateTime)Model.TermDate).ToString("MM/dd/yyyy")) %></td>
        </tr>
    </table>
<% } %>