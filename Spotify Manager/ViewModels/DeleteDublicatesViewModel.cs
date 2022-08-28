using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Services;
using SpotifyAPI.Web;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    public class DeleteDublicatesViewModel : BaseViewModel
    {
        private readonly ISpotifyDataStorage _spotifyDataStorage;
        private readonly ISpotifyDataService _spotifyDataService;
        private SimplePlaylist _selectedPlaylist;
        private bool _isValid = false;

        public ObservableCollection<SimplePlaylist> Playlists { get; private set; }
        public Command DeleteCommand { get; }
        public Command LoadPlaylistsCommand { get; }

        public DeleteDublicatesViewModel()
        {
            IsBusy = true;

            Title = "Dubletten entfernen";

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();

            Playlists = _spotifyDataStorage.UsersPlaylists;

            DeleteCommand = new Command(async () => await ExecuteDeleteCommand());
            LoadPlaylistsCommand = new Command(async () => await ExecuteLoadPlaylistsCommand());

            IsBusy = false;
        }

        private async Task LoadPlaylists()
        {
            IsBusy = true;
            try
            {
                var userId = await _spotifyDataService.GetCurrentUserId();
                Playlists.Clear();

                var playlists = await _spotifyDataStorage.RefreshUsersPlaylists();

                foreach (var playlist in playlists)
                {
                    if (playlist.Owner.Id == userId)
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
            try
            {
                IsBusy = true;
                await LoadPlaylists();
            }
            finally
            {
                IsBusy = false;
            }
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

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    SetProperty(ref _isValid, value);
                    OnPropertyChanged();
                }
            }
        }
        public async override Task Initialize()
        {
            await base.Initialize();
            await ExecuteLoadPlaylistsCommand();
        }
    }
}
