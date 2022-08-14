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
            _viewmodel.IsValid = true;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            PlaylistPicker.IsVisible = !e.Value;
            EntryPLaylistName.IsVisible= e.Value;
            Validate();

        }

        private void EntryPLaylistName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        private void Validate()
        {
            _viewmodel.IsValid = true;
            if (((EntryPLaylistName.Text == "" ||EntryPLaylistName.Text == null)  && _viewmodel.IsNewPlaylist) || !_viewmodel.IsNewPlaylist && PlaylistPicker.SelectedItem == null)
            {
                _viewmodel.IsValid = false;
            }
        }
    }
}