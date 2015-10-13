<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="BulkClliTitle" ContentPlaceHolderID="TitleContent" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="jscript" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#clliSubmit").click(function () {
                var clliString = $.trim($("#clliTextArea").val()).replace(/[^A-Z0-9]+/ig, ', ');
                var splitClli = clliString.split(", ");

                for (var i = 0; i < splitClli.length; i++) {
                    if (splitClli[i].length != 8) {
                        $("#showResult").text("Something is incorrect in the input, please check each CLLI Code for 8 characters and spacing."
                            + " Issue is somehwere near '" + splitClli[i] + "'")
                                        .css("color", "red");
                        break;
                    }
                    //} else if (splitClli.length > 150) {
                    //    $("#showResult").text("Sorry, at this moment Bulk Clli requests are limited to 150 per request.")
                    //                    .css("color", "red");
                    //    break;
                    //}
                    if (i === splitClli.length - 1) {
                        $("#clliTextArea").text(clliString);
                        $("#showResult").text("Processing request (This may take up to 3 minutes)... ")
                                        .css("color", "green");

                        var form = document.createElement("form");
                        form.method = "POST";
                        form.action = "BulkClli/ExportBulkClli";

                        $(form).append(createInput("number", "distance", $("#distance").val()));
                        $(form).append(createInput("number", "topCount", $("#topCount").val()));
                        $(form).append(createInput("text", "clliString", clliString));
                        $(form).append($("<input>").attr("type", 'submit').val('Submit'));
                
                        $("#gridDisplay").css("display", "none").append(form);
                        form.submit();

                        function createInput(type, name, value) {
                            return $("<input>").attr("type", type).attr("name", name).val(value);
                        };

                        <%--uri = "<%= Url.Content("BulkClli/ExportBulkClli") %>?clliString=" + clliString
                            + "&distance=" + $("#distance").val()
                            + "&topCount=" + $("#topCount").val();
                        //window.open(encodeURIComponent(uri));
                        window.open(uri);--%>
                    }
                }
            });
            loadEnd();
        });
    </script>
</asp:Content>

<asp:Content ID="BulkContent" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br />
    <div id="search" style="padding-bottom: 150px; width: 95%; padding-left: 30px">
        <label for="clliTextArea" style="font-size: 0.95em">Enter Clli Codes to lookup here. Please separate with any non-alpha or numeric character (e.g. ";", ", ")</label>
        <label id="showResult"> </label>
        <textarea id="clliTextArea" rows="15" cols="50"></textarea>
        <br />
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
                <td rowspan="2"><input id="clliSubmit" type="button" value="Submit" /></td>
            </tr>
            <tr>
                <td><label for="topCount" style="display: inline-block">Results per CLLI: </label></td>
                <td><select id="topCount" style="display: inline-block">
                        <option value="2">2 Results</option>
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
    </div>
    <div id="gridDisplay">

    </div>
</asp:Content>
