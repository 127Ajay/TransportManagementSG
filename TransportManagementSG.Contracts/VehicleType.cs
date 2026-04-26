using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts
{
    public class VehicleType
    {
        public int VehicleTypeID { get; set; }
        public string VehicleTypeName { get; set; }
        public int Wheel { get; set; }
        public int Feet { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
