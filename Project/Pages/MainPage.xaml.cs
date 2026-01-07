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

    private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}