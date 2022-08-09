using Spotify_Manager.Models.SpotifyStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public class GetUsersPalylistsResponse
    {
        public string href { get; set; }
        public List<Playlist> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }
    }
}
