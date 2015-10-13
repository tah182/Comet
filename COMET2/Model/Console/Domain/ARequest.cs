using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;

namespace COMET.Model.Console.Domain {
    public class ARequest : IRequest {

        private DateTime requestedDate;
        private DateTime? estimatedDueDate;
        private string requestSummary;
        private string description;
        private string valueReason;
        private string managerNote;

        /// <summary>
        /// Creates a new request 
        /// </summary>
        /// <param name="">The employee ID of the employee</param>
        public ARequest( int requestedBy,
                        int submittedBy,
                        int requestArea,
                        int requestType,
                        string requestSummary,
                        string requestDescription,
                        int valueDriver,
                        decimal value,
                        string valueDescription,
                        DateTime desiredDueDate ) {

                            this.RequestBy = requestedBy;
                            this.SubmittedBy = submittedBy;
                            this.SupportAreaID = requestArea;
                            this.TypeID = requestType;
                            this.RequestSummary = requestSummary;
                            this.Description = requestDescription;
                            this.ValueDriverID = valueDriver;
                            this.Value = value;
                            this.ValueReason = valueDescription;
                            this.requestedDate = desiredDueDate;
        }

        /// <summary>
        /// Creates a filled out request from database
        /// </summary>
        public ARequest(int id,
                        int statusID,
                        int requestedBy,
                        int submittedBy,
                        DateTime submittedDate,
                        DateTime requestedDueDate,
                        DateTime? estimatedDueDate,
                        DateTime? managerQueueDate,
                        DateTime? managerApprovedDate,
                        DateTime? holdDate,
                        DateTime? resumeDate,
                        DateTime? closedDate,
                        DateTime lastUpdatedDate,
                        string requestSummary, 
                        string description, 
                        int typeID,
                        int? programID,
                        int supportAreaID,
                        int valueDriverID,
                        decimal value,
                        decimal? estimatedHours,
                        decimal? estimatedCost,
                        string valueReason,
                        string managerNote) :
                            this (requestedBy,
                                  submittedBy,
                                  supportAreaID,
                                  typeID,
                                  requestSummary,
                                  description,
                                  valueDriverID,
                                  value,
                                  valueReason,
                                  requestedDueDate) {
            this.ID = id;
            this.LastUpdatedDate = lastUpdatedDate;
            this.SubmittedDate = submittedDate;
            //percentComplete
            //actualCost
        }

        public int ID {
            get;
            private set;
        }


        public int StatusID {
            get;
            private set;
        }

        //Set the status ID value.
        public bool setStatusID(int statusID) {
            this.StatusID = statusID;
            return true;
        }

        public int RequestBy {
            get;
            private set;
        }
        public int SubmittedBy {
            get;
            private set;
        }

        public DateTime SubmittedDate {
            get;
            protected set;
        }

        public DateTime RequestedDueDate {
            get {
                return this.requestedDate;
            }
            private set {
                if ( DateTime.Compare( value, DateTime.Today ) < 0 ) {
                    throw new ArgumentException( "Cannot set the Requested Due Date before Today." );
                }
                this.requestedDate = value;
            }
        }

        public DateTime? EstimatedDueDate {
            get {
                return this.estimatedDueDate;
            }
            set {
                if ( DateTime.Compare( (DateTime)value, DateTime.Today ) < 0 ) {
                    throw new ArgumentException( "Cannot set the Estimated Due Date before Today." );
                }
                this.estimatedDueDate = value;
            }
        }

        public DateTime? ManagerQueueDate {
            get;
            private set;
        }

        public DateTime? ManagerApprovedDate {
            get;
            private set;
        }

        public void setManagerApprovedDate() {
            if ( this.ManagerApprovedDate != null ) {
                throw new ArgumentException( "Cannot set Manager Approved Date more than once." );
            }
            this.ManagerApprovedDate = DateTime.Today;
        }

        public DateTime? HoldDate {
            get;
            private set;
        }


        public void setHoldDate() {
            if ( this.HoldDate == null ) {
                this.HoldDate = DateTime.Today;
            }
        }

        public DateTime? ResumeDate {
            get;
            private set;
        }

        public void setResumeDate() {
            if ( this.HoldDate != null ) {
                this.ResumeDate = DateTime.Today;
            } else {
                throw new InvalidOperationException( "Cannot set resume date, as no hold date was set." );
            }

        }

        public DateTime? ClosedDate {
            get;
            private set;
        }

        public void setClosedDate(bool reopen) {
            //Called by setStatusID
            if ( reopen ) {
                this.ClosedDate = null;
                this.setStatusID( 2 );//in progress
            } else if ( this.ClosedDate == null ) {
                this.ClosedDate = DateTime.Today;
            } else {
                throw new ArgumentException( "Cannot set closed date again." );
            }
        }

        public DateTime LastUpdatedDate {
            get;
            protected set;
        }

        public void setLastUpdatedDate() {
            this.LastUpdatedDate = DateTime.Today;
        }

        public string RequestSummary {
            get{
                return this.requestSummary;
            }
            private set {
                if ( value.Length > 100 ) {
                    throw new ArgumentException( "Summary must be less that 100 characters long" );
                }
                if(value.Trim().Length < 1 ){
                    throw new ArgumentException("Summary must be provided");
                }
                this.requestSummary = value.Trim();
            }
        }

        public string Description {
            get{
                return this.description;
            }
            private set {
                if ( value.Length > 500 ) {
                    throw new ArgumentException( "Description must be less that 500 characters long" );
                }
                if(value.Trim().Length < 1 ){
                    throw new ArgumentException("Description must be provided");
                }
                this.description = value.Trim();
            }
        }

        public int TypeID {
            get;
            private set; //Do I need to check if they are changing it to what it already is?
        }

        public int ProgramID {
            get;
            private set;
        }

        public int SupportAreaID {
            get;
            private set;
        }

        public int ValueDriverID {
            get;
            private set;
        }

        //Value is required for Project - Any, Tactical - New, Tactical - Enhancement
        public decimal Value {
            get;
            private set; //
        }

        public decimal EstimatedHours {
            get;
            private set;
        }

        public decimal PercentComplete {
            get;
            private set;
        }

        public decimal EstimatedCost {
            get;
            private set; //this is going to be set through configuration file calculations.
        }

        /// <summary>
        /// Actual/Final Cost for the Request
        /// </summary>
        public decimal ActualCost {
            get;
            private set;//caluated by the # hours multipled by the cost variable.
        }

        /// <summary>
        /// Estimated Value to Cost ratio for the Request
        /// </summary>
        //public decimal EstimatedValueOverCost {
        //    get { throw new NotImplementedException(); } // Calculated 
        //}

        /// <summary>
        /// Actual/Final Value to Cost ratio for the Request
        /// </summary>
        //public decimal ActualValueOverCost {
        //    get { throw new NotImplementedException(); } // Calculated 
        //}

        /// <summary>
        /// Reasoning behind the value for the Request.
        /// </summary>
        public string ValueReason {
            get {
                return this.valueReason;
            }
            private set {
                if ( value.Length > 500 ) {
                    throw new ArgumentException( "Value Reason must be less that 500 characters long" );
                }
                if ( value.Trim().Length < 1 ) {
                    throw new ArgumentException( "Value Reason must be provided" );
                }
                this.valueReason = value.Trim();
            }
        }

        /// <summary>
        /// Manager's note for the Request
        /// </summary>
        public string ManagerNote {
            get { return this.managerNote; }
            set {
                if ( value.Length > 200 ) {
                    throw new ArgumentException( "Manager note must be less that 200 characters long" );
                }
                this.managerNote = value.Trim();
            }
 
        }
    }    
}