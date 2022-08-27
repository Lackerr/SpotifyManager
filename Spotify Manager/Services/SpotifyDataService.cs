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

        ISpotifyDataProvider _provider;
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

        public Task<ITrackInfromation> GetTrackInformationAsync(string trackId)
        {
            throw new NotImplementedException();
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
            await _provider.PlaylistDeleteDublicates(playlistId, tracks);
        }

        public async Task<FullPlaylist> PlaylistCreate(string name)
        {
            var playlist = await _provider.PlaylistCreate(name);
            return playlist;
        }

        public async Task<string> GetCurrentUserId()
        {
            var user = await _provider.GetCurrentUser();
            return user.Id;
        }

        public async Task<IEnumerable<TrackAudioFeatures>> GetAudioFeaturesAsync(IEnumerable<string> trackIds)
        {
            return await _provider.GetAudioFeaturesAsync(trackIds);
        }

        public async Task<string> ReorderTrackAsync(string playlistId, string trackId, int oldPos, int newPos, string snapshotId)
        {
            var snapshot = await _provider.ReorderTrackAsync(playlistId, trackId, oldPos, newPos, snapshotId);
            return snapshot;
        }
    }
}
