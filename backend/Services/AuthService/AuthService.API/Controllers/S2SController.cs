using AuthService.Business.Abstract;
using AuthService.Entities.DTOs;
using Core.Extensions;
using Core.Utilities.Security.S2S;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    /// <summary>
    /// Service-to-Service (S2S) token işlemleri
    /// OAuth 2.0 Client Credentials Grant implementasyonu
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class S2SController : ControllerBase
    {
        private readonly IS2STokenService _s2sTokenService;

        public S2SController(IS2STokenService s2sTokenService)
        {
            _s2sTokenService = s2sTokenService;
        }

        /// <summary>
        /// S2S Token alma (Client Credentials Grant)
        /// Servisler arası iletişim için token alır
        /// </summary>
        /// <remarks>
        /// Kullanım:
        /// ```
        /// POST /api/s2s/token
        /// {
        ///     "clientId": "svc_xxxxxxxxxxxx",
        ///     "clientSecret": "your-secret",
        ///     "scope": "CategoryService InventoryService" // opsiyonel
        /// }
        /// ```
        /// </remarks>
        [HttpPost("token")]
        [ProducesResponseType(typeof(ApiResponse<S2STokenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult GetToken([FromBody] S2STokenRequestDto request)
        {
            var result = _s2sTokenService.CreateToken(request);

            if (!result.IsSuccess)
                return Unauthorized(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<S2STokenResponseDto>.SuccessResponse(result.Data, result.Message));
        }

        /// <summary>
        /// Token doğrulama
        /// Gateway veya diğer servisler tarafından token doğrulamak için kullanılır
        /// Hem user hem de service token'larını doğrular
        /// </summary>
        /// <remarks>
        /// Kullanım:
        /// ```
        /// POST /api/s2s/validate
        /// {
        ///     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        ///     "requiredScope": "CategoryService" // opsiyonel
        /// }
        /// ```
        /// </remarks>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(ApiResponse<TokenValidationResponseDto>), StatusCodes.Status200OK)]
        public IActionResult ValidateToken([FromBody] TokenValidationRequestDto request)
        {
            var result = _s2sTokenService.ValidateToken(request);

            // Validation sonucu her zaman 200 döner, IsValid ile kontrol edilir
            return Ok(ApiResponse<TokenValidationResponseDto>.SuccessResponse(result.Data));
        }

        /// <summary>
        /// Token introspection (RFC 7662)
        /// Servisler kendi token'larının geçerliliğini kontrol edebilir
        /// </summary>
        [HttpPost("introspect")]
        [ProducesResponseType(typeof(ApiResponse<TokenValidationResponseDto>), StatusCodes.Status200OK)]
        public IActionResult IntrospectToken([FromForm] string token)
        {
            var request = new TokenValidationRequestDto { Token = token };
            var result = _s2sTokenService.ValidateToken(request);

            return Ok(ApiResponse<TokenValidationResponseDto>.SuccessResponse(result.Data));
        }

        #region Service Client Management (Admin Only)

        /// <summary>
        /// Yeni ServiceClient oluşturur (Admin)
        /// DİKKAT: ClientSecret sadece bir kez gösterilir!
        /// </summary>
        [HttpPost("clients")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<ServiceClientCreatedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateServiceClient([FromBody] ServiceClientDto dto)
        {
            var result = _s2sTokenService.CreateServiceClient(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<ServiceClientCreatedDto>.SuccessResponse(result.Data, result.Message));
        }

        /// <summary>
        /// ServiceClient günceller (Admin)
        /// </summary>
        [HttpPut("clients/{clientId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public IActionResult UpdateServiceClient(string clientId, [FromBody] ServiceClientDto dto)
        {
            var result = _s2sTokenService.UpdateServiceClient(clientId, dto);

            if (!result.IsSuccess)
                return NotFound(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// ServiceClient deaktif eder (Admin)
        /// </summary>
        [HttpDelete("clients/{clientId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public IActionResult DeactivateServiceClient(string clientId)
        {
            var result = _s2sTokenService.DeactivateServiceClient(clientId);

            if (!result.IsSuccess)
                return NotFound(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// ClientSecret yeniler (Admin)
        /// DİKKAT: Yeni secret sadece bir kez gösterilir!
        /// </summary>
        [HttpPost("clients/{clientId}/regenerate-secret")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<ServiceClientCreatedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public IActionResult RegenerateClientSecret(string clientId)
        {
            var result = _s2sTokenService.RegenerateClientSecret(clientId);

            if (!result.IsSuccess)
                return NotFound(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<ServiceClientCreatedDto>.SuccessResponse(result.Data, result.Message));
        }

        #endregion
    }
}
