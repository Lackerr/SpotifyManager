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
        Task<IEnumerable<FullTrack>> GetTracksAsync(string playlistId);
        Task<ITrackInfromation> GetTrackInformationAsync(string trackId);
        Task AddTracksAsync(IEnumerable<FullTrack> tracks, string playlistId);

        Task PlaylistDeleteDublicates(string playlistId, IEnumerable<FullTrack> tracks);
        Task PlaylistRemoveTracks(string playlistId, IEnumerable<string> trackUris);
        Task<FullPlaylist> PlaylistCreate(string name);
        Task<PrivateUser> GetCurrentUser();
        Task<IEnumerable<TrackAudioFeatures>> GetAudioFeaturesAsync(IEnumerable<string> trackIds);
        Task PlaylistClearAsync(string playlistId);
    }
}
