using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Jwt
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
        public string Issuer { get; set; }
        public string AudienceId { get; set; }
    }
}
