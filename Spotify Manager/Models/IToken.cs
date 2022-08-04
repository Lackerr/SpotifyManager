using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    internal interface IToken 
    {
        string access_token { get; set; }
        string token_type { get; set; }
        string expires_in { get; set; }
        string scope { get; set; }
    }
}
