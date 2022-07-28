using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public interface ITrack
    {

        string Id { get; set; }
        string Name { get; set; }
        string Uri { get; set; }
        bool IsLocal { get; set; }
    }
}
