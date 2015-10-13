<%@ Import Namespace="COMET.Model.Console.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="DashboardTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/Content/themes/base/jquery.multiselect.css") %>
    <%: Styles.Render("~/Content/themes/base/daterangepicker.css") %>
    <%: Styles.Render("~/Content/Console.css") %>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.multiselect.min.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.multiselect.filter.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.moment.min.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.daterangepicker.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.event.drag-2.2.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/slick.core.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/slick.grid.js") %>
    <%: Scripts.Render("~/Scripts/PageScripts/console.2.js") %>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="search" class="align-right"><a class="link" href="#" onclick="getGrid()">Grid</a><input id="globalSearch" type="text" class="search" /></div>
    <div id="content">
        <div id="menu">
            <ul>
                <li id="PendingPromotes" onclick="openMenu(this, this.id)" title="&#9993; Pending Projects<%= ((int)ViewData["pendingRequests"] <= 0 ? "" : " - " + ViewData["pendingRequests"]) %>">
                    &#9993; Pending Projects<%= ((int)ViewData["pendingRequests"] <= 0 ? "" : " - " + ViewData["pendingRequests"]) %>
                </li>
                <li id="ManageGroups" onclick="openMenu(this, this.id)" title="&#9986; Manage Groups">&#9986; Manage Groups</li>
                <li id="AddFields" onclick="openMenu(this, this.id)" title="&#9769; Add Fields">&#9769; Add Fields</li>
                <li id="Settings" onclick="openMenu(this, this.id)" title="&#9762; Settings">&#9762; Settings</li>
            </ul>
        </div>
        <div id="rightContent">
            <div id="displayDiv" class="displayDiv">    
                <div>
                    <% if(ViewData["type"] != null) { 
                        if ((new string[] {"PendingPromotes"}).Contains((string)ViewData["type"]))
                            Html.RenderPartial("~/Views/Console/Partial/_" + ViewData["type"] + ".ascx", ViewData["partialData"]);
                        else
                            Html.RenderPartial("~/Views/Console/Partial/_" + ViewData["type"] + "View.ascx", ViewData["partialData"]); 
                    } else {
                        Html.RenderPartial("~/Views/Console/Partial/_DashboardGrid.ascx", ViewData["partialData"]); 
                    } %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>