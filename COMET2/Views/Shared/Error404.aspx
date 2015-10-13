<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="ToolTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Page Not Found
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    Page Not Found
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/Content/NNI_Calc.css") %>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
</asp:Content>

<asp:Content ID="ErrorContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <h2 class="section_header">
            <hr class="left" />
            Venn of a 404
            <hr class="right" />
        </h2>
        <div class="row">
            <div class="span4 margin-T-20">
                <img src="../../Images/404.png" alt="404" class="fourofour" />
            </div>
            <div class="span8 margin-T-20-2">
                <h2>The Venn Diagram</h2>
                <p>
                    Venn diagrams or set diagrams are diagrams that show all hypothetically possible logical 
                    relations between a finite collection of sets (groups of things). Venn diagrams were conceived 
                    around 1880 by John Venn. They are used in many fields, including set theory, probability, 
                    logic, statistics, computer science, and trying to visit web pages that don't exist.
                </p>
            </div>
        </div>
    </div>
</asp:Content>