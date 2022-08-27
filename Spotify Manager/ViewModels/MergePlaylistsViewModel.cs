using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using SpotifyAPI.Web;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    internal class MergePlaylistsViewModel : BaseViewModel
    {
        private readonly ISpotifyDataStorage _spotifyDataStorage;
        private ObservableCollection<object> _selectedPlaylists;
        private bool _isSelected = false;

        public ObservableCollection<SimplePlaylist> Playlists { get; }

        public Command LoadPlaylistsCommand { get; }
        public Command SelectionChangedCommand { get; }
        public Command ContinueCommand { get; }
        public Command SelectAllCommand { get; }



        public MergePlaylistsViewModel()
        {
            IsBusy = true;
            Title = "Playlists zusammenführen";

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            Playlists = _spotifyDataStorage.UsersPlaylists;

            SelectedPlaylists = new ObservableCollection<object>();
            _selectedPlaylists = new ObservableCollection<object>();

            LoadPlaylistsCommand = new Command(async () => await ExecuteLoadPlaylistsCommand());
            SelectionChangedCommand = new Command(() => ExecuteSelectionChangedCommand());
            ContinueCommand = new Command(() => ExecuteContinue());
            SelectAllCommand = new Command(() => ExecuteSelectAllCommand());


            OnPropertyChanged();
            IsBusy = false;
        }

        private void ExecuteSelectionChangedCommand()
        {
            IsSelected = false;
            if (SelectedPlaylists.Count > 0)
            {
                IsSelected = true;
            }
            OnPropertyChanged();
        }

        private void ExecuteSelectAllCommand()
        {
            _selectedPlaylists.Clear();
            foreach (var item in Playlists)
            {
                _selectedPlaylists.Add(item);
            }
        }

        private async Task ExecuteLoadPlaylistsCommand()
        {
            IsBusy = true;
            SelectedPlaylists.Clear();
            Playlists.Clear();
            try
            {
                var playlists = await _spotifyDataStorage.RefreshUsersPlaylists();
                foreach (var playlist in playlists)
                {
                    Playlists.Add(playlist);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ExecuteContinue()
        {
            IUserSelection userSelection = Startup.ServiceProvider.GetService<IUserSelection>();
            var source = new ObservableCollection<SimplePlaylist>();

            foreach (var item in SelectedPlaylists)
            {
                source.Add(item as SimplePlaylist);
            }
            userSelection.SourcePlaylists = source;

            Shell.Current.GoToAsync(nameof(SelectTargetPlaylistPage));
        }

        public ObservableCollection<object> SelectedPlaylists
        {
            get => _selectedPlaylists;
            set
            {
                SetProperty(ref _selectedPlaylists, value);
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                OnPropertyChanged();
            }
        }


        public override async Task Initialize()
        {
            _selectedPlaylists.Clear();
            await base.Initialize();
        }
    }
}
