using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.Models.SpotifyStructure;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public class SpotifyDataService : ISpotifyDataService
    {
        List<Playlist> _playlists;
        ISpotifyDataProvider _provider;
        public SpotifyDataService()
        {
            _provider = Startup.ServiceProvider.GetService<ISpotifyDataProvider>();
            //_playlists = new List<Playlist>()
            //{
            //    new Playlist { Id = "432424", Name = "Playlist1", Description = "Test Desc",
            //    Tracks = new List<ITrack>(){ new Track { Name = "Track 1", Id = "1", IsLocal = false, Uri = "424243" } } }
            //};
        }

        public Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SimplePlaylist>> GetPlaylistsAsync(string userId)
        {
            var playlists = await _provider.GetUsersPlaylistsAsync(userId);
            return playlists;
        }

        public Task<ITrackInfromation> GetTrackInformationAsync(string trackId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITrack>> GetTracksAsync(string playlistId)
        {
            throw new NotImplementedException();
        }
    }
}
