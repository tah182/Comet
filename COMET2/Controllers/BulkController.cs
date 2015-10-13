using System;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web.SessionState;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

using COMET.Model.Domain;
using COMET.Model.Business.Service;
using COMET.Model.Business.Factory;

namespace COMET.Controllers {
    public class BulkController : Controller {
        double bulkProgress = 0;

        // GET: Bulk
        public ActionResult Index() {
            ILogSvc logger = MainFactory.getLogSvc();
            ViewBag.Message = "Bulk Address Lookup";

            logger.logAction(ViewBag.Message);

            return View("../Main/Bulk");
        }

        public ActionResult BulkAddressTemplate() {
            return File("~/App_data/BulkAddressTemplate.xlsx", "application/vnd.ms-excel");
        }

        [HttpPost]
        public ActionResult ExcelToGrid() {
            for (int i = 0; i < Request.Files.Count; i++)
                try {
                    HttpPostedFileBase fileUploadExcel = Request.Files[i];
                    if (fileUploadExcel.ContentLength > 0) {

                        string fileExt = Path.GetExtension(fileUploadExcel.FileName);
                        if (fileExt.ToLower().Equals(".xls"))
                            return PartialView("~/Views/Partial/_BulkGrid.ascx", new ExcelData(ErrorType.FileExtension));

                        ExcelData excelData = new ExcelData();
                        try {
                            excelData.dataTable.Columns["reference_id"].Unique = true;
                            excelData.dataTable.Columns["reference_id"].DataType = typeof (int);
                            excelData.setDataTable(fileUploadExcel.InputStream);
                        } catch (AmbiguousMatchException e) {
                            System.Diagnostics.Debug.WriteLine(e.ToString());
                            return PartialView("~/Views/Partial/_BulkGrid.ascx", new ExcelData(ErrorType.ReferenceIdDup));
                        }
                        excelData.dataTable.Columns.Add("GoogleLat", typeof(decimal));
                        excelData.dataTable.Columns.Add("GoogleLng", typeof(decimal));
                        string mapsAPIUrl = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false";

                        int processed = 0;
                        foreach (DataRow row in excelData.dataTable.Rows) {
                            bulkProgress = processed * 1.0 / excelData.dataTable.Rows.Count;
                            string address = "";
                            try {
                                address = row["address"].ToString() + ", " + row["city"].ToString() + " " + row["state_code"].ToString();
                            } catch (ArgumentException ae) {
                                System.Diagnostics.Debug.WriteLine(ae.ToString());
                                return PartialView("~/Views/Partial/_BulkGrid.ascx", new ExcelData(ErrorType.ExcelFormat));
                            }

                            var requestUri = string.Format(mapsAPIUrl, Uri.EscapeDataString(address));

                            try {
                                var request = WebRequest.Create(requestUri);
                                var response = request.GetResponse();
                                var xdoc = XDocument.Load(response.GetResponseStream());

                                int counter = 0;
                                while (xdoc.Element("GeocodeResponse").Element("status").Value.Equals("OVER_QUERY_LIMIT")) {
                                    if (counter > 3)
                                        return PartialView("~/Views/Partial/_BulkGrid.ascx", new ExcelData(ErrorType.GoogleAPI));
                                    System.Threading.Thread.Sleep(2000);
                                    request = WebRequest.Create(requestUri);
                                    response = request.GetResponse();
                                    xdoc = XDocument.Load(response.GetResponseStream());
                                    counter++;
                                }
                                var result = xdoc.Element("GeocodeResponse").Element("result");

                                // ZipCode
                                string postalCode = "";
                                foreach (XElement xElement in xdoc.Element("GeocodeResponse").Descendants("address_component")) {
                                    if (xElement.Element("type") == null)
                                        break;
                                    else if (xElement.Element("type").Value.ToLower().Equals("postal_code"))
                                        postalCode = xElement.Element("long_name").Value;
                                }

                                var locationElement = result.Element("geometry").Element("location");
                                var lat = locationElement.Element("lat").Value;
                                var lng = locationElement.Element("lng").Value;

                                row["GoogleLat"] = lat;
                                row["GoogleLng"] = lng;
                                row["postal_code"] = postalCode;
                                processed++;
                            } catch (NullReferenceException nre) {
                                row["postal_code"] = "UNKNOWN";
                                row["GoogleLat"] = 0;
                                row["GoogleLng"] = 0;
                                System.Diagnostics.Debug.WriteLine(nre.ToString());
                            }
                        }

                        Session["BulkClliDS"] = excelData.dataTable;
                        var subTable = excelData.dataTable.AsEnumerable().Take(200);
                        excelData.pages = (int)Math.Ceiling(excelData.dataTable.Rows.Count / 200.0);
                        excelData.dataTable = subTable.CopyToDataTable<DataRow>();
                        return PartialView("~/Views/Partial/_BulkClliGrid.ascx", excelData);
                    }
                } catch (Exception e) {
                    ILogSvc logger = MainFactory.getLogSvc();
                    logger.logError("Bulk", MainFactory.getCurrentMethod(), "b-01", "Error in Bulk " + e.Message + " \n " + e.StackTrace);
                    return Redirect("~/Partial/PartialError");
                }
            return PartialView("~/Views/Partial/_BulkGrid.ascx", new ExcelData(ErrorType.FileExtension));
        }
    }
}