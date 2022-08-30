using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public interface ISpotifyDataService
    {
        Task<IEnumerable<SimplePlaylist>> GetPlaylistsAsync(string userId);
        Task<IEnumerable<FullTrack>> GetTracksAsync(string playlistId);
        Task<bool> AddTracksAsync(IEnumerable<FullTrack> tracks, string playlistId);
        Task MergePlaylists(IEnumerable<SimplePlaylist> sourcePlaylists, SimplePlaylist targetPlaylist);
        Task PlaylistDeleteDublicates(string playlistId, IEnumerable<FullTrack> tracks = null);
        Task<FullPlaylist> PlaylistCreate(string name);
        Task<string> GetCurrentUserId();
        Task<IEnumerable<TrackAudioFeatures>> GetAudioFeaturesAsync(IEnumerable<string> trackIds);
        Task<string> ReorderTrackAsync(string playlistId, string trackId, int oldPos, int newPos, string snapshotId);
    }
}
