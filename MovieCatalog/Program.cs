using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;

class Program
{
    static void Main()
    {
        DatabaseHelper.Initialize();

        var service = new MovieService();

        while (true)
        {
            Console.WriteLine("\n1. Add Movie");
            Console.WriteLine("2. View Movies");
            Console.WriteLine("3. Add Rating");
            Console.WriteLine("4. User Management");
            Console.WriteLine("5. Delete Movie");
            Console.WriteLine("6. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Title: ");
                    string title = Console.ReadLine() ?? "";

                    Console.Write("Year: ");
                    int year = int.Parse(Console.ReadLine() ?? "");

                    service.ShowGenres();
                    Console.Write("GenreId: ");
                    int genre = int.Parse(Console.ReadLine() ?? "");

                    service.AddMovie(title, year, genre);
                    break;

                case "2":
                    ShowViewMenu(service);
                    break;

                case "3":
                    Console.WriteLine("\n--- Add Rating ---");

                    Console.Write("Movie ID: ");
                    int movieId = int.Parse(Console.ReadLine() ?? "0");

                    service.ShowUsers();
                    Console.Write("User ID: ");
                    int userId = int.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Score (1-10): ");
                    int score = int.Parse(Console.ReadLine() ?? "0");

                    service.AddRating(movieId, userId, score);
                    break;

                case "4":
                    service.ShowUserMenu(service);
                    break;

                case "5":
                    Console.Write("Movie ID: ");
                    int id = int.Parse(Console.ReadLine() ?? "");
                    service.DeleteMovie(id);
                    break;

                case "6":
                    return;
            }
        }
    }
    static void ShowViewMenu(MovieService service)
    {
        while (true)
        {
            Console.WriteLine("\n--- View Menu ---");
            Console.WriteLine("1. View All Movies");
            Console.WriteLine("2. Movies by Genre");
            Console.WriteLine("3. Average Ratings");
            Console.WriteLine("4. Top Rated Movies");
            Console.WriteLine("5. Search Movie");
            Console.WriteLine("6. Back");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    service.GetAllMovies();
                    break;

                case "2":
                    service.GetAllGenres();

                    Console.Write("\nEnter Genre ID: ");
                    int genreId = int.Parse(Console.ReadLine() ?? "0");

                    service.GetMoviesByGenre(genreId);
                    break;

                case "3":
                    service.GetAverageRatings();
                    break;

                case "4":
                    service.GetTopRatedMovies();
                    break;

                case "5":
                    Console.Write("Search keyword: ");
                    string keyword = Console.ReadLine() ?? "";
                    service.SearchMovies(keyword);
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
