using System;
using System.IO;
using Xunit;
using Microsoft.Data.Sqlite;
// Ensure you have a 'using' statement for your main project's namespace if needed

namespace MovieCatalog.Tests;

public class MovieServiceTests : IDisposable
{
    private readonly MovieService _service;
    private readonly string _testDbFile;

    public MovieServiceTests()
    {
        _testDbFile = $"test_movies_{Guid.NewGuid()}.db";
        DatabaseHelper.ConnectionString = $"Data Source={_testDbFile}";
        DatabaseHelper.Initialize();

        _service = new MovieService();
    }

    [Fact]
    public void GivenValidUserDetails_WhenAddUserIsCalled_ThenUserIsSavedToDatabase()
    {
        // Given
        string testUser = "ci_test_user";
        string testPass = "securepassword";

        // When
        _service.AddUser(testUser, testPass);

        // Then
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = $username";
        cmd.Parameters.AddWithValue("$username", testUser);

        var count = Convert.ToInt32(cmd.ExecuteScalar());

        Assert.Equal(1, count);
    }

    [Fact]
    public void GivenValidMovieDetails_WhenAddMovieIsCalled_ThenMovieIsSavedToDatabase()
    {
        // Given
        string testTitle = "GitHub Actions: The Movie";
        int testYear = 2026;
        int testGenreId = 1;

        // When
        _service.AddMovie(testTitle, testYear, testGenreId);

        // Then
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Movies WHERE Title = $title";
        cmd.Parameters.AddWithValue("$title", testTitle);

        var count = Convert.ToInt32(cmd.ExecuteScalar());

        Assert.Equal(1, count);
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        if (File.Exists(_testDbFile))
        {
            File.Delete(_testDbFile);
        }
    }
}