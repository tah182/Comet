<%@ import Namespace="COMET.Model.Domain" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ExcelData>" %>

<% if (Model.errorType != null) {
    string returnValue = "";
    switch (Model.errorType) {
        case ErrorType.ExcelFormat : 
            returnValue = "The Excel File columns doesn't match the format in the template. Please refer to <a href='" + Url.Action("BulkAddressTemplate", "Bulk") + ">this template</a>";
            break;
        case ErrorType.ExcelOutput : 
            returnValue = "Something happened in generating your file. We're sorry this has happened. Please refresh the page and try again";
            break;
        case ErrorType.FileExtension :
            returnValue = "The Excel File isn't in 2007+ format. It needs to end in '.xlsx'. Please refer to <a href='" + Url.Action("BulkAddressTemplate", "Bulk") + ">this template</a>";
            break;
        case ErrorType.GoogleAPI : 
            returnValue = "Google's address matching failed. There is a chance we may have exceeded our limit for the day. Please try again tomorrow";
            break;
        case ErrorType.HistoricalMaxConnection :
        case ErrorType.SessionOutdated :
            returnValue = "Too much time has elapsed since the excel file was uploaded. Please upload the file again and retry the request";
            break;
        case ErrorType.ImportStoredProcedure :
            returnValue = "Something has happened when processing your request";
            break;
        case ErrorType.ReferenceIdDup :
            returnValue = "One of the reference_id's is duplicated or is incorrect. Only numbers are allowed as a reference_id.";
            break;
        default:
            break;
    }
    %>
    <span style="color: red;"> <%= returnValue %>.</span> 
<% }
        %>
<% else if (Model != null && Model.fitsTemplate) { %>
    <% if (Model.pages > 1) { 
           for (int i = 0; i < Model.pages; i++) { 
                if (i == Model.displayPage - 1) { %>
                    <div style="border: solid 1px #aaa; 
                                background-color: #ffffff; 
                                width: 15px; 
                                text-align: center; 
                                display:inline-block; 
                                padding-left: 3px; 
                                padding-right: 3px;" ><%= i + 1 %>
                    </div> 
            <% } else { %>
                    <div onclick="getPage(<%= i + 1 %>)" style="border: solid 1px #aaa; 
                                                                background-color: #b4c7db; 
                                                                width: 15px; 
                                                                text-align: center;  
                                                                display:inline-block" 
                        class="link"><%= i + 1 %>
                    </div> 
    <% } } } %>
    <table id="excelGrid" class="grid">
        <thead>
            <tr>
                <% foreach (System.Data.DataColumn col in Model.dataTable.Columns) { %>
                    <th><%= col.Caption %></th>
                <% } %>
            </tr>
        </thead>
        <tbody>
            <% foreach (System.Data.DataRow row in Model.dataTable.Rows) { %>
                <tr>
                    <% foreach (var cell in row.ItemArray) { %>
                        <td><%= cell.ToString() %></td>
                    <% } %>
                </tr>
            <% } %>
        </tbody>
    </table>
<% } else { %>
    <span style="color: red;">Something is incorrect with the Excel file. Please check to see if it fits <a href="<%= Url.Action("BulkAddressTemplate", "Bulk") %>">this template</a>.</span>
<% } %>

<script>
    $(document).ready(function () {
        $("#gridView").css("height", ($(window).height() - Math.floor($("#gridView").offset().top) - 55));
        <% if (Model != null && Model.fitsTemplate) { %>
            $("#template").css("display", "none");
            $("#run").css("display", "block");
        <% } %>
    });

    $(window).resize(function () {
        $("#gridView").css("height", ($(window).height() - Math.floor($("#gridView").offset().top) - 55));
    });

    function getPage(page) {
        $.ajax({
            type: "GET",
            url: '<%= Url.Action("ExcelGridSort", "Partial") %>',
            async: true,
            data: { "page": page },
            beforeSend: function () {
                $("#gridView").empty();
                var clone = $("#loader").clone();
                $(clone).appendTo("#gridView");
                $(clone).css("display", "block");
            },
            success: function (returnView) {
                $("#gridView").fadeOut("fast");
                $("#gridView").html(returnView);
                $("#gridView").fadeIn("normal");
            },
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "BulkImportExport";
                Insert.StepName = "get";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-bie-j-g-01";
                    $("#girdView").html(error);
                }   // end if readyState 4
                if (request.readyState == 0) {
                    Insert.ErrorCode = "c-bie-j-g-02";
                    $("#girdView").html(error);
                }   // end if readyState 0
            }   // end error
        });
    }
</script>