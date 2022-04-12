using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;

namespace Services.Common
{
    public class Config
    {
        private readonly IConfiguration _config;
        public Config(IConfiguration config)
        {
            _config = config;
        }
        public string Conn_AccD
        {
            get
            {
                return _config.GetConnectionString("Conn_AccD");
            }
        }
        public Boolean SqlIP
        {
            get
            {
                return Convert.ToBoolean(_config.GetSection("SqlIP").Value);
            }
        }
        public Boolean Domain
        {
            get
            {
                return Convert.ToBoolean(_config.GetSection("Domain").Value);
            }
        }
        public  String Secret
        {
            get
            {
                return _config.GetSection("Secret").Value;
            }
        }
        public String Id
        {
            get
            {
                return _config.GetSection("Id").Value;
            }
        }
        public string RedisLocal
        {
            get
            {
                return _config.GetSection("Redis:RedisLocal").Value;

            }
        }
        public int RedisLocaldb
        {

            get
            {
                return Convert.ToInt32(_config.GetSection("Redis:RedisLocaldb").Value.ToString());
            }
        }
        public Boolean IP_API
        {
            get
            {
                return Convert.ToBoolean(_config.GetSection("IP_API").Value);
            }
        }

        public Boolean Vpn
        {
            get
            {
                return Convert.ToBoolean(_config.GetSection("Vpn").Value);
            }
        }
        public Boolean Hosting
        {
            get
            {
                return Convert.ToBoolean(_config.GetSection("Hosting").Value);
            }
        }
        public String IPUrl
        {
            get
            {
                return _config.GetSection("IPUrl").Value;
            }
        }
        public string Conn_LogD
        {
            get
            {
                return _config.GetConnectionString("Conn_LogD");
            }
        }

        public string[] ExWords
        {
            get
            {
                return _config.GetSection("ExWords").Value.Split(',');
            }
        }
        public string Cid
        {
            get
            {
                return _config.GetSection("Cid").Value;

            }
        }
        public string Conn_CasinoMaster
        {
            get
            {
                return _config.GetConnectionString("Conn_CasinoMaster");
            }
        }
    }
}
