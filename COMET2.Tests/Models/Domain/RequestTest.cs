using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using COMET.Model.Domain.Console;
using Moq;

namespace COMET.Tests.Models.Domain.Console {
    [TestClass]
    public class RequestTest {
        private string summary = "test summary";
        private string description = "test description";
        private string valueReason = "test value reason";
        private DateTime testDateToday = DateTime.Today;
        private DateTime testDateTomorrow = DateTime.Today.AddDays( 1 );
        //private Mock<IRequest> ireq = new Mock<IRequest>();
            
        [TestMethod]
        public void newRequestTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday);
            Assert.AreEqual(request.RequestBy, 123);
            Assert.AreEqual(request.TypeID, 456);
            Assert.IsTrue(request.RequestSummary.Equals(summary));
            Assert.IsTrue(request.Description.Equals(description));
            Assert.IsTrue(request.ValueReason.Equals(valueReason));
            Assert.AreEqual(request.RequestedDueDate, DateTime.Today.AddDays(5)); //good
            Assert.AreEqual(request.SupportAreaID, 890);
            Assert.AreEqual(request.ValueDriverID, 789);
            
        }
        public void newRequestTestFail(){
            // I feel like I shoudl be writing something I know will fail?
        }

        [TestMethod]
        public void requestByTest() {
            //Looking up MOQ
        }

        [TestMethod]
        public void submittedByTest() {
            //Looking up MOQ
        }

        [TestMethod]
        public void assignedToTest() {
            //Looking up MOQ
        }
        
        [TestMethod]
        public void invalidSummaryLengthTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            summary = "This string has 150 characters 1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111";
            request.RequestSummary = summary;
            Assert.AreEqual( 100, summary.Length);
        }
        
        [TestMethod]
        //Submit new request test - to make sure everything automated was set
        public void submitNewRequestTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            Assert.AreEqual( null, request.RequestID,"requestID is null." );
            // Assert.AreEqual( null, request.AssignedTo, "AssignedToID is null." ); //Nullable
            Assert.AreEqual( null, request.SubmittedBy, "SubmittedByID is null." );
            Assert.AreEqual( null, request.RequestBy, "RequestedByID is null." );
            Assert.AreEqual( null, request.SupportAreaID, "SupportAreaID is null." );
            Assert.AreEqual( null, request.TypeID, "RequestTypeID is null." );
            //Not testing category here, as category is only for requests, not projects
            Assert.AreEqual( null, request.StatusID, "RequestedStatus is null." );
            Assert.IsTrue( request.StatusID > 10, "StatusID is out of rante." );
            Assert.AreEqual( null, request.LastUpdatedDate, "LastUpdatedDate is null." );
            Assert.AreEqual( null, request.SubmittedDate, "SubmittedDate is null." );
            Assert.AreEqual( null, request.RequestedDueDate, "RequestedDueDate is null." );
            Assert.AreEqual( null, request.RequestSummary, "Request Summary is null." );
            Assert.AreEqual( "", request.RequestSummary.Trim(), "RequestSummary is an empty string." );
            Assert.AreEqual( null, request.Description, "Request Description is null." );
            Assert.AreEqual( "", request.Description.Trim(), "RequestSummary is an empty string." );
            
        }
       /*
        int requestedBy, 
        int submittedBy, 
        int requestArea, 
        int requestType, 
        string requestSummary,
        string requestDescription
        int valueDriver,
        int value,
        string valueDescription,
        DateTime desiredDueDate) 

        */
        [TestMethod]
        // Check desired due date before submit date
        public void requestedDueDateTest() {
            Request request = new Request( 123, 456, 654, 321, summary, description, 987, 0, valueReason, testDateToday );
            request.RequestedDueDate = testDateTomorrow;
            Assert.IsTrue( request.RequestedDueDate >= request.SubmittedDate.AddDays( 5 ), "requestedDueDate is not 5 days after submitDate" );
        }


        [TestMethod]
        public void requestStatusTest() {
            //Test for status changes that are invalid
            // Moving to "Resume" from anything but "hold"
            // Moving from "In Queue" to anything other than "Assigned" or "Rejected" or "Cancelled"
            // Moving from 
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            request.setStatusID(2);
            Assert.AreEqual( 2, request.StatusID );
            int currStatusID = request.StatusID;
            //Assert.AreEqual()

            
        }

        [TestMethod]
        public void assignedToTest() {
            // Test for valid developer
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
        }

        [TestMethod]
        public void submittedByTest() {
            //Test that user is able to change the submitted by employee.
        }

        [TestMethod]
        public void setCloseDateTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            request.setClosedDate();
            Assert.AreEqual( DateTime.Today, request.ClosedDate);
        }
        
        [TestMethod]
        public void setHoldDateTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            request.setHoldDate();
            Assert.AreEqual( DateTime.Today, request.HoldDate);
        }

        [TestMethod]
        public void setResumeDateTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            request.setResumeDate();
            Assert.AreEqual( DateTime.Today, request.ResumeDate );
        }

        [TestMethod]
        public void setLastUpdatedDateTest() {
            Request request = new Request(123, 456, 654,321, summary, description, 987,0, valueReason, testDateToday );
            request.setLastUpdatedDate();
            Assert.AreEqual( DateTime.Today, request.LastUpdatedDate);
        }
        

    }
}
