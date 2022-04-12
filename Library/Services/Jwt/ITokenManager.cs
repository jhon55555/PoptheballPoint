using System.Threading.Tasks;

namespace Services.Jwt
{
    public interface ITokenManager
    {
        void AddToken(string username, string token,int db);
        Task DeactivateAsync((string username, string token) pair);
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync((string username, string token) pair);
        Task<bool> IsActiveAdminSub((string username, string token) pair);
        Task<bool> IsActivePoptheball((string username, string token) pair);
        Task<bool> IsCurrentActiveToken();
        Task<bool> IsActiveTokenAdminSub();
        Task<bool> IsActiveTokenPoptheball();
    }
}