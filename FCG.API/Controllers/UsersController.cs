using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FCG.Application.DTOs;
using FCG.Domain.Enums;
using FCG.Application.Interfaces;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            var isAdmin = User.IsInRole(nameof(UserRole.Admin));

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            var library = await _userService.GetByIdAsync(id, cancellationToken);
            return Ok(library);

        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            var isAdmin = User.IsInRole(nameof(UserRole.Admin));
            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }
            var updatedUser = await _userService.UpdateAsync(id, request, cancellationToken);
            return Ok(updatedUser);
        }

        [HttpPatch("{id:guid}/password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId != id)
            {
                return Forbid();
            }
            await _userService.ChangePasswordAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:guid}/promote")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PromoteToAdmin(Guid id, CancellationToken cancellationToken)
        {
            await _userService.PromoteToAdminAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:guid}/library")]
        [ProducesResponseType(typeof(IEnumerable<UserGameResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLibrary(Guid id, [FromServices] IGameService gameService, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            var isAdmin = User.IsInRole(nameof(UserRole.Admin));

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }
            var library = await gameService.GetUserLibraryAsync(id, cancellationToken);
            return Ok(library);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new InvalidOperationException("User ID claim is missing or invalid.");
            }
            return userId;
        }
    }
}
