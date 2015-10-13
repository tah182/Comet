using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COMET.Model.Domain;

namespace COMET.Model.Business.Service {
    public interface IRegion {
        /// <summary>
        /// Thread interface run
        /// </summary>
        void ThreadRun();

        /// <summary>
        /// Returns key, value pair of Region and data values.
        /// </summary>
        /// <returns></returns>
        IList<KeyValue<string, string>> getData();
    }
}
