using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sharks
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await UpdatePlaylists();
        }

        private async Task UpdatePlaylists()
        {
            Loading.IsActive = true;
            try
            {
                var access = new DataAccess();
                var playlists = await access.GetPlayListsAsync();
                Playlists.DataContext = playlists;
            }
            catch (Exception)
            {
                var message = new MessageDialog("We could not connect to the server.", "Error");
                message.ShowAsync();
            }
            finally
            {
                Loading.IsActive = false;
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await UpdatePlaylists();
        }

        private void Playlist_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            if (button == null) return;
            App.CurrentPlaylist = button.DataContext as Playlist;
            this.Frame.Navigate(typeof(PlaylistView));
        }
    }
}
