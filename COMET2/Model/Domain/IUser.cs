using System;

namespace COMET.Model.Domain {
    /// <summary>
    /// Interface of User Classes
    /// </summary>
    public interface IUser {
        /// <summary>
        /// The Authorization level of the user
        /// </summary>
        UserType UserType { get; }

        /// <summary>
        /// Sets the Enum UserType based on the integer level passed in
        /// </summary>
        /// <param name="level">The DB integer identification of the User Level</param>
        void setUserType(int level);

        /// <summary>
        /// The Last, First name format of the user
        /// </summary>
        string Name         { get; set; }

        /// <summary>
        /// The First Last Name format of the user
        /// </summary>
        string EnglishName  { get; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        string FirstName    { get; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        string LastName     { get; }

        /// <summary>
        /// The NT Login for the user
        /// </summary>
        string NTLogin      { get; set; }

        /// <summary>
        /// The user's job title
        /// </summary>
        string Title        { get; set; }

        /// <summary>
        /// The department that the user belongs to
        /// </summary>
        string DeptName     { get; set; }

        /// <summary>
        /// The User's Manager's Employee ID
        /// </summary>
        int? ManagerID       { get; set; }

        /// <summary>
        /// The building location that the user's office is located
        /// </summary>
        OfficeBuilding Building  { get; set; }

        /// <summary>
        /// The employee ID of the user.
        /// </summary>
        int EmployeeID      { get; }

        /// <summary>
        /// The flag if the user has acknowledged the latest version notes.
        /// </summary>
        bool AcknowledgedUpdateNotes { get; set; }

        /// <summary>
        /// Sets the flag that the user has aknowledged the user has read the update message.
        /// </summary>
        void setAcknowledged();

        /// <summary>
        /// The email address of the user;
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// The user's office phone number
        /// </summary>
        string OfficePhone { get; set; }

        /// <summary>
        /// The user's mobile phone number. May be null
        /// </summary>
        string MobilePhone { get; set; }

        /// <summary>
        /// The hire date of the user
        /// </summary>
        DateTime HireDate { get; set; }

        /// <summary>
        /// The term date of the user. Can be null if active
        /// </summary>
        DateTime? TermDate { get; set; }

        /// <summary>
        /// Validates the user object to make sure critical information is complete
        /// </summary>
        /// <returns>True if the user is valid</returns>
        bool isValid();

        /// <summary>
        /// The user is able to impersonate another user
        /// </summary>
        /// <returns>Returns true if the user is able to impersonate another user. False if not</returns>
        bool canImpersonate();

        /// <summary>
        /// The user is the ultimate user of the system (BI Manager)
        /// </summary>
        /// <returns>Returns true if the user is a BI Manager. False if not</returns>
        bool isBIManager();

        /// <summary>
        /// Compares this user with another user.
        /// </summary>
        /// <param name="user">The user to compare against</param>
        /// <returns>True if the employee Ids equal</returns>
        bool Equals(IUser user);
    }
}