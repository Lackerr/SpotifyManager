using Spotify_Manager.Models.SpotifyStructure;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public interface ISpotifyDataService
    {
        Task<IEnumerable<SimplePlaylist>> GetPlaylistsAsync(string userId);
        Task<IEnumerable<ITrack>> GetTracksAsync(string playlistId);
        Task<ITrackInfromation> GetTrackInformationAsync(string trackId);
        Task<bool> AddTracksAsync(IEnumerable<ITrack> tracks, string playlistId);
        Task MergePlaylists(IEnumerable<SimplePlaylist> sourcePlaylists, SimplePlaylist targetPlaylist);
        Task PlaylistDeleteDublicates(string playlistId, IEnumerable<FullTrack> tracks = null);
        Task<FullPlaylist> PlaylistCreate(string name);
    }
}
