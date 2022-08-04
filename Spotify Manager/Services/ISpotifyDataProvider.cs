using Spotify_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public interface ISpotifyDataProvider
    {
        Task<IEnumerable<IPlaylist>> GetUsersPlaylistsAsync(string userId, int limit = 0, int offset = 0);
        Task<IEnumerable<ITrack>> GetTracksAsync(string playlistId);
        Task<ITrackInfromation> GetTrackInformationAsync(string trackId);
        Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId);
    }
}
