using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsBackend.Models
{
    // Azure AD Configurations details
    public class AzureAdOptions
    {
 
        public string Instance { get; set; }


        public string ClientId { get; set; }


        public string ClientSecret { get; set; }


        public string TenantId { get; set; }
               

        public string ApplicationIdUri { get; set; }

  
        public string GraphScope { get; set; }


        public string Authority => Instance + TenantId;
    }
}

