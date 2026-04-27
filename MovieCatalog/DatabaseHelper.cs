using Microsoft.Data.Sqlite;

public static class DatabaseHelper
{
    private const string ConnectionString = "Data Source=moviecatalog.db";
    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }

    public static void Initialize()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS Users (
                UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Genres (
                GenreId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Movies (
                MovieId INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                ReleaseYear INTEGER,
                GenreId INTEGER,
                FOREIGN KEY (GenreId) REFERENCES Genres(GenreId)
            );
            CREATE TABLE IF NOT EXISTS Ratings (
                RatingId INTEGER PRIMARY KEY AUTOINCREMENT,
                MovieId INTEGER,
                UserId INTEGER,
                Score INTEGER,
                FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
                FOREIGN KEY (UserId) REFERENCES Users(UserId)
            );
            CREATE TABLE IF NOT EXISTS Watchlist (
                WatchlistId INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER,
                MovieId INTEGER,
                Status TEXT,
                FOREIGN KEY (UserId) REFERENCES Users(UserId),
                FOREIGN KEY (MovieId) REFERENCES Movies(MovieId)
            );";

        cmd.ExecuteNonQuery();
        SeedData(connection);
    }
    private static void SeedData(SqliteConnection connection)
    {
        using var cmd = connection.CreateCommand();

        cmd.CommandText = "SELECT COUNT(*) FROM Genres";
        object? scalar = cmd.ExecuteScalar();
        long count = 0;
        if (scalar != null && scalar != DBNull.Value)
        {
            count = Convert.ToInt64(scalar);
        }

        if (count > 0)
            return;

        cmd.CommandText = @"
        INSERT INTO Genres (Name) VALUES ('Action');
        INSERT INTO Genres (Name) VALUES ('Comedy');
        INSERT INTO Genres (Name) VALUES ('Drama');
        INSERT INTO Genres (Name) VALUES ('Horror');
        INSERT INTO Genres (Name) VALUES ('Sci-Fi');";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
        INSERT INTO Movies (Title, ReleaseYear, GenreId) VALUES ('Inception', 2010, 5);
        INSERT INTO Movies (Title, ReleaseYear, GenreId) VALUES ('The Dark Knight', 2008, 1);
        INSERT INTO Movies (Title, ReleaseYear, GenreId) VALUES ('Superbad', 2007, 2);
        INSERT INTO Movies (Title, ReleaseYear, GenreId) VALUES ('The Conjuring', 2013, 4);
        INSERT INTO Movies (Title, ReleaseYear, GenreId) VALUES ('Interstellar', 2014, 5);";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
        INSERT INTO Users (Username, Password) VALUES ('alice', '123');
        INSERT INTO Users (Username, Password) VALUES ('bob', '123');";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
        INSERT INTO Ratings (MovieId, UserId, Score) VALUES (1, 1, 9);
        INSERT INTO Ratings (MovieId, UserId, Score) VALUES (1, 2, 8);
        INSERT INTO Ratings (MovieId, UserId, Score) VALUES (2, 1, 10);
        INSERT INTO Ratings (MovieId, UserId, Score) VALUES (3, 2, 7);";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
        INSERT INTO Watchlist (UserId, MovieId, Status) VALUES (1, 1, 'Completed');
        INSERT INTO Watchlist (UserId, MovieId, Status) VALUES (1, 3, 'Watching');
        INSERT INTO Watchlist (UserId, MovieId, Status) VALUES (2, 2, 'Planned');";
        cmd.ExecuteNonQuery();
    }
}