namespace Project
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                ExcitedBtn.Text = $"Excited {count} time";
            else
                ExcitedBtn.Text = $"Excited {count} times";

            SemanticScreenReader.Announce(ExcitedBtn.Text);
        }
    }
}
