using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts.Model
{
    public class VehicleType : BaseModel
    {
        public int VehicleTypeID { get; set; }
        public string VehicleTypeName { get; set; }
        public int WheelCount { get; set; }
        public int LengthFeet { get; set; }
        public bool IsActive { get; set; }
    }
}
