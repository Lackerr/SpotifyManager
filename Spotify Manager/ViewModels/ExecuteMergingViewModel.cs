﻿using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Services;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    public class ExecuteMergingViewModel : BaseViewModel
    {
        private readonly ISpotifyDataService _spotifyDataService;
        private readonly IUserSelection _userSelection;

        private readonly ObservableCollection<SimplePlaylist> _sourcePlaylists;
        private readonly SimplePlaylist _targetPlaylist;

        private bool _waiting = true;
        private bool _finished = false;
        public Command StartMergingCommand { get; }
        public Command FinishedCommand { get; }
        public ExecuteMergingViewModel()
        {
            IsBusy = true;
            Title = "Playlists zusammenführen";

            _spotifyDataService = Startup.ServiceProvider.GetService<ISpotifyDataService>();
            _userSelection = Startup.ServiceProvider.GetService<IUserSelection>();

            _sourcePlaylists = (ObservableCollection<SimplePlaylist>)_userSelection.SourcePlaylists;
            _targetPlaylist = _userSelection.DestinationPlaylist;

            StartMergingCommand = new Command(async () => await ExecuteStartMergingCommand());
            FinishedCommand = new Command(async () => await ExecuteFinishedCommand());

            IsBusy = false;
        }


        public string TargetPlaylistName
        {
            get => _userSelection.DestinationPlaylist.Name;
            private set { }
        }

        public IEnumerable<SimplePlaylist> SourcePlaylists
        {
            get => _userSelection.SourcePlaylists;
            private set { }
        }

        private async Task ExecuteFinishedCommand()
        {
            await Shell.Current.GoToAsync("../../..");
        }
        

        private async Task ExecuteStartMergingCommand()
        {
            try
            {
                Waiting = false;
                IsBusy = true;
                await _spotifyDataService.MergePlaylists(_sourcePlaylists, _targetPlaylist);
                
            }
            finally
            {
                Finished = true;
                IsBusy = false;
            }
        }

        public bool Waiting
        {
            get => _waiting;
            set
            {
                SetProperty(ref _waiting, value);
                OnPropertyChanged();
            }
        }

        public bool Finished
        {
            get => _finished;
            set
            {
                SetProperty(ref _finished, value);
                OnPropertyChanged();
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
        }
    }
}
