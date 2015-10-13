<%@ Import Namespace="COMET.Model.Domain" %>
<%@ Import Namespace="COMET.Model.Console.Domain.View" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RequestView>" %>
<% string closedDate = Model.ClosedDate == null ? "N/A" : Model.ClosedDate.Value.ToString("MM/dd/yyyy"); %>
<div class="fullWidth solidBottom">
    <label for="title" style="font-size: 1.3em;" class="inline">
        <%= Model.Status.Text.ToLower().Equals("moved to project") && Model.Parent == null ? "Pending Project Approval" : Model.Status.Text %> - <%= Model.Summary %></label>
        <% if(Model.Parent != null) { %>
            <%= Html.Hidden("hasParent", "PartialProject/" + Model.Parent.ID) %>
            <%= Html.Hidden("parentName", "Project: " + Model.Parent.Summary) %>
        <% } %>
    <% if ((bool)ViewData["isAdmin"]) { %>
        <span class="align-right pointer link" onclick="location.href='/console/request/<%= Model.ID %>'")">Edit</span>
    <% } %>
</div>
<div id="server-message" class="invalid"></div>
<div id="detailContainer" class="fullWidth" data-ng-app="requestAng">
    <div id="primaryDetailContainer" class="editable">
        <table>
            <tr>
                <th><label for="assignedTo">Assigned To:</label></th>
                <td>
                    <span class="field">
                        <span class="editable-field" id="assignedTo" onmouseover="showUserDetails(this)"><%= Model.AssignedTo.EnglishName %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Assigned To</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                    <br />
                    <span id="assignedToUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", Model.AssignedTo); %></span>
                </td>
            </tr>
            <tr>
                <th><label for="requestedBy">Requested By:</label></th>
                <td>
                    <span class="field">
                        <span class="editable-field" id="requestedBy" onmouseover="showUserDetails(this)"><%= Model.RequestedBy.EnglishName %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Requested By</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                    <br />
                    <span id="requestedByUser" class="userDisplay"><% Html.RenderPartial("../Partial/_EmployeeDetails", Model.RequestedBy); %></span>
                </td>
            </tr>
            <tr>
                <td colspan ="2" />
            </tr>
            <tr>
                <th><label for="supportArea">Support Area:</label></th>
                <td id="supportArea">
                    <span class="field">
                        <span class="editable-field"><%= Model.SupportArea.Text %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Support Area</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="type">Request Type:</label></th>
                <td id="type">
                    <span class="field">
                        <span class="editable-field"><%= Model.CType.Text %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Request Type</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="category">Category:</label></th>
                <td id="category">
                    <span class="field">
                        <span class="editable-field"><%= Model.RequestCategory.Text %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Category</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="program">Program:</label></th>
                <td id="program">
                    <span class="field">
                        <span class="editable-field"><%= Model.Program == null ? "" : Model.Program.Text %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Program</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <div id="datesContainer" class="editable">
        <table>
            <tr>
                <th><label for="lastUpdatedDate">Last Updated:</label></th>
                <td id="lastUpdatedDate" class="rightAlign">
                    <span class="field">
                        <span class="uneditable"><%= Model.LastUpdated.ToString("MM/dd/yyyy") %></span>
                        <button class="edit-button" style="visibility: hidden">
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="submitDate">Submitted:</label></th>
                <td id="submitDate" class="rightAlign">
                    <span class="field">
                        <span class="uneditable"><%= Model.OpenDate.ToString("MM/dd/yyyy") %></span>
                        <button class="edit-button" style="visibility: hidden">
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="desiredDate">Requested Due Date:</label></th>
                <td id="desiredDate" class="rightAlign">
                    <span class="field">
                        <span class="editable-field"><%= Model.RequestedDueDate == null ? "" : ((DateTime)Model.RequestedDueDate).ToString("MM/dd/yyyy") %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Requested Due Date</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="estimateddate">Estimated Due Date:</label></th>
                <td id="estimateddate" class="rightAlign">
                    <span class="field">
                        <span class="editable-field"><%= Model.EstimatedDueDate == null ? "" : ((DateTime)Model.EstimatedDueDate).ToString("MM/dd/yyyy") %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Estimated Due Date</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="holdDate">Hold:</label></th>
                <td id="holdDate" class="rightAlign">
                    <span class="field">
                        <span class="uneditable"><%= Model.HoldDate == null ? "" : ((DateTime)Model.HoldDate).ToString("MM/dd/yyyy") %></span>
                        <button class="edit-button" style="visibility: hidden">
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="resumeDate">Resume:</label></th>
                <td id="resumeDate" class="rightAlign">
                    <span class="field">
                        <span class="uneditable"><%= Model.ResumeDate == null ? "" : ((DateTime)Model.ResumeDate).ToString("MM/dd/yyyy") %></span>
                        <button class="edit-button" style="visibility: hidden">
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="managerQueueDate">Manager Queue:</label></th>
                <td id="managerQueueDate" class="rightAlign">
                    <span class="field">
                        <span class="uneditable"><%= Model.ManagerQueueDate == null ? "" : ((DateTime)Model.ManagerQueueDate).ToString("MM/dd/yyyy") %></span>
                        <button class="edit-button" style="visibility: hidden">
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="approvedDate">Manager Approved:</label></th>
                <td id="approvedDate" class="rightAlign">
                    <span class="field">
                        <span class="editable-field"><%= Model.ManagerApprovedDate == null ? "" : ((DateTime)Model.ManagerApprovedDate).ToString("MM/dd/yyyy") %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Manager Approved Date</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
            <tr>
                <th><label for="closedDate">Closed:</label></th>
                <td id="closedDate" class="rightAlign">
                    <span class="field">
                        <span class="editable-field"><%= Model.ClosedDate == null ? "" : ((DateTime)Model.ClosedDate).ToString("MM/dd/yyyy") %></span>
                        <button class="showEditable edit-button">
                            <span class="edit-text">Edit Manager Approved Date</span>
                            <i class="edit-field-icon"></i>
                        </button>
                    </span>
                </td>
            </tr>
        </table>
    </div>
        <table>
            <tr>
                <td rowspan="2">
                    <table class="lightBorder" style="min-width:225px">
                        <tr>
                            <td></td>
                            <th class="rightAlign">Estimated</th>
                            <th class="rightAlign">Actual</th>
                        </tr>
                        <tr>
                            <th>Hours</th>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.EstimatedHours ?? 0) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.Hours) %></td>
                        </tr>
                        <tr>
                            <th>Cost</th>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.EstimatedCost ?? 0) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.ActualCost ?? 0) %></td>
                        </tr>
                        <tr>
                            <th>Value / Cost</th>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.EstimatedCost == 0 ? 0 : (Model.Value / Model.EstimatedCost)) %></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00}", Model.ActualCost == 0 ? 0 : (Model.Value / Model.ActualCost)) %></td>
                        </tr>
                        <tr>
                            <th>Complete</th>
                            <td class="rightAlign"></td>
                            <td class="rightAlign"><%= string.Format("{0: 0.00%}", Model.PercentComplete / 100) %></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <label for="valueDriver" class="inline">Value Driver: </label>&nbsp;<%= Model.ValueDriver.Text %>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="valueDescription" class="header">Value Description</label><p class="header"><%= Model.ValueReason %></p>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <th><label for="itFeature">Related IT Feature:</label></th>
                <td id="itFeature"><%= Model.ITFeatures %></td>
            </tr>
            <tr>
                <th><label for="toa">Related TOA:</label></th>
                <td id="toa"><%= Model.TopOffnetAttributeNumber %></td>
            </tr>
        </table>
        <br />
        <label for="description" class="header">Description</label><br /><p class="header"><%= Model.Description %></p>
        <br />
        <label for="resoltuion" class="header">Resolution</label><br /><p class="header"><%= Model.Resolution %></p>
        <br />
        <label for="elements" style="font-size: 1.2em">Elements:</label>
        <table id="elements" style="font-size: 0.83em;" class="report">
            <tr>
                <th>Assigned To</th>
                <th>Status</th>
                <th class="rightAlign">Hours</th>
                <th class="rightAlign">Complete</th>
                <th>Summary</th>
                <th class="rightAlign">Assigned</th>
                <th class="rightAlign">Closed</th>
            </tr>
            <% foreach(ElementView element in Model.ElementList) { %>
            <tr>
                <td><%= element.AssignedTo.EnglishName %></td>
                <td><%= element.Status.Text %></td>
                <td class="rightAlign"><%= element.Hours.ToString("0.00") %></td>
                <td class="rightAlign"><%= element.PercentComplete %>%</td>
                <td class="link ellipse" onclick="openItem('PartialElement/<%= element.ID %>')" title="<%= element.Summary %>"><%= element.Summary %></td>
                <td class="rightAlign"><%= element.OpenDate.ToString("MM/dd/yyyy") %></td>
                <td class="rightAlign"><%= element.ClosedDate == null ? "" : ((DateTime)element.ClosedDate).ToString("MM/dd/yyyy") %></td>
            </tr>
            <% } %>
        </table>
    <% if (Model.Status.Text.ToLower().Equals("moved to project") && Model.Parent == null && ((IUser)Session["user"]).isBIManager()) { %>
        <input type="date" id="startDate" name="startDate" />
        <input type="button" value="Promote to Project" onclick="<%= "promote(" + Model.ID + ")" %>"/>
    <% } %>
</div>