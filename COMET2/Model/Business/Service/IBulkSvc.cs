using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COMET.Model.Domain;

namespace COMET.Model.Business.Service {
    public interface IBulkSvc {
        IList<ClliSearchVendor> getBulkByCLLI(string clliString, float distance, int topCount);
    }
}
