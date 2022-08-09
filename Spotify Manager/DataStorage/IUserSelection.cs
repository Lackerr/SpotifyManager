using Spotify_Manager.Models.SpotifyStructure;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.DataStorage
{
    internal interface IUserSelection
    {
        IEnumerable<SimplePlaylist> SourcePlaylists{get;set;}
        IPlaylist DestinationPlaylist { get;set;}
    }
}
