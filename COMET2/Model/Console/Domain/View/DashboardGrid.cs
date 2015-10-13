using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Domain.View {
    public class DashboardGrid {
        public DashboardGrid (List<User> requestorList,
                              List<User> assignedList,
                              List<LookupActive> categoryList,
                              List<LookupActive> typeList,
                              List<SupportArea> areaList,
                              string summary,
                              string submittedRange,
                              string dueDateRange,
                              List<LookupSorted> statusList,
                              string closedRange,
                              IList<RequestView> data) {
            this.RequestorList = requestorList;
            this.AssignedList = assignedList;
            this.CategoryList = categoryList;
            this.TypeList = typeList;
            this.AreaList = areaList;
            this.EnteredSummary = summary;
            this.SubmittedRange = submittedRange;
            this.DueDateRange = dueDateRange;
            this.StatusList = statusList;
            this.ClosedRange = closedRange;
            this.Data = data;
        }

        public int? SelectedId {
            get { return this.Data.Count() == 1 ? this.Data.Select(x => x.ID).FirstOrDefault() : (int?)null; }
        }

        public List<User> RequestorList { get; private set; }
        
        public int[] SelectedRequestors {
            get { return this.Data.Select(x => x.RequestedBy).Select(y => y.EmployeeID).Distinct().ToArray(); }
        }
                
        public List<User> AssignedList { get; private set; }

        public int[] SelectedAssignee {
            get { return this.Data.Select(x => x.AssignedTo).Select(y => y.EmployeeID).Distinct().ToArray(); }
        }

        public List<LookupActive> CategoryList { get; private set; }

        public int[] SelectedCategory {
            get { return this.Data.Select(x => x.RequestCategory).Select(y => y.ID).Distinct().ToArray(); }
        }

        public List<LookupActive> TypeList { get; private set; }

        public int[] SelectedType {
            get { return this.Data.Select(x => x.CType).Select(y => y.ID).Distinct().ToArray(); }
        }

        public List<SupportArea> AreaList { get; private set; }

        public int[] SelectedArea {
            get { return this.Data.Select(x => x.SupportArea).Select(y => y.ID).Distinct().ToArray(); }
        }

        public string EnteredSummary {
            get;
            private set;
        }

        public string SubmittedRange {
            get;
            private set;
        }

        public string DueDateRange {
            get;
            private set;
        }

        public List<LookupSorted> StatusList { get; private set; }

        public int[] SelectedStatus {
            get { return this.Data.Select(x => x.Status).Select(y => y.ID).Distinct().ToArray(); }
        }

        public string ClosedRange {
            get;
            private set;
        }

        public IList<RequestView> Data { get; set; }
    }
}