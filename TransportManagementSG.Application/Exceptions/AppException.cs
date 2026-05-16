using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Application.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message): base(message)
        {
        }
    }
}
