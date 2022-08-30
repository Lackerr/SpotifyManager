using Microsoft.Extensions.DependencyInjection;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public class SpotifyDataService : ISpotifyDataService
    {
        readonly ISpotifyDataProvider _provider;
        public SpotifyDataService()
        {
            _provider = Startup.ServiceProvider.GetService<ISpotifyDataProvider>();
        }

        public async Task<bool> AddTracksAsync(IEnumerable<FullTrack> tracks, string playlistId)
        {
            await _provider.AddTracksAsync(tracks, playlistId);
            return true;
        }

        public async Task<IEnumerable<SimplePlaylist>> GetPlaylistsAsync(string userId)
        {
            var playlists = await _provider.GetUsersPlaylistsAsync(userId);
            return playlists;
        }



        public async Task<IEnumerable<FullTrack>> GetTracksAsync(string playlistId)
        {
            return await _provider.GetTracksAsync(playlistId);
        }

        public async Task MergePlaylists(IEnumerable<SimplePlaylist> sourcePlaylists, SimplePlaylist targetPlaylist)
        {
            ObservableCollection<SimplePlaylist> source = sourcePlaylists as ObservableCollection<SimplePlaylist>;
            List<FullTrack> sourceTracks = new List<FullTrack>();
            foreach (SimplePlaylist playlist in source)
            {
                var tracks = await _provider.GetTracksAsync(playlist.Id);
                foreach (var item in tracks)
                {
                    if (!sourceTracks.Exists(x => x.Id == item.Id))
                        sourceTracks.Add(item);
                }
            }

            await _provider.AddTracksAsync(sourceTracks, targetPlaylist.Id);
        }

        public async Task PlaylistDeleteDublicates(string playlistId, IEnumerable<FullTrack> tracks = null)
        {
            if (tracks == null)
            {
                tracks = await _provider.GetTracksAsync(playlistId);
            }
            await _provider.PlaylistDeleteDublicatesAsync(playlistId, tracks);
        }

        public async Task<FullPlaylist> PlaylistCreate(string name)
        {
            var playlist = await _provider.PlaylistCreateAsync(name);
            return playlist;
        }

        public async Task<string> GetCurrentUserId()
        {
            var user = await _provider.GetCurrentUserAsync();
            return user.Id;
        }

        public async Task<IEnumerable<TrackAudioFeatures>> GetAudioFeaturesAsync(IEnumerable<string> trackIds)
        {
            return await _provider.GetAudioFeaturesAsync(trackIds);
        }

        public async Task<string> ReorderTrackAsync(string playlistId, string trackId, int oldPos, int newPos, string snapshotId)
        {
            await _provider.PlaylistClearAsync(playlistId);
            return null;
        }
    }
}
