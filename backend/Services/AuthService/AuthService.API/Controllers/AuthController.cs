using AuthService.Business.Abstract;
using AuthService.Entities.DTOs;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            var result = _authService.Register(userForRegisterDto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<TokenDto>.SuccessResponse(result.Data, result.Message));
        }

        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var result = _authService.Login(userForLoginDto);

            if (!result.IsSuccess)
                return Unauthorized(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<TokenDto>.SuccessResponse(result.Data, result.Message));
        }

        /// <summary>
        /// Access token yenileme
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = _authService.RefreshToken(refreshTokenDto.RefreshToken);

            if (!result.IsSuccess)
                return Unauthorized(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<TokenDto>.SuccessResponse(result.Data, result.Message));
        }

        /// <summary>
        /// Kullanıcı çıkışı
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Logout([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = _authService.Logout(refreshTokenDto.RefreshToken);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Şifre sıfırlama emaili gönder
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var result = _authService.ForgotPassword(forgotPasswordDto);
            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Şifre sıfırlama
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = _authService.ResetPassword(resetPasswordDto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Email doğrulama
        /// </summary>
        [HttpGet("verify-email")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            var result = _authService.VerifyEmail(token);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Doğrulama emaili yeniden gönder
        /// </summary>
        [HttpPost("resend-verification-email")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult ResendVerificationEmail([FromBody] ForgotPasswordDto dto)
        {
            var result = _authService.ResendVerificationEmail(dto.Email);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Şifre değiştirme (giriş yapmış kullanıcı için)
        /// </summary>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(ApiResponse.ErrorResponse("Yetkisiz erişim."));

            var result = _authService.ChangePassword(userId, changePasswordDto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }
    }
}
