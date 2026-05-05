# FCG - FIAP Cloud Games API

## Sumário

## 1. INSTRUÇÕES PARA EXECUTAR O PROJETO

Pré-requisitos:
  - .NET 8 SDK ou superior
  - Visual Studio 2022 (recomendado) ou VS Code
  - SQL Server LocalDB instalado
  - Git

Passos de Instalação:

1. Clone o repositório:
   $ git clone https://github.com/edubr90/FGC.git
   $ cd FGC

2. Restaure as dependências:
   $ dotnet restore

3. Configure o banco de dados:
   $ dotnet ef database update --project FCG.Infrastructure

4. Execute a API:
   $ dotnet run --project FCG.API

5. Acesse a API:
   - URL: https://localhost:5001
   - Swagger: https://localhost:5001/swagger/index.html

## 2. ENDPOINTS PRINCIPAIS

Autenticação:
  POST   /api/auth/register    - Registrar novo usuário
  POST   /api/auth/login       - Login (retorna JWT)

Usuários:
  GET    /api/users            - Listar usuários (Admin only)
  GET    /api/users/{id}       - Obter usuário por ID
  DELETE /api/users/{id}       - Deletar usuário

Jogos:
  GET    /api/games            - Listar todos os jogos
  GET    /api/games/{id}       - Obter jogo por ID
  POST   /api/games            - Criar novo jogo
  PUT    /api/games/{id}       - Atualizar jogo
  DELETE /api/games/{id}       - Deletar jogo

## 3. ESTRUTURA DE CAMADAS

FCG.Domain (Camada de Domínio):
  - Entidades: User, Game
  - Enums: UserRole, GameGenre, Platform
  - Interfaces: IUserRepository, IGameRepository
  - Padrões: Entidades com lógica de negócio

FCG.Application (Camada de Aplicação):
  - Serviços: UserService, GameService, JwtService
  - DTOs: UserRequest, UserResponse, GameRequest, GameResponse
  - Interfaces: IUserService, IGameService, IJwtService
  - Casos de Uso: Autenticação, CRUD de usuários e jogos

FCG.Infrastructure (Camada de Infraestrutura):
  - Contexto EF: FcgDbContext
  - Repositórios: UserRepository, GameRepository
  - Segurança: JwtService
  - Migrations: Histórico de mudanças no BD

FGC.IoC (Configuração de Dependências):
  - DependencyInjection.cs: Registro central de serviços
  - ApplicationModule: Registro de serviços da aplicação
  - AuthenticationModule: Configuração de autenticação JWT
  - InfrastructureModule: Registro de repositórios e contexto

FCG.API (Camada de Apresentação):
  - Controllers: AuthController, UsersController, GamesController
  - Program.cs: Configuração e inicialização da aplicação
  - appsettings.json: Configurações de produção
  - appsettings.Development.json: Configurações de desenvolvimento

## 4. CONFORMIDADE COM REQUISITOS

[Marcar com X os requisitos atendidos]

Autenticação e Autorização:
  [✓] JWT implementado
  [✓] Roles (Admin/Player) implementados
  [✓] Endpoints protegidos com [Authorize]
  [✓] Hash de senha seguro

Arquitetura:
  [✓] Separação em camadas (Domain, Application, Infrastructure, API)
  [✓] Padrão SOLID aplicado
  [✓] Clean Code
  [✓] Injeção de dependência centralizada
  [✓] Repositórios com interfaces

Funcionalidades:
  [✓] CRUD completo para usuários
  [✓] CRUD completo para jogos
  [✓] Validação robusta
  [✓] Tratamento de erros
  [✓] Respostas padronizadas

Boas Práticas:
  [✓] .editorconfig configurado
  [✓] Nomenclatura consistente
  [✓] Comentários e documentação
  [✓] File-scoped namespaces
  [✓] Async/await em operações I/O

Documentação:
  [✓] README.md completo
  [✓] CONTRIBUTING.md com padrões
  [✓] Swagger/OpenAPI configurado
  [✓] Comentários XML nos controllers

## 📋 Objetivos

A **FCG.API** é uma API RESTful moderna desenvolvida em **.NET 8** que gerencia um sistema de autenticação seguro e um catálogo de jogos. O projeto foi desenvolvido com foco em boas práticas de arquitetura, segurança e escalabilidade.

## Demonstração dos Event-Stormings e Documentação DDD

O Board do Miro poderá ser acessado através do link abaixo:

