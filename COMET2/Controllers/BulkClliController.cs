using ClosedXML.Excel;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Domain;
using COMET.Model.Business.Service;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;

namespace COMET.Controllers.Console {
    public class BulkClliController : Controller {
        private ILogSvc logger = MainFactory.getLogSvc();

        // GET: BulkClli
        public ActionResult Index() {
            ViewBag.Message = "Bulk CLLI Lookup";
            logger.logAction(ViewBag.Message);

            return View("../Main/BulkClli");
        }

        [HttpPost]
        public ActionResult ExportBulkClli(string clliString, float distance, int topCount) {
            logger.logAction("Bulk CLLI to Excel");
            BulkMgr bulkMgr = new BulkMgr(MainFactory.getBulkSvc());

            DataTable dt = new DataTable();
            dt.Columns.Add("Entered Clli", typeof(string));
            dt.Columns.Add("Entered Street", typeof(string));
            dt.Columns.Add("Entered City", typeof(string));
            dt.Columns.Add("Entered State", typeof(string));
            dt.Columns.Add("Entered Latitude", typeof(decimal));
            dt.Columns.Add("Entered Longitude", typeof(decimal));
            dt.Columns.Add("LATA", typeof(decimal));
            dt.Columns.Add("Vendor", typeof(string));
            dt.Columns.Add("Vendor Type", typeof(string));
            dt.Columns.Add("Vendor Clli", typeof(string));
            dt.Columns.Add("Vendor Street", typeof(string));
            dt.Columns.Add("Vendor City", typeof(string));
            dt.Columns.Add("Vendor State", typeof(string));
            dt.Columns.Add("Vendor Latitude", typeof(decimal));
            dt.Columns.Add("Vendor Longitude", typeof(decimal));
            dt.Columns.Add("Distance (mi)", typeof(decimal));

            IList<ClliSearchVendor> clliList = bulkMgr.getBulkByCLLI(clliString, distance, topCount);

            foreach (var clli in clliList)
                dt.Rows.Add(clli.EnteredClli,
                            clli.EnteredStreet,
                            clli.EnteredCity,
                            clli.EnteredState,
                            clli.EnteredLat,
                            clli.EnteredLng,
                            clli.LATA,
                            clli.Vendor,
                            clli.VendorType,
                            clli.CLLI_Code,
                            clli.Street,
                            clli.City,
                            clli.State,
                            clli.Lat,
                            clli.Lng,
                            clli.Distance_miles);
            

            Response.ClearContent();
            Response.Buffer = true;
            DateTime currentDate = DateTime.Now;
            string fileName = "BulkClli-Request_" +
                    currentDate.Year + "-" +
                    currentDate.Month + "-" +
                    currentDate.Day + "_" +
                    currentDate.Hour +
                    currentDate.Minute +
                    currentDate.Second + ".xlsx";

            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet(dt, "Data");

            int contents;
            using (MemoryStream ms = new MemoryStream()) {
                workbook.SaveAs(ms);
                ms.Position = 0;
                ms.CopyTo(Response.OutputStream);
                contents = (int)ms.Length;
            }

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Length", contents.ToString());
            Response.AddHeader("content-disposition", "attachment;filename=\"" + fileName + "\"");
            Response.Flush();
            Response.End();

            return View();
        }

        [HttpPost]
        public ContentResult BulkClliJson(string clliString, float distance, int topCount) {
            BulkMgr bulkMgr = new BulkMgr(MainFactory.getBulkSvc());
            logger.logAction("Bulk CLLI to JSON");

            IList<ClliSearchVendor> clliList = bulkMgr.getBulkByCLLI(clliString, distance, topCount);
            return Jsonify<IList<ClliSearchVendor>>.Serialize(clliList);
        }
    }
}