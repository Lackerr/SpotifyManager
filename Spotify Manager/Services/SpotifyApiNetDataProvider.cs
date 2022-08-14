using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.Models.SpotifyStructure;
using Spotify_Manager.Secrets;
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

        ITokenProvider _tokenProvider;
        ISpotifyClientProvider _spotifyClientProvider;
        public SpotifyApiNetDataProvider()
        {
            _tokenProvider = Startup.ServiceProvider.GetService<ITokenProvider>();
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

                    int maxTracks = counter + 100;
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
                    counter += 100;


                } while (!isFinished);

            }
            catch (Exception x)
            {
                var f = x.Message;
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
        public Task<ITrackInfromation> GetTrackInformationAsync(string trackId)
        {
            throw new NotImplementedException();
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
                    offset += 100;
                }
                while (pagingTracks.Next != null);
            }
            catch (Exception x)
            {
                var f = x;
            }

            return tracks;
        }

        public async Task<IEnumerable<SimplePlaylist>> GetUsersPlaylistsAsync(string userId)
        {
            int offset = 0;


            Paging<SimplePlaylist> playlists = null;
            ObservableCollection<SimplePlaylist> usersPlaylists = new ObservableCollection<SimplePlaylist>();

            

            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();


            try
            {
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
            catch (Exception x)
            {
                var f = x;
            }

            return usersPlaylists;
        }

        public async Task PlaylistDeleteDublicates(string playlistId, IEnumerable<FullTrack> tracks)
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
                //var request = new SearchRequest(SearchRequest.Types.Track, uri);
                //var h = await client.Search.Item(request);
            }
            await PlaylistRemoveTracks(playlistId, dublicates);
            await AddTracksAsync(doubleTracks, playlistId);

        }

        public async Task PlaylistRemoveTracks(string playlistId, IEnumerable<string> trackUris)
        {
            int counter = 0;
            bool isFinished = false;
            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            do
            {
                var itemsToRemove = new List<PlaylistRemoveItemsRequest.Item>();
                int maxItems = counter + 100;

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
                counter += 100;

            } while (!isFinished);
        }

        public async Task<FullPlaylist> PlaylistCreate(string name)
        {
            SpotifyClient client = await _spotifyClientProvider.CreateSpotifyClient();

            var request = new PlaylistCreateRequest(name)
            {
                Description = "Playlist created with Spotify-Manager"
            };
            var fullPlaylist = await client.Playlists.Create(AppSecret.UserId, request);
            return fullPlaylist;
        }
    }
}
