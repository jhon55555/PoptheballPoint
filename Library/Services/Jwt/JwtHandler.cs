using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Poptheball;
using Services.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Jwt
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly SecurityKey _securityKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly ITokenManager _tokenManager;
        public JwtHandler(IOptions<JwtOptions> options, ITokenManager tokenManager)
        {
            _options = options.Value;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenManager = tokenManager;
        }
        //public JsonWebToken Create(string sauidint, string saguid, string uguid/*Ppart for front*/, string host = "", string vpn = "")
        public JsonWebToken Create(JwtPar jwtPar)
        {
            string audienceId = _options.AudienceId;
            var nowUtc = DateTime.UtcNow;
            var expires1 = nowUtc.AddMinutes(_options.ExpiryMinutes);
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)(new TimeSpan(expires1.Ticks - centuryBegin.Ticks).TotalSeconds);
            var iat = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);

            var issued = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);

            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim("saguid", jwtPar.saguid));
            identity.AddClaim(new Claim("sauidint", jwtPar.sauidint));
            identity.AddClaim(new Claim("uguid", jwtPar.uguid));
            identity.AddClaim(new Claim("ppart", jwtPar.ppart));
            identity.AddClaim(new Claim("host", jwtPar.host));
            identity.AddClaim(new Claim("vpn", jwtPar.vpn));
            identity.AddClaim(new Claim("uredis", jwtPar.uredis));
            identity.AddClaim(new Claim("levno", jwtPar.levno));
            identity.AddClaim(new Claim("boutypeid", jwtPar.boutypeid));
            identity.AddClaim(new Claim("scode", jwtPar.scode));
            identity.AddClaim(new Claim("uidint", jwtPar.uidint));
            identity.AddClaim(new Claim("pcode", jwtPar.pcode));
            var jwt = new JwtSecurityToken(_options.Issuer, audienceId, identity.Claims, issued, expires, _signingCredentials);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);
            return new JsonWebToken
            {
                AccessToken = token,
                Expires = exp
            };
        }
        public string GetUGuid(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "uguid").Value;

            return unique_name;
        }
        public string GetUid(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "sauidint").Value;

            return unique_name;
        }
        public string GetUidint(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "uidint").Value;

            return unique_name;
        }
        public string GetSaGuid(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "saguid").Value;

            return unique_name;
        }
        public string GetSauid(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "sauidint").Value;

            return unique_name;
        }
        public string Getppart(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "ppart").Value;

            return unique_name;
        }
        public bool Geturedis(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            bool unique_name = Convert.ToBoolean(tokenS.Claims.First(claim => claim.Type == "uredis").Value);

            return unique_name;
        }
        public string getboutypeid(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "boutypeid").Value;

            return unique_name;
        }
        public string getlevno(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "levno").Value;

            return unique_name;
        }
        public string getscode(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "scode").Value;

            return unique_name;
        }
        public string Getpcode(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authHeader.Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;

            var unique_name = tokenS.Claims.First(claim => claim.Type == "pcode").Value;

            return unique_name;
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),

                    ValidIssuer = _options.Issuer,
                    ValidateIssuer = true,

                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                return principal;

            }
            catch (Exception)
            {
                throw new SecurityTokenException("Invalid token");
            }
            

        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
    }
}