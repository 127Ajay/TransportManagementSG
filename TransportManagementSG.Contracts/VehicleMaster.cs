using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TransportManagementSG.Contracts
{
    public class VehicleMaster
    {
        public int VehicleMasterID { get; set; }
        public int StateID { get; set; }
        public string VehicleNumber { get; set; }
        public int VehicleTypeID { get; set; }
        public int Model { get; set; }
        public int RegistrationYear { get; set; }
        public int StatusID { get; set; }
        public int IsActive { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
