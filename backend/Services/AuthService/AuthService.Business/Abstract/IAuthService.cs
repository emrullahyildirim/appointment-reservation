using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Utilities.Result;

namespace AuthService.Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<TokenDto> Register(UserForRegisterDto userForRegisterDto);
        IDataResult<TokenDto> Login(UserForLoginDto userForLoginDto);
        IDataResult<TokenDto> RefreshToken(string refreshToken);
        IResult Logout(string refreshToken);
        IResult ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        IResult ResetPassword(ResetPasswordDto resetPasswordDto);
        IResult VerifyEmail(string token);
        IResult ResendVerificationEmail(string email);
        IResult ChangePassword(int userId, ChangePasswordDto changePasswordDto);
        IResult UserExists(string email);
    }
}

