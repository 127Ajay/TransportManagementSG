using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts.Model
{
    public class State : BaseModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }
    }
}
