<%@ Import Namespace="COMET.Model.Console.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NewRequestModel>" %>

<asp:Content ID="NewRequestTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/Content/Console.css") %>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <script>
        var isAdmin = Boolean("<%= ViewData["isAdmin"] %>" === "True");
        //var requestTypes, requestAreas;
        var request;
        $(document).ready(function () {
            request = new Request(<%: ((IUser)Session["User"]).EmployeeID %>);
            <% if (ViewData["displayMessage"] != null) { %>
            <% if ((bool)ViewData["success"]) { %>
            $("#errorDetails").removeClass("invalid").addClass("valid").html("<%= ViewData["displayMessage"] %>");
            <% } else { %>
            $("#errorDetails").removeClass("valid").addClass("invalid").html("<%= ViewData["displayMessage"] %>");
            <% } %>
            <% } %>
            $("#requestor").val("<%: ((IUser)Session["User"]).EnglishName %>");
            continueLoad();
        });
    </script>
    <%: Scripts.Render("~/Scripts/PageScripts/Console.NewRequest.2.js") %>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <div id="contentHolder">
            <div id="errorDetails" style="position:relative" class="invalid"></div>
            <div id="displayDiv" style="padding-top: 50px;">
                <table id="contentTable" style="width: 80%; margin: auto">
                    <tr style="border-bottom: 1pt solid #AAA">
                        <td><table class="fullWidth">
                            <tr>
                                <th><label for="requestor">Requestor: </label></th>
                                <td><div id="hiddenRequestor" style="display:none;"><%: ((IUser)Session["User"]).EmployeeID %></div>
                                <% if (!(bool) ViewData["isAdmin"]) { %>
                                    <span><%= ((IUser)Session["User"]).EnglishName %></span>
                                    <% } else { %>
                                    <input type="text" id="requestor" value=""/>
                                    <% } %>
                                </td>
                                <th><label>Submitted By: </label></th>
                                <td><%: ((IUser)Session["User"]).EnglishName %></td>
                            </tr>
                            <tr>
                                <th><label>Group Name: </label></th>
                                <td><div id="groupName"><%: (string)ViewData["groupName"] %></div></td>
                                <th><label>Group Manager: </label></th>
                                <td><div id="groupManager"><%: (string)ViewData["groupManager"] %></div></td>
                            </tr>
                        </table></td>
                    </tr>
                    <tr>
                        <td><table class="fullWidth">
                            <tr>
                                <th class="align-bottom"><label for="requestArea">Request Area*</label></th>
                                <th class="align-bottom tooltip"><label for="requestType">Request Type* ?</label></th>
                                    <th class="align-bottom tooltip">
                                        <label for="requestCategory" style="<%: ((bool)ViewData["isAdmin"] ? "" : "visibility: hidden") %>">Request Category* ?</label>
                                    </th>
                                <th></th>
                            </tr>
                            <tr>
                                <td class="align-top"><select id="requestArea" class="inline"></select></td>
                                <td class="align-top"><select id="requestType" class="inline"></select></td>
                                    <td class="align-top">
                                        <select id="requestCategory" class="inline" style="<%: ((bool)ViewData["isAdmin"] ? "" : "visibility: hidden") %>"></select>
                                    </td>
                                <td></td>
                            </tr>
                            <tr>
                                <th colspan="2" class="align-bottom"><label for="requestSummary">Request Summary*</label></th>
                                <th class="align-bottom tooltip"><label for="valueDriver">Value Driver* ?</label></th>
                                <th class="align-bottom"><label for="value">Value</label></th>
                            </tr>
                            <tr>
                                <td colspan="2" class="align-top"><input type="text" id="requestSummary" class="inline" size="50" required/>&nbsp;&nbsp;<div id="summaryRemaining" class="inline" style="font-size:0.75em"></div></td>
                                <td class="align-top"><select id="valueDriver" class="inline"></select></td>
                                <td class="align-top"><input type="number" id="value" style="width: 80px"/></td>
                            </tr>
                            <tr>
                                <th colspan="2" class="align-bottom"><label for="requestDescription">Description*</label></th>
                                <th colspan="2" class="align-bottom"><label for="valueDescription">Value Description*</label></th>
                            </tr>
                            <tr>
                                <td colspan="2" class="align-top"><textArea rows="12" id="requestDescription" required></textArea></td>
                                <td colspan="2" class="align-top"><textarea rows="12" id="valueDescription" required></textarea></td>
                            </tr>
                            <tr>
                                <td colspan="2"><label for="dueDate">Desired Due Date</label> <input type="date" id="dueDate" /></td>
                                <td colspan="2" style="text-align: right"><input type="button" onclick="submit()" value="Submit"/></td>
                            </tr>
                        </table></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>