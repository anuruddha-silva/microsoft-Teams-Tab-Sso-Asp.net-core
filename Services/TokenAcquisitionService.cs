using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsBackend.Models;

namespace TeamsBackend.Services
{
    public class TokenAcquisitionService : ITokenAcquisitionService
    {
        private readonly IOptions<AzureAdOptions> azureAdOptions;

        private readonly string[] scopesRequestedByMsalNet = new string[]
        {
            "openid",
            "profile",
            "offline_access",
        };

        public TokenAcquisitionService(IOptions<AzureAdOptions> azureAdOptions)
        {
            this.azureAdOptions = azureAdOptions;
        }

        // Exchange client token to access token
        public async Task<string> GetOnBehalfAccessTokenAsync(string graphScopes, string ClientToken)
        {
            try
            {
                UserAssertion userAssertion = new UserAssertion(ClientToken, "urn:ietf:params:oauth:grant-type:jwt-bearer");
                IEnumerable<string> requestedScopes = graphScopes.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();

                var confidentialClientApp = ConfidentialClientApplicationBuilder.Create(azureAdOptions.Value.ClientId)
                     .WithAuthority(azureAdOptions.Value.Authority)
                     .WithClientSecret(azureAdOptions.Value.ClientSecret)
                     .Build();

                var result = await confidentialClientApp.AcquireTokenOnBehalfOf(
                    requestedScopes.Except(scopesRequestedByMsalNet),
                    userAssertion)
                    .ExecuteAsync();
                return result.AccessToken;
            }
            catch (Exception ex )
            {

                throw ex;
            }
           
               
      
            }
        
          

   
        }
    
}
