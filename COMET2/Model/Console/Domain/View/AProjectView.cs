using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using COMET.Model.Console.Domain;
using COMET.Server.Domain;
using COMET.Model.Domain;

namespace COMET.Model.Console.Domain.View {
    public class AProjectView : ARequestView {
        private DateTime? estimatedDueDate, holdDate, resumeDate;

        public AProjectView() {
            
        }

        public AProjectView (PROJECT project,
                            User submittedBy,
                            User requestedBy,
                            SupportArea supportArea,
                            Program program,
                            ValueDriver valueDriver,
                            LookupSorted projectStatus,
                            LookupActive projectType, 
                            decimal? internalHoursMultiplier,
                            decimal? externalHoursMultiplier,
                            User assignedTo) 
            : base(project.PROJECT_ID, 
                    projectStatus,
                    project.SUBMITTED_DATE, 
                    project.CLOSED_DATE, 
                    project.LAST_UPDATED_DATE, 
                    project.PROJECT_SUMMARY){

            createDetails(requestedBy,
                            submittedBy,
                            projectType,
                            supportArea,
                            valueDriver,
                            project.REQUESTED_DUE_DATE,
                            project.ESTIMATED_DUE_DATE,
                            project.HOLD_DATE,
                            project.RESUME_DATE,
                            project.MANAGER_QUEUE_DATE,
                            project.MANAGER_APPROVED_DATE,
                            project.ESTIMATED_HOURS,
                            (decimal)project.VALUE,
                            project.ESTIMATED_COST,
                            project.ACTUAL_COST,
                            project.PROJECT_DESCRIPTION,
                            project.VALUE_REASON,
                            project.MANAGER_NOTE,
                            program,
                            internalHoursMultiplier,
                            externalHoursMultiplier,
                            assignedTo);
        }

        public AProjectView(REQUEST request,
                            User submittedBy,
                            User requestedBy,
                            SupportArea supportArea,
                            Program program,
                            ValueDriver valueDriver,
                            LookupSorted requestStatus,
                            LookupActive requestType,
                            decimal? internalHoursMultipleir,
                            decimal? externalHoursMultiplier,
                            User assignedTo)
            : base(request.REQUEST_ID, 
                    requestStatus, 
                    request.SUBMITTED_DATE, 
                    request.CLOSED_DATE, 
                    request.LAST_UPDATED_DATE, 
                    request.REQUEST_SUMMARY) {

            createDetails (requestedBy,
                            submittedBy,
                            requestType,
                            supportArea,
                            valueDriver,
                            request.REQUESTED_DUE_DATE,
                            request.ESTIMATED_DUE_DATE,
                            request.HOLD_DATE,
                            request.RESUME_DATE,
                            request.MANAGER_QUEUE_DATE,
                            request.MANAGER_APPROVED_DATE,
                            request.ESTIMATED_HOURS,
                            (decimal)request.VALUE,
                            request.ESTIMATED_COST,
                            request.ACTUAL_COST,
                            request.REQUEST_DESCRIPTION,
                            request.VALUE_REASON,
                            request.MANAGER_NOTE,
                            program, 
                            internalHoursMultipleir,
                            externalHoursMultiplier,
                            assignedTo);
        }

        private void createDetails(User requestedBy,
                                     User submittedBy,
                                     LookupActive type,
                                     SupportArea supportArea,
                                     ValueDriver valueDriver,
                                     DateTime requestedDueDate,
                                     DateTime? estimatedDueDate,
                                     DateTime? holdDate,
                                     DateTime? resumeDate,
                                     DateTime? managerQueueDate,
                                     DateTime? managerApproveDate,
                                     decimal? estimatedHours,
                                     decimal value,
                                     decimal? estimatedCost,
                                     decimal? actualCost,
                                     string description,
                                     string valueReason,
                                     string managerNote,
                                     Program program,
                                     decimal? internalHoursMultiplier,
                                     decimal? externalHoursMultiplier,
                                     User assignedTo) {
            this.RequestedBy = requestedBy;
            this.SubmittedBy = submittedBy;
            this.AssignedTo = assignedTo;
            this.CType = type;
            this.SupportArea = supportArea;
            this.Program = program;
            this.ValueDriver = valueDriver;
            this.RequestedDueDate = requestedDueDate;
            this.EstimatedDueDate = estimatedDueDate;
            this.holdDate = holdDate;
            this.resumeDate = resumeDate;
            this.ManagerQueueDate = managerQueueDate;
            this.ManagerApprovedDate = managerApproveDate;
            this.EstimatedHours = estimatedHours;
            this.Value = value;
            this.EstimatedCost = estimatedCost;
            this.ActualCost = actualCost;
            this.Description = description;
            this.ValueReason = valueReason;
            this.ManagerNote = managerNote;
            this.InternalHoursMultiplier = internalHoursMultiplier;
            this.ExternalHoursMultiplier = externalHoursMultiplier;
        }

        public User AssignedTo {
            get;
            set;
        }

        public User SubmittedBy {
            get;
            set;
        }
        
        public User RequestedBy {
            get;
            set;
        }

        public LookupActive CType {
            get;
            set;
        }

        public SupportArea SupportArea {
            get;
            set;
        }

        public Program Program {
            get;
            set;
        }

        public ValueDriver ValueDriver {
            get;
            set;
        }
        
        public DateTime? RequestedDueDate {
            get;
            set;
        }

        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString= "{0: MM/dd/yyyy}")]
        public DateTime? EstimatedDueDate {
            get { return this.estimatedDueDate; }
            set {
                //if (this.isNew && value < DateTime.Today)
                //    throw new ArgumentOutOfRangeException("Cannot set a Estimated Due Date before today.");
                this.estimatedDueDate = value;
            }
        }
        
        public DateTime? ManagerQueueDate {
            get;
            set;
        }

        public DateTime? ManagerApprovedDate {
            get;
            set;
        }

        public DateTime? HoldDate {
            get { return this.holdDate; }
            set { this.holdDate = value; }
        }

        public void setHoldDate() {
            if (this.holdDate == null)
                this.holdDate = DateTime.Today;

            this.resumeDate = null;
        }

        public DateTime? ResumeDate {
            get { return this.resumeDate; }
            set { this.resumeDate = value; }
        }

        public void setResumeDate() {
            if (this.holdDate != null)
                this.resumeDate = DateTime.Today; 
        }

        public decimal? EstimatedHours {
            get;
            set;
        }

        public decimal Value {
            get;
            set;
        }

        public decimal? EstimatedCost {
            get;
            set;
        }

        public decimal? ActualCost {
            get;
            set;
        }

        public decimal EstimatedValueToCost {
            get {
                if (this.InternalHoursMultiplier == null || this.EstimatedCost == null)
                    return 0;
                return this.EstimatedCost == 0 ? 0 : (decimal)(this.Value * this.ExternalHoursMultiplier / this.EstimatedCost);
            }
        }

        public decimal ActualValueToCost {
            get {
                if (this.ExternalHoursMultiplier == null || this.ActualCost == null)
                    return 0;
                return this.ActualCost == 0 ? 0 : (decimal)(this.Value * this.ExternalHoursMultiplier / this.ActualCost);
            }
        }

        public string ValueReason {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public string ManagerNote {
            get;
            set;
        }

        public decimal? InternalHoursMultiplier {
            get;
            set;
        }

        public decimal? ExternalHoursMultiplier {
            get;
            set;
        }
    }
}