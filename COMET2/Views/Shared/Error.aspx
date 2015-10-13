<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error - Comet
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1 class="error">Error.</h1>
        <h2 class="error">An error occurred while processing your request.</h2>
    </hgroup>
    <span id="details"><%= Session["ApplicationError"] %></span>
</asp:Content>
