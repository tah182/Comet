using System;

namespace COMET.Model.Domain {
    /// <summary>
    /// An office building that a user may be located in.<br />
    /// Inherits abstract class ABuilding
    /// </summary>
    public class OfficeBuilding : ABuilding {
        private string officeName;

        /// <summary>
        /// Constructor for Office Buildings
        /// </summary>
        /// <param name="officeName"></param>
        /// <param name="streetAddress"></param>
        /// <param name="city"></param>
        /// <param name="postalCode"></param>
        /// <param name="stateCode"></param>
        public OfficeBuilding(string officeName, string streetAddress, string city, string postalCode, string stateCode) {
            this.officeName = officeName;

            // strip out the premise from the street Address
            if (streetAddress != null) {
                string[] streetSplit = streetAddress.Split(new Char[] { ' ' }, 2);
                if (streetSplit.Length > 1) {
                    Premise = streetSplit[0];
                    Street = streetSplit[1];
                } else {
                    Premise = "";
                    Street = streetSplit[0];
                }
            } else {
                Premise = "";
                Street = "";
            }

            City = city;
            PostalCode = postalCode;
            State = stateCode;
        }

        /// <summary>
        /// Strips out the Street, City, State from the Office Name in the LDAP and return basic information.
        /// </summary>
        public string OfficeName {
            get {
                // strip out colon from the string
                if (this.officeName != null && this.officeName.Contains(":") && City != null && Street != null && State != null)
                    return this.officeName = officeName.Replace(":", "").Replace(Street, "").Replace(City, "").Replace(State, "");

                return this.officeName;
            }
        }

        /// <summary>
        /// Hides the AddressID Accessor from Office Building
        /// </summary>
        new private int AddressID {
            get { throw new NotImplementedException("Address IDs are not used for office buildings."); }
            set { throw new NotImplementedException("Address IDs are not used for office buildings."); }
        }
    }
}