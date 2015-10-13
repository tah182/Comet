using System.Collections.Generic;

namespace COMET.Model.Domain {
    public class UserType {
        public static readonly UserType ADMIN_MANAGER   = new UserType(-2);
        public static readonly UserType ADMIN           = new UserType(-1);
        public static readonly UserType SUPER_USER      = new UserType(0);
        public static readonly UserType MANAGEMENT      = new UserType(1);
        public static readonly UserType LEAD            = new UserType(2);
        public static readonly UserType USER            = new UserType(3);
        public static readonly UserType PLANNER_911     = new UserType(4);
        
        public static IEnumerable<UserType> Values {
            get {
                yield return ADMIN_MANAGER;
                yield return ADMIN;
                yield return SUPER_USER;
                yield return MANAGEMENT;
                yield return LEAD;
                yield return USER;
                yield return PLANNER_911;
            }
        }

        private readonly int AUTH_LEVEL;

        UserType(int authLevel) {
            this.AUTH_LEVEL = authLevel;
        }

        public int authLevel {
            get { return AUTH_LEVEL; }
        }
    }
}