using Project.Models;
using Project.Services;

namespace Project.Pages;


public partial class FavouritesPage : ContentPage
{
    private DataService DataService;
    private UserData UserData;

    public FavouritesPage()
    {
        InitializeComponent();
        DataService = new DataService();
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
        UserData = await DataService.LoadUserDataAsync();


        FavouritesList.ItemsSource = UserData.Favourites.OrderByDescending(f => f.AddedDate).ToList();
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
            UserData.Favourites.Remove(fav);
            await DataService.SaveUserDataAsync(UserData);
            await LoadFavourites();
        }

        FavouritesList.SelectedItem = null;
    }
}