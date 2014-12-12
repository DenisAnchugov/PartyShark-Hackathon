using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Sharks
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
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
                var playlists = await access.GetPlayLists();
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
            var button = sender as Button;

            if (button != null)
            {
                var playlist = button.DataContext as Playlist;
                if (playlist != null) this.Frame.Navigate(typeof (PlaylistView), playlist);
            }
        }
    }
}
