using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models.SpotifyStructure
{
    public interface IPlaylist
    {
        bool collaborative { get; set; }
        string description { get; set; }
        SpotifyUrl external_urls { get; set; }
        IEnumerable<object> followers { get; set; }
        string href { get; set; }

        string id { get; set; }
        IEnumerable<object> images { get; set; }
        string name { get; set; }
        bool is_public { get; set; }
        IEnumerable<PlaylistTracks> tracks { get; set; }
        string Type { get; set; }
        string uri { get; set; }


    }
}
