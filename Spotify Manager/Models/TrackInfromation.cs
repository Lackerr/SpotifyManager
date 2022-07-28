using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    internal class TrackInfromation : ITrackInfromation
    {
        public string Id { get; set; }
        public string Uri { get; set; }
        public float Acousticness { get; set; }
        public float Danceability { get; set; }
        public int DurationInMs { get; set; }

        public float Energy { get; set; }
        public float Instrumentalness { get; set; }
        public int Key { get; set; }
        public float Liveness { get; set; }
        public float Loudness { get; set; }
        public float Speechiness { get; set; }
        public float Valence { get; set; }
    }
}
