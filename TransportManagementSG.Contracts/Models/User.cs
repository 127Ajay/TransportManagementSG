using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts.Model
{
    public class User : BaseModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public bool IsActive { get; set; }
        public string LoginPassword { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