https://miro.com/welcomeonboard/S1JuQUFUWmtsYUkzK2hLRm9OZWc5d3BzWDNmT1lKbk93dG94WHpka3hsTWV5SlI3NWpiWXpicUlPTUk0N0hKU0VGL25OZjI0UnJOSGJ0UjlLNHYzRE1GaTFWVFZUYzl3dlhzakdCZkdJcFMrSUc2dUx1UFFTa3M5YVd2K2pRT0d0R2lncW1vRmFBVnlLcVJzTmdFdlNRPT0hdjE=?share_link_id=303491922041

### Objetivos Principais

- ✅ Autenticação e autorização robustas com JWT
- ✅ Gerenciamento de usuários com roles (Admin, Player)
- ✅ Catálogo de jogos com operações CRUD
- ✅ Arquitetura em camadas (Domain, Application, Infrastructure, API)
- ✅ Padrões SOLID e clean code
- ✅ Injeção de dependência centralizada
- ✅ Persistência de dados com Entity Framework Core

---

## 🏗️ Arquitetura

O projeto segue uma **arquitetura em camadas** com separação clara de responsabilidades:

FCG.API 
├── FCG.Domain              # Entidades e interfaces de negócio 
├── FCG.Application         # Serviços, DTOs e casos de uso 
├── FCG.Infrastructure      # Acesso a dados, segurança, contexto EF 
├── FGC.IoC                 # Injeção de dependência 
└── FCG.API                 # Controllers e configuração da API


### Componentes Principais

- **Domain**: Entidades de negócio (`User`, `Game`)
- **Application**: Serviços (`UserService`, `GameService`, `JwtService`)
- **Infrastructure**: Acesso a dados (`FcgDbContext`), segurança (`JwtService`)
- **IoC**: Configuração de dependências (`DependencyInjection.cs`)
- **API**: Controllers REST (`AuthController`, `UsersController`, `GamesController`)

---

## 🚀 Começando

### Pré-requisitos

- **.NET 8 SDK** ou superior
- **SQL Server LocalDB** ou SQL Server instalado
- **Visual Studio 2022** (recomendado)

### Instalação

1. **Clone o repositório**

git clone https://github.com/seu-usuario/fcg.git cd fcg

2. **Restaure as dependências**

dotnet restore

3. **Configure o banco de dados**

dotnet ef database update --project FCG.Infrastructure

4. **Execute a API**

dotnet run --project FCG.API

A API estará disponível em `https://localhost:5001` (HTTPS) ou `http://localhost:5000` (HTTP).

---

## 🔑 Autenticação

A API utiliza **JWT (JSON Web Tokens)** para autenticação e autorização.

### Endpoints de Autenticação

#### Registrar novo usuário

POST /api/auth/register Content-Type: application/json
{ "email": "usuario@example.com", "password": "SenhaForte@123", "firstName": "João", "lastName": "Silva" }

**Response (201 Created)**

{ "user": { "id": "550e8400-e29b-41d4-a716-446655440000", "email": "usuario@example.com", "firstName": "João", "lastName": "Silva", "role": "Player" }, "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }

#### Login

POST /api/auth/login Content-Type: application/json
{ "email": "usuario@example.com", "password": "SenhaForte@123" }

**Response (200 OK)**

{ "user": { "id": "550e8400-e29b-41d4-a716-446655440000", "email": "usuario@example.com", "firstName": "João", "lastName": "Silva", "role": "Player" }, "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }

### Usando o Token

Adicione o token JWT ao header `Authorization` de requisições autenticadas:

Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

---

## 👥 Endpoints de Usuários

> ⚠️ Todos os endpoints requerem autenticação com token JWT válido.

### Listar todos os usuários

GET /api/users Authorization: Bearer {token}

**Response (200 OK) - Apenas Admin**

[ { "id": "550e8400-e29b-41d4-a716-446655440000", "email": "usuario@example.com", "firstName": "João", "lastName": "Silva", "role": "Player", "createdAt": "2026-05-05T10:30:00Z" } ]

### Obter usuário por ID

GET /api/users/{id} Authorization: Bearer {token}

**Response (200 OK)**

{ "id": "550e8400-e29b-41d4-a716-446655440000", "email": "usuario@example.com", "firstName": "João", "lastName": "Silva", "role": "Player", "createdAt": "2026-05-05T10:30:00Z" }

### Deletar usuário

DELETE /api/users/{id} Authorization: Bearer {token}

**Response (204 No Content)**

---

## 🎮 Endpoints de Jogos

> ℹ️ Listar jogos é público. Outras operações requerem autenticação.

### Listar todos os jogos

GET /api/games

**Response (200 OK)**

[ { "id": "660e8400-e29b-41d4-a716-446655440001", "title": "Adventure Quest", "description": "Um épico jogo de aventura", "releaseDate": "2026-03-15", "genre": "RPG", "platform": "PC", "price": 49.99, "rating": 4.5 } ]

### Obter jogo por ID

GET /api/games/{id}

**Response (200 OK)**

{ "id": "660e8400-e29b-41d4-a716-446655440001", "title": "Adventure Quest", "description": "Um épico jogo de aventura", "releaseDate": "2026-03-15", "genre": "RPG", "platform": "PC", "price": 49.99, "rating": 4.5 }

### Criar novo jogo

POST /api/games Authorization: Bearer {token} Content-Type: application/json
{ "title": "New Game", "description": "Descrição do jogo", "releaseDate": "2026-06-01", "genre": "Action", "platform": "PS5", "price": 59.99 }

**Response (201 Created)**

### Atualizar jogo

PUT /api/games/{id} Authorization: Bearer {token} Content-Type: application/json
{ "title": "Updated Title", "price": 39.99 }

**Response (200 OK)**

### Deletar jogo

DELETE /api/games/{id} Authorization: Bearer {token}

**Response (204 No Content)**

---

## ⚙️ Configuração

### appsettings.json

{ "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning", "Microsoft.EntityFrameworkCore": "Warning" } }, "AllowedHosts": "*", "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=FcgDb;Trusted_Connection=True;TrustServerCertificate=True;" }, "JwtSettings": { "SecretKey": "sua-chave-secreta-aqui", "Issuer": "FCG", "Audience": "FCG", "ExpireInMinutes": "60" } }


