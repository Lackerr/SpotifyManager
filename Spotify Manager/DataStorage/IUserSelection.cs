using SpotifyAPI.Web;
using System.Collections.Generic;

namespace Spotify_Manager.DataStorage
{
    internal interface IUserSelection
    {
        IEnumerable<SimplePlaylist> SourcePlaylists{get;set;}
        SimplePlaylist DestinationPlaylist { get;set;}
        FullPlaylist NewDestinationPlaylist { get;set;}
    }
}
