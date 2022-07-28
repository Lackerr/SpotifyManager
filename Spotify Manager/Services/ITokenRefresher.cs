using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Manager.Services
{
    internal interface ITokenRefresher
    {
        string GetRefreshToken();
    }
}
