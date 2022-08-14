using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spotify_Manager.Models;
using Spotify_Manager.Models.SpotifyStructure;
using Spotify_Manager.Secrets;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.Services
{
    public class SpotifyDataProvider : ISpotifyDataProvider
    {
        private ITokenProvider _tokenProvider;
        public SpotifyDataProvider()
        {
            _tokenProvider = Startup.ServiceProvider.GetService<ITokenProvider>();
        }
        public Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId)
        {
            throw new NotImplementedException();
        }

        public async Task AddTracksAsync(IEnumerable<FullTrack> sourceTracks, string playlistId)
        {
            int limit = 50;
            int offset = 0;
            var userId = AppSecret.UserId;

            string result = string.Empty;
            List<IPlaylist> playlists = new List<IPlaylist>();

            var client = new HttpClient();

            var token = await _tokenProvider.GetAccessToken();


            string url = $"https://api.spotify.com/v1/users/{userId}/playlists?limit={limit}&offset={offset}";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");


            try
            {
                var response = client.GetAsync(url).Result;
                result = response.Content.ReadAsStringAsync().Result;;

                GetUsersPalylistsResponse res = JsonConvert.DeserializeObject<GetUsersPalylistsResponse>(result);

                foreach (var item in res.items)
                {
                    playlists.Add(item);
                }



                //return playlists;
              
            }
            catch (Exception x)
            {
                var ex = x.Message;
               
            }

        }

        public Task<ITrackInfromation> GetTrackInformationAsync(string trackId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITrack>> GetTracksAsync(string playlistId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SimplePlaylist>> GetUsersPlaylistsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<FullPlaylist> PlaylistCreate(string name)
        {
            throw new NotImplementedException();
        }

        public Task PlaylistDeleteDublicates(string playlistId, IEnumerable<FullTrack> tracks)
        {
            throw new NotImplementedException();
        }

        public Task PlaylistRemoveTracks(string playlistId, IEnumerable<string> trackUris)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<FullTrack>> ISpotifyDataProvider.GetTracksAsync(string playlistId)
        {
            throw new NotImplementedException();
        }
    }
}
