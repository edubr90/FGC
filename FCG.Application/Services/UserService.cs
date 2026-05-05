// "// Copyright (c) FIAP Cloud Games. All rights reserved."

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new ArgumentException("Current password is incorrect.");

        ValidatePassword(request.NewPassword);
        user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(request.NewPassword));

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        user.Deactivate();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(MapToResponse);
    }

    public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        return MapToResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
        ?? throw new ArgumentException("Invalid email or password.");

        if (!user.IsActive)
            throw new ArgumentException("User account is inactive.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new ArgumentException("Invalid credentials");

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse(token, MapToResponse(user));

    }

    public async Task PromoteToAdminAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        user.PromoteToAdmin();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);


    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        ValidatePassword(request.Password);

        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ArgumentException("Email is already in use.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.Name, request.Email, hash);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse(token, MapToResponse(user));

    }

    public Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static void ValidatePassword(string password)
    {
        if (password.Length < 8) throw new ArgumentException("Password must be at least 8 characters long.");
        if (!password.Any(char.IsLetter)) throw new ArgumentException("Password must contain at least one letter.");
        if (!password.Any(char.IsDigit)) throw new ArgumentException("Password must contain at least one number.");
        if (!password.Any(ch => !char.IsLetterOrDigit(ch))) throw new ArgumentException("Password must contain at least one special character.");
    }

    private static UserResponse MapToResponse(User user) => new(user.Id, user.Name, user.Email, user.Role, user.IsActive, user.CreatedAt);
}
