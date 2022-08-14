using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    public interface ISpotifyClientProvider
    {
        Task<SpotifyClient> CreateSpotifyClient();
    }
}
