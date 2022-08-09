using Spotify_Manager.Models.SpotifyStructure;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public interface ISpotifyDataProvider
    {
        Task<IEnumerable<SimplePlaylist>> GetUsersPlaylistsAsync(string userId);
        Task<IEnumerable<ITrack>> GetTracksAsync(string playlistId);
        Task<ITrackInfromation> GetTrackInformationAsync(string trackId);
        Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId);
    }
}
