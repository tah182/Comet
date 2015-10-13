<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" MasterPageFile="~/Views/Shared/WebForm.Master" Inherits="AmoBi.Reports.ReportViewer" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="ErrorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    AMO DataTeam Reports
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    AMO DataTeam Reports
</asp:Content>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadEnd();
        });
    </script>
</asp:Content>

<asp:Content ID="ReportContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="reportViewerTable">
            <tr>
                <td>
                <rsweb:ReportViewer runat="server" ID="ReportPortal" Width="99%" Height="99%" ShowPrintButton="true"
                        DocumentMapCollapsed="true" ProcessingMode="Remote" ShowBackButton="true" EnableViewState="true"
                        ShowDocumentMapButton="false" ShowExportControls="true" AsyncRendering="true" BackColor="#f5f5fa"
                        InternalBorderColor="black" InternalBorderStyle="solid" InternalBorderWidth="1px" 
                        SizeToReportContent="true" PromptAreaCollapsed="true" ShowToolBar="true" ZoomMode="PageWidth">                        
                    <ServerReport 
                            ReportPath="" 
                            ReportServerUrl="http://vidcrpt0001/reportserver" />
                </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </form>
</asp:Content>
