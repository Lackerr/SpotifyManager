using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Secrets;
using Spotify_Manager.Services;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    public class DeleteDublicatesViewModel : BaseViewModel
    {
        private readonly ISpotifyDataStorage _spotifyDataStorage;
        private ISpotifyDataService _spotifyDataService;
        public ObservableCollection<SimplePlaylist> Playlists { get; private set; }

        private SimplePlaylist _selectedPlaylist;
        public Command DeleteCommand { get; }
        public Command LoadPlaylistsCommand { get; }
        public DeleteDublicatesViewModel()
        {
            IsBusy = true;

            Title = "Dubletten entfernen";

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();

            Playlists =  _spotifyDataStorage.UsersPlaylists;

            DeleteCommand = new Command(async () => await ExecuteDeleteCommand());
            LoadPlaylistsCommand = new Command(async () => await ExecuteLoadPlaylistsCommand());

            IsBusy = false;
        }

        private async Task LoadPlaylists()
        {
            IsBusy = true;
            try
            {
                Playlists.Clear();
                var playlists = await _spotifyDataStorage.RefreshUsersPlaylists();
                foreach (var playlist in playlists)
                {
                    if (playlist.Owner.Id == AppSecret.UserId)
                        Playlists.Add(playlist);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLoadPlaylistsCommand()
        {
            IsBusy = true;
            await LoadPlaylists();
            IsBusy = false;
        }

        private async Task ExecuteDeleteCommand()
        {
            try
            {
                IsBusy = true;
                await _spotifyDataService.PlaylistDeleteDublicates(_selectedPlaylist.Id);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public SimplePlaylist SelectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                SetProperty(ref _selectedPlaylist, value);
                OnPropertyChanged();
            }
        }

        public async override Task Initialize()
        {
            await base.Initialize();
        }
    }
}
