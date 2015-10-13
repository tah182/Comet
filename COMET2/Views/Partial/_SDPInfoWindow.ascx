<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<COMET.Model.Domain.EntranceFacility>" %>

<div id="location" class="location">
<% if (Model != null) {%>
<label for="title"><%= Model.Address.FullStreet + " (" + Model.Address.City + ", " + Model.Address.State + ") - " + Model.Address.CLLI %>
        <% if (Model.hasNni) { %><img src="../../Images/star.png" /> <% } %>
        <% if (Model.hasLowSpeedShared) { %><img src="../../Images/Triangle.png" /> <% } %>
        <% if (Model.hasHighSpeedShared) { %><img src="../../Images/Square.png" /> <% } %>
        <% if (Model.hasIlecColo) { %><img src="../../Images/pentagon.png" /><% } %>
    </label>
        <% if ((Model.hasHighSpeedShared || Model.hasLowSpeedShared || Model.hasNni) && Model.hasIlecColo) { %>
            <span id="sdp" class="tab selected" onclick="ShowLocationSwitch(this)">SDP</span>
            <span id="ilec" class="tab" onclick="ShowLocationSwitch(this)">ILEC COLO</span> 
        <% } %>
        <% if (-1 > 0) { %><span id="nni" class="tab " onclick="ShowLocationSwitch(this)">Lat/Lon</span><% } %>
    <% if (Model.hasHighSpeedShared || Model.hasLowSpeedShared || Model.hasNni) { %>
        <div id="sdpDiv">
            <table id="sdpTable">
                <tbody>
                    <tr>
                        <th class='textCell'>Count of SDP Trails:</th>
                        <td class='numCell'><%= Model.NumTrails %></td>
                        <th class='textCell padLeft'></th>
                        <td class='numCell'></td>
                    </tr>
                    <tr>
                        <th class='textCell'>Utilization LSEF:</th>
                        <td class='numCell floatRight'><%= String.Format("{0:P2}", Model.LowSpeedSharedUtilization / 100) %></td>
                        <td class="textCell">
                            <% if (Model.LsefTrending == TrendSlope.NONE) { %>
                            <img src="<%= Url.Content(Model.LsefTrending.Image) %>" />
                            <% } else { %>
                            <img src="<%= Url.Content(Model.LsefTrending.Image) %>" class="cursor" onclick="getTrend(<%= Model.Address.AddressID %>, 0)" title="Show Trends"/>
                            <% } %>
                        </td>
                        <% if (Model.hasLowSpeedShared) { %>
                            <th class="textCell padLeft linkDark" onclick="fillEfInfo(1, <%= Model.Address.AddressID %>, 'false')" title="Show Low Speed Trails")>Number of LSEF's:</th>
                        <% } else { %>
                            <th class="textCell padLeft">Number of LSEF's:</th>
                        <% } %>
                        <td class='numCell'><%= Model.NumLowSpeedShared %></td>
                    </tr>
                    <tr>
                        <th class='textCell'>Utilization of NNI:</th>
                        <td class='numCell'><%= String.Format("{0:P2}", Model.NniUtilization / 100) %></td>
                        <td class="textCell">
                            <% if (Model.NniTrending == TrendSlope.NONE) { %>
                            <img src="<%= Url.Content(Model.NniTrending.Image) %>" />
                            <% } else { %>
                            <img src="<%= Url.Content(Model.NniTrending.Image) %>" class="cursor" onclick="getTrend(<%= Model.Address.AddressID %>, 2)" title="Show Trends"/>
                            <% } %>
                        </td>
                        <% if(Model.hasNni) { %>
                            <th class='textCell padLeft linkDark' onclick='fillNNIInfo(<%= Model.Address.AddressID %>)' title="Show NNI Trails">Number of NNI's:</th>
                        <% } else { %>
                            <th class='textCell padLeft'>Number of NNI's:</th>
                        <% } %>
                        <td class='numCell'><%= Model.NumNni %></td>
                    </tr>
                    <tr>
                        <th class='textCell'>Utilization of HSEF:</th>
                        <td class='numCell'><%= String.Format("{0:P2}", Model.HighSpeedSharedUtilization / 100) %></td>
                        <td class="textCell">
                            <% if (Model.HsefTrending == TrendSlope.NONE) { %>
                            <img src="<%= Url.Content(Model.HsefTrending.Image) %>" />
                            <% } else { %>
                            <img src="<%= Url.Content(Model.HsefTrending.Image) %>" class="cursor" onclick="getTrend(<%= Model.Address.AddressID %>, 1)" title="Show Trends"/>
                            <% } %>
                        </td>
                        <% if(Model.hasHighSpeedShared) { %>
                            <th class='textCell padLeft linkDark' onclick='fillEfInfo(1, <%= Model.Address.AddressID %>, "true")' title="Show High Speed Trails">Number of HSEF's:</th>
                        <% } else { %>
                            <th class='textCell padLeft'>Number of HSEF's:</th>
                        <% } %>
                        <td class='numCell'><%= Model.NumHighSpeedShared %></td>
                    </tr>
                </tbody>
            </table>
            <table id='percTable' class='grid'>
                <tr>
                    <th>Cust</th><th>Cust Hybrid</th><th>Switch</th><th>IP Net</th><th>Net Other</th><th>No Prod</th>
                </tr>
                <tr><td><%= String.Format("{0:P2}", Model.CustIndPerc / 100) %></td> 
                    <td><%= String.Format("{0:P2}", Model.CustHybridIndPerc / 100) %></td> 
                    <td><%= String.Format("{0:P2}", Model.SwitchIndPerc / 100) %></td> 
                    <td><%= String.Format("{0:P2}", Model.IpNetIndPerc / 100) %></td>
                    <td><%= String.Format("{0:P2}", Model.NetOtherIndPerc / 100) %></td>
                    <td><%= String.Format("{0:P2}", Model.NoProdIndPerc / 100) %></td>
                </tr>
            </table>
        </div>
    <% } %>
    <% if (Model.hasIlecColo) { %>
        <div id='ilecDiv' <%= (Model.hasHighSpeedShared || Model.hasLowSpeedShared) ? "style='opacity: 0;" : "style='opacity: 1;" %>'>
            <table>
                <tbody>
                    <tr>
                        <th colspan ="2"><%= (Model.IlecColo.RecordOwner + " - " + Model.IlecColo.FacilityType) %></th>
                        </tr>
                    <tr>
                        <th>Node Name: </th>
                        <td style="padding-left: 5px"><%= Model.IlecColo.NodeName %></td>
                        </tr>
                    <tr>
                        <th>Status: </th>
                        <td style="padding-left: 5px"><%= Model.IlecColo.LifeCycleStatus %></td>
                        </tr>
                    <tr>
                        <th>Primary Homing Gateway: </th>
                        <td style="padding-left: 5px"><%= Model.IlecColo.PrimaryHomingGateway %></td>
                        </tr>
                </tbody>
            </table>
        </div>
    <% } %>
            
    <table id="locationDetailsTable">
        <tbody>
            <tr>
                <th>LATA:</th>
                <td class='numCell'><%= Model.Lata %></td>
                <td rowspan="4" style="padding-left: 15px"><img src="//maps.googleapis.com/maps/api/staticmap?size=150x80&zoom=13&markers=size:small|color:red|<%= Model.Address.LatLng.Lat + "," + Model.Address.LatLng.Lng %>" /></td>
            </tr>
            <tr>
                <th>Latitude:</th>
                <td class='numCell' style="padding-left: 10px"><%= Model.Address.LatLng.Lat %></td>
            </tr>
            <tr>
                <th>Longitude:</th>
                <td class='numCell' style="padding-left: 10px"><%= Model.Address.LatLng.Lng %></td>
            </tr>
            <tr>
                <th>GTMI Building ID:</th>
                <td class='numCell' style="padding-left: 10px"></td>
            </tr>
        </tbody>
    </table>
<% } else { %>
    <h3 style="color: black">Location Details were not found. </h3>
    <p>Issue has been logged. Please contact <a href="mailto:amo_datateam@level3.com">AMO DataTeam</a> if you you need further assistance.</p>
    <% } %>
</div>

<div id="trendGraph" class="floatRight">
    <div id="graph"></div>
    <div id="close" class="topRight" onclick="hideTrend()">&#10006;</div>
</div>
