using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TransportManagementSG.Contracts.Model
{
    public class Vehicle : BaseModel
    {
        public int VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public int StateID { get; set; }
        public int VehicleTypeID { get; set; }
        public int ModelNumber { get; set; }
        public int RegistrationYear { get; set; }
        public int TransportStatusId { get; set; }
        public int IsActive { get; set; }
    }
}
