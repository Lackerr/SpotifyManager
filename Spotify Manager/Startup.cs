using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Spotify_Manager.Services;
using Spotify_Manager.Models;

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
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                //services.AddSingleton<IDataService, DummyDataService>();
            }
            else
            {
                //services.AddSingleton<IDataService, DataDataService>();
            }
            services.AddTransient<AppShell>();
            services.AddSingleton<ITokenProvider, TokenProviderDemo>();
            services.AddSingleton<ISpotifyDataProvider, SpotifyDataProvider>();
            services.AddSingleton<ISpotifyDataService, SpotifyDataService>();
            services.AddTransient<Token>();
            services.AddTransient<Playlist>();
            services.AddTransient<TrackInfromation>();
            services.AddTransient<Track>();

        }
    }
}
