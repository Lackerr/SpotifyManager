using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Models;
using Spotify_Manager.Secrets;
using Spotify_Manager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    //[QueryProperty(nameof(SourcePlaylists), nameof(SourcePlaylists))]
    public class SelectTargetPlaylistViewModel : BaseViewModel
    {
        private IEnumerable<IPlaylist> _sourcePlaylistsList;
        public ObservableCollection<IPlaylist> SourcePlaylists { get; set; }
        //private string _sourcePlaylists;
        //public string SourcePlaylists
        //{
        //    get => _sourcePlaylists;
        //    set
        //    {
        //        _sourcePlaylists = value;
        //        OnPropertyChanged();
        //        ConvertSourcePlaylists(_sourcePlaylists);
        //    }
        //}

        //private void ConvertSourcePlaylists(string playlists)
        //{
        //    var sourcePlaylists =  JsonConvert.DeserializeObject<List<IPlaylist>>(playlists);
        //    if(_sourcePlaylistsList != null)
        //    {
        //        _sourcePlaylistsList = new ObservableCollection<IPlaylist>(sourcePlaylists);
        //    }

        //}

        ISpotifyDataService _spotifyDataService;

        public ObservableCollection<IPlaylist> Playlists { get; }
        public int SelectedItemIndex { get; set; }

        public SelectTargetPlaylistViewModel()
        {
            IsBusy = true;

            Playlists = new ObservableCollection<IPlaylist>();
            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();

            IUserSelection userSelection = Startup.ServiceProvider.GetService<IUserSelection>();
            _sourcePlaylistsList = userSelection.SourcePlaylists;

            IsBusy = false;
        }

        private async void LoadPlaylists()
        {
            IsBusy = true;
            try
            {
                Playlists.Clear();
                var playlists = await _spotifyDataService.GetPlaylistsAsync(AppSecret.UserId);
                foreach (var playlist in playlists)
                {
                    Playlists.Add(playlist as Playlist);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }




        public override async Task Initialize()
        {

            await base.Initialize();
            LoadPlaylists();
        }
    }
}
