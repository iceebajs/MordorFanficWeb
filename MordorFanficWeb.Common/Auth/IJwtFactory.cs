using System.Threading.Tasks;
using System.Security.Claims;

namespace MordorFanficWeb.Common.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string email, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string email, string id);
    }
}
