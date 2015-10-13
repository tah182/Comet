<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="ErrorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
   &#9732;  <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/Content/Activity.css") %>
    <%: Scripts.Render("https://www.google.com/jsapi") %>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <%: Scripts.Render("~/Scripts/PageScripts/Activity.js") %>
</asp:Content>

<asp:Content ID="ErrorContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <div id="menu">
            <ul>
                <li id="activityByGroup" class="selected" onclick="openMenu(this, this.id)" title="&#9773; Activity By Group">
                    &#9773; Activity By Group
                </li>
                <li id="activityByUser" onclick="openMenu(this, this.id)" title="&#10004; Activity By User">&#10004; Activity By User</li>
                <li id="trends" onclick="openMenu(this, this.id)" title="&#0916; Trends">&#0916; Trends</li>
                <li id="futures" onclick="openMenu(this, this.id)" title="&#9728; Futures">&#9728; Futures</li>
            </ul>
        </div>
        <div id="rightContent">
            <div id="showFullScreen" class="float-left-absolute button">Full Screen</div>
            <div id="usageByTime" class="graph"></div>
            <div id="graphDetails"></div>
        </div>
    </div>
</asp:Content>
