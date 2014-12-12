using System;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Sharks
{
    public sealed partial class Search : Page
    {
        private DataAccess dataAccess;
        public Search()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private async void Search_OnClick(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            dataAccess = new DataAccess();
            var results = await dataAccess.GetSongsAsync(SearchBox.Text);
            SearchResult.DataContext = results;
            Loading.IsActive = false;
        }

        private void Playlist_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void Send_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarButton;
            if (button != null)
            {
                var song = button.DataContext as SearchEntity;
                dataAccess = new DataAccess();
                MessageDialog message;
                if (await dataAccess.SendAsync(song, App.CurrentPlaylist.id) == "Error")
                {
                    message = new MessageDialog("We could not connect to the server.", "Error");
                }
                else
                {
                    message = new MessageDialog("Your song has been added.", "Success!");
                }
                await message.ShowAsync();
            }
        }
    }
}
