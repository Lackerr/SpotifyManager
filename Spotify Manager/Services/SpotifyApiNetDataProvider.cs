using Spotify_Manager.Models.SpotifyStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.Secrets;
using SpotifyAPI.Web.Http;
using System.Collections.ObjectModel;

namespace Spotify_Manager.Services
{
    public class SpotifyApiNetDataProvider : ISpotifyDataProvider
    {
    
        ITokenProvider _tokenProvider;
        public SpotifyApiNetDataProvider()
        {
            _tokenProvider = Startup.ServiceProvider.GetService<ITokenProvider>();
        }
        public Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId)
        {
            throw new NotImplementedException();
        }

        public Task<ITrackInfromation> GetTrackInformationAsync(string trackId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITrack>> GetTracksAsync(string playlistId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SimplePlaylist>> GetUsersPlaylistsAsync(string userId)
        {
            int offset = 0;
            int total = 0;

            Paging<SimplePlaylist> playlists = null;
            ObservableCollection<SimplePlaylist> usersPlaylists = new ObservableCollection<SimplePlaylist>();

            string token = await _tokenProvider.GetAccessToken();

            SpotifyClient client = new SpotifyClient(token);
            //int total = (int)playlists.Total;

            try
            {
                do
                {
                    var request = new PlaylistCurrentUsersRequest
                    {
                        Limit = 20,
                        Offset = offset
                    };


                    playlists = await client.Playlists.CurrentUsers(request);
                    foreach (SimplePlaylist item in playlists.Items)
                    {
                        usersPlaylists.Add(item);
                    }
                    offset += 20;
                }
                while (playlists.Next != null);
            }
            catch(Exception x)
            {
                var f = x;
            }

            return usersPlaylists;
        }
    }
}
