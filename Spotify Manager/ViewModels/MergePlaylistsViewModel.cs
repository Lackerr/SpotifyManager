using Spotify_Manager.Models;
using Spotify_Manager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    internal class MergePlaylistsViewModel : BaseViewModel
    {
       public ObservableCollection<IPlaylist> Playlists { get; }
       

       public Command LoadPlaylistsCommand { get; }

        public MergePlaylistsViewModel()
        {
            Title = "Playlists zusammenführen";
            Playlists = new ObservableCollection<IPlaylist>();
            LoadPlaylistsCommand = new Command(async () => await ExecuteLoadPlaylistsCommand());
        }

        private async Task ExecuteLoadPlaylistsCommand()
        {
            IsBusy = true;

            try
            {
                Playlists.Clear();
                var playlists = await SpotifyDataService.GetPlaylistsAsync("dummy");
                foreach (var playlist in playlists)
                {
                    Playlists.Add(playlist);
                }




                TokenRefresher refresher = new TokenRefresher();
                var uri = refresher.GerneratingLoginUri();
                //var token = await refresher.GetCallback("");

            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async Task Initialize()
        {
            await ExecuteLoadPlaylistsCommand();
            await base.Initialize();
        }
    }
}
