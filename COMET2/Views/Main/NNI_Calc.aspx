<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="ToolTitle" ContentPlaceHolderID="TitleContent" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <style>
        label {
            display: inline-block;
            cursor: pointer;
        }
        td {
            vertical-align: bottom;
            padding: 0;
            margin: 0;
        }
        td.radio {
            padding-left: 8px;
        }
        span {
            position: relative;
            left: 20px;
        }
        input {
            margin: 5px 10px;
        }
        input[type="number"] {
            padding: 2px 0 2px 5px;
            display: inline-block;
            min-width: 120px;
            width: 50%;
        }
        input[type="radio"] {
            padding: 2px 0 2px 5px;
            display: inline-block;
            background: transparent;
            cursor: pointer;
            border-collapse: collapse;
            border: none;
        }
        input#NNI_MRC,
        input#Total {
            text-indent: 5px;
        }
        div#content {
            padding-left: 100px;
        }
    </style>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <script>
        function NNICalc() {
            this.oneGB = 1000;
            this.tenGB = 10000;
            this.pricePerMeg1000 = 885.28;
            this.pricePerMeg10000 = 954;
            this.factor = 1.25;
        }
        NNICalc.prototype = {
            evc: function () {
                return Number($("#EVC").val());
            },
            nni_mrc: function () {
                return Number($("#NNI_MRC").val());
            },
            mrc: function () {
                return Boolean($("input:radio[name='gateway']:checked").val() === "Yes") ?
                    0 : (this.perMeg() === 1000 ? this.pricePerMeg1000 : this.pricePerMeg10000);
            },
            perMeg: function () {
                return Number($("input[name='gb']:checked").val()) * 1000;
            },

            calculateTotal: function () {
                var total = Math.round((((this.nni_mrc() + this.mrc()) / this.perMeg() * this.evc() * 1.25) * 100) / 100);
                $("#Total").val(total);
            }
        }
        $(document).ready(function () {
            var nniCalc = new NNICalc();
            $("input").bind("change", function () {
                nniCalc.calculateTotal();
            });
            loadEnd();
        });
    </script>
</asp:Content>

<asp:Content ID="ErrorContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <table class="content">
            <tr>
                <td>
                    <label id="NNI_MRCLabel" for="NNI_MRC" class="label">NNI MRC</label>
                </td>
                <td colspan="2">
                    <span>$</span><input type="number" id="NNI_MRC" />
                </td>
            </tr>
            <tr>
                <td>
                    <h5>NNI Size</h5>
                </td>
                <td class="radio">
                    <input type="radio" name="gb" id="oneGB" value="1" checked /><label for="oneGB">1 GB</label>
                </td>
                <td class="radio">
                    <input type="radio" name="gb" id="tenGB" value="10" /><label for="tenGB">10 GB</label>
                </td>
            </tr>
            <tr>
                <td>
                    <h5>Is the NNI at a Gateway?</h5>
                </td>
                <td class="radio">
                    <input type="radio" id="yesGateway" name="gateway" value="Yes" checked/><label for="yesGateway" class="inline">Yes</label>
                </td>
                <td class="radio">
                    <input type="radio" id="noGateway" name="gateway" value="No" /><label for="noGateway" class="inline">No</label>
                </td>
            </tr>
            <tr>
                <td>
                    <label id="EVCLabel" for="EVC" class="label">EVC size</label>
                </td>
                <td colspan="2" style="padding-left: 8px;">
                    <input type="number" id="EVC" name="form" class="forminput"/>
                </td>
            </tr>
            <tr>
                <td>
                    <label id="TotalLabel" for="Total" class="label">NNI Entrance Cost</label>
                </td>
                <td colspan="2">
                    <span>$</span><input type="number" id="Total" readonly="readonly"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>