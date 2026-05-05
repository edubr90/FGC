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
public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;

    public GameService(IGameRepository gameRepository, IUserRepository userRepository, IUnitOfWork uow)
    {
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _uow = uow;
    }

    public async Task AcquireGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        var game = await _gameRepository.GetByIdAsync(gameId, cancellationToken)
            ?? throw new KeyNotFoundException("Game not found.");

        if (!game.IsActive)
            throw new InvalidOperationException("Game is not available for acquisition.");

        user.AddGame(game);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
    }

    public async Task<GameResponse> CreateAsync(CreateGameRequest request, CancellationToken cancellationToken = default)
    {
        var game = new Game(
            request.Title,
            request.Description,
            request.Price,
            request.Genre,
            request.Developer,
            request.ReleaseDate
        );

        await _gameRepository.AddAsync(game, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
        return MapToResponse(game);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Game not found.");

        game.Deactivate();
        await _gameRepository.UpdateAsync(game, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
    }

    public async Task<IEnumerable<GameResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var games = await _gameRepository.GetAllAsync(cancellationToken);
        return games.Select(MapToResponse);
    }

    public async Task<GameResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Game not found.");

        return MapToResponse(game);
    }

    public async Task<IEnumerable<UserGameResponse>> GetUserLibraryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        return user.Library.Select(ug => new UserGameResponse(
            ug.GameId,
            ug.Game.Title ?? string.Empty,
            ug.Game.Description ?? string.Empty,
            ug.Game.Price,
            ug.Game.Genre,
            ug.Game.Developer ?? string.Empty,
            ug.Game.ReleaseDate,
            ug.AcquiredAt
        ));
    }

    public async Task<GameResponse> UpdateAsync(Guid id, UpdateGameRequest request, CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Game not found.");

        if (!string.IsNullOrEmpty(request.Title))
            game.SetTitle(request.Title);

        if (!string.IsNullOrEmpty(request.Description))
            game.SetDescription(request.Description);

        if (request.Price.HasValue)
            game.SetPrice(request.Price.Value);

        if (request.Genre.HasValue)
            game.SetGenre(request.Genre.Value);

        if (!string.IsNullOrEmpty(request.Developer))
            game.SetDeveloper(request.Developer);

        await _gameRepository.UpdateAsync(game, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
        return MapToResponse(game);
    }

    private static GameResponse MapToResponse(Game game)
    {
        return new GameResponse(
            game.Id,
            game.Title,
            game.Description,
            game.Price,
            game.Genre,
            game.Developer,
            game.ReleaseDate,
            game.IsActive
        );
    }
}
