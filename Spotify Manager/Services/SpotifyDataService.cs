using Spotify_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public class SpotifyDataService : ISpotifyDataService
    {
        List<Playlist> _playlists;

        public SpotifyDataService()
        {
            _playlists = new List<Playlist>()
            {
                new Playlist { Id = "432424", Name = "Playlist1", Description = "Test Desc",
                Tracks = new List<ITrack>(){ new Track { Name = "Track 1", Id = "1", IsLocal = false, Uri = "424243" } } }
            };
        }

        public Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IPlaylist>> GetPlaylistsAsync(string userId)
        {
            return await Task.FromResult(_playlists);
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
