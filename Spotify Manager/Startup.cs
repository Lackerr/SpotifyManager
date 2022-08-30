using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Spotify_Manager.DataStorage;
using Spotify_Manager.Models;
using Spotify_Manager.Services;
using System;
using Xamarin.Essentials;

namespace Spotify_Manager
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static App Init(Action<HostBuilderContext, IServiceCollection>
            configurePlattformServices)
        {
            var host = new HostBuilder().ConfigureHostConfiguration(c =>
            {
                c.AddCommandLine(new[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                c.AddJsonFile(new EmbeddedFileProvider(typeof(Startup).Assembly, typeof(Startup).Namespace), "appsettings.json", false, false);
            })
                .ConfigureServices((c, x) =>
                {
                    configurePlattformServices(c, x);
                    ConfigureServices(c, x);
                })
                .Build();

            ServiceProvider = host.Services;
            return new App();
        }

        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddTransient<AppShell>();
            services.AddSingleton<ISpotifyDataStorage, SpotifyDataStorage>();
            services.AddSingleton<ISpotifyDataProvider, SpotifyApiNetDataProvider>();
            services.AddSingleton<ISpotifyDataService, SpotifyDataService>();
            services.AddSingleton<IUserSelection, UserSelection>();
            services.AddSingleton<ISpotifyClientProvider, SpotifyClientProviderOAuth>();
            services.AddTransient<Token>();
        }
    }
}
