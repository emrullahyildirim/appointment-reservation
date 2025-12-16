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
    [Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Mevcut kullanıcı bilgilerini getir
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(ApiResponse.ErrorResponse("Yetkisiz erişim."));

            var result = _userService.GetUserDto(userId);

            if (!result.IsSuccess)
                return NotFound(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<UserDto>.SuccessResponse(result.Data, "Kullanıcı bilgileri getirildi."));
        }

        /// <summary>
        /// Tüm kullanıcıları getir (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<UserDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();
            return Ok(ApiResponse<List<UserDto>>.SuccessResponse(result.Data, "Kullanıcılar listelendi."));
        }

        /// <summary>
        /// ID'ye göre kullanıcı getir (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetUserDto(id);

            if (!result.IsSuccess)
                return NotFound(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse<UserDto>.SuccessResponse(result.Data, "Kullanıcı bilgileri getirildi."));
        }

        /// <summary>
        /// Kullanıcı sil (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id);

            if (!result.IsSuccess)
                return NotFound(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Kullanıcıya rol ekle (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/roles/{roleName}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult AddRole(int id, string roleName)
        {
            var result = _userService.AddRole(id, roleName);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }

        /// <summary>
        /// Kullanıcıdan rol kaldır (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/roles/{roleName}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult RemoveRole(int id, string roleName)
        {
            var result = _userService.RemoveRole(id, roleName);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse.ErrorResponse(result.Message));

            return Ok(ApiResponse.SuccessResponse(result.Message));
        }
    }
}
