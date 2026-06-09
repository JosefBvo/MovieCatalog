using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;

public class MovieService
{
    public void AddMovie(string title, int year, int genreId)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Movies (Title, ReleaseYear, GenreId)
                            VALUES ($title, $year, $genreId)";
        cmd.Parameters.AddWithValue("$title", title);
        cmd.Parameters.AddWithValue("$year", year);
        cmd.Parameters.AddWithValue("$genreId", genreId);

        cmd.ExecuteNonQuery();
    }

    public void GetAllMovies()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT m.MovieId, m.Title, m.ReleaseYear, g.Name 
                            FROM Movies m
                            LEFT JOIN Genres g ON m.GenreId = g.GenreId";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Title"]} ({reader["ReleaseYear"]}) - {reader["Name"]}");
        }
    }

    public void DeleteMovie(int id)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Movies WHERE MovieId = $id";
        cmd.Parameters.AddWithValue("$id", id);

        cmd.ExecuteNonQuery();
    }
    public void ShowUserMenu(MovieService service)
    {
        while (true)
        {
            Console.WriteLine("\n--- User Menu ---");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. View Users");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Delete User");
            Console.WriteLine("5. Back");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Username: ");
                    string username = Console.ReadLine() ?? "";

                    Console.Write("Password: ");
                    string password = Console.ReadLine() ?? "";

                    service.AddUser(username, password);
                    break;

                case "2":
                    service.GetAllUsers();
                    break;

                case "3":
                    Console.Write("User ID: ");
                    int id = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("New Username: ");
                    string newUsername = Console.ReadLine() ?? "";

                    service.UpdateUser(id, newUsername);
                    break;

                case "4":
                    Console.Write("User ID: ");
                    int deleteId = int.Parse(Console.ReadLine() ?? "0");

                    service.DeleteUser(deleteId);
                    break;

                case "5":
                    return;
            }
        }
    }
    public void AddUser(string username, string password)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO Users (Username, Password)
        VALUES ($username, $password)";

        cmd.Parameters.AddWithValue("$username", username);
        cmd.Parameters.AddWithValue("$password", password);

        cmd.ExecuteNonQuery();

        Console.WriteLine("User added successfully!");
    }
    public void UpdateUser(int userId, string newUsername)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        UPDATE Users
        SET Username = $username
        WHERE UserId = $id";

        cmd.Parameters.AddWithValue("$username", newUsername);
        cmd.Parameters.AddWithValue("$id", userId);

        cmd.ExecuteNonQuery();

        Console.WriteLine("User updated successfully!");
    }
    public void DeleteUser(int userId)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Users WHERE UserId = $id";

        cmd.Parameters.AddWithValue("$id", userId);

        cmd.ExecuteNonQuery();

        Console.WriteLine("User deleted successfully!");
    }
    public void AddRating(int movieId, int userId, int score)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO Ratings (MovieId, UserId, Score)
        VALUES ($movieId, $userId, $score)";

        cmd.Parameters.AddWithValue("$movieId", movieId);
        cmd.Parameters.AddWithValue("$userId", userId);
        cmd.Parameters.AddWithValue("$score", score);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Rating added successfully!");
    }
    public void GetAllUsers()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT UserId, Username FROM Users";

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Users ---");

        while (reader.Read())
        {
            Console.WriteLine($"{reader["UserId"]}: {reader["Username"]}");
        }
    }
    public void ShowGenres()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT GenreId, Name FROM Genres";

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Available Genres ---");

        while (reader.Read())
        {
            Console.WriteLine($"{reader["GenreId"]}: {reader["Name"]}");
        }
    }
    public void ShowUsers()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT UserId, Username FROM Users";

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Users ---");

        while (reader.Read())
        {
            Console.WriteLine($"{reader["UserId"]}: {reader["Username"]}");
        }
    }
    public void GetAllGenres()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT GenreId, Name FROM Genres";

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Genres ---");

        while (reader.Read())
        {
            Console.WriteLine($"{reader["GenreId"]}. {reader["Name"]}");
        }
    }

    //Query Functions
    public void GetMoviesByGenre(int genreId)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT m.Title, m.ReleaseYear, g.Name
        FROM Movies m
        JOIN Genres g ON m.GenreId = g.GenreId
        WHERE m.GenreId = $id";

        cmd.Parameters.AddWithValue("$id", genreId);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Movies ---");

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Title"]} ({reader["ReleaseYear"]}) - {reader["Name"]}");
        }
    }
    public void GetAverageRatings()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT m.Title, AVG(r.Score) AS AvgRating
        FROM Movies m
        LEFT JOIN Ratings r ON m.MovieId = r.MovieId
        GROUP BY m.MovieId";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Title"]} - Avg Rating: {reader["AvgRating"]}");
        }
    }
    public void GetWatchlist()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT u.Username, m.Title, w.Status
        FROM Watchlist w
        JOIN Users u ON w.UserId = u.UserId
        JOIN Movies m ON w.MovieId = m.MovieId";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Username"]} - {reader["Title"]} ({reader["Status"]})");
        }
    }
    public void GetTopRatedMovies()
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT m.Title, AVG(r.Score) AS Rating
        FROM Movies m
        JOIN Ratings r ON m.MovieId = r.MovieId
        GROUP BY m.MovieId
        ORDER BY Rating DESC
        LIMIT 5";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Title"]} - {reader["Rating"]}");
        }
    }
    public void SearchMovies(string keyword)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT Title, ReleaseYear
        FROM Movies
        WHERE Title LIKE $keyword";

        cmd.Parameters.AddWithValue("$keyword", "%" + keyword + "%");

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Title"]} ({reader["ReleaseYear"]})");
        }
    }
}
