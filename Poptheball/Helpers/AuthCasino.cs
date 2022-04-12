using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Services.Jwt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Poptheball.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthCasino : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            Microsoft.Extensions.Primitives.StringValues authTokens;
            filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);
            var _token = authTokens.FirstOrDefault();
            if (filterContext != null)
            {
                if (_token != null)
                {
                    var services = filterContext.HttpContext.RequestServices;
                    var log = (ITokenManager)services.GetService(typeof(ITokenManager));
                    var isactive = log.IsActiveTokenPoptheball();
                    if (isactive != null && isactive.Result == true)
                    {

                    }
                    else
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                        filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                        filterContext.Result = new ObjectResult("Unauthorized access")
                        {
                            Value = new
                            {
                                status = 401,
                                msg = "Unauthorized access"
                            },
                        };
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                    filterContext.Result = new ObjectResult("Unauthorized access")
                    {
                        Value = new
                        {
                            status = 401,
                            msg = "Unauthorized access"
                        },
                    };
                }
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                filterContext.Result = new ObjectResult("Please Provide authToken")
                {
                    Value = new
                    {
                        status = 401,
                        msg = "Unauthorized access"
                    },
                };
            }
        }

    }
}
