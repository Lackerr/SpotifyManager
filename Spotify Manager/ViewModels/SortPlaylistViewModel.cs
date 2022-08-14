using Microsoft.Extensions.DependencyInjection;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Models;
using Spotify_Manager.Secrets;
using SpotifyAPI.Web;
using System.Collections.ObjectModel;
 using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager.ViewModels
{
    public class SortPlaylistViewModel : BaseViewModel
    {
        private readonly ISpotifyDataStorage _spotifyDataStorage;
        private ObservableCollection<SimplePlaylist> _playlists;
        private SimplePlaylist _selectedPlaylist;
        private ObservableCollection<SortingType> _sortingTypes;
        private Sorting _sorting;

        public Command SortCommand { get; }
        public Command LoadPlaylistsCommand { get; }

        public SortPlaylistViewModel()
        {
            IsBusy = true;

            Title = "Playlist sortieren";
            _spotifyDataStorage = Startup.ServiceProvider.GetService<ISpotifyDataStorage>();
            _playlists = _spotifyDataStorage.UsersPlaylists;
            _sorting = new Sorting();
            SortingTypes  = _sorting.GetSortingTypes() as ObservableCollection<SortingType>;

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
            //var list = mainList.Where(x => x.detailList.All(y => y.property));
        }

        private async Task ExecuteSortCommand()
        {

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

        public ObservableCollection<SortingType> SortingTypes
        {
            get => _sortingTypes;
            set
            {
                SetProperty(ref _sortingTypes, value);
                OnPropertyChanged();
            }
        }

        public override async Task Initialize()
        {

            await base.Initialize();
            await ExecuteLoadPlaylistCommand();
        }
    }
}
