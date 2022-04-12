using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Services.CacheManager;
using Services.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Jwt
{
    public class TokenManager : ITokenManager
    {
        private readonly IRedis _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly Config _config;

        public TokenManager(IRedis cache,
                IHttpContextAccessor httpContextAccessor,
                IOptions<JwtOptions> jwtOptions, Config config
            )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
            _config = config;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());
        public async Task<bool> IsActiveTokenAdminSub()
           => await IsActiveAdminSub(GetCurrentAsync());
        public async Task<bool> IsActiveTokenPoptheball()
           => await IsActivePoptheball(GetCurrentAsync());
        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());
        public async Task<bool> IsActiveAsync((string username, string token) pair)
        {
            if (pair.username == null) return false;
            if (_cache.IsExist(string.Format(GlobalCacheKey.AdminAccessToken, pair.username), _config.RedisLocaldb, (byte)RedisServer.Local))
            {
                if (!pair.token.Equals(_cache.Get<string>(string.Format(GlobalCacheKey.AdminAccessToken, pair.username), _config.RedisLocaldb, (byte)RedisServer.Local)))
                {
                    return false;
                }
                else
                    return true;
            }
            return false;
        }
        public async Task<bool> IsActiveAdminSub((string username, string token) pair)
        {
            if (pair.username == null) return false;
            if (_cache.IsExist(string.Format(GlobalCacheKey.AdminSubAccessToken, pair.username), _config.RedisLocaldb, (byte)RedisServer.Local))
            {
                if (!pair.token.Equals(_cache.Get<string>(string.Format(GlobalCacheKey.AdminSubAccessToken, pair.username), _config.RedisLocaldb, (byte)RedisServer.Local)))
                {
                    return false;
                }
                else
                    return true;
            }
            return false;
        }
        public async Task<bool> IsActivePoptheball((string username, string token) pair)
        {
            if (pair.username == null) return false;
            if (_cache.IsExist(string.Format(GlobalCacheKey.UserAccessToken, pair.username), _config.RedisLocaldb, (byte)RedisServer.Local))
            {
                if (!pair.token.Equals(_cache.Get<string>(string.Format(GlobalCacheKey.UserAccessToken, pair.username), _config.RedisLocaldb, (byte)RedisServer.Local)))
                {
                    return false;
                }
                else
                    return true;
            }
            return false;
        }
        
        public async Task DeactivateAsync((string username, string token) pair)
        {
            //if (!string.IsNullOrEmpty(pair.username)) await _cache.RemoveAsync(GetKey(pair.username));
        }

        public void AddToken(string username, string token, int db)
        {
            _cache.SetInMin(username, _jwtOptions.Value.ExpiryMinutes, token, db, (byte)RedisServer.Local);
        }

        private (string username, string token) GetCurrentAsync()
        {
            try
            {
                var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(authorizationHeader.ToString().Replace("bearer", "").Replace("Bearer", "").Trim()) as JwtSecurityToken;
                var unique_name = tokenS.Claims.First(claim => claim.Type == "saguid").Value;
                //var identity1 = _httpContextAccessor.HttpContext.User.Claims;
                //var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                //var claims = identity.Claims.ToList();
                if (!unique_name.Any()) return (null, null);
                var token = authorizationHeader == StringValues.Empty
                    ? string.Empty
                    : authorizationHeader.Single().Split(" ").Last();

                return (unique_name, token);
            }
            catch (Exception)
            {
                return (null, null);
            }

        }
    }
}
