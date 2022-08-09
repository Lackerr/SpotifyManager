using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace Spotify_Manager.DataStorage
{
    internal interface ISpotifyDataStorage
    {
        ObservableCollection<SimplePlaylist> UsersPlaylists { get; }
        Task<IEnumerable<SimplePlaylist>> RefreshUsersPlaylists();
    }
}
