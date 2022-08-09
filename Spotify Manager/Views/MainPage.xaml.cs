using Spotify_Manager.Merge;
using Spotify_Manager.Services;
using Spotify_Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spotify_Manager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MainPageViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.Initialize();
            await _viewModel.LoadData();
        }

        private async void Test()
        {
            ISpotifyDataProvider spotifyDataProvider = new SpotifyApiNetDataProvider();
            var playlists = await spotifyDataProvider.GetUsersPlaylistsAsync(Secrets.AppSecret.UserId);
        }
    }
}
