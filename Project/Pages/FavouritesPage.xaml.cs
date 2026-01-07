using Project.Models;
using Project.Services;

namespace Project.Pages;


public partial class FavouritesPage : ContentPage
{
    private DataService dataService;
    private UserData userData;

    public FavouritesPage()
    {
        InitializeComponent();
        dataService = new DataService();
    }


    //reloading favopurites in case new ones were added
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFavourites();
    }

    //loafing the favourite movies, newest first
    private async Task LoadFavourites()
    {
        userData = await dataService.LoadUserDataAsync();


        FavouritesList.ItemsSource = userData.Favourites.OrderByDescending(f => f.AddedDate).ToList();
    }

    //removing a favourite movie
    private async void FavouritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        var fav = e.CurrentSelection[0] as FavouriteMovie;
        if (fav == null) return;

        //disploy alert to confirm removal
        bool remove = await DisplayAlert("Remove Favourite", $"Do you want to remove '{fav.Title}' from your favourites?", "Yes", "No");

        //removes the movie and refreshes the list
        if (remove)
        {
            userData.Favourites.Remove(fav);
            await dataService.SaveUserDataAsync(userData);
            await LoadFavourites();
        }

        FavouritesList.SelectedItem = null;
    }
}