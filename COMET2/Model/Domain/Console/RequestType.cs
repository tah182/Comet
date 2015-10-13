using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.Console {
    public class RequestType : ALookupActive {
        public RequestType(string text)
            : base(text) {
        }

        public RequestType(int id, string text, bool active) : base(id, text, active) {
        }
    }
}