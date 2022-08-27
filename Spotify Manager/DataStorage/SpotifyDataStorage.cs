using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.Secrets;
using Spotify_Manager.Services;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Spotify_Manager.DataStorage
{
    internal class SpotifyDataStorage : ISpotifyDataStorage
    {
        private readonly ISpotifyDataService _spotifyDataService;

        public ObservableCollection<SimplePlaylist> UsersPlaylists { get; private set; }

        public SpotifyDataStorage()
        {
            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();
        }
        


        public async Task<IEnumerable<SimplePlaylist>> RefreshUsersPlaylists()
        {
            UsersPlaylists = (ObservableCollection<SimplePlaylist>)await _spotifyDataService.GetPlaylistsAsync(await _spotifyDataService.GetCurrentUserId());
            return UsersPlaylists;
        }
    }
}
