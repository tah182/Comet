using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using COMET.Model.Console.Domain;
using Moq;

namespace COMET.Tests.Models.Domain.Console {
    [TestClass]
    public class RequestTest {
        private string summary = "test summary";
        private string description = "test description";
        private string valueReason = "test value reason";
        private DateTime testDateToday = DateTime.Today;
        private DateTime testDateTomorrow = DateTime.Today.AddDays( 1 );
        private DateTime testDateYesterday = DateTime.Today.AddDays( -1 );
        private DateTime testDate5Days = DateTime.Today.AddDays( 5 );
        //private Mock<IRequest> ireq = new Mock<IRequest>();

//        [Ignore]
//        public void newRequestTest() {
//            Request request = new Request(123, 456, 890, 321,10, summary, description, 987,0, valueReason, testDate5Days);
//            Assert.AreEqual(123, request.RequestBy);
//            Assert.AreEqual(456, request.SubmittedBy);
//            Assert.AreEqual(890, request.SupportAreaID);
//            Assert.AreEqual(321, request.TypeID);
//            Assert.AreEqual(10, request.CategoryID);
//            Assert.IsTrue(request.RequestSummary.Equals(summary));
//            Assert.IsTrue(request.Description.Equals(description));
//            Assert.AreEqual( 987, request.ValueDriverID);
//            Assert.AreEqual( 0, request.Value);
//            Assert.IsTrue( request.ValueReason.Equals( valueReason ) );
//            Assert.AreEqual(DateTime.Today.AddDays(5), request.RequestedDueDate); //good
//            Assert.AreNotEqual( "", request.RequestSummary.Trim(), "RequestSummary is an empty string." );
//            Assert.AreNotEqual( "", request.Description.Trim(), "Description is an empty string." );
//            Assert.AreNotEqual( null, request.RequestSummary, "Request RequestSummary is null." );
//            Assert.AreNotEqual( null, request.Description, "Request Description is null." );
//        }
//        //public void newRequestTestFail(){
//        //    // I feel like I shoudl be writing something I know will fail?
//        //}

//        [TestMethod]
//        public void requestByTest() {
//            //Looking up MOQ
//        }

//        [TestMethod]
//        public void submittedByTest() {
//            //Test that user is able to change the submitted by employee.
//        }

//        [Ignore]
//        public void assignedToTest() {
//            // Test for valid developer
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//        }
        
//        [TestMethod]
//        [ExpectedException( typeof( ArgumentException ) )]
//        public void invalidSummaryLengthTest() {
//            summary = "This string has 150 characters 1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111";
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            Assert.AreEqual( 100, summary.Length);
//        }

//        [Ignore]
//        //Submit new request test - to make sure everything automated was set
//        public void submitNewRequestTest() {
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            Assert.IsNotNull( request.ID,"requestID is null." );
//            // Assert.AreEqual( null, request.AssignedTo, "AssignedToID is null." ); //Nullable
//            //Assert.AreEqual( null, request.SubmittedBy, "SubmittedByID is null." );
//            //Assert.AreEqual( null, request.RequestBy, "RequestedByID is null." );
//            //Assert.AreEqual( null, request.SupportAreaID, "SupportAreaID is null." );
//            //Assert.AreEqual( null, request.TypeID, "RequestTypeID is null." );
//            //Not testing category here, as category is only for requests, not projects
//            Assert.IsNotNull(request.StatusID, "Requested Status is null." );
//            //Assert.IsTrue( request.StatusID < 10, "StatusID is out of range." );
//            Assert.IsNotNull( request.LastUpdatedDate, "LastUpdatedDate is null." );
//            Assert.IsNotNull( request.SubmittedDate, "SubmittedDate is null." );
//            //Assert.AreEqual( null, request.RequestedDueDate, "RequestedDueDate is null." );
//            //Assert.AreEqual( null, request.RequestSummary, "Request Summary is null." );
//            //Assert.AreEqual( "", request.RequestSummary.Trim(), "RequestSummary is an empty string." );
//            //Assert.AreEqual( null, request.Description, "Request Description is null." );
//            //Assert.AreEqual( "", request.Description.Trim(), "RequestSummary is an empty string." );
            
//        }
//       /*
//        int requestedBy, 
//        int submittedBy, 
//        int requestArea, 
//        int requestType, 
//        string requestSummary,
//        string requestDescription
//        int valueDriver,
//        int value,
//        string valueDescription,
//        DateTime desiredDueDate) 

//        */
//        [Ignore]
//        // Check desired due date before submit date
//        public void requestedDueDateTest() {
//            Request request = new Request( 123, 456, 654, 321,10, summary, description, 987, 0, valueReason, testDateTomorrow );
//            Assert.IsTrue( request.RequestedDueDate >= request.SubmittedDate, "requestedDueDate is before the submitDate" );
//        }

//        [Ignore]
//        [ExpectedException( typeof( ArgumentException) )]
//        //Test inverse of requesdDueDateTEst
//        public void requestedDueDateFAILTest() {
//            Request request = new Request( 123, 456, 654, 321, 10, summary, description, 987, 0, valueReason, testDateYesterday );
//            Assert.IsTrue( request.RequestedDueDate <= request.SubmittedDate, "requestedDueDate is before the submitDate" );
//        }



//        [Ignore]
//        public void requestStatusTest() {
//            //Test for status changes that are invalid
//            // Moving to "Resume" from anything but "hold"
//            // Moving from "In Queue" to anything other than "Assigned" or "Rejected" or "Cancelled"
//            // Moving to "In Queue" from anything else.
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            request.setStatusID(2);
//            Assert.AreEqual( 2, request.StatusID );
////            int currStatusID = request.StatusID;
//        }

//        [Ignore]
//        [ExpectedException( typeof( ArgumentException ) )]
//        public void requestStatusFAILTest() {
//            Request request = new Request( 123, 456, 654, 321, 10, summary, description, 987, 0, valueReason, testDateToday );
//            request.setStatusID( 3 );
//            request.setStatusID( 1 ); //This exception not working
//        }

//        //Test for Resolution on Complete Status



//        [Ignore]
//        public void setCloseDateTest() {
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            request.setClosedDate(false);
//            Assert.AreEqual( DateTime.Today, request.ClosedDate);
//            request.setClosedDate( true );
//            Assert.AreEqual( null, request.ClosedDate );
//        }

//        [Ignore]
//        [ExpectedException (typeof(ArgumentException))]
//        public void setClosedDateTestFAIL(){
//            Request request = new Request( 123, 456, 654, 321, 10, summary, description, 987, 0, valueReason, testDateToday );
//            request.setClosedDate( false );
//            request.setClosedDate( false );
//        }



//        [Ignore]
//        public void setHoldDateTest() {
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            request.setHoldDate();
//            Assert.AreEqual( DateTime.Today, request.HoldDate);
//        }

//        [Ignore]
//        [ExpectedException(typeof(InvalidOperationException))]
//        public void invalidSetResumeDateNoHoldTest() {
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            request.setResumeDate();
//            Assert.AreEqual( DateTime.Today, request.ResumeDate );
//        }

//        [Ignore]
//        public void setResumeDateWithHoldTest() {
//            Request request = new Request( 123, 456, 654, 321, 10, summary, description, 987, 0, valueReason, testDateToday );
//            request.setHoldDate();
//            request.setResumeDate();
//            Assert.AreEqual( DateTime.Today, request.ResumeDate );
//        }


//        [Ignore]
//        public void setLastUpdatedDateTest() {
//            Request request = new Request(123, 456, 654,321,10, summary, description, 987,0, valueReason, testDateToday );
//            request.setLastUpdatedDate();
//            Assert.AreEqual( DateTime.Today, request.LastUpdatedDate);
//        }

//        [Ignore]
//        public void setManagerApprovedDateTest() {
//            Request request = new Request( 123, 456, 654, 321, 10, summary, description, 987, 0, valueReason, testDateToday );
//            request.setManagerApprovedDate();
//            Assert.AreEqual( DateTime.Today, request.ManagerApprovedDate );
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void setManagerApprovedDateFAILTest() {
//            Request request = new Request( 123, 456, 654, 321, 10, summary, description, 987, 0, valueReason, testDateToday );
//            request.setManagerApprovedDate();
//            request.setManagerApprovedDate();
//        }

    }
}
