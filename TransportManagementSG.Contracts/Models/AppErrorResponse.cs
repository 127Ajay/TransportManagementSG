using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Contracts.Models
{
    public class AppErrorResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string TraceId { get; set; }
    }
}
