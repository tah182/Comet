using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

using COMET.Server.Domain;
using COMET.Model.Business.Factory;
using COMET.Controllers;

namespace COMET.Model.Business.Service {
    public class EmailSvc {
        //private static readonly string DEFAULT_CC = "amo_datateam@level3.com";

        /// <summary>
        /// Sends an HTML Email out with any email address
        /// </summary>
        /// <param name="from">The Email address to send from</param>
        /// <param name="to">A string of email addresses delimited by a pipe "|"</param>
        /// <param name="cc">A string of email addresses delimited by a pipe "|"</param>
        /// <param name="subject">The Subject of the Email</param>
        /// <param name="body">The body of the email surrounded by &lt;html&gt; tags</param>
        public static void Email(string from, string to, string cc, string subject, string body) {
            Configuration config = MainFactory.getConfiguration();
            if (Convert.ToBoolean(config.get("EmailActive"))) {
                MailMessage mail = new MailMessage();

                string addresses = to.Length <= 1 ? (string)new Configuration().get("ErrorEmailTo") : to;
                string[] addressTo = addresses.Split('|');
                foreach (string a in addressTo)
                    mail.To.Add(a);

                //addresses = cc.Length <= 1 ? DEFAULT_CC : cc;
                if (cc.Length > 1) {
                    addresses = cc;
                    string[] addressCC = addresses.Split('|');
                    foreach (string a in addressCC)
                        mail.CC.Add(a);
                }

                mail.Subject = subject;
                mail.From = new System.Net.Mail.MailAddress(from);
                mail.Body = body;

                SmtpClient smtp = new SmtpClient("smtp.level3.com");
                mail.IsBodyHtml = true;

                smtp.Send(mail);
            }
        }

        /// <summary>
        /// Sends an HTML Email out using the AMO_DataTeam inbox
        /// </summary>
        /// <param name="to">A string of email addresses delimited by a pipe "|"</param>
        /// <param name="cc">A string of email addresses delimited by a pipe "|"</param>
        /// <param name="subject">The Subject of the Email</param>
        /// <param name="body">The body of the email surrounded by &lt;html&gt; tags</param>
        public static void Email(string to, string cc, string subject, string body) {
            Email("Amo_DataTeam@Level3.com", to, cc, subject, body);
        }

        /// <summary>
        /// Emails the Error to the team
        /// <see cref="COMET.Model.Business.Service.Email"/>
        /// </summary>
        /// <param name="ae">The Application Error that failed to insert into the database</param>
        /// <param name="insertException">The Exception that occurred trying to insert into the table</param>
        /// <param name="mainException">The Exception that occurred during client transaction</param>
        public static void EmailError(ApplicationErrors2 ae, string insertException, string mainException) {
            string user = ae.EmployeeID == 0 || ae.EmployeeID == 999999 ? ContextController.getContextUser().NTLogin : ae.EmployeeID.ToString();

            string msgBody = "<html><body><table style=\"font-family:Arial;font-size:9pt;border-collapse:collapse;border:solid .5pt;border-color:RGB(150,150,150);padding-left:10px;padding-right:10px;\">";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Application: </td><td align=\"right\">" + ae.ApplicationName + "</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Page: </td><td align=\"right\">" + ae.PageName + "</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Step: </td><td align=\"right\">" + ae.StepName + "</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">User: </td><td align=\"right\">" + user + "</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Error Time: </td><td align=\"right\">" + ae.ErrorTime + "</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Code: </td><td align=\"right\">" + ae.ErrorCode + "</td></tr></table>";
            msgBody = msgBody + "<br><br><b>Details: </b><br>" + mainException + "<br /><br /><b>Error relating to DB insert</b> <br />" + insertException + "</body></html>";

            Email((string) new Configuration().get("EmailTo"), "", "Comet Error", msgBody);
        }
    }
}