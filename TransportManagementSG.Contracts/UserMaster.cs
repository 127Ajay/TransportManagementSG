using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts
{
    public class UserMaster
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public bool IsActive { get; set; }
        public string LoginPassword { get; set; }
        public DateTime LastLoginDateRoleName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
