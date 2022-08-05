using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Models;
using Spotify_Manager.Secrets;
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
        ISpotifyDataService _spotifyDataService;
        public ObservableCollection<Playlist> Playlists { get; }

        private ObservableCollection<object> _selectedPlaylists;


        public Command LoadPlaylistsCommand { get; }
        public Command ContinueCommand { get; }
        public Command SelectAllCommand { get; }



        public MergePlaylistsViewModel()
        {
            IsBusy = true;
            Title = "Playlists zusammenführen";
            Playlists = new ObservableCollection<Playlist>();

            SelectedPlaylists = new ObservableCollection<object>();
            _selectedPlaylists = new ObservableCollection<object>();

            ContinueCommand = new Command(() =>  ExecuteContinue());
            LoadPlaylistsCommand = new Command(async () => await ExecuteLoadPlaylistsCommand());
            SelectAllCommand = new Command(() => ExecuteSelectAllCommand());

            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();

            OnPropertyChanged();
            IsBusy = false;
        }

        private void ExecuteSelectAllCommand()
        {
            IsBusy = true;
            SelectedPlaylists.Clear();
            foreach (var item in Playlists)
            {
                SelectedPlaylists.Add(item);
            }

        }

        private async Task ExecuteLoadPlaylistsCommand()
        {
            IsBusy = true;
            try
            {
                SelectedPlaylists.Clear();
                Playlists.Clear();
                var playlists = await _spotifyDataService.GetPlaylistsAsync(AppSecret.UserId);
                foreach (var playlist in playlists)
                {
                    Playlists.Add(playlist as Playlist);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private  void ExecuteContinue()
        {
            IUserSelection userSelection = Startup.ServiceProvider.GetService<IUserSelection>();
            ObservableCollection<IPlaylist> source = new ObservableCollection<IPlaylist>(userSelection.SourcePlaylists);

            source.Clear();

            foreach (var item in SelectedPlaylists)
            {
                source.Add(item as IPlaylist);
            }
            userSelection.SourcePlaylists = source;
            //string jsonStr = JsonConvert.SerializeObject(list);

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



        public override async Task Initialize()
        {
            _selectedPlaylists.Clear();
            await base.Initialize();
            await ExecuteLoadPlaylistsCommand();
        }
    }
}
