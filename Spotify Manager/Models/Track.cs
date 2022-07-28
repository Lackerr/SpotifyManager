using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    public class Track : ITrack
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public bool IsLocal { get; set; }
    }
}
