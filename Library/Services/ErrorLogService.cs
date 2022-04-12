using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Services
{
    public class ErrorLogService
    {
        private readonly IConfiguration _config;
        private IHostEnvironment _env;
        public ErrorLogService(IConfiguration config, IHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void WriteLog(String str, String request = "", String ErrorMesage = "")
        {
            try
            {
                //if (_config.GetValue<bool>("ErrorLog"))
                //{
                    var path = _env.ContentRootPath + "\\Logs\\Service/Error_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt";
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "Logs/Error_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt");
                    if (!File.Exists(path))
                    {
                        var myFile = File.Create(path);
                        myFile.Close();
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("---------------------------------------------------------------------------------------------");
                        sw.Write(DateTime.Now);
                        sw.WriteLine(Environment.NewLine + str + Environment.NewLine + request+Environment.NewLine + ErrorMesage);
                    }
                //}
            }
            catch (Exception)
            { }
        }

        public void WriteLogAll(String str, String request = null)
        {
            try
            {
                if (_config.GetValue<bool>("ErrorLog"))
                {
                    var path = _env.ContentRootPath + "\\Logs\\Bet/Log_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt";
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "Logs/Request/Log_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt");
                    if (!File.Exists(path))
                    {
                        var myFile = File.Create(path);
                        myFile.Close();
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("---------------------------------------------------------------------------------------------");
                        sw.Write(DateTime.Now);
                        sw.WriteLine(Environment.NewLine + str + Environment.NewLine + request);
                    }
                }
            }
            catch (Exception)
            { 

            }
        }
    }
}
