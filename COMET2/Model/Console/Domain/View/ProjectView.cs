using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Console.Domain;
using COMET.Server.Domain;
using COMET.Model.Domain;

namespace COMET.Model.Console.Domain.View {
    public class ProjectView : AProjectView {
        public ProjectView() {
            
        }

        public ProjectView(RequestView request) {
            this.RequestedBy = request.RequestedBy;
            this.SubmittedBy = request.SubmittedBy;
            this.AssignedTo = request.AssignedTo;
            this.CType = request.CType;
            this.SupportArea = request.SupportArea;
            this.Program = request.Program;
            this.ValueDriver = request.ValueDriver;
            this.RequestedDueDate = request.RequestedDueDate;
            this.EstimatedDueDate = request.EstimatedDueDate;
            this.ResumeDate = request.ResumeDate;
            this.ManagerQueueDate = request.ManagerQueueDate;
            this.ManagerApprovedDate = request.ManagerApprovedDate;
            this.EstimatedHours = request.EstimatedHours;
            this.Value = request.Value;
            this.EstimatedCost = request.EstimatedCost;
            this.ActualCost = request.ActualCost;
            this.Description = request.Description;
            this.ValueReason = request.ValueReason;
            this.ManagerNote = request.ManagerNote;
            this.Summary = request.Summary;
            this.RequestList = new List<RequestView>();
            this.RequestList.Add(request);
            this.setLastUpdated();
        }

        public ProjectView(PROJECT project,
                            User submittedBy,
                            User requestedBy,
                            IList<RequestView> requestList,
                            SupportArea supportArea,
                            Program program,
                            ValueDriver valueDriver,
                            LookupSorted projectStatus,
                            LookupActive projectType,
                            decimal? internalHoursMultiplier,
                            decimal? externalHoursMultiplier,
                            User assignedTo) 
            : base (project,
                    submittedBy,
                    requestedBy, 
                    supportArea,
                    program,
                    valueDriver,
                    projectStatus,
                    projectType,
                    internalHoursMultiplier,
                    externalHoursMultiplier,
                    assignedTo) {

            this.StartDate = project.START_DATE;
            this.RequestList = requestList == null ? new List<RequestView>() : requestList;
        }

        public DateTime StartDate {
            get;
            set;
        }

        public decimal PercentComplete {
            get {
                if (this.RequestList == null)
                    return 0;
                decimal totalNeeded = this.RequestList.Sum(x => x.PercentComplete) == 0 ? 0 : this.RequestList.Sum(x => x.PercentComplete == 0 ? 0 : x.Hours / x.PercentComplete);
                return totalNeeded == 0 ? 0 : this.RequestList.Sum(x => x.Hours) / totalNeeded;
            }
        }

        public IList<RequestView> RequestList {
            get;
            set;
        }

        public decimal Hours {
            get {
                if (this.RequestList == null) 
                    return 0;

                return this.RequestList.Sum(x => x.Hours);
            }
        }

        public bool Equals(ProjectView pv) {
            if ((object)pv == null)
                return false;

            return this.ID == pv.ID;
        }

    }
}