using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using Services.Common;
using Services.Poptheball;
using Services.Jwt;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Poptheball.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthKeyfilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            Microsoft.Extensions.Primitives.StringValues authTokens;
            filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);

            //var ddd = JsonConvert.SerializeObject(body);
            var _token = authTokens.FirstOrDefault();
            if (filterContext != null)
            {
                if (_token != null)
                {
                    var services = filterContext.HttpContext.RequestServices;
                    string uguid = string.Empty;
                    string uid = string.Empty;
                    var jwt = (IJwtHandler)services.GetService(typeof(IJwtHandler));
                    uguid = jwt.GetSaGuid(_token);
                    uid = jwt.GetUid(_token);
                    Guid uu_id;
                    bool res = Guid.TryParse(uguid, out uu_id);

                    if (!res)
                        filterContext.Result = new ObjectResult("Unauthorized access")
                        {
                            Value = new
                            {
                                status = "300",
                                msg = "token not valid."
                            },
                        };
                    var par = filterContext.HttpContext.Request.Path.Value.ToString();
                    var syncIOFeature = filterContext.HttpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;

                        var req = filterContext.HttpContext.Request;

                        req.EnableBuffering();

                        if (req.Body.CanSeek)
                        {
                            req.Body.Seek(0, SeekOrigin.Begin);
                            using (var reader = new StreamReader(
                                 req.Body,
                                 encoding: Encoding.UTF8,
                                 detectEncodingFromByteOrderMarks: false,
                                 bufferSize: 8192,
                                 leaveOpen: true))
                            {
                                var jsonString = reader.ReadToEnd();
                                string ip = GetIPAddress();
                                if (!string.IsNullOrEmpty(jsonString))
                                {
                                    //var _Elog = (ErrorLog)services.GetService(typeof(ErrorLog));
                                    try
                                    {
                                        JObject jo = JObject.Parse(jsonString);
                                        bool val = CheckWords(jo, filterContext);
                                        if (val)
                                        {

                                        }
                                        else
                                        {
                                           
                                            //_Elog.WriteLogFilter("AuthKeyfilter", jsonString+ ip+ par+ uid+ uguid);
                                            //var chkword = (IFrontService)services.GetService(typeof(IFrontService));
                                            //var chk = chkword.CheckWords(jsonString, ip, par, uid, uguid);
                                            //if (chk.Tables[0].Rows[0]["id"].ToString() == "1")
                                            //{
                                            //    filterContext.Result = new ObjectResult("Unauthorized access")
                                            //    {
                                            //        Value = new
                                            //        {
                                            //            status = 301,
                                            //            msg = "User block"
                                            //        },
                                            //    };
                                            //}
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //_Elog.WriteLogFilter("AuthKeyfilter", jsonString + ip + par + uid + uguid);
                                        filterContext.Result = new ObjectResult("Unauthorized access")
                                        {
                                            Value = new
                                            {
                                                status = 400,
                                                msg = "Invalid inputs"
                                            },
                                        };
                                    }

                                }
                                filterContext.HttpContext.Items.Add("request_body", jsonString);
                            }

                            // go back to beginning so json reader get's the whole thing
                            req.Body.Seek(0, SeekOrigin.Begin);
                        }
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
        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static bool CheckWords(JObject jObject, AuthorizationFilterContext filterContext)
        {
            var services = filterContext.HttpContext.RequestServices;
            var _config = (Config)services.GetService(typeof(Config));
            foreach (var task in jObject)
            {
                if (!string.IsNullOrEmpty(task.Value.ToString()) && _config.ExWords.Contains(task.Value.ToString().ToLower()))
                    //if (!string.IsNullOrEmpty(item.Value.ToString()) && _config.ExWords.Contains(item.Value.ToString()))
                    return false;
            }
            return true;
        }

    }
}
