using Spotify_Manager.Merge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spotify_Manager
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private void BttnMerge_Clicked(object sender, EventArgs e)
        {
            FormMerge frmMerge = new FormMerge();
            
        }
    }
}
