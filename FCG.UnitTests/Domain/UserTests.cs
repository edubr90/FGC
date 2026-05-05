// Copyright (c) FIAP Cloud Games. All rights reserved.

using FCG.Domain.Entities;
using FCG.Domain.Enums;
using Xunit;

namespace FCG.UnitTests.Domain;

public class UserTests
{
    private User _user;
    private const string ValidName = "John Doe";
    private const string ValidEmail = "john@example.com";
    private const string ValidPassword = "hashedPassword123";

    public UserTests()
    {
        _user = new User(ValidName, ValidEmail, ValidPassword);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_CreatesUserSuccessfully()
    {
        _user.PromoteToAdmin();
        _user.Activate();

        // Assert
        Assert.NotEqual(Guid.Empty, _user.Id);
        Assert.Equal(ValidName, _user.Name);
        Assert.Equal(ValidEmail.ToLowerInvariant(), _user.Email);
        Assert.Equal(ValidPassword, _user.PasswordHash);
        Assert.Equal(UserRole.Admin, _user.Role);
        Assert.True(_user.IsActive);
        Assert.Empty(_user.Library);
    }

    [Fact]
    public void Constructor_WithValidParameters_GeneratesUniqueId()
    {
        // Arrange
        var user1 = new User(ValidName, ValidEmail, ValidPassword);
        var user2 = new User(ValidName, ValidEmail, ValidPassword);

        // Assert
        Assert.NotEqual(user1.Id, user2.Id);
    }

    #endregion

    #region SetName Tests

    [Fact]
    public void SetName_WithValidName_UpdatesSuccessfully()
    {
        // Arrange
        var newName = "Jane Doe";

        // Act
        _user.SetName(newName);

        // Assert
        Assert.Equal(newName, _user.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetName_WithNullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.SetName(invalidName));
    }

    [Fact]
    public void SetName_WithNameHavingWhitespace_TrimsNameSuccessfully()
    {
        // Arrange
        var nameWithWhitespace = "  Jane Doe  ";
        var expectedName = "Jane Doe";

        // Act
        _user.SetName(nameWithWhitespace);

        // Assert
        Assert.Equal(expectedName, _user.Name);
    }

    #endregion

    #region SetEmail Tests

    [Fact]
    public void SetEmail_WithValidEmail_UpdatesSuccessfully()
    {
        // Arrange
        var newEmail = "jane@example.com";

        // Act
        _user.SetEmail(newEmail);

        // Assert
        Assert.Equal(newEmail.ToLowerInvariant(), _user.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetEmail_WithNullOrEmptyEmail_ThrowsArgumentException(string invalidEmail)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.SetEmail(invalidEmail));
    }

    [Theory]
    [InlineData("invalidemail")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    [InlineData("invalid email@test.com")]
    public void SetEmail_WithInvalidEmailFormat_ThrowsArgumentException(string invalidEmail)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.SetEmail(invalidEmail));
    }

    [Fact]
    public void SetEmail_WithValidEmail_StoredAsLowercase()
    {
        // Arrange
        var email = "John.Doe@Example.COM";

        // Act
        _user.SetEmail(email);

        // Assert
        Assert.Equal(email.ToLowerInvariant(), _user.Email);
    }

    [Fact]
    public void SetEmail_WithEmailHavingWhitespace_TrimsEmailSuccessfully()
    {
        // Arrange
        var emailWithWhitespace = "  jane@example.com  ";
        var expectedEmail = "jane@example.com";

        // Act
        _user.SetEmail(emailWithWhitespace);

        // Assert
        Assert.Equal(expectedEmail, _user.Email);
    }

    #endregion

    #region UpdatePassword Tests

    [Fact]
    public void UpdatePassword_WithValidPassword_UpdatesSuccessfully()
    {
        // Arrange
        var newPassword = "newHashedPassword456";

        // Act
        _user.UpdatePassword(newPassword);

        // Assert
        Assert.Equal(newPassword, _user.PasswordHash);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdatePassword_WithNullOrEmptyPassword_ThrowsArgumentException(string invalidPassword)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.UpdatePassword(invalidPassword));
    }

    #endregion

    #region Role Management Tests

    [Fact]
    public void PromoteToAdmin_WhenCalled_ChangesRoleToAdmin()
    {
        // Act
        _user.PromoteToAdmin();

        // Assert
        Assert.Equal(UserRole.Admin, _user.Role);
    }

    [Fact]
    public void DemoteToUser_WhenCalled_ChangesRoleToUser()
    {
        // Arrange
        _user.PromoteToAdmin();

        // Act
        _user.DemoteToUser();

        // Assert
        Assert.Equal(UserRole.User, _user.Role);
    }

    #endregion

    #region Deactivate/Activate Tests

    [Fact]
    public void Deactivate_WhenCalled_DeactivatesUserSuccessfully()
    {
        // Act
        _user.Deactivate();

        // Assert
        Assert.False(_user.IsActive);
    }

    [Fact]
    public void Activate_WhenCalled_ActivatesUserSuccessfully()
    {
        // Arrange
        _user.Deactivate();

        // Act
        _user.Activate();

        // Assert
        Assert.True(_user.IsActive);
    }

    [Fact]
    public void Deactivate_MultipleTimesOnSameUser_RemainsInactive()
    {
        // Act
        _user.Deactivate();
        _user.Deactivate();

        // Assert
        Assert.False(_user.IsActive);
    }

    #endregion

    #region AddGame Tests

    [Fact]
    public void AddGame_WithValidGame_AddsGameToLibrarySuccessfully()
    {
        // Arrange
        var game = new Game("Test Game", "Description", 59.99m, GameGenre.Action, "Developer", DateTime.UtcNow);

        // Act
        _user.AddGame(game);

        // Assert
        Assert.Single(_user.Library);
    }

    [Fact]
    public void AddGame_WithDuplicateGame_ThrowsInvalidOperationException()
    {
        // Arrange
        var game = new Game("Test Game", "Description", 59.99m, GameGenre.Action, "Developer", DateTime.UtcNow);
        _user.AddGame(game);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _user.AddGame(game));
    }

    [Fact]
    public void AddGame_WithMultipleGames_AddsAllGamesToLibrary()
    {
        // Arrange
        var game1 = new Game("Game 1", "Description 1", 59.99m, GameGenre.Action, "Developer 1", DateTime.UtcNow);
        var game2 = new Game("Game 2", "Description 2", 49.99m, GameGenre.Adventure, "Developer 2", DateTime.UtcNow);

        // Act
        _user.AddGame(game1);
        _user.AddGame(game2);

        // Assert
        Assert.Equal(2, _user.Library.Count);
    }

    #endregion

    #region Invariant Tests

    [Fact]
    public void User_AfterCreation_HasValidId()
    {
        // Assert
        Assert.NotEqual(Guid.Empty, _user.Id);
        Assert.IsType<Guid>(_user.Id);
    }

    [Fact]
    public void User_HasCreatedAtTimestamp()
    {
        // Assert
        Assert.NotEqual(default(DateTime), _user.CreatedAt);
        Assert.True(_user.CreatedAt <= DateTime.UtcNow);
    }

    #endregion
}
