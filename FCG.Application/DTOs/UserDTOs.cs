// "// Copyright (c) FIAP Cloud Games. All rights reserved."

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCG.Domain.Enums;

namespace FCG.Application.DTOs;

public record RegisterUserRequest(
    string Name,
    string Email,
    string Password
);

public record LoginRequest(
    string Email,
    string Password
);

public record UpdateUserRequest(
    string? Name,
    string? Email
);

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt
);

public record AuthResponse(
    string Token,
    UserResponse User
);
