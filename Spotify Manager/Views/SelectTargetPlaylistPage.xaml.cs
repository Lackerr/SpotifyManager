using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify_Manager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spotify_Manager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectTargetPlaylistPage : ContentPage
    {
        private SelectTargetPlaylistViewModel _viewmodel;

        public SelectTargetPlaylistPage()
        {
            InitializeComponent();
            BindingContext = _viewmodel = new SelectTargetPlaylistViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewmodel.Initialize();
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var index = picker.SelectedIndex;
            
            if(index != -1)
            {
                _viewmodel.SelectedItemIndex = index;
            }
        }
    }
}