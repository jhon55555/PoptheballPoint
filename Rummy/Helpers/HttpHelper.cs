using System;
using System.IO;
using System.Net;
using System.Text;

namespace Rummy.Helpers
{
    public class HttpHelper
    {
        private readonly ErrorLog _Elog;
        public HttpHelper(ErrorLog Elog)
        {
            _Elog = Elog;
        }
        public string Post(string uri, string data, string contentType, string method = "POST", string authHeader = "")
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;

                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add("X-Client", authHeader);

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'Unauthorized access'}";
            }
            catch (Exception ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'" + ex.ToString() + "'}";
            }
        }
        public HttpWebResponse Post1(string uri, string data, string contentType, string method = "POST", string authHeader = "")
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;

                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add("Authorization", "Bearer " + authHeader);

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response;
                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //{
                //    using (Stream stream = response.GetResponseStream())
                //    {
                //        using (StreamReader reader = new StreamReader(stream))
                //        {
                //            return reader.ReadToEnd();
                //        }
                //    }
                //}
            }
            catch (UnauthorizedAccessException ex)
            {
                return null;
            }
            catch (WebException ex)
            {
                _Elog.WriteLog(ex.Response.ToString());
                return (HttpWebResponse)ex.Response;
            }
        }
        public  string Get(string uri, string authHeader = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add("Authorization", "bearer " + authHeader);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'Unauthorized access'}";
            }
            catch (Exception ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'" + ex.ToString() + "'}";
            }
        }

        public string Get1(string uri, string authHeader = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Headers.Add("Time-Zone", "Asia/Shanghai");
                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add("Authorization", "Bearer " + authHeader);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'Unauthorized access'}";
            }
            catch (Exception ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'" + ex.ToString() + "'}";
            }
        }

        public string Delete(string uri, string authHeader = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Method = "DELETE";

                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add(string.Format("Autherization: bearer {0}", authHeader));

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'Unauthorized access'}";
            }
            catch (Exception ex)
            {
                _Elog.WriteLog(ex.ToString());
                return "{'status':'400','message':'" + ex.ToString() + "'}";
            }
        }
    }
}
