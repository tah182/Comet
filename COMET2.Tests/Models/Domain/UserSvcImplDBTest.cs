using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.Resources;

using Moq;

using COMET.Model.Business.Service;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;
using COMET.Model.Domain;
using System.Web.Routing;

namespace COMET.Tests.Models.Domain {
    [TestClass]
    public class UserSvcImplDBTest {
        private Mock<IUserSvc> svc = new Mock<IUserSvc>();

        [TestMethod]
        public void TestGetByEmpID() {
            UserMgr userSvc = new UserMgr(svc.Object);
            IUser user = userSvc.getUser(198724);

            Assert.AreEqual(user.EnglishName.ToLower(), "takashi tatsumoto");
            Assert.AreEqual(user.ManagerID, 9218);
        }

        [TestInitialize]
        public void Initialize() {
            List<IUser> users = new List<IUser> {
                new User(198724),
                new User(9218)
            };
            users = users.Where(x => x.EmployeeID == 198724).Select(usr => { usr.Name = "tatsumoto, takashi"; return usr; }).ToList();
            users = users.Where(x => x.EmployeeID == 198724).Select(usr => { usr.NTLogin = "takashi.tatsumoto"; return usr; }).ToList();
            users = users.Where(x => x.EmployeeID == 198724).Select(usr => { usr.ManagerID = 9218; return usr; }).ToList();

            //svc.Setup(x => x.getUser(It.IsAny<int>())).Returns((int empID) => { return users.Single(y => y.EmployeeID == empID); });
            //svc.Setup(x => x.getAllUsers()).Returns(users);
            svc.Setup(x => x.getAllUsers()).Returns(users);
        }

        [TestCleanup]
        public void TearDown() {
            HttpContext.Current = null;
        }
    }
}
