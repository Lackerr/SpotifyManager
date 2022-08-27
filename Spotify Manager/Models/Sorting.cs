using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace Spotify_Manager.Models
{
    public class Sorting
    {
        public ObservableCollection<SortingType> Types { get; private set; }
        private ISpotifyDataService _spotifyDataService;
        public Sorting()
        {
            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();

            Types = new ObservableCollection<SortingType>();
            LoadTypes();
        }

        private void LoadTypes()
        {
            Types.Add(new SortingType("Energy"));
            Types.Add(new SortingType("Tempo"));
            Types.Add(new SortingType("Danceability"));
            Types.Add(new SortingType("Speechiness"));
            Types.Add(new SortingType("Liveness"));
            Types.Add(new SortingType("Instrumentalness"));
            Types.Add(new SortingType("Loudness"));
            Types.Add(new SortingType("Key"));
        }

        public ObservableCollection<SortingType> GetSortingTypes()
        {
            return Types;
        }


        public async Task PlaylistSort(SortingType type, SimplePlaylist playlist)
        {
            var snapshot = playlist.SnapshotId;

            var tracks = await _spotifyDataService.GetTracksAsync(playlist.Id);

            var trackIds = new List<string>();
            foreach (var track in tracks)
            {
                trackIds.Add(track.Id);
            }

            var audioFeatures = await _spotifyDataService.GetAudioFeaturesAsync(trackIds);
            var newTracks =  GetSortedTracks(audioFeatures, type);

            await _spotifyDataService.ReorderTrackAsync(playlist.Id, /*track.Id*/ "", /*oldPos*/ 0, tracks.Count(), snapshot);

            await _spotifyDataService.AddTracksAsync(newTracks, playlist.Id);
        }

        private  IEnumerable<FullTrack> GetSortedTracks(IEnumerable<TrackAudioFeatures> features, SortingType type)
        {
            if (type.Name == "Energy")
            {
                features = features.OrderBy(x => x.Energy).ToList();
            }
            else if (type.Name == "Tempo")
            {
                features = features.OrderBy(x => x.Tempo).ToList();
            }
            else if (type.Name == "Danceability")
            {
                features = features.OrderBy(x => x.Danceability).ToList();
            }
            else if (type.Name == "Speechiness")
            {
                features = features.OrderBy(x => x.Speechiness).ToList();
            }
            else if (type.Name == "Liveness")
            {
                features = features.OrderBy(x => x.Liveness).ToList();
            }
            else if (type.Name == "Instrumentalness")
            {
                features = features.OrderBy(x => x.Instrumentalness).ToList();
            }
            else if (type.Name == "Loudness")
            {
                features = features.OrderBy(x => x.Loudness).ToList();
            }
            else if (type.Name == "Key")
            {
                features = features.OrderBy(x => x.Key).ToList();
            }

            List<FullTrack> tracks = new List<FullTrack>();
            foreach (var item in features)
            {
                tracks.Add(new FullTrack()
                {
                    Id = item.Id,
                    Uri = item.Uri,
                });
            }

            return tracks;
        }
    }
}
