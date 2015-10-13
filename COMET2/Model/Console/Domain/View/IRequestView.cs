using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Domain.View {
    public interface IRequestView {

        int ID { get; }
        
        LookupSorted Status { get; set; }
                
        DateTime OpenDate { get; }
        
        DateTime? ClosedDate { get; }
        
        void setClosed();
        
        DateTime LastUpdated { get; }
        
        void setLastUpdated();
        
        string Summary { get; set; }
        
        bool isNew { get; }
    }
}