using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.Models;
using Spotify_Manager.Secrets;
using Spotify_Manager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.ViewModels
{
    internal class SelectTargetPlaylistViewModel : BaseViewModel
    {
        ISpotifyDataService _spotifyDataService;

        public ObservableCollection<IPlaylist> Playlists { get; }
        public int SelectedItemIndex { get; set; }

        public SelectTargetPlaylistViewModel()
        {
            IsBusy = true;

            Playlists = new ObservableCollection<IPlaylist>();
            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();


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