### Variáveis de Ambiente

Para desenvolvimento local, configure no arquivo `appsettings.Development.json`:

{ "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=FcgDb_Dev;Trusted_Connection=True;TrustServerCertificate=True;" }, "JwtSettings": { "SecretKey": "chave-desenvolvimento-local" } }

---

## 🗄️ Banco de Dados

O projeto utiliza **Entity Framework Core** com SQL Server.

### Migrações

Criar nova migração:

dotnet ef migrations add NomeDaMigracao --project FCG.Infrastructure

Aplicar migrações:

dotnet ef database update --project FCG.Infrastructure

Remover última migração:

dotnet ef migrations remove --project FCG.Infrastructure

---

## 📋 Padrões e Convenções

### Nomenclatura

- **Classes**: `PascalCase` (e.g., `UserService`, `GameController`)
- **Métodos**: `PascalCase` (e.g., `GetAllAsync`, `RegisterAsync`)
- **Campos privados**: `_camelCase` com `readonly` (e.g., `_userService`, `_dbContext`)
- **Variáveis locais**: `camelCase` (e.g., `user`, `gameId`)

### Namespaces

Utilize **file-scoped namespaces**:

namespace FCG.Application.Services;
public class UserService { }

### Commits

Siga o padrão:

type(scope): descrição
feat(auth): adicionar serviço JWT fix(users): corrigir validação de email docs(readme): atualizar instruções de instalação

---

## 🧪 Testes

Para executar os testes:

dotnet test

Para testes com cobertura:

dotnet test /p:CollectCoverage=true

---

## 🔒 Segurança

- ✅ Senhas são hashadas com algoritmos modernos
- ✅ JWT com expiração configurável
- ✅ HTTPS obrigatório em produção
- ✅ Validação de entrada em todos os endpoints
- ✅ Authorization por roles (Admin, Player)

### Secrets em Desenvolvimento

Não confirme credenciais no repositório. Use User Secrets:

dotnet user-secrets set "JwtSettings:SecretKey" "sua-chave-secreta"


---

## 📦 Dependências Principais

- **.NET 8**
- **ASP.NET Core 8**
- **Entity Framework Core 8**
- **System.IdentityModel.Tokens.Jwt**
- **Microsoft.AspNetCore.Authentication.JwtBearer**

---

## 📞 Suporte e Contribuição

Para contribuir ao projeto, consulte [CONTRIBUTING.md](CONTRIBUTING.md).

Para dúvidas, abra uma [Issue](https://github.com/seu-usuario/fcg/issues) no repositório.

---

## 📄 Licença

Copyright (c) FIAP Cloud Games. Todos os direitos reservados.

---

## 🔗 Links Úteis

- [Documentação .NET 8](https://learn.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-8)
- [ASP.NET Core Docs](https://learn.microsoft.com/pt-br/aspnet/core/)
- [JWT.io](https://jwt.io/)
- [Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/)

---

**Última atualização**: 05 de maio de 2026
