using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts.Model
{
    public class TransportStatus : BaseModel
    {
        public int TransportStatusID { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
    }
}
