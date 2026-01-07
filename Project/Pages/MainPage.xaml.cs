using Project.Models;
using Project.Services;
namespace Project.Pages
{
    public partial class MainPage : ContentPage
    {
        private DataService DataService;
        private List<Movie> allMovies;
        private UserData userData;

        public MainPage()
        {
            InitializeComponent();
            DataService = new DataService();
            allMovies = new List<Movie>();
        }

        //runs when page appears
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDataAsync();
        }

        //load all the data
        private async Task LoadDataAsync()
        {
            userData = await DataService.LoadUserDataAsync();

            //check if user needs to enter their name
            if (string.IsNullOrWhiteSpace(userData.UserName))
            {
                string name = await DisplayPromptAsync("Welcome to Movie Explorer!", "Please enter your name:");
                if (!string.IsNullOrWhiteSpace(name))
                {
                    userData.UserName = name;
                    await DataService.SaveUserDataAsync(userData);
                }
            }

            WelcomeLabel.Text = $"Welcome, {userData.UserName}!";

            //load movies
            allMovies = await DataService.GetMoviesAsync();
            MoviesList.ItemsSource = allMovies;

            //setup genre picker
            var genres = allMovies.SelectMany(m => m.Genre).Distinct().OrderBy(g => g).ToList();
            genres.Insert(0, "All Genres");
            GenrePicker.ItemsSource = genres;
            GenrePicker.SelectedIndex = 0;

        }

        // Filter movies when search changes
        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMovies();
        }

        //filter movies when genre changes
        private void GenrePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterMovies();
        }

        //filters the movie list based on search and genre
        private void FilterMovies()
        {
            var filtered = allMovies.AsEnumerable();

            string searchText = SearchEntry.Text?.ToLower() ?? "";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(m => m.Title.ToLower().Contains(searchText));
            }

            //filter by genre
            if (GenrePicker.SelectedIndex > 0)
            {
                string selectedGenre = GenrePicker.SelectedItem?.ToString() ?? "";
                filtered = filtered.Where(m => m.Genre.Contains(selectedGenre));
            }


            MoviesList.ItemsSource = filtered.ToList();


        }

        //when a movie is selected, show options
        private async void MoviesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
            {
                return;
            }

            var movie = e.CurrentSelection[0] as Movie;
            if (movie != null) return;

            //show movie details and options
            string action = await DisplayActionSheet($"{movie.Emoji}{movie.Title} ({movie.Year})\nDirector: {movie.Director}\nIMDB: {movie.ImdbRating}\nGenres: {movie.GenreString}",
                "Close", null, "Add to Favorites", "Mark as Watched");

            if (action == "Add to Favourites")
            {
                //checking if already in favourites
                bool alreadyFav = userData.Favourites.Any(f => f.Title == movie.Title && f.Year == movie.Year);
                if (alreadyFav)
                {
                    await DisplayAlert("Info", "This movie is already in your favourites.", "OK");
                }
                else
                {
                    var fav = new FavouriteMovie
                    {
                        Title = movie.Title,
                        Year = movie.Year,
                        Genre = movie.Genre,
                        Emoji = movie.Emoji,
                        AddedDate = DateTime.Now
                    };

                    userData.Favourites.Add(fav);
                    await DataService.SaveUserDataAsync(userData);
                    await DisplayAlert("Success", "Movie added to favourites!", "OK");
                }




            }

            else if (action == "Mark as Watched")
            {
                //checking if already marked as watched
                bool alreadyWatched = userData.ViewHistory.Any(v => v.Title == movie.Title && v.Year == movie.Year);
                if (alreadyWatched)
                {
                    await DisplayAlert("Info", "You have already marked this movie as watched.", "OK");
                }
                else
                {
                    var watched = new ViewedMovie
                    {
                        Title = movie.Title,
                        Year = movie.Year,
                        Genre = movie.Genre,
                        Emoji = movie.Emoji,
                        ViewedDate = DateTime.Now
                    };
                    userData.ViewHistory.Add(watched);
                    await DataService.SaveUserDataAsync(userData);
                    await DisplayAlert("Success", "Movie marked as watched!", "OK");
                }
            }

            //deselect item
            MoviesList.SelectedItem = null;
        }



    }


}

