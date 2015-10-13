<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="BulkTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Bulk Import/Export
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" /> 
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <script src="http://malsup.github.com/jquery.form.js"></script>
    <script>
        $(document).ready(function () {
            loadEnd();

            $("#template").click(function () {
                window.open('<%= Url.Content("~/Bulk/BulkAddressTemplate") %>');
            });

            $("#run").click(function () {
                $("#gridView").html("<span style='color: green'>Please wait... This process may take a few minutes</span>");
                var uri = "<%= Url.Content("~/Bulk/ExcelGridRun") %>"
                            + "?distance=" + $("#distance").val()
                            + "&topCount=" + $("#topCount").val();
                window.open(uri);
            });

            $("#form0").submit(function (event) {
                var fileName = $("#uploadFile").val();
                var fileSplit = fileName.split(".");

                var formData = new FormData($("#form0").get(0));
                //var fileInput = document.getElementById("uploadFile");
                //for (i = 0 ; i < fileInput.files.length; i++)
                //    formData.append(fileInput.files[i].name, fileInput.files[i]);
                event.preventDefault();
                var action = $("#form0").attr("action");
                contentType = false;
                processData = false;
                var updateTimer = 0;
                $.ajax({
                    type: "POST",
                    url: action,
                    data: formData,
                    contentType: contentType,
                    processData: processData,
                    beforeSend: function () {
                        $("#gridView").empty();
                        var clone = $("#loader").clone();
                        $(clone).appendTo("#gridView");
                        $(clone).css("display", "block");
                        updateTimer = setInterval(updateProgress, 10000);
                    },
                    success: function (returnView) {
                        clearInterval(updateTimer);
                        $("#gridView").fadeOut("fast");
                        $("#gridView").html(returnView);
                        $("#gridView").fadeIn("normal");
                    },
                    complete: function () {
                        clearInterval(updateTimer);
                    },
                    error: function (request, status, error) {
                        var Insert = new ErrorInsert();
                        Insert.PageName = "BulkImportExport";
                        Insert.StepName = "post";
                        Insert.ErrorDetails = request.responseText + " -- " + error;
                        if (request.readyState == 4) {
                            Insert.ErrorCode = "c-bie-j-p-01";
                            $("#girdView").html(error);
                        }   // end if readyState 4
                        if (request.readyState == 0) {
                            Insert.ErrorCode = "c-bie-j-p-02";
                            $("#girdView").html(error);
                        }   // end if readyState 0
                    }   // end error
                });
            });
        });

        function CheckFileName() {
            var fileName = $("#uploadFile").val();
            var fileSplit = fileName.split(".");
            //debugger;
            if (fileName == "") {
                $("#gridView").html("<span style='color: red;'>Browse to upload a valid File with xls / xlsx extension</span>");
                return false;
            } else if (fileSplit[fileSplit.length - 1].toLowerCase() == "xls" || fileSplit[fileSplit.length - 1].toLowerCase() == "xlsx") {
                return true;
            } else {
                $("#gridView").html("<span style='color: red;'>File with " + fileSplit[fileSplit.length - 1] + " is invalid. Upload a validfile with xls / xlsx extensions</span>");
                return false;
            }
            return true;
        }

        function updateProgress() {
            $.ajax({
                type: "GET",
                url: "<%= Url.Content("~/Bulk/getProgress") %>",
                success: function (result) {
                    $("#gridView").height("200");
                    $('#gridView').html("<progress id='bar' value='" + result.toFixed(2) * 100 + "' max='100'></progress> " + (result * 100).toFixed(2) + "% completed").show();
                },
                error: function (request, status, error) {
                    $("#gridView").height("200");
                    $('#gridView').html("Could not get status of progress.<br>" + error).show();
                }   // end error
            });
        }
    </script>
</asp:Content>

<asp:Content ID="BulkContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id='loader' style='display:none; font-size:20px; font-weight: bold; color:#D3ACCC;'>
        loading...
    </div>
    <div style="margin-top: 20px; padding-bottom: 30px;">
        <% using (Ajax.BeginForm("ExcelToGrid", "Bulk", new { id = "form0" }, new AjaxOptions {}, new { enctype = "multipart/form-data"})) { %>
            <label for="distance" style="display: inline-block">Upload File Here &nbsp;&nbsp; </label>
            <input id="uploadFile" name="uploadFile" type="file" />&nbsp;&nbsp; <input id="uploadSubmit" type="submit" value="UploadFile" title="Upload File" onclick="return CheckFileName()"/>
        <% } %>
        <table>
            <tr>
                <td><label for="distance" style="display: inline-block">How far will you like to search?</label></td>
                <td><select id="distance" style="display: inline-block">
                    <option value="0">OnNet Only</option>    
                    <option value="0.2">0.2 Mile</option>
                    <option value="0.4">0.4 Mile</option>
                    <option value="0.6">0.6 Mile</option>
                    <option value="0.8">0.8 Mile</option>
                    <option value="1">1 Mile</option>
                    <option value="2" selected>2 Miles</option>
                    <option value="3">3 Miles</option>
                    <option value="4">4 Miles</option>
                    <option value="5">5 Miles</option>
                    <option value="6">6 Miles</option>
                    <option value="7">7 Miles</option>
                    <option value="8">8 Miles</option>
                    <option value="9">9 Miles</option>
                    <option value="10">10 Miles</option>
                    <option value="15">15 Miles</option>
                    <option value="20">20 Miles</option>
                    </select>
                </td>
                <td rowspan="2">        
                    <input id="template" type="submit" value="Template" title="Get Template"/>
                    <input id="run" type="submit" value="Run" title="Run these values" style="display:none;"/>
                </td>
            </tr>
            <tr>
                <td><label for="topCount" style="display: inline-block">Results per Address: </label></td>
                <td><select id="topCount" style="display: inline-block">
                        <option value="1">1 Results</option>
                        <option value="4">4 Results</option>
                        <option value="6">6 Results</option>
                        <option value="8">8 Results</option>
                        <option value="10" selected>10 Results</option>
                        <option value="12">12 Results</option>
                        <option value="14">14 Results</option>
                        <option value="16">16 Results</option>
                        <option value="18">18 Results</option>
                        <option value="20">20 Results</option>
                        <option value="25">25 Results</option>
                        <option value="40">40 Results</option>
                    </select>
                </td>
            </tr>
        </table>
        <br /><br />
        <div id="gridView" style="width: 97%; max-width: 97%; background-color: white; overflow: auto;">
            &nbsp;
        </div>
    </div>
</asp:Content>