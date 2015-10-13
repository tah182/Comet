<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
    &#9732; <%: ViewBag.Message %>
</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" /> 
    <%: Styles.Render("~/Content/Index.css") %>
    <%: Styles.Render("~/Content/themes/base/jquery.multiselect.css") %>
</asp:Content>

<asp:Content ID="JavascriptContent" ContentPlaceHolderID="jscript" runat="server">
    <!--// key = generated, sensor (true = gps enabled)  -->
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=places"></script> 
    <script src="https://www.google.com/jsapi?key=ABQIAAAA553X4pnNSV4ZnFXCz4NMAxT3UCxAsTt6fsD2mGK9lS_FZYs-kBShPddvkyIiFXktufMOMsRtm6bwmQ" type="text/javascript"></script>
    <script type="text/javascript" src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/maplabel/src/maplabel-compiled.js"></script>
    
    <%: Scripts.Render("~/Scripts/PageScripts/E911JS.js") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="overlay" style="position:absolute;width:100%;">
        <div id="legend" class="legend">
            <table>
                <tr>
                    <td class="tooltip">County <img src="/Images/BlackLine.png"/>
                        <div><ul>
                            <li><label for="CountyPolyShowHideMap" class="checkbox"><input type="checkbox" id="CountyPolyShowHideMap" onclick="getCounty(this)" checked />Shape</label></li>
                            <li><label for="CountyLabelShowHideMap" class="checkbox"><input type="checkbox" id="CountyLabelShowHideMap" onclick="getCounty(this)" checked />Label</label></li>
                        </ul></div>
                    </td>
                    <td class="tooltip">Rate Center <img src="/Images/RedLine.png"/>
                        <div><ul>
                            <li><label for="RateCenterPolyShowHideMap" class="checkbox"><input type="checkbox" id="RateCenterPolyShowHideMap" onclick="getRateCenter(this)" checked />Shape</label></li>
                            <li><label for="RateCenterLabelShowHideMap" class="checkbox"><input type="checkbox" id="RateCenterLabelShowHideMap" onclick="getRateCenter(this)" checked />Label</label></li>
                        </ul></div>
                    </td>
                    <td><input type="checkbox" id="PsapPolyShowHideMap" onclick="getPsap(this)" checked /><label for="PsapPolyShowHideMap" class="checkbox">PSAP <img src="/Images/BlueLine.png"/></label></td>
                    <td><input type="checkbox" id="LataPolyShowHideMap" onclick="getLata(this)" /><label for="LataPolyShowHideMap" class="checkbox">LATA <img src="/Images/PinkLine.png"/></label></td>
                </tr>
            </table>
        </div>
        <div id="map-right" class="map-right">
            <div id="search" class="search" style="z-index: 3">
                <div id="searchCriteria" class="content">
                    <label id="searchLabel" for="searchMethods" class="label" style="display: inline-block;">Search By:</label>
                        <span id="address" class="tab selected" onclick="searchTableSelect(this)">Address</span>
                        <span id="project" class="tab " onclick="searchTableSelect(this)">Project</span>
                        <span id="latLng" class="tab " onclick="searchTableSelect(this)">Lat/Lng</span>
                    <div id="searchMethods">
                        <div id="addressSearchDiv" style="position:relative;" class="selected">
                            <input id="pac-input" class="controls" type="text" placeholder="Address Search" autocomplete="off" />
                        </div>
                        <div id="projectSearchDiv" style="display:none; opacity: 0; position:relative;">
                            <input id="project-input" class="controls" type="text" placeholder="Project Search" autocomplete="off" />
                        </div>
                        <div id="latLngSearchDiv" style="display:none; opacity: 0; position:relative;">
                            <input id="lat-input" class="controls" type="text" placeholder="Latitude" autocomplete="off" />
                            <input id="lng-input" class="controls" type="text" placeholder="Longitude" autocomplete="off" />
                            <input id="searchLatLng" type="button" value="Search" onclick="searchLatLng()" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="selectDetail" class="minimized expandableDiv search" style="display: none;">
                <div class="content"></div>
                <div class="title">Details</div>
            </div>
            <div id="MinMaxSearch" class="minmaxOpen" title="Minimize / Maximize"></div>
        </div>
    </div>
    <div id="map-canvas" class="maps">&nbsp;</div>
</asp:Content>
