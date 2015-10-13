using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMET.Model.Console.Domain {
    public interface ILookup {
        int ID { get; }

        string Text { get; }
    }
}
