using Spotify_Manager.Models.SpotifyStructure;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Spotify_Manager.DataStorage
{
    public class UserSelection : IUserSelection
    {
        public IEnumerable<SimplePlaylist> SourcePlaylists { get; set; }
        public IPlaylist DestinationPlaylist { get; set; }

        public UserSelection()
        {
            SourcePlaylists = new ObservableCollection<SimplePlaylist>();
        }
    }
}
