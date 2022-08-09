using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models.SpotifyStructure
{
    public interface ITrack
    {

        string id { get; set; }
        string name { get; set; }
        string uri { get; set; }
        bool is_local { get; set; }
        string href { get; set; }
    }
}
