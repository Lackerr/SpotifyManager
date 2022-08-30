using Microsoft.Extensions.DependencyInjection;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public class SpotifyApiNetDataProvider : ISpotifyDataProvider
    {
        private readonly ISpotifyClientProvider _spotifyClientProvider;
        private const int requestLimit = 100;
        public SpotifyApiNetDataProvider()
        {
            _spotifyClientProvider = Startup.ServiceProvider.GetService<ISpotifyClientProvider>();
        }
        public async Task AddTracksAsync(IEnumerable<FullTrack> sourceTracks, string playlistId)
        {
            var targetTracks = new List<FullTrack>();
            foreach (var track in await GetTracksAsync(playlistId))
            {
                targetTracks.Add(track);
            }


            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();
            List<FullTrack> tracks = CheckForDublicates(sourceTracks, targetTracks) as List<FullTrack>;


            int counter = 0;
            bool isFinished = false;
            try
            {
                do
                {
                    List<string> uris = new List<string>();

                    int maxTracks = counter + requestLimit;
                    if (maxTracks > tracks.Count - 1)
                    {
                        maxTracks = tracks.Count;
                        isFinished = true;
                    }


                    for (int i = counter; i < maxTracks; i++)
                    {
                        uris.Add(tracks[i].Uri);
                    }
                    var request = new PlaylistAddItemsRequest(uris);
                    await client.Playlists.AddItems(playlistId, request);
                    counter += requestLimit;


                } while (!isFinished);

            }
            catch
            {

            }
        }

        private IEnumerable<FullTrack> CheckForDublicates(IEnumerable<FullTrack> sourceTracks, List<FullTrack> targetTracks)
        {
            List<FullTrack> tracks = new List<FullTrack>();
            List<FullTrack> source = new List<FullTrack>(sourceTracks);

            foreach (var track in source)
            {
                if (!targetTracks.Exists(x => x.Id == track.Id))
                {
                    tracks.Add(track);
                }
            }
            return tracks;
        }


        public async Task<IEnumerable<FullTrack>> GetTracksAsync(string playlistId)
        {
            int offset = 0;

            ObservableCollection<FullTrack> tracks = new ObservableCollection<FullTrack>();
            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();
            Paging<PlaylistTrack<IPlayableItem>> pagingTracks;

            try
            {
                do
                {
                    var request = new PlaylistGetItemsRequest
                    {
                        Offset = offset
                    };
                    pagingTracks = await client.Playlists.GetItems(playlistId, request);
                    foreach (var item in pagingTracks.Items)
                    {
                        if (!item.IsLocal)
                            tracks.Add(item.Track as FullTrack);
                    }
                    offset += requestLimit;
                }
                while (pagingTracks.Next != null);
            }
            catch
            {

            }

            return tracks;
        }

        public async Task<IEnumerable<SimplePlaylist>> GetUsersPlaylistsAsync(string userId)
        {
            int offset = 0;

            ObservableCollection<SimplePlaylist> usersPlaylists = new ObservableCollection<SimplePlaylist>();

            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            try
            {
                Paging<SimplePlaylist> playlists = null;
                do
                {
                    var request = new PlaylistCurrentUsersRequest
                    {
                        Limit = 20,
                        Offset = offset
                    };


                    playlists = await client.Playlists.CurrentUsers(request);
                    foreach (SimplePlaylist item in playlists.Items)
                    {
                        usersPlaylists.Add(item);
                    }
                    offset += 20;
                }
                while (playlists.Next != null);
            }
            catch
            {

            }
            return usersPlaylists;
        }

        public async Task PlaylistDeleteDublicatesAsync(string playlistId, IEnumerable<FullTrack> tracks)
        {
            List<FullTrack> trackList = new List<FullTrack>(tracks);
            List<string> uris = trackList.Select(o => o.Uri).ToList();

            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            List<string> dublicates = uris.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(y => y.Key)
                            .ToList();

            List<FullTrack> doubleTracks = new List<FullTrack>();
            foreach (var uri in dublicates)
            {
                doubleTracks.Add(new FullTrack
                {
                    Uri = uri,
                    Id = uri.Substring(uri.LastIndexOf(":") + 1)
                });
            }
            try
            {
                await PlaylistRemoveTracksAsync(playlistId, dublicates);
                await AddTracksAsync(doubleTracks, playlistId);
            }
            catch
            {

            }
        }

        public async Task PlaylistRemoveTracksAsync(string playlistId, IEnumerable<string> trackUris)
        {
            int counter = 0;
            bool isFinished = false;

            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            do
            {
                var itemsToRemove = new List<PlaylistRemoveItemsRequest.Item>();
                int maxItems = counter + requestLimit;

                if (maxItems > trackUris.Count() - 1)
                {
                    maxItems = trackUris.Count();
                    isFinished = true;
                }

                for (int i = counter; i < maxItems; i++)
                {
                    itemsToRemove.Add(new PlaylistRemoveItemsRequest.Item
                    {
                        Uri = trackUris.ElementAt(i)
                    });
                }
                try
                {
                    var request = new PlaylistRemoveItemsRequest { Tracks = itemsToRemove };
                    await client.Playlists.RemoveItems(playlistId, request);
                }
                catch (Exception x)
                {
                    var f = x.Message;
                }
                counter += requestLimit;

            } while (!isFinished);
        }

        public async Task<FullPlaylist> PlaylistCreateAsync(string name)
        {
            FullPlaylist fullPlaylist = null;
            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            var request = new PlaylistCreateRequest(name)
            {
                Description = "Playlist created with Spotify-Manager"
            };
            try
            {
                var user = await client.UserProfile.Current();
                fullPlaylist = await client.Playlists.Create(user.Id, request);
            }
            catch (Exception x)
            {
                var f = x.Message;
            }
            return fullPlaylist;
        }

        public async Task<PrivateUser> GetCurrentUserAsync()
        {
            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();
            try
            {
                return await client.UserProfile.Current();
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<TrackAudioFeatures>> GetAudioFeaturesAsync(IEnumerable<string> trackIds)
        {
            int counter = 0;
            bool isFinished = false;

            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            ObservableCollection<TrackAudioFeatures> audioFeatures = new ObservableCollection<TrackAudioFeatures>();

            do
            {
                var ids = new List<string>();
                int maxItems = counter + requestLimit;

                if (maxItems > trackIds.Count() - 1)
                {
                    maxItems = trackIds.Count();
                    isFinished = true;
                }

                for (int i = counter; i < maxItems; i++)
                {
                    ids.Add(trackIds.ElementAt(i));
                }
                try
                {
                    var request = new TracksAudioFeaturesRequest(ids);
                    var response = await client.Tracks.GetSeveralAudioFeatures(request);

                    foreach (var item in response.AudioFeatures)
                    {
                        audioFeatures.Add(item);
                    }
                }
                catch (Exception x)
                {
                    var f = x.Message;
                }
                counter += requestLimit;

            } while (!isFinished);
            return audioFeatures;
        }

        public async Task PlaylistClearAsync(string playlistId)
        {
            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            PlaylistReplaceItemsRequest request = new PlaylistReplaceItemsRequest(new List<string>());
            try
            {
                await client.Playlists.ReplaceItems(playlistId, request);
            }
            catch (Exception x)
            {
                var f = x.Message;
            }
        }
    }
}
