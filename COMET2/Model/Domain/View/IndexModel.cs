using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;

namespace COMET.Model.Domain.View {
    /// <summary>
    /// Model to be used for the Index View Page.
    /// </summary>
    public class IndexModel {

        /// <summary>
        /// Constructor creates readonly states list and providers list.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="providers"></param>
        public IndexModel(IList<KeyValue<string, string>> states, IList<KeyValue<string, string>> providers) {
            this.States = states;
            this.Providers = providers;
        }

        /// <summary>
        /// Returns a list of States
        /// </summary>
        public IList<KeyValue<string, string>> States {
            get;
            private set;
        }

        /// <summary>
        /// Returns a list of Providers
        /// </summary>
        public IList<KeyValue<string, string>> Providers {
            get;
            private set;
        }

        public IEnumerable<string> SelectedStates {
            get { return this.States.Select(x => x.Value); }
        }
        public IEnumerable<string> SelectedProviders {
            get { return this.Providers.Select(x => x.Value); }
        }
    }
}