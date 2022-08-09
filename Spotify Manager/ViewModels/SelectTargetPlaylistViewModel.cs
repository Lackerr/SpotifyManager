using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Secrets;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{

    public class SelectTargetPlaylistViewModel : BaseViewModel
    {

        private readonly ISpotifyDataStorage _spotifyDataStorage;

        private bool _isNewPlaylist = false;

        public ObservableCollection<SimplePlaylist> Playlists { get; private set; }
        public SimplePlaylist SelectedItem { get; set; }

        public Command ContinueCommand { get; }

        public SelectTargetPlaylistViewModel()
        {
            IsBusy = true;

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            Playlists = _spotifyDataStorage.UsersPlaylists;

            ContinueCommand = new Command(() => ExecuteContinue());

            IsBusy = false;
        }

        private void ExecuteContinue()
        {
            throw new NotImplementedException();
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

        public override async Task Initialize()
        {

            await base.Initialize();
            await LoadPlaylists();
        }
    }
}
