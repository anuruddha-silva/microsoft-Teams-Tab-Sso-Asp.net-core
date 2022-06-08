using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.Identity.Web;
using TeamsBackend.Services;

namespace TeamsBackend.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthController : ControllerBase
    {
        private IConfiguration Configuration;
        private ITokenAcquisitionService tokenAcquisitionService;
        public ApiAuthController(IConfiguration configuration, ITokenAcquisitionService tokenAcquisitionService)
        {
            Configuration = configuration;
            this.tokenAcquisitionService = tokenAcquisitionService;
        }

        [Route("auth/token")]
        [HttpGet]
        public async Task<string> GetExchangeTokenAndUserData()
        {
            try
            {
                var ClientToken = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"].ToString()).Parameter;
                string accessToken = await GetAccessTokenAsync(ClientToken);


                // Graph call 
                string[] scopes = { Configuration["GraphAPI:Scope"] };

                var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider
                 (
                    request =>
                    {
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                            "Bearer", accessToken);
                        return Task.CompletedTask;
                    }));

                var user = await graphClient.Me.Request().Select(x => new { x.Mail, x.DisplayName, x.GivenName, x.UserPrincipalName, x.MobilePhone, x.CompanyName, x.JobTitle }).GetAsync();

                return ($"Received Access Token: {accessToken}, DisplayName: { user.DisplayName}, Email: {user.UserPrincipalName}, MobilePhone: {user.MobilePhone}, Company Name: {user.CompanyName}, Job Title: {user.JobTitle}");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Excahnge Client token to server token
        private async Task<string> GetAccessTokenAsync(string clientToken)
        {
            try
            {
                string graphEndpoint = Configuration["GraphAPI:Scope"];
                var accesstoken = await tokenAcquisitionService.GetOnBehalfAccessTokenAsync(graphEndpoint, clientToken);
                return accesstoken;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}