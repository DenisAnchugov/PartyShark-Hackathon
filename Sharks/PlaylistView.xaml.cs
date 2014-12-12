using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Sharks
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistView : Page
    {
        public int CurrentPlaylist { get; set; }
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

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            var parameter = e.Parameter as Playlist;
            PlaylistName.DataContext = parameter.name;
            CurrentPlaylist = parameter.id;
            await UpdateAsync();
        }

        private async Task UpdateAsync()
        {
            Loading.IsActive = true;
            try
            {
                var access = new DataAccess();
                if (CurrentPlaylist != 0)
                {
                    var playlist = await access.GetPlaylistAsync(CurrentPlaylist);
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
                Song song = appBarButton.DataContext as Song;
                DataAccess access = new DataAccess();
                try
                {
                    if (song != null) await access.VoteAsync(song.id, 'u');
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
                Song song = appBarButton.DataContext as Song;
                DataAccess access = new DataAccess();
                try
                {
                    if (song != null) await access.VoteAsync(song.id, 'd');
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
