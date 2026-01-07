using Project.Models;
using Project.Services;

namespace Project.Pages;

public partial class WatchedPage : ContentPage
{
    private DataService dataService;
    private UserData userData;
    public WatchedPage()
    {
        InitializeComponent();
        dataService = new DataService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadWatched();
    }

    private async Task LoadWatched()
    {
        userData = await dataService.LoadUserDataAsync();
        WatchedList.ItemsSource = userData.ViewHistory.OrderByDescending(v => v.ViewedDate).ToList();
    }

    //clears the watched history after confirmation
    private async void ClearWatchedButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Clear watched History", "Are you sure you want to clear your view history?", "Yes", "No");

        if (confirm)
        {
            //remove all from the list and refresh the page
            userData.ViewHistory.Clear();
            await dataService.SaveUserDataAsync(userData);
            await LoadWatched();
        }
    }
}
