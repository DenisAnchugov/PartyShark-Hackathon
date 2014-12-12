using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Search : Page
    {
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

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            var access = new DataAccess();
            var results = await access.GetSongs(SearchBox.Text);
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
                var access = new DataAccess();
                MessageDialog message;
                if (await access.SendAsync(song, 11) == "Error")
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
