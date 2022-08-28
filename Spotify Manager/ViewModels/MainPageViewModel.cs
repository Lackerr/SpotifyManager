using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        readonly ISpotifyDataStorage _spotifyDataStorage;
        public Command OpenMergePlaylistCommand { get; }
        public Command OpenSortPlaylistCommand { get; }
        public Command OpenDeleteDubletsCommand { get; }

        public MainPageViewModel()
        {
            IsBusy = true;
            Title = "Spotify Manager";

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            OpenMergePlaylistCommand = new Command(() => OpenMergePlaylist());
            OpenSortPlaylistCommand = new Command(() => OpenSortPlaylist());
            OpenDeleteDubletsCommand = new Command(() => OpenDeleteDublets());

            OnPropertyChanged();
            IsBusy = false;
        }

        private void OpenDeleteDublets()
        {
            Shell.Current.GoToAsync(nameof(DeleteDublicatesPage));
        }

        private void OpenSortPlaylist()
        {
            Shell.Current.GoToAsync(nameof(SortPlaylistPage));
        }

        private void OpenMergePlaylist()
        {
            Shell.Current.GoToAsync(nameof(MergePlaylistsPage));
        }

        public async Task LoadData()
        {
            IsBusy = true;
            await _spotifyDataStorage.RefreshUsersPlaylists();
            IsBusy = false;
        }
        public override async Task Initialize()
        {

            await base.Initialize();
        }
    }
}
