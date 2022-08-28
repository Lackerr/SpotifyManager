using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Models;
using Spotify_Manager.Secrets;
using Spotify_Manager.Services;
using SpotifyAPI.Web;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    public class SortPlaylistViewModel : BaseViewModel
    {
        private readonly ISpotifyDataStorage _spotifyDataStorage;
        private readonly ISpotifyDataService _spotifyDataService;

        private ObservableCollection<SimplePlaylist> _playlists;
        private ObservableCollection<SortingType> _sortingTypes;

        private bool _isValid = false;
        private SimplePlaylist _selectedPlaylist;
        private SortingType _selectedSortingType;
        private readonly Sorting _sorting;

        public Command SortCommand { get; }
        public Command LoadPlaylistsCommand { get; }

        public SortPlaylistViewModel()
        {
            IsBusy = true;

            Title = "Playlist sortieren";

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            _playlists = _spotifyDataStorage.UsersPlaylists;

            _sorting = new Sorting();
            SortingTypes = _sorting.GetSortingTypes();

            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();

            SortCommand = new Command(async () => await ExecuteSortCommand());
            LoadPlaylistsCommand = new Command(async () => await ExecuteLoadPlaylistCommand());

            IsBusy = false;
        }

        private async Task ExecuteLoadPlaylistCommand()
        {
            IsBusy = true;
            SelectedPlaylist = null;

            Playlists.Clear();
            try
            {
                var playlists = await _spotifyDataStorage.RefreshUsersPlaylists();
                var userId = await _spotifyDataService.GetCurrentUserId();

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

        private async Task ExecuteSortCommand()
        {
            try
            {
                IsBusy = true;
                await _sorting.PlaylistSort(_selectedSortingType, _selectedPlaylist);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<SimplePlaylist> Playlists
        {
            get => _playlists;
            set
            {
                if (value != _playlists)
                {
                    SetProperty(ref _playlists, value);
                    OnPropertyChanged();
                }
            }
        }

        public SimplePlaylist SelectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedPlaylist, value);
                    OnPropertyChanged();
                }
            }
        }

        public SortingType SelectedSortingType
        {
            get => _selectedSortingType;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedSortingType, value);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SortingType> SortingTypes
        {
            get => _sortingTypes;
            set
            {
                SetProperty(ref _sortingTypes, value);
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if(value != _isValid)
                {
                    SetProperty(ref _isValid, value);
                    OnPropertyChanged();
                }
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await ExecuteLoadPlaylistCommand();
        }
    }
}
