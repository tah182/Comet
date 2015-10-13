using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Domain.View {
    public class NewRequestModel {
        //public int SubmittedBy { get; set; }
        //public int RequestBy { get; set; }
        //public IEnumerable<ILookup> SupportAreaID { get; set; }
        //public IEnumerable<ILookup> TypeID { get; set; }
        //public IEnumerable<ILookup> RequestCategory { get; set; }
        //[DataType(DataType.DateTime)]
        //public DateTime RequestedDueDate { get; set; }
        //public string RequestSummary { get; set; }
        //public string RequestDescription { get; set; }
        //public IEnumerable<ILookup> ValueDriverID { get; set; }
        //public int Value { get; set; }
        //public string ValueReason { get; set; }

        public int SubmittedBy { get; set; }
        public int RequestBy { get; set; }
        public int SupportAreaID { get; set; }
        public int TypeID { get; set; }
        public int RequestCategory { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RequestedDueDate { get; set; }
        public string RequestSummary { get; set; }
        public string RequestDescription { get; set; }
        public int ValueDriverID { get; set; }
        public int Value { get; set; }
        public string ValueReason { get; set; }

        override public string ToString() {
            return "Submitted By: " + SubmittedBy + "\n" +
                   "RequestCategory: " + RequestCategory + "\n" +
                   "RequestSummary: " + RequestSummary;
        }
    }
}