using System;
using System.Text.RegularExpressions;

namespace COMET.Model.Domain {
    /// <summary>
    /// A User object used for employees.
    /// </summary>
    public class User : IUser {
        private string name;
        private bool isBiManager;

        public User() {

        }

        /// <summary>
        /// Creates a new User from the employeeID of the Employee.
        /// </summary>
        /// <param name="empID">The employee ID of the user.</param>
        public User (int empID){
            this.EmployeeID = empID;
        }

        public User (int employeeID,
                    string name,
                    string displayName,
                    string ntLogin,
                    string emailAddress,
                    string title,
                    int? managerEmployeeID,
                    string department,
                    string officePhone,
                    string mobilePhone,
                    DateTime hireDate,
                    DateTime? termDate,
                    OfficeBuilding office,
                    int authLevel,
                    bool acknowledgedUpdateNote,
                    bool isBiManager) {
            this.EmployeeID = employeeID;
            this.Name = displayName == null ? name : displayName;
            this.NTLogin = ntLogin;
            this.EmailAddress = emailAddress;
            this.Title = title;
            this.ManagerID = managerEmployeeID;
            this.DeptName = department;
            this.OfficePhone = officePhone;
            this.MobilePhone = mobilePhone;
            this.HireDate = hireDate;
            this.TermDate = termDate;
            this.Building = office;
            setUserType(authLevel);
            this.AcknowledgedUpdateNotes = acknowledgedUpdateNote;
            this.isBiManager = isBiManager;
        }

        /// <summary>
        /// Accessor only for the userType
        /// </summary>
        public UserType UserType {
            get;
            set;
        }

        /// <summary>
        /// Sets the Enum UserType based on the integer level passed in
        /// </summary>
        /// <param name="level">The DB integer identification of the User Level</param>
        public void setUserType(int level) {
            this.UserType = UserType.USER;
            foreach (UserType u in UserType.Values)
                if (u.authLevel == level)
                    this.UserType = u;
        }

        /// <summary>
        /// The employeeID of the user for logging
        /// </summary>
        public int EmployeeID {
            get;
            set;
        }

        /// <summary>
        /// The name of the user in "Last, First" format.
        /// </summary>
        public string Name {
            get { return this.name; }
            set {
                string regex = "(\\(.*\\))";
                this.name = Regex.Replace(value, regex, "").Trim();
            }
        }

        /// <summary>
        /// Returns the "First Last" name format.
        /// </summary>
        /// <remarks>Uses</remarks>
        public string EnglishName {
            get {
                return this.FirstName + " " + this.LastName;
            }
        }

        /// <summary>
        /// Retrieves just the last name from the Name property
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when Name has not been set.</exception>
        public string LastName {
            get {
                if (!isValid())
                    throw new InvalidOperationException("User is not valid.");
                return this.Name.Split(new string[] { ", " }, StringSplitOptions.None)[0];
            }
        }

        /// <summary>
        /// Retrieves just the first name from the Name property
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the Name has not been set.</exception>"
        public string FirstName {
            get {
                if (!isValid())
                    throw new InvalidOperationException("User is not valid.");
                return Name.Split(new string[] { ", " }, StringSplitOptions.None)[1];
            }
        }

        /// <summary>
        /// The NT Login of the user
        /// </summary>
        public string NTLogin { get; set; }

        /// <summary>
        /// The Job Title of the User
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Department name of the user
        /// </summary>
        public string DeptName { get; set; }

        public string OfficePhone { get; set; }

        public string MobilePhone { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TermDate { get; set; }

        /// <summary>
        /// The User's Manager's Employee ID
        /// </summary>
        public int? ManagerID { get; set; }

        /// <summary>
        /// The <seealso cref="COMET2.Models.Domain.OfficeBuilding.CS">OfficeBuilding</seealso> of the employee
        /// </summary>
        public OfficeBuilding Building { get; set; }
        
        /// <summary>
        /// The Email Address of the User
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Validates that the employee has all the required information to be a qualified User in the system.
        /// </summary>
        /// <returns>True if the Name, NTLogin, and EmpID Fields have data.</returns>
        public bool isValid() {
            return this.Name != null && this.NTLogin != null & this.EmployeeID != 0;
        }

        /// <summary>
        /// The flag if the user has acknowledged the latest version notes.
        /// </summary>
        public bool AcknowledgedUpdateNotes { get; set; }

        public void setAcknowledged() {
            this.AcknowledgedUpdateNotes = true;
        }

        /// <summary>
        /// Informs if the user can impersonate another user.
        /// </summary>
        /// <returns>True if the member is labeled as an admin in the COMET.Application_Roles table</returns>
        public bool canImpersonate() {
            return this.UserType == UserType.ADMIN || this.UserType == UserType.ADMIN_MANAGER;
        }

        public bool isBIManager() {
            return this.isBiManager;
        }

        /// <summary>
        /// Compares this user with another user. Compares employeeID
        /// </summary>
        /// <param name="user">The employee to compare against</param>
        /// <returns>True if the two employee IDs equal.</returns>
        public bool Equals(IUser user) {
            if (user == null)
                return false;

            return this.EmployeeID == user.EmployeeID;
        }

        public override string ToString() {
            return "Employee ID: " + EmployeeID + "\n" +
                "English Name: " + EnglishName + "\n" +
                "NTLogin: " + NTLogin + "\n" +
                "Name: " + Name;
        }
    }
}