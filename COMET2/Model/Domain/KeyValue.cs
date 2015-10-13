using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class KeyValue<T, V> {
        public T Key {
            get;
            set;
        }

        public V Value {
            get;
            set;
        }
    }
}