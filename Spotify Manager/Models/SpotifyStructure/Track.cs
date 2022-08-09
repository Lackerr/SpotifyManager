using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models.SpotifyStructure
{
    public class Track : ITrack
    {
        public string id { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public bool is_local { get; set; }
        public string href { get; set; }
    }
}
