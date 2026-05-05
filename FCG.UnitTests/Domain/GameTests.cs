// Copyright (c) FIAP Cloud Games. All rights reserved.

using FCG.Domain.Entities;
using FCG.Domain.Enums;
using Xunit;

namespace FCG.UnitTests.Domain;

public class GameTests
{
    private Game _game;
    private const string ValidTitle = "The Legend of Zelda";
    private const string ValidDescription = "An epic adventure game";
    private const string ValidDeveloper = "Nintendo";
    private const decimal ValidPrice = 59.99m;
    private static readonly GameGenre ValidGenre = GameGenre.Adventure;
    private static readonly DateTime ValidReleaseDate = new(2023, 5, 15);

    public GameTests()
    {
        _game = new Game(
            ValidTitle,
            ValidDescription,
            ValidPrice,
            ValidGenre,
            ValidDeveloper,
            ValidReleaseDate
        );
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_CreatesGameSuccessfully()
    {
        // Assert
        Assert.NotEqual(Guid.Empty, _game.Id);
        Assert.Equal(ValidTitle, _game.Title);
        Assert.Equal(ValidDescription, _game.Description);
        Assert.Equal(ValidPrice, _game.Price);
        Assert.Equal(ValidGenre, _game.Genre);
        Assert.Equal(ValidDeveloper, _game.Developer);
        Assert.Equal(ValidReleaseDate, _game.ReleaseDate);
        Assert.True(_game.IsActive);
    }

    [Fact]
    public void Constructor_WithValidParameters_GeneratesUniqueId()
    {
        // Arrange
        var game1 = new Game(ValidTitle, ValidDescription, ValidPrice, ValidGenre, ValidDeveloper, ValidReleaseDate);
        var game2 = new Game(ValidTitle, ValidDescription, ValidPrice, ValidGenre, ValidDeveloper, ValidReleaseDate);

        // Assert
        Assert.NotEqual(game1.Id, game2.Id);
    }

    #endregion

    #region SetTitle Tests

    [Fact]
    public void SetTitle_WithValidTitle_UpdatesSuccessfully()
    {
        // Arrange
        var newTitle = "New Game Title";

        // Act
        _game.SetTitle(newTitle);

        // Assert
        Assert.Equal(newTitle, _game.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetTitle_WithNullOrEmptyTitle_ThrowsArgumentException(string invalidTitle)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _game.SetTitle(invalidTitle));
    }

    [Fact]
    public void SetTitle_WithTitleHavingWhitespace_TrimsTitleSuccessfully()
    {
        // Arrange
        var titleWithWhitespace = "  New Game Title  ";
        var expectedTitle = "New Game Title";

        // Act
        _game.SetTitle(titleWithWhitespace);

        // Assert
        Assert.Equal(expectedTitle, _game.Title);
    }

    #endregion

    #region SetDescription Tests

    [Fact]
    public void SetDescription_WithValidDescription_UpdatesSuccessfully()
    {
        // Arrange
        var newDescription = "Updated description";

        // Act
        _game.SetDescription(newDescription);

        // Assert
        Assert.Equal(newDescription, _game.Description);
    }

    [Fact]
    public void SetDescription_WithDescriptionHavingWhitespace_TrimsDescriptionSuccessfully()
    {
        // Arrange
        var descriptionWithWhitespace = "  New Description  ";
        var expectedDescription = "New Description";

        // Act
        _game.SetDescription(descriptionWithWhitespace);

        // Assert
        Assert.Equal(expectedDescription, _game.Description);
    }

    #endregion

    #region SetPrice Tests

    [Theory]
    [InlineData(0)]
    [InlineData(9.99)]
    [InlineData(99.99)]
    [InlineData(199.99)]
    public void SetPrice_WithValidPrice_UpdatesSuccessfully(decimal price)
    {
        // Act
        _game.SetPrice(price);

        // Assert
        Assert.Equal(price, _game.Price);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.50)]
    [InlineData(-0.01)]
    public void SetPrice_WithNegativePrice_ThrowsArgumentException(decimal negativePrice)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _game.SetPrice(negativePrice));
    }

    #endregion

    #region SetGenre Tests

    [Theory]
    [InlineData(GameGenre.Action)]
    [InlineData(GameGenre.Adventure)]
    [InlineData(GameGenre.RPG)]
    [InlineData(GameGenre.Puzzle)]
    [InlineData(GameGenre.Sports)]
    public void SetGenre_WithValidGenre_UpdatesSuccessfully(GameGenre genre)
    {
        // Act
        _game.SetGenre(genre);

        // Assert
        Assert.Equal(genre, _game.Genre);
    }

    #endregion

    #region SetDeveloper Tests

    [Fact]
    public void SetDeveloper_WithValidDeveloper_UpdatesSuccessfully()
    {
        // Arrange
        var newDeveloper = "Rockstar Games";

        // Act
        _game.SetDeveloper(newDeveloper);

        // Assert
        Assert.Equal(newDeveloper, _game.Developer);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetDeveloper_WithNullOrEmptyDeveloper_ThrowsArgumentException(string invalidDeveloper)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _game.SetDeveloper(invalidDeveloper));
    }

    [Fact]
    public void SetDeveloper_WithDeveloperHavingWhitespace_TrimsDeveloperSuccessfully()
    {
        // Arrange
        var developerWithWhitespace = "  EA Games  ";
        var expectedDeveloper = "EA Games";

        // Act
        _game.SetDeveloper(developerWithWhitespace);

        // Assert
        Assert.Equal(expectedDeveloper, _game.Developer);
    }

    #endregion

    #region Deactivate Tests

    [Fact]
    public void Deactivate_WhenCalled_DeactivatesGameSuccessfully()
    {
        // Act
        _game.Deactivate();

        // Assert
        Assert.False(_game.IsActive);
    }

    [Fact]
    public void Deactivate_MultipleTimesOnSameGame_RemainsInactive()
    {
        // Act
        _game.Deactivate();
        _game.Deactivate();

        // Assert
        Assert.False(_game.IsActive);
    }

    #endregion

    #region Activate Tests

    [Fact]
    public void Activate_WhenCalled_ActivatesGameSuccessfully()
    {
        // Arrange
        _game.Deactivate();

        // Act
        _game.Activate();

        // Assert
        Assert.True(_game.IsActive);
    }

    [Fact]
    public void Activate_WhenCalled_UpdatesTimestamp()
    {
        // Arrange
        _game.Deactivate();
        var originalUpdatedAt = _game.GetType().GetProperty("UpdatedAt").GetValue(_game);

        // Act
        System.Threading.Thread.Sleep(10);
        _game.Activate();

        // Assert
        var newUpdatedAt = _game.GetType().GetProperty("UpdatedAt").GetValue(_game);
        Assert.True((DateTime?)newUpdatedAt > (DateTime?)originalUpdatedAt);
    }

    [Fact]
    public void Activate_OnAlreadyActiveGame_RemainsActive()
    {
        // Act
        _game.Activate();

        // Assert
        Assert.True(_game.IsActive);
    }

    #endregion

    #region State Transition Tests

    [Fact]
    public void StateTransition_DeactivateThenActivate_WorksCorrectly()
    {
        // Arrange
        Assert.True(_game.IsActive);

        // Act
        _game.Deactivate();
        Assert.False(_game.IsActive);

        _game.Activate();

        // Assert
        Assert.True(_game.IsActive);
    }

    [Fact]
    public void StateTransition_MultiplePropertyUpdates_MaintainsConsistency()
    {
        // Act
        _game.SetTitle("Updated Title");
        _game.SetPrice(39.99m);
        _game.SetGenre(GameGenre.RPG);

        // Assert
        Assert.Equal("Updated Title", _game.Title);
        Assert.Equal(39.99m, _game.Price);
        Assert.Equal(GameGenre.RPG, _game.Genre);
        Assert.True(_game.IsActive);
    }

    #endregion

    #region Invariant Tests

    [Fact]
    public void Game_AfterCreation_HasValidId()
    {
        // Assert
        Assert.NotEqual(Guid.Empty, _game.Id);
        Assert.IsType<Guid>(_game.Id);
    }

    [Fact]
    public void Game_AllPropertiesAreReadable()
    {
        // Assert
        Assert.NotNull(_game.Title);
        Assert.NotNull(_game.Description);
        Assert.NotNull(_game.Developer);
        Assert.True(_game.Price >= 0);
        Assert.NotNull(_game.Genre);
        Assert.NotEqual(default(DateTime), _game.ReleaseDate);
    }

    #endregion
}
