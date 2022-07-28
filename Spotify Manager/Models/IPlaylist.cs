using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public interface IPlaylist
    {
        string Name { get; set; }
        string Description { get; set; }
        string Id { get; set; }
        List<ITrack> Tracks { get; set; }


    }
}
