using Project.Models;
using Project.Services;

namespace Project.Pages;

public partial class WatchedPage : ContentPage
{
    public partial class WatchedPage : ContentPage
    {
        private DataService DataService;
        private UserData UserData;
        public WatchedPage()
        {
            InitializeComponent();
            DataService = new DataService();
        }
    }