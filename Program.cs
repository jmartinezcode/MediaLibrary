using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

string choice;
do
{
    // Prompt user
    Console.WriteLine("1) Add Movie");
    Console.WriteLine("2) Display All Movies");
    Console.WriteLine("3) Find Movie");
    Console.WriteLine("Press Enter to quit.");
    choice = Console.ReadLine();
    logger.Info("User choice: {Choice}", choice);

    switch (choice)
    {
        case "1":
            // Add a movie
            var movie = new Movie();

            Console.WriteLine("Enter movie title");
            movie.title = Console.ReadLine();

            movie.genres = ReadGenres();

            Console.WriteLine("Enter movie director");
            movie.director = Console.ReadLine();

            Console.WriteLine("Enter running time (h:mm:ss)");
            movie.runningTime = TimeSpan.Parse(Console.ReadLine());

            // Add the movie to the file
            movieFile.AddMovie(movie);
            break;

        case "2":
            // Display all movies
            foreach (var m in movieFile.Movies)
            {
                Console.WriteLine(m.Display());
            }
            break;
        case "3":
            // Search for movie 
            Console.WriteLine("Enter movie title to search:");
            string userInput = Console.ReadLine();
            string input = userInput.ToLower(); // convert for searching
            // Display result and number of matches
            Console.ForegroundColor = ConsoleColor.Green;
            var titles = movieFile.Movies
                .Where(m => m.title.ToLower().Contains(input))
                .Select(m => m.title);
            Console.WriteLine($"There are {titles.Count()} movies with \"{userInput}\" in the title:");
            foreach (string t in titles)
                Console.WriteLine($"\t{t}");
            Console.ResetColor();
            break;
        default:
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nPlease enter a valid option\n");
            Console.ResetColor();
            break;
    }

} while (!string.IsNullOrEmpty(choice));


logger.Info("Program ended");

static List<string> ReadGenres()
{    
    string genre = "";
    var genres = new List<string>();
    do
    {
        Console.WriteLine("Enter genre (or type 'done' to finish)");
        genre = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(genre) && genre.ToLower() != "done")
        {
            genres.Add(genre);
        }
    } while (genre.ToLower() != "done");
    if (genres.Count == 0)
        genres.Add("(no genres listed)");
    return genres;
}  
