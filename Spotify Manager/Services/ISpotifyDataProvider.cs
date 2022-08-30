using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public interface ISpotifyDataProvider
    {
        Task<IEnumerable<SimplePlaylist>> GetUsersPlaylistsAsync(string userId);
        Task<IEnumerable<FullTrack>> GetTracksAsync(string playlistId);
        Task AddTracksAsync(IEnumerable<FullTrack> tracks, string playlistId);

        Task PlaylistDeleteDublicatesAsync(string playlistId, IEnumerable<FullTrack> tracks);
        Task PlaylistRemoveTracksAsync(string playlistId, IEnumerable<string> trackUris);
        Task<FullPlaylist> PlaylistCreateAsync(string name);
        Task<PrivateUser> GetCurrentUserAsync();
        Task<IEnumerable<TrackAudioFeatures>> GetAudioFeaturesAsync(IEnumerable<string> trackIds);
        Task PlaylistClearAsync(string playlistId);
    }
}
