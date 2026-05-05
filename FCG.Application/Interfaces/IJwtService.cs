// "// Copyright (c) FIAP Cloud Games. All rights reserved."

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FCG.Domain.Entities;

namespace FCG.Application.Interfaces;
public interface IJwtService
{
    string GenerateToken(User user);
}
