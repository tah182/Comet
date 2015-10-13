<%@ Import Namespace="COMET.Model.Domain.Shape" %>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
    <script type="text/javascript" src="https://www.google.com/jsapi?key=ABQIAAAA553X4pnNSV4ZnFXCz4NMAxT3UCxAsTt6fsD2mGK9lS_FZYs-kBShPddvkyIiFXktufMOMsRtm6bwmQ"></script>
    <script type="text/javascript" src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/maplabel/src/maplabel-compiled.js"></script>
    
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.multiselect.min.js") %>
    <%: Scripts.Render("~/Scripts/GeneralScripts/jquery.multiselect.filter.js") %>
    <%: Scripts.Render("~/Scripts/PageScripts/IndexJS.js") %>
    <%: Scripts.Render("~/Scripts/PageScripts/Index.subactions.js") %>   
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="overlay" style="position:absolute;width:100%;">
        <div id="legend" class="legend">
            <table>
                <tr>
                    <td class="tooltip">Fiber
                        <div id="fiberSpan">
                            <ul>
                            <li><label for="level3" class="checkbox">
                                <input type="checkbox" id="level3" onclick="getFiber('level3')" />Level 3<span style="background-color: #f00"></span>
                                </label></li>
                            <li><label for="tw" class="checkbox">
                                <input type="checkbox" id="tw" onclick="getFiber('timeWarner')" />TW Telecom<span style="background-color: #00f"></span>
                                </label></li>
                            <% Dictionary<string, string> vendorDict = (Dictionary<string, string>)ViewData["fiberVendors"];
                                foreach (var pair in vendorDict) { %>
                                <li><label for="<%= pair.Key %>" class="checkbox"><input type="checkbox" id="<%= pair.Key %>" <%: new HtmlString("onclick=\"getFiber('" + pair.Key + "')\"") %> />
                                    <%= pair.Key %><span <%: new HtmlString("style='background-color: " + pair.Value + "'") %>></span></label></li>
                            <% } %>
                            </ul>
                        </div>
                    </td>
                    <td class="tooltip">Boundaries
                        <div id="boundarySpan"><ul>
                            <li><label for="lataShowHideMap" class="checkbox">
                                <input type="checkbox" id="lataShowHideMap" onclick="getBoundary('lata')" />Lata Boundary<img src="/Images/dotted.png" style="height:10px"/>
                                </label></li>
                            <li><label for="swcShowHideMap" class="checkbox">
                                <input type="checkbox" id="swcShowHideMap" onclick="getBoundary('swc')" />SWC Boundary<img src="/Images/dotted.png" style="height:10px"/>
                                </label></li>
                            <li><label for="msoShowHideMap" class="checkbox">
                                <input type="checkbox" id="msoShowHideMap" onclick="getBoundary('mso')" />MSO
                                </label></li>
                        </ul></div>
                    </td>
                    <td class="tooltip"><input type="checkbox" id="CLECShowHideMap" onclick="showMarkers()" checked /><label for="CLECShowHideMap" class="checkbox">CLEC <img src="/Images/CircleGrey.png" style="height:10px"/></label>
                        <div>
                            <ul>
                                <li><img src="/Images/CircleFire.png" style="height:10px" /> Level 3 CLEC Building</li>
                                <li><img src="/Images/CircleTeal.png" style="height:10px" /> Level 3/TW CLEC Building</li>
                                <li><img src="/Images/CircleBlue.png" style="height:10px" /> Vendor CLEC Building</li>
                            </ul>
                        </div>
                    </td>
                    <td><input type="checkbox" id="NNIShowHideMap" onclick="showMarkers(this)" checked /><label for="NNIShowHideMap" class="checkbox">NNI <img src="/Images/star.png"/></label></td>
                    <td><input type="checkbox" id="ILECShowHideMap" onclick="showMarkers(this)" checked /><label for="ILECShowHideMap" class="checkbox">ILEC COLO <img src="/Images/pentagon.png"/></label></td>
                    <td title="High Speed Entrance Facility"><input type="checkbox" id="HIGHShowHideMap" onclick="showMarkers(this)" checked /><label for="HIGHShowHideMap" class="checkbox">HSEF <img src="/Images/Square.png"/></label></td>
                    <td title="Low Speed Entrance Facility"><input type="checkbox" id="LOWShowHideMap" onclick="showMarkers(this)" checked /><label for="LOWShowHideMap" class="checkbox">LSEF <img src="/Images/Triangle.png"/></label></td>
                </tr>
            </table>
        </div>
        <div id="map-right" class="map-right selectArea">
            <div id="MinMaxSearch" class="minmaxOpen" title="Minimize / Maximize"></div>
            <div id="search" class="search" style="z-index: 10">
                <div id="searchCriteria" class="content">
                <label id="searchLabel" for="searchMethods" class="label" style="display: inline-block;">Search By:</label>
                    <span id="address" class="tab selected" onclick="searchTableSelect(this)">Address</span>
                    <span id="clli" class="tab " onclick="searchTableSelect(this)">CLLI</span>
                    <span id="latLng" class="tab " onclick="searchTableSelect(this)">Lat/Lng</span>
                <div id="searchMethods">
                    <div id="addressSearchDiv" style="position:relative;" class="selected">
                        <input id="pac-input" class="controls" type="text" placeholder="Detailed Search" autocomplete="off" />
                    </div>
                    <div id="clliSearchDiv" style="display:none; opacity: 0; position:relative;">
                        <input id="clli-input" class="controls" type="text" placeholder="CLLI Code Search" autocomplete="off" />
                    </div>
                    <div id="latLngSearchDiv" style="display:none; opacity: 0;position:relative;">
                        <input id="lat-input" class="controls" type="text" placeholder="Latitude" autocomplete="off" />
                        <input id="lng-input" class="controls" type="text" placeholder="Longitude" autocomplete="off" />
                        <input type="button" id="addressSearch" value="Search"/>
                    </div>
                </div>
                </div><label for="amount" class="inline" style="padding-top: 5px">Range: 2 miles</label>
                <div id="slider-vertical"></div>
            </div>
            <div id="cableLegend" class="minimized expandableDiv search" style="z-index: 9">
                <div class="content">
                    <% foreach (var poly in ((List<ICoordShape>)ViewData["msoVendors"]).Cast<Polygon>()) { %>
                        <div <%: new HtmlString("style='background-color: " + poly.Color + "'") %>><%= poly.Name %></div>
                    <% } %>
                </div>
                <div class="title"><span>MSO Legend</span></div>
            </div>
        </div>
        <div id="detailsLayer">
            <div id="header">
                <ul>
	                <li id="cogDetails" class="details selected" onclick="showHideDetails('cog')">COG</li>
                    <li id="quoteDetails" onclick="showHideDetails('quote')">QUOTE</li>
	                <li id="efDetails" onclick="showHideDetails('ef')">EF</li>
	                <li id="nniDetails" onclick="showHideDetails('nni')">NNI</li>
	                <!-- <li><span>NearNet</span></li>
                    <li><span>OPP RPT</span></li> -->
                </ul>
            </div>
            <div id="content">
                <div id="hider" class="topRight" onclick="raiseDetails(false, '')" style="top:30px;right:10px;">Hide Details</div>
                <div id="subcontent"></div>
            </div>
        </div>
    </div>
    <div id="map-canvas" class="maps">&nbsp;</div>
</asp:Content>
