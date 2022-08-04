using System;
using System.Collections.Generic;
using System.Text;

namespace Spotify_Manager.Models
{
    internal class Token : IToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }
}
