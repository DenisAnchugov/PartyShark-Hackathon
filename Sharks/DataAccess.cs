using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Newtonsoft.Json;

namespace Sharks
{
    class DataAccess
    {
        public async Task<List<SearchEntity>> GetSongs(string searchParameter)
        {
            var httpClient = new HttpClinet();
            var response = await httpClient.GetAsync(new Uri("https://api.soundcloud.com/tracks?client_id=b365d76bf1520df0559a12bd6fee5519&q=" + searchParameter));
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<SearchEntity>>(response));
        }

        public async Task<Playlist> GetPlaylist(int id)
        {
            var httpClient = new HttpClinet();
            var response = await httpClient.GetAsync(new Uri("http://172.16.0.127:8000/api/playlists/" + id));
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Playlist>(response));
        }

        public async Task<string> SendAsync(SearchEntity song, int playlistId)
        {
            var httpClient = new HttpClinet();
            var s = new Song() { soundCloudID = song.id, songTitle = song.title, isPlayed = false, playlist = playlistId};
            string message = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(s));
            return await httpClient.PostAsync(new Uri("http://172.16.0.127:8000/api/songs/"), message);
        }

        public async Task<List<Playlist>> GetPlayLists()
        {
            var httpClient = new HttpClinet();
            var response = await httpClient.GetAsync(new Uri("http://172.16.0.127:8000/api/playlists/"));
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Playlist>>(response));           
        } 
    }
}
