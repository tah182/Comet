<%@Import Namespace="COMET.Model.Domain" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<COMET.Model.Domain.View.PlanningSearch>" %>

<% if (Model != null) { %>
    <div id="lataSearch" class="expandableDiv search dynamic" style="z-index: 5">
        <div class="content">
            <h4 style="padding-bottom: 10px">Lata Territory </h4>
            <div class="contentData"><%= (Model.Lata ?? "Unable to Determine") %></div>
        </div>
        <div class="title" style="display:none;">
            <span>Lata Territory</span> <%= (Model.Lata ?? "Unable to Determine") %>
        </div>
    </div>
    <div id="swcSearch" class="expandableDiv search dynamic" style="z-index: 4">
        <div class="content">
            <h4 style="padding-bottom: 10px">SWC Territory </h4>
            <div class="contentData"><%= (Model.SWC ?? "Unable to Determine") + (Model.LECVendor == null ? "" : " - " + Model.LECVendor) %></div>
        </div>
        <div class="title" style="display:none;">
            <span>SWC Territory</span> <%= (Model.SWC ?? "Unable to Determine") + (Model.LECVendor == null ? "" : " - " + Model.LECVendor) %>
        </div>
    </div>
    <div id="msoSearch" class="expandableDiv search dynamic" style="z-index: 3">
        <div class="content">
            <h4 style="padding-bottom: 10px">MSO Territory </h4>
            <% if(Model.MSO.Length < 1) { %>
                <div class="contentData">None found</div>
            <% } else {
                foreach (string s in Model.MSO) { %>
                    <div class="cursor contentData" onclick="fillContactInfo('<%= s %>')"><%= s %></div>
            <% }} %>
        </div>
        <div class="title" style="display:none;">
            <span>MSO Territory</span> <%= (Model.MSO.Length < 1 ? "None found" : Model.MSO.Length == 1 ? Model.MSO[0] : Model.MSO.Length + " Found") %>
        </div>
    </div>
    <div id="onNetSearch" class="expandableDiv search dynamic" style="z-index: 2">
        <div class="content">
            <% IEnumerable<string> vendors = Model.NetBuildList.Where(a => a.Rank == 0).Select(b => b.Vendor).Distinct().OrderBy(x => x); %>
                <h4 style="padding-bottom: 10px">CLEC On-Net <%= vendors.Count() > 0 ? " - " + Model.NetBuildList.Where(a => a.Rank == 0).OrderBy(b => b.DistanceMiles).Select(x => x.CLLI ?? "").FirstOrDefault() : "" %></h4>
            <% if(vendors.Count() < 1) { %>
                <div class="contentData">None found</div>
            <% } else {
                foreach (string netBuild in vendors) { 
                    if (netBuild.ToLower().Equals("level3") || netBuild.ToLower().Equals("level3_tw")) { %>
                        <div class="cursor contentData linkLevel3" onclick="fillContactInfo('Level3')"><%= netBuild %></div>
                    <% } else { %>
                        <div class="cursor contentData" onclick="fillContactInfo('<%= netBuild %>')"><%= netBuild %></div>
                    <% }
                }
            } %>
        </div>
        <div class="title" style="display:none;">
            <span>CLEC On-Net</span> <%= vendors.Count() + " found" %>
        </div>
    </div>
    <div id="nearNetSearch" class="expandableDiv search dynamic">
        <div class="content">
            <% vendors = Model.NetBuildList.Where(a => a.Rank != 0).Select(b => b.Vendor).Distinct().OrderBy(x => x); 
               List<string> vendorRank = Model.NetBuildList.OrderBy(x => x.Rank).Select(y => y.Vendor).Distinct().ToList(); %>
                <h4>CLECs in range </h4>
            <% if(vendors.Count() < 1) { %>
                <div class="contentData">None found</div>
            <% } else { %>
            <table id="clecGrid" class="grid">
                <tr>
                    <th>Vendor Parent Name</th>
                    <th>CLLI_CD</th>
                    <th>Rank</th>
                    <th>Dist.(mi)</th>
                </tr>
                <% foreach (string netBuild in vendors) { 
                    List<NetworkBuilding> buildingList = Model.NetBuildList.Where(x => x.Vendor.Equals(netBuild) && x.Rank != 0).ToList(); %>        
                    <tr class="headRow">
                        <td><img src="../../Images/expand.gif" class="cursor" onclick="expandCollapse('clecGrid', this, '<%= netBuild.Substring(0, netBuild.IndexOf(" ") > 0 ? netBuild.IndexOf(" ") : netBuild.Length).Replace("&", "") %>')" />
                            <span class="<%= netBuild.ToLower().Equals("level3") || netBuild.ToLower().Equals("level3_tw") 
                                ? "linkLevel3" 
                                : "cursor" %>" onclick="fillContactInfo('<%= netBuild %>')">
                            <%= netBuild %></span>
                        </td>
                        <td class="textCell"><%= buildingList.Count() %> Bldgs</td>
                        <td class="numCell"><%= vendorRank.FindIndex(x => x.Equals(netBuild)) + 1 %></td>
                        <td class="numCell"><%= buildingList.Select(x => x.DistanceMiles).Min() %></td>
                    </tr>
                    <% foreach(NetworkBuilding nb in buildingList) { %>
                    <tr class="<%= netBuild.Substring(0, netBuild.IndexOf(" ") > 0 ? netBuild.IndexOf(" ") : netBuild.Length).Replace("&", "") %>"style="display: none;">
                        <td></td>
                        <td class="textCell"><%= nb.CLLI %></td>
                        <td class="numCell"></td>
                        <td class="numCell"><%= nb.DistanceMiles %></td>
                    </tr>
                    <% }
                }
            } %>
            </table>
        </div>
        <div class="title" style="display:none;">
            <span>CLECs in range</span> <%= vendors.Count() + " found" %>
        </div>
    </div>
<% } %>