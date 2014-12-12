using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sharks
{
    public class Song
    {
        public int id { get; set; }
        public int soundCloudID { get; set; }
        public string songTitle { get; set; }
        public bool isPlayed { get; set; }
        public int karma { get; set; }
        public int playlist { get; set; }

        public override string ToString()
        {
            return songTitle;
        }
    }

    public class Playlist
    {
        public int id { get; set; }
        public List<Song> songs { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

}
