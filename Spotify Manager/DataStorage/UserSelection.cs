using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spotify_Manager.DataStorage
{
    public class UserSelection : IUserSelection
    {
        public IEnumerable<SimplePlaylist> SourcePlaylists { get; set; }
        public SimplePlaylist DestinationPlaylist { get; set; }
        public FullPlaylist NewDestinationPlaylist { get; set; }

        public UserSelection()
        {
            SourcePlaylists = new ObservableCollection<SimplePlaylist>();
            NewDestinationPlaylist = new FullPlaylist();
        }
    }
}
