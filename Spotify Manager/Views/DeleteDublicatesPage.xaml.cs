using Spotify_Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spotify_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteDublicatesPage : ContentPage
    {
        private DeleteDublicatesViewModel _viewModel;
        public DeleteDublicatesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DeleteDublicatesViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.Initialize();
        }
    }
}