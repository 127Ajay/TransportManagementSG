using System;
using System.Collections.Generic;
using System.Text;

namespace TransportManagementSG.Application.Interfaces.Repository
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role);
    }
}
