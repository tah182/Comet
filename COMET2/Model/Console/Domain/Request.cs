using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using COMET.Server.Domain;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;

namespace COMET.Model.Console.Domain {
    public class Request : ARequest {
        private int? assignedTo;
        private string itFeature, topOffnetAttribute;
        
        /// <summary>
        /// Used for creating new requests
        /// </summary>
        /// <param name="requestedBy"></param>
        /// <param name="submittedBy"></param>
        /// <param name="requestArea"></param>
        /// <param name="requestType"></param>
        /// <param name="requestCategory"></param>
        /// <param name="requestSummary"></param>
        /// <param name="requestDescription"></param>
        /// <param name="valueDriver"></param>
        /// <param name="value"></param>
        /// <param name="valueDescription"></param>
        /// <param name="desiredDueDate"></param>
        public Request(int requestedBy, 
                       int submittedBy, 
                       int requestArea, 
                       int requestType, 
                       int requestCategory,
                       string requestSummary,
                       string requestDescription,
                       int assignedTo,
                       int valueDriver,
                       int value,
                       string valueDescription,
                       DateTime desiredDueDate) : base(requestedBy, 
                                                       submittedBy, 
                                                       requestArea, 
                                                       requestType, 
                                                       requestSummary,  
                                                       requestDescription, 
                                                       valueDriver, 
                                                       value, 
                                                       valueDescription, 
                                                       desiredDueDate) {
                                                           this.CategoryID = requestCategory;
                                                           setStatusID(1);
                                                           setLastUpdatedDate();
                                                           this.SubmittedDate = DateTime.Today;
                                                           this.AssignedTo = assignedTo;
        }

        /// <summary>
        /// Used for creating requests that exist in the data store
        /// </summary>
        /// <param name="request">The Server based object of request</param>
        public Request(REQUEST request) : base(request.REQUEST_ID,
                                               request.REQUEST_STATUS_ID,
                                               request.REQUESTED_BY_ID,
                                               request.SUBMITTED_BY_ID,
                                               request.SUBMITTED_DATE,
                                               request.REQUESTED_DUE_DATE,
                                               request.ESTIMATED_DUE_DATE,
                                               request.MANAGER_QUEUE_DATE,
                                               request.MANAGER_APPROVED_DATE,
                                               request.HOLD_DATE,
                                               request.RESUME_DATE,
                                               request.CLOSED_DATE,
                                               request.LAST_UPDATED_DATE,
                                               request.REQUEST_SUMMARY,
                                               request.REQUEST_DESCRIPTION,
                                               request.REQUEST_TYPE_ID,
                                               request.PROGRAM_ID,
                                               (int)request.SUPPORT_AREA_ID,
                                               (int)request.VALUE_DRIVER_ID,
                                               (decimal)request.VALUE,
                                               request.ESTIMATED_HOURS,
                                               request.ESTIMATED_COST,
                                               request.VALUE_REASON,
                                               request.MANAGER_NOTE){
            setStatusID(request.REQUEST_STATUS_ID);
            this.AssignedTo = request.ASSIGNED_TO_ID;
            this.ProjectID = request.PARENT_PROJECT_ID;
            this.Resolution = request.RESOLUTION;
            this.ITFeature = request.IT_FEATURE;
            this.TopOffnetAttribute = request.TOP_OFFNET_ATTRIBUTE_NUMBER;
        }
        public int? AssignedTo {
            get { return this.assignedTo; }
            private set {
                if (value < 1)
                    throw new ArgumentException("Cannot assign to " + value + ". Not a valid developer.");
                this.assignedTo = value;
            }
        }


        public int? ProjectID {
            get;
            set;
        }

        /// <summary>
        /// Sets the StatusID of the Request.
        /// </summary>
        /// <returns>True if the update was successful. False if there was an issue.</returns>
        public new bool setStatusID( int statusID ) {
            //if ( ( statusID == 1 /*In Queue*/ && this.StatusID != 0)//( this.StatusID == 3 /*Complete*/ || this.StatusID == 6 /*In Production*/) )
                
            //  ) {
            //    throw new ArgumentException( "Status cannot change from " + this.StatusID + " to " + statusID );
            //}
            base.setStatusID(statusID);//setStatusID(statusID);

            switch ( statusID ) {
                case 2: //2 = "On Hold"
                    setHoldDate();
                    break;
                case 3: //3 = "Complete"
                    setClosedDate(false);
                    break;
                case 4: //4 = "Rejected"
                    setClosedDate(false);
                    break;
                case 5: //5 = "Cancelled"
                    setClosedDate(false);
                    break;
            }
            return true;
        }


        public int CategoryID {
            get;
            set;
        }

        public string Resolution {
            get;
            set;
        }

        public string ITFeature {
            get {
                return this.itFeature;
            }
            private set {
                if (value != null) {
                    if ( value.Length > 10 ) 
                    throw new ArgumentException( "IT Feature must be less that 10 characters long" );
                    this.itFeature = value;
                }
            }
        }

        public string TopOffnetAttribute {
            get {
                return this.topOffnetAttribute;
            }
            private set {
                if (value != null) {
                    if (value.Length > 10)
                        throw new ArgumentException("Top Offne Attribute list must be less that 10 characters long");
                    this.topOffnetAttribute = value;
                }
            }
        }

    }
}
