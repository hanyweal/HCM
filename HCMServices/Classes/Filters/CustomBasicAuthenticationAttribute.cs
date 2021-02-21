using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HCMServices.Filters
{
    public class CustomBasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string userName = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                if (IsAuthenticated(userName, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

        private bool IsAuthenticated(string userName, string password)
        {
            string LocalUserName = ConfigurationManager.AppSettings["UserName"];
            string LocalPassword = ConfigurationManager.AppSettings["Password"];
            //string LocalUserName = Utilities.DecryptString(ConfigurationManager.AppSettings["UserName"]);
            //string LocalPassword = Utilities.DecryptString(ConfigurationManager.AppSettings["Password"]);

            return (userName == LocalUserName && password == LocalPassword);
        }
    }
}