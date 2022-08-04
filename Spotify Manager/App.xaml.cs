using Spotify_Manager.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spotify_Manager
{
    public partial class App : Application
    {
        public App()
        {
            var shell = Startup.ServiceProvider.GetService<AppShell>();
            InitializeComponent();
            MainPage = shell;
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
