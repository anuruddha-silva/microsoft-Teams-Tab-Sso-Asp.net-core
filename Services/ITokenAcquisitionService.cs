using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsBackend.Services
{
    public interface ITokenAcquisitionService  
    {
        Task<string> GetOnBehalfAccessTokenAsync(string graphScope, string jwtToken);
    }

  
}
