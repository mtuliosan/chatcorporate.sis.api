using IdentityService.Domain;

namespace IdentityService.Service
{
    public interface ITokenService
    {
        string Create(User user, AppSession session, int expirationTime = 8);

        Task<LoginToken> LoginByToken (string accessToken);
    }
}
