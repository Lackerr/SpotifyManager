using Spotify_Manager.Services;
using Spotify_Manager.Views;
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
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SelectTargetPlaylistPage), typeof(SelectTargetPlaylistPage));
            Routing.RegisterRoute(nameof(ExecuteMergingPage), typeof(ExecuteMergingPage));
            Routing.RegisterRoute(nameof(MainPage),typeof(MainPage));
            Routing.RegisterRoute(nameof(SortPlaylistPage), typeof(SortPlaylistPage));

                                         
        }
    }
}