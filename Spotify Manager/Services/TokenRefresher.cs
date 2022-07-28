using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using Spotify_Manager;
using System.Net;
using System.IO;

namespace Spotify_Manager.Services
{
    internal class TokenRefresher
    {
        string _verifier = string.Empty;
        string _challange = string.Empty;
        string _clientId = "d0e10dce08c14a6c9ddb4274e5bf9630";
        string _refreshToken = string.Empty;


        public TokenRefresher()
        {
            GetRefreshToken();
        }
        public void GetRefreshToken()
        {
            var (verifier, challange) = PKCEUtil.GenerateCodes();
            _verifier = verifier;
            _challange = challange;

        }

        public Uri GerneratingLoginUri()
        {
            var loginRequest = new LoginRequest(new Uri("http://localhost:5000/callback"), _clientId, LoginRequest.ResponseType.Code)
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = _challange,
                Scope = new[] { Scopes.PlaylistModifyPrivate, Scopes.PlaylistReadPrivate}
            };
            WebRequest request = WebRequest.Create(
                loginRequest.ToUri().ToString());

            WebResponse response = request.GetResponse();

            using(Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseText = reader.ReadToEnd();
            }
            return loginRequest.ToUri();


        }

        public async Task<SpotifyClient> GetCallback(string code)
        {
            var initialResponse = await new OAuthClient().RequestToken(
                new PKCETokenRequest(_clientId, "AQCRPRe7587Fy6i0SxZyoe6u-C8QR9PD1wvrzWIj07_OE6KfTqw9Tv7Rv0FMzZoJBPSOe0ocYfMAc77MytGslAAKaTfkwBoafDRKP93k0FUedqy2VO7QW5VavXajBuQCzHvSJCUlmbr6t9DZWBqHGK8cn2KwATkkMpceMKF1B19KUZddVLQGOe5fFghkLE3sZumhzcoPd8fffG2Rpyew3QpWmCLFw-sFMyN8pfqPZg-RDM9V4_Qh8-d7nL5VupYwGR_8u4j9yzicZEmpSE9YpmrwyPOmXAcjj-PeRcRyJceE4uEi-w", new Uri("http://localhost:5000/callback"), _verifier));

            _refreshToken = initialResponse.RefreshToken;

            var spotify = new SpotifyClient(initialResponse.AccessToken);
            return spotify;
        }

        public async Task<SpotifyClient> GetRefresh()
        {
            var newReponse = await new OAuthClient().RequestToken(
                new PKCETokenRefreshRequest(_clientId, _refreshToken));

            return new SpotifyClient(newReponse.AccessToken);
        }
    }
}
