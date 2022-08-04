using Spotify_Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spotify_Manager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MergePlaylistsPage : ContentPage
    {
        private MergePlaylistsViewModel _viewModel;
        public MergePlaylistsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MergePlaylistsViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.Initialize();
        }

    }
}