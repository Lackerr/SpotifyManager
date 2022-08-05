using Spotify_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.DataStorage
{
    internal interface IUserSelection
    {
        IEnumerable<IPlaylist> SourcePlaylists{get;set;}
        IPlaylist DestinationPlaylist { get;set;}
    }
}
