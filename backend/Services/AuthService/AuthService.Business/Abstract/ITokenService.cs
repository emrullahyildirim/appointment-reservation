using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Utilities.Security.JWT;

namespace AuthService.Business.Abstract
{
    public interface ITokenService
    {
        AccessToken CreateAccessToken(User user, List<OperationClaim> claims);
        RefreshToken CreateRefreshToken();
        TokenDto CreateTokenDto(User user, List<OperationClaim> claims);
    }
}

