using Project.Models;
using Project.Services;
namespace Project.Pages;

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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        //load movies 
        allMovies = await DataService.GetMoviesAsync();
        MoviesList.ItemsSource = allMovies;

        //setup genre picker
        var genres = allMovies.SelectMany(m => m.Genre).Distinct().OrderBy(g => g).ToList();
        genres.Insert(0, "All Genres");
        GenrePicker.ItemsSource = genres;
        GenrePicker.SelectedIndex = 0;

    }

    // Filter movies when genre changes
    private void Search_Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterMovies();
    }

    private void FilterMovies()
    {
        var filtered = allMovies.AsEnumerable();

        string searchText = SearchEntry.Text?.ToLower() ?? "";
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filtered = filtered.Where(m => m.Title.ToLower().Contains(searchText));
        }

        if (GenrePicker.SelectedIndex > 0)
        {
            string selectedGenre = GenrePicker.SelectedItem.ToString() ?? "";
            filtered = filtered.Where(m => m.Genre.Contains(selectedGenre));
        }

        MoviesList.ItemsSource = filtered.ToList();


    }

    private void MoviesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
        {
            Navigation.PushAsync(new MovieDetailPage(selectedMovie, userData));
            MoviesList.SelectedItem = null; // Deselect item
        }
    }
}