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
    }
}
