// "// Copyright (c) FIAP Cloud Games. All rights reserved."

using FCG.Api.Controllers;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers;
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest req, CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterAsync(req, cancellationToken);
        return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = result.User.Id }, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken cancellationToken)
    {
        var result = await _userService.LoginAsync(req, cancellationToken);
        if (result == null) return Unauthorized();
        return Ok(result);
    }
}
