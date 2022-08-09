using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models.SpotifyStructure
{
    public interface ITrackInfromation
    {
        string Id { get; set; }
        string Uri { get; set; }
        float Acousticness { get; set; }
        float Danceability { get; set; }
        int DurationInMs { get; set; }
        float Energy { get; set; }
        float Instrumentalness { get; set; }
        int Key { get; set; }
        float Liveness { get; set; }
        float Loudness { get; set; }
        float Speechiness { get; set; }
        float Valence { get; set; }
    }
}
