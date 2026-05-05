// Copyright (c) FIAP Cloud Games. All rights reserved.

using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Interfaces;
using Moq;
using Xunit;

namespace FCG.UnitTests.Application;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _mockGameRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GameService _gameService;

    private readonly Guid _gameId;
    private readonly Guid _userId;
    private readonly Game _game;
    private readonly User _user;

    public GameServiceTests()
    {
        _mockGameRepository = new Mock<IGameRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _gameService = new GameService(
            _mockGameRepository.Object,
            _mockUserRepository.Object,
            _mockUnitOfWork.Object
        );

        _gameId = Guid.NewGuid();
        _userId = Guid.NewGuid();

        _game = new Game(
            "Test Game",
            "Test Description",
            59.99m,
            GameGenre.Action,
            "Test Developer",
            DateTime.UtcNow
        );

        _user = new User(
            "Test User",
            "test@example.com",
            "hashedPassword"
        );
    }

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesGameSuccessfully()
    {
        // Arrange
        var request = new CreateGameRequest(
            "New Game",
            "New Description",
            49.99m,
            GameGenre.Adventure,
            "New Developer",
            DateTime.UtcNow
        );

        _mockGameRepository.Setup(r => r.AddAsync(It.IsAny<Game>(), default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _gameService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Game", result.Title);
        Assert.Equal(49.99m, result.Price);
        Assert.Equal(GameGenre.Adventure, result.Genre);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CallsAddAsyncOnRepository()
    {
        // Arrange
        var request = new CreateGameRequest(
            "New Game",
            "New Description",
            49.99m,
            GameGenre.Adventure,
            "New Developer",
            DateTime.UtcNow
        );

        _mockGameRepository.Setup(r => r.AddAsync(It.IsAny<Game>(), default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        await _gameService.CreateAsync(request);

        // Assert
        _mockGameRepository.Verify(
            r => r.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CallsCommitAsync()
    {
        // Arrange
        var request = new CreateGameRequest(
            "New Game",
            "New Description",
            49.99m,
            GameGenre.Adventure,
            "New Developer",
            DateTime.UtcNow
        );

        _mockGameRepository.Setup(r => r.AddAsync(It.IsAny<Game>(), default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        await _gameService.CreateAsync(request);

        // Assert
        _mockUnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithExistingGame_ReturnsGameSuccessfully()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_game);

        // Act
        var result = await _gameService.GetByIdAsync(_gameId);  

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_game.Title, result.Title);
        Assert.Equal(_game.Price, result.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentGame_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _gameService.GetByIdAsync(_gameId)
        );
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryWithCorrectId()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_game);

        // Act
        await _gameService.GetByIdAsync(_gameId);

        // Assert
        _mockGameRepository.Verify(
            r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_WithMultipleGames_ReturnsAllGames()
    {
        // Arrange
        var games = new List<Game>
        {
            _game,
            new Game("Game 2", "Description 2", 39.99m, GameGenre.RPG, "Developer 2", DateTime.UtcNow),
            new Game("Game 3", "Description 3", 29.99m, GameGenre.Puzzle, "Developer 3", DateTime.UtcNow)
        };

        _mockGameRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(games);

        // Act
        var result = await _gameService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithNoGames_ReturnsEmptyCollection()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Game>());

        // Act
        var result = await _gameService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepositoryGetAllAsync()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Game>());

        // Act
        await _gameService.GetAllAsync();

        // Assert
        _mockGameRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesGameSuccessfully()
    {
        // Arrange
        var updateRequest = new UpdateGameRequest(
            "Updated Title",
            "Updated Description",
            29.99m,
            GameGenre.RPG,
            "Updated Developer",
            null
        );

        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, default))
            .ReturnsAsync(_game);
        _mockGameRepository.Setup(r => r.UpdateAsync(_game, default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _gameService.UpdateAsync(_gameId, updateRequest);

        // Assert
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal(29.99m, result.Price);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentGame_ThrowsKeyNotFoundException()
    {
        // Arrange
        var updateRequest = new UpdateGameRequest(null, null, null, null, null, null);

        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _gameService.UpdateAsync(_gameId, updateRequest)
        );
    }

    [Fact]
    public async Task UpdateAsync_WithPartialUpdate_UpdatesOnlyProvidedFields()
    {
        // Arrange
        var originalPrice = _game.Price;
        var updateRequest = new UpdateGameRequest(
            "New Title",
            null,
            null,
            null,
            null,
            null
        );

        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, default))
            .ReturnsAsync(_game);
        _mockGameRepository.Setup(r => r.UpdateAsync(_game, default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _gameService.UpdateAsync(_gameId, updateRequest);

        // Assert
        Assert.Equal("New Title", result.Title);
        Assert.Equal(originalPrice, result.Price);
    }

    [Fact]
    public async Task UpdateAsync_CallsUpdateAsyncOnRepository()
    {
        // Arrange
        var updateRequest = new UpdateGameRequest(
            "Updated Title",
            null,
            null,
            null,
            null,
            null
        );

        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, default))
            .ReturnsAsync(_game);
        _mockGameRepository.Setup(r => r.UpdateAsync(_game, default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        await _gameService.UpdateAsync(_gameId, updateRequest);

        // Assert
        _mockGameRepository.Verify(
            r => r.UpdateAsync(_game, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WithExistingGame_DeactivatesGameSuccessfully()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, default))
            .ReturnsAsync(_game);
        _mockGameRepository.Setup(r => r.UpdateAsync(_game, default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        await _gameService.DeleteAsync(_gameId);

        // Assert
        Assert.False(_game.IsActive);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentGame_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _gameService.DeleteAsync(_gameId)
        );
    }

    [Fact]
    public async Task DeleteAsync_CallsUpdateAndCommit()
    {
        // Arrange
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, default))
            .ReturnsAsync(_game);
        _mockGameRepository.Setup(r => r.UpdateAsync(_game, default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        await _gameService.DeleteAsync(_gameId);

        // Assert
        _mockGameRepository.Verify(
            r => r.UpdateAsync(_game, It.IsAny<CancellationToken>()),
            Times.Once
        );
        _mockUnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region AcquireGameAsync Tests

    [Fact]
    public async Task AcquireGameAsync_WithValidUserAndGame_AddGameToUserLibrary()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, default))
            .ReturnsAsync(_user);
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, default))
            .ReturnsAsync(_game);
        _mockUserRepository.Setup(r => r.UpdateAsync(_user, default))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CommitAsync(default))
            .ReturnsAsync(1);

        // Act
        await _gameService.AcquireGameAsync(_userId, _gameId);

        // Assert
        _mockUserRepository.Verify(
            r => r.UpdateAsync(_user, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task AcquireGameAsync_WithNonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _gameService.AcquireGameAsync(_userId, _gameId)
        );
    }

    [Fact]
    public async Task AcquireGameAsync_WithNonExistentGame_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _gameService.AcquireGameAsync(_userId, _gameId)
        );
    }

    [Fact]
    public async Task AcquireGameAsync_WithInactiveGame_ThrowsInvalidOperationException()
    {
        // Arrange
        _game.Deactivate();

        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);
        _mockGameRepository.Setup(r => r.GetByIdAsync(_gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_game);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _gameService.AcquireGameAsync(_userId, _gameId)
        );
    }

    #endregion

    #region GetUserLibraryAsync Tests

    [Fact]
    public async Task GetUserLibraryAsync_WithUserHavingGames_ReturnsUserLibrary()
    {
        // Arrange
        _user.AddGame(_game);

        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, default))
            .ReturnsAsync(_user);

        // Act
        var result = await _gameService.GetUserLibraryAsync(_userId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetUserLibraryAsync_WithUserHavingNoGames_ReturnsEmptyCollection()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);

        // Act
        var result = await _gameService.GetUserLibraryAsync(_userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserLibraryAsync_WithNonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _gameService.GetUserLibraryAsync(_userId)
        );
    }

    #endregion
}
