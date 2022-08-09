using Spotify_Manager.ViewModels;
using SpotifyAPI.Web;
using System;
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
            _viewmodel.SelectedItem = picker.SelectedItem as SimplePlaylist;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            PlaylistPicker.IsEnabled = !e.Value;
            EntryPLaylistName.IsEnabled = e.Value;
        }
    }
}