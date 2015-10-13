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
    <%: Scripts.Render("~/Scripts/angular.min.js") %>
    <%: Scripts.Render("~/Scripts/angular-route.min.js") %>
    <%--<%: Scripts.Render("~/Scripts/GeneralScripts/jquery.event.drag-2.2.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/slick.core.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/slick.grid.js") %>--%>
    <%: Scripts.Render("~/Scripts/PageScripts/console.2.js") %>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="search" class="float-right-absolute">
    <% if ((bool) ViewData["isAdmin"]) { %>
        <a class="link" href="#" onclick="getGrid()">Grid</a>
    <% } %>
        <input id="globalSearch" type="text" class="search" />
    </div>
    <div id="content">
        <div id="leftContent">
            <div class="roundedPanel">
                <label for="outstanding" class="inline"><%= ((List<Link>)ViewData["openRequests"]).Count() %> Outstanding Requests</label>
                <div id="outstanding"><% Html.RenderPartial("Partial/_Open", (List<Link>)ViewData["openRequests"]); %></div>
            </div>
            <% if((bool) ViewData["isAdmin"]) { %>
                <div class="roundedPanel">
                    <label for="openElements" class="inline"><%= ((List<Link>)ViewData["openElements"]).Count() %> Tasks still open</label>
                    <div id="openElements"><% Html.RenderPartial("Partial/_Open", (List<Link>)ViewData["openElements"]); %></div>
                </div>
                <div class="roundedPanel">
                    <label for="lastViewed" class="inline"><%= ((List<Link>)ViewData["openProjects"]).Count() %> Projects I'm part of</label>
                    <div id="lastViewed"><% Html.RenderPartial("Partial/_Open", (List<Link>)ViewData["openProjects"]); %></div>
                </div>
            <% } %>
        </div>
        <div id="rightContent">
            <div id="toParent" class="tree" tooltip="To Parent"></div>
            <div id="displayDiv" class="displayDiv">    
                <div>
                    <% if(ViewData["type"] != null) {  
                           Html.RenderPartial("Partial/_" + ViewData["type"] + "View", ViewData["partialData"]);
                    } else { 
                        if((bool) ViewData["isAdmin"])
                            Html.RenderPartial("Partial/_DashboardGrid", ViewData["partialData"]); 
                    } %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>