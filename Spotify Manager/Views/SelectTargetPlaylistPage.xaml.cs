using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spotify_Manager.Models;
using Spotify_Manager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spotify_Manager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class SelectTargetPlaylistPage : ContentPage
    {
        private SelectTargetPlaylistViewModel _viewmodel;
        public string SourcePlaylists { get; set; }
        public SelectTargetPlaylistPage()
        {
            InitializeComponent();
            BindingContext = _viewmodel = new SelectTargetPlaylistViewModel();
            //var f = JsonConvert.DeserializeObject<List<IPlaylist>>(SourcePlaylists);
            //var o = f;
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

            if (index != -1)
            {
                _viewmodel.SelectedItemIndex = index;
            }
        }
    }
}