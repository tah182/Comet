using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class ApplicationStore<T> {
        public ApplicationStore (DateTime expiration, T data) {
            this.Expiration = expiration;
            this.Data = data;
        }

        public DateTime Expiration {
            get;
            private set;
        }

        public T Data {
            get;
            private set;
        }

        public bool isValid() {
            return Expiration >= DateTime.Now;
        }
    }
}