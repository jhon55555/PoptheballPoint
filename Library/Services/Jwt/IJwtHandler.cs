
using Models.Poptheball;

namespace Services.Jwt
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public long Expires { get; set; }
    }
    public interface IJwtHandler
    {
        JsonWebToken Create(JwtPar jwtPar);
        //JsonWebToken Create(string sauidint, string saguid, string uguid, string host="", string vpn="");
        string GetUGuid(string authHeader);
        string GetUid(string authHeader);
        string GetUidint(string authHeader);
        string GetSaGuid(string authHeader);
        string GetSauid(string authHeader);
        string Getppart(string authHeader);
        bool Geturedis(string authHeader);
        string getboutypeid(string authHeader);
        string getlevno(string authHeader);
        string getscode(string authHeader);
        string Getpcode(string authHeader);
        string GenerateRefreshToken();
    }
}