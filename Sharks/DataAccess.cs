using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sharks
{
    class DataAccess
    {
        private HttpProvider httClient;

        const string Url = "http://partyshark.cloudapp.net/api/";
        public async Task<List<SearchEntity>> GetSongsAsync(string searchParameter)
        {
            httClient = new HttpProvider();
            var response = await httClient.GetAsync(new Uri("https://api.soundcloud.com/tracks?client_id=b365d76bf1520df0559a12bd6fee5519&q=" + searchParameter));
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<SearchEntity>>(response));
        }
        public async Task<Playlist> GetPlaylistAsync(int id)
        {
            var httpClient = new HttpProvider();
            var response = await httpClient.GetAsync(new Uri(Url + "playlists/" + id));
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Playlist>(response));
        }
        public async Task<string> SendAsync(SearchEntity song, int playlistId)
        {
            var httpClient = new HttpProvider();
            var s = new Song() { soundCloudID = song.id, songTitle = song.title, isPlayed = false, playlist = playlistId };
            string message = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(s));
            return await httpClient.PostAsync(new Uri(Url + "songs/"), message);
        }
        public async Task<List<Playlist>> GetPlayListsAsync()
        {
            var httpClient = new HttpProvider();
            var response = await httpClient.GetAsync(new Uri(Url + "playlists/"));
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Playlist>>(response));
        }
        public async Task VoteAsync(int id, char t)
        {
            try
            {
                var httpClient = new HttpProvider();
                await httpClient.GetAsync(new Uri(Url + "api/vote/" + id + "/" + t));
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
