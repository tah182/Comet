using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Server.Domain;
using COMET.Model.Business.Factory;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Service to retreieve SDP buildings
    /// </summary>
    public class QuoteSvcImplDB : IQuoteSvc {
        LocationsDataContext dc;

        public QuoteSvcImplDB(LocationsDataContext dc) {
            this.dc = dc;
        }

        public QuoteSvcImplDB() {
            dc = (LocationsDataContext)MainFactory.getDb("Locations", true);
        }

        public void close() {
            try {
                dc.Dispose();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "i-c-01", se.Message + "<br>" + se.StackTrace);
            }
        }

        /// <summary>
        /// Retreives SDP buildings from the database
        /// </summary>
        /// <returns>A list of buildings that are Service Deliverypoints</returns>
        public IList<Quote> getNearQuotes(decimal lat, decimal lng, int range) {
            List<Quote> quoteList = new List<Quote>();
            try {
                quoteList = (from quote in dc.GEO_QUOTE_LKP(lat.ToString(), lng.ToString(), range)
                             select new Quote(
                                 (int)quote.RANK,
                                 quote.Address_ID,
                                 quote.EntityLineItemId,
                                 quote.BandwidthDesc,
                                 quote.ProductDesc,
                                 (decimal)quote.mrcUSD,
                                 quote.Vendor,
                                 quote.Vendor_Type,
                                 (int)quote.PriceTermId,
                                 (decimal)quote.IncrementalMrCost,
                                 quote.IncrementalNrCost,
                                 (bool)quote.IsWin,
                                 (int?)quote.BandwidthEthernet,
                                 (int)quote.BandwidthCapacity,
                                 (DateTime)quote.QuoteCreateDate,
                                 quote.Zside_Premise,
                                 quote.Zside_street,
                                 quote.Zside_City,
                                 quote.Zside_State,
                                 quote.CLLI_CD,
                                 quote.SWC_CLLI,
                                 (decimal)quote.Distance,
                                 new LatLng((decimal)quote.Zside_Latitude, (decimal)quote.Zside_Longitude)
                                 )).ToList();
            } catch (SqlException se) {
                    MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "q-quote-01", se.Message + "<br>" + se.StackTrace);
            }
            return quoteList;
        }
    }
}