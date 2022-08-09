using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spotify_Manager.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        ISpotifyDataStorage _spotifyDataStorage;
        public MainPageViewModel()
        {
            IsBusy = true;

            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();

            OnPropertyChanged();
            IsBusy = false;
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
