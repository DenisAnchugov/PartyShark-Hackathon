using System;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sharks
{
    public sealed partial class PlaylistView : Page
    {
        private DataAccess dataAccess;
        public PlaylistView()
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            PlaylistName.DataContext = App.CurrentPlaylist.name;
            await UpdateAsync();
        }

        private async Task UpdateAsync()
        {
            Loading.IsActive = true;
            try
            {
                dataAccess = new DataAccess();
                if (App.CurrentPlaylist.id != 0)
                {
                    var playlist = await dataAccess.GetPlaylistAsync(App.CurrentPlaylist.id);
                    PlayListView.DataContext = playlist;
                }
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

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Search));
        }

        private async void Like_OnClick(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            var appBarButton = sender as AppBarButton;
            if (appBarButton != null)
            {
                var song = appBarButton.DataContext as Song;
                dataAccess = new DataAccess();
                try
                {
                    if (song != null) await dataAccess.VoteAsync(song.id, 'u');
                }
                catch (Exception)
                {
                    var message = new MessageDialog("Only one vote per minute allowed.", "Error");
                    message.ShowAsync();
                }
                Loading.IsActive = false;
            }
            await UpdateAsync();
        }

        private async void Dislike_OnClick(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            var appBarButton = sender as AppBarButton;
            if (appBarButton != null)
            {
                var song = appBarButton.DataContext as Song;
                dataAccess = new DataAccess();
                try
                {
                    if (song != null) await dataAccess.VoteAsync(song.id, 'd');
                }
                catch (Exception)
                {
                    var message = new MessageDialog("Only one vote per minute allowed.", "Error");
                    message.ShowAsync();
                }
                Loading.IsActive = false;
            }
            await UpdateAsync();
        }
    }
}
