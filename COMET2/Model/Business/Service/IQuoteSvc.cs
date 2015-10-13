using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;

namespace COMET.Model.Business.Service {
    public interface IQuoteSvc {
        void close();

        IList<Quote> getNearQuotes(decimal lat, decimal lng, int range);
    }
}