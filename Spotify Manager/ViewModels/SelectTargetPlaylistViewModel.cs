using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Secrets;
using Spotify_Manager.Services;
using Spotify_Manager.Views;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{

    public class SelectTargetPlaylistViewModel : BaseViewModel
    {

        private readonly ISpotifyDataStorage _spotifyDataStorage;

        private bool _isNewPlaylist = false;
        private bool _isValid = false;
        private string _newPlaylistName = "";

        public ObservableCollection<SimplePlaylist> Playlists { get; private set; }
        public SimplePlaylist SelectedItem { get; set; }

        public Command ContinueCommand { get; }

        public SelectTargetPlaylistViewModel()
        {
            IsBusy = true;

            Title = "Playlists zusammenführen";
            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            Playlists = _spotifyDataStorage.UsersPlaylists;

            ContinueCommand = new Command(() => ExecuteContinue());

            IsBusy = false;
        }

        private async void ExecuteContinue()
        {
            IsBusy = true;
            IUserSelection userSelection = Startup.ServiceProvider.GetService<IUserSelection>();
            if (_isNewPlaylist)
            {
                ISpotifyDataService dataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();
                var newPlaylist = await dataService.PlaylistCreate(_newPlaylistName);

                Playlists = (ObservableCollection<SimplePlaylist>)await _spotifyDataStorage.RefreshUsersPlaylists();
                SelectedItem = Playlists.Where(x => x.Id == newPlaylist.Id).First();

            }
            userSelection.DestinationPlaylist = SelectedItem;

            await Shell.Current.GoToAsync(nameof(ExecuteMergingPage));
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


        public bool IsNewPlaylist
        {
            get => _isNewPlaylist;
            set
            {
                if (value != _isNewPlaylist)
                {
                    _isNewPlaylist = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewPlaylistName
        {
            get => _newPlaylistName;
            set
            {
                if (value != _newPlaylistName)
                {
                    _newPlaylistName = value;
                    OnPropertyChanged();
                }
            }
        }

        public override async Task Initialize()
        {

            await base.Initialize();
            await LoadPlaylists();
        }
    }
}
