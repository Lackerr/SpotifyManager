using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public class Playlist : IPlaylist
    {
        
        public string name { get; set; }
        public string Description { get; set; }
        public string description { get; set; }
      
        public string id { get; set; }
        public List<PlaylistTracks> items{ get; set; }
        public bool collaborative { get; set; }
        public SpotifyUrl external_urls { get; set; }
        public IEnumerable<object> followers { get; set; }
        public string href { get; set; }
        public IEnumerable<object> images { get; set; }
        public bool is_public { get; set; }
        public string Type { get; set; }
        public string uri { get; set; }
    }
}
