using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Console.Domain;
using COMET.Model.Console.Domain.View;
using COMET.Model.Console.Business.Service;

namespace COMET.Model.Business.Factory {
    public class ConsoleFactory {
        /// <summary>
        /// Retreives the service to create employees with
        /// </summary>
        /// <returns>The active service for creating employees</returns>
        public static IEmployeeSvc getEmployeeSvc() {
            return new EmployeeSvcImplDB();
        }

        /// <summary>
        /// Retreieves the service to create new requests with
        /// </summary>
        /// <returns>The active service for creating requests.</returns>
        public static IRequestSvc getRequestSvc() {
            return new RequestSvcImplDB();
        }

        public static IList<Link> createLink(EOpenType type, IList<ARequestView> items, bool isPartial) {
            List<Link> links = new List<Link>();
            string link = "";
            link += isPartial ? "Partial" : "";
            switch (type) {
                case EOpenType.Element :
                    link +=  "Element";
                    break;
                case EOpenType.Project:
                    link += "Project";
                    break;
                case EOpenType.Request: 
                    link += "Request";
                    break;
                default:
                    break;
            }

            foreach (ARequestView item in items) {
                Link l = new Link(type.ToString().Left(1) + item.ID + " - " + item.Summary, item.Summary.Replace("\"", "&quot;"), link + "/" + item.ID);
                if (type == EOpenType.Request && ((RequestView)item).RequestedDueDate < DateTime.Today)
                    l.Class = "invalid";
                links.Add(l);
            }

            return links;
        }

        public static string requestEmailSupportBody(RequestView request) {
            string url = "http://" + MainFactory.getInstance() + "/Console/Dashboard/Request/" + request.ID;

            string email = "<html>";
            email += "<h3>A new request has been assigned to you.</h3><br /><table>";
            email += "<tr><th style='text-align: left'>Requested by: </th><td style='text-align: left'>" + request.RequestedBy.EnglishName + "</td></tr>";
            email += "<tr><th style='text-align: left'>Request Area: </th><td style='text-align: left'>" + request.SupportArea.Text + "</td></tr>";
            email += "<tr><th style='text-align: left'>Request Summary: </th><td style='text-align: left'>" + request.Summary + "</td></tr></table>";
            email += "<br><br>You can view the entire summary at <a href='" + url + "'>" + url + "</a>";
            email += "<br><br>Or view all your pending details in the <a href='http://" + MainFactory.getInstance() + "/Console" + "'>Dashboard</a>";
            email += "</html>";

            return email;
        }

        public static string requestEmailSubmitterBody(RequestView request) {
            string url = MainFactory.getInstance() + "/Console/Dashboard/Request/" + request.ID;

            string email = "<html>";
            email += "We appreciate your business and are currently working on your case for : <b>" + request.Summary + "</b>";
            if (request.SubmittedBy != request.RequestedBy)
                email += "<br>This case was submitted on your behalf by <b>" + request.SubmittedBy.EnglishName + "</b>";
            email += "<br><br>Your case is being worked by a BI Support member.";
            email += "<br><br>Please feel free to check the status anytime by going to the following web site:";
            email += "<br><a href='" + url + "'>" + url + "</a>";
            email += "<br><br>Or view all your pending details in the <a href='http://" + MainFactory.getInstance() + "/Console" + "'>Dashboard</a>";
            email += "</html>";

            return email;
        }

        public static string requestEmailSubmitterStatusChange(RequestView request) {
            string url = MainFactory.getInstance() + "/Console/Dashboard/Request/" + request.ID;

            string email = "<html>";
            email += "This is to notify you of a status change to your request : <b>" + request.Summary + "</b>";
            email += "<br><br>The status of this request has been marked as " + request.Status.Text + ".";
            
            email += "<br><br>You may choose to read the details at :";
            email += "<br><a href='" + url + "'>" + url + "</a>";
            email += "<br><br>Or view all your pending details in the <a href='http://" + MainFactory.getInstance() + "/Console" + "'>Dashboard</a>";
            email += "</html>";

            return email;
        }

        public static string requestEmailPromoteBody(RequestView request) {
            string url = MainFactory.getInstance() + "/Console/Dashboard/Request/" + request.ID;

            string email = "<html>";
            email += "A Request to promote to project has been made for: <b>" + request.Summary + "</b>";
            email += "<br><br>To View the Request: <a href='" + url + "'>" + url + "</a>";
            email += "<br><br>Or view all pending requests in the <a href='http://" + MainFactory.getInstance() + "/Console/Manager" + "'>Manager Dashboard</a>";
            email += "</html>";

            return email;
        }

        public static string elementEmailSupportBody(ElementView element, IUser requestor) {
            string url = "http://" + MainFactory.getInstance() + "/Console/Dashboard/Element/" + element.ID;

            string email = "<html>";
            email += "<h3>A new element has been assigned to you.</h3><br /><table>";
            email += "<tr style='text-align: left'><th>Requested by: </th><td style='text-align: left'>" + requestor.EnglishName + "</td></tr>";
            email += "<tr style='text-align: left'><th>Parent Request Summary: </th><td style='text-align: left'>" + element.Parent.Summary + "</td></tr></table>";
            email += "<br><br>You can view the entire summary at <a href='" + url + "'>" + url + "</a>";
            email += "<br><br>Or view all your pending details in the <a href='http://" + MainFactory.getInstance() + "/Console" + "'>Dashboard</a>";
            email += "</html>";

            return email;
        }
    }
}