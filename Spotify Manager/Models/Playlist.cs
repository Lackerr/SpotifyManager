using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public class Playlist : IPlaylist
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public List<ITrack> Tracks { get; set; }
    }
}
