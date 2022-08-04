using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Spotify_Manager.Models;
using Spotify_Manager.Secrets;

namespace Spotify_Manager.Services
{
    internal class TokenProviderDemo : ITokenProvider
    {


        public async Task<string> GetAccessToken()
        {
            string url = AppSecret.TokenUrl;
            string base64 = AppSecret.Base64;
            string refreshToken = AppSecret.RefreshToken;
            var result = "";

            var values = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                {"refresh_token" , refreshToken}
            };
            var data = new FormUrlEncodedContent(values);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64);

            try
            {
                var response =  client.PostAsync(url, data).Result;
                result =  response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception x)
            {
                var ex = x.Message;
                return String.Empty;

            }

            string token = GetAccessTokenFromResponse(result);
            return token;
        }

        private string GetAccessTokenFromResponse(string result)
        {
            IToken ftoken = Startup.ServiceProvider.GetService<IToken>();
            var json = JsonConvert.DeserializeObject<Token>(result);
            var token = json.access_token;
            return token;
        }


        //public void GetRefreshToken()
        //{
        //    var (verifier, challange) = PKCEUtil.GenerateCodes();
        //    _verifier = verifier;
        //    _challange = challange;

        //}

        //public Uri GerneratingLoginUri()
        //{
        //    var loginRequest = new LoginRequest(new Uri("http://localhost:5000/callback"), _clientId, LoginRequest.ResponseType.Code)
        //    {
        //        CodeChallengeMethod = "S256",
        //        CodeChallenge = _challange,
        //        Scope = new[] { Scopes.PlaylistModifyPrivate, Scopes.PlaylistReadPrivate}
        //    };
        //    //WebRequest request = WebRequest.Create(
        //    //    loginRequest.ToUri().ToString());

        //    //WebResponse response = request.GetResponse();

        //    //using(Stream dataStream = response.GetResponseStream())
        //    //{
        //    //    StreamReader reader = new StreamReader(dataStream);
        //    //    string responseText = reader.ReadToEnd();
        //    //}
        //    Browser.OpenAsync(loginRequest.ToUri(), BrowserLaunchMode.SystemPreferred);
        //    return loginRequest.ToUri();


        //}

        //public async Task<SpotifyClient> GetCallback(string code)
        //{
        //    var initialResponse = await new OAuthClient().RequestToken(
        //        new PKCETokenRequest(_clientId, "AQCRPRe7587Fy6i0SxZyoe6u-C8QR9PD1wvrzWIj07_OE6KfTqw9Tv7Rv0FMzZoJBPSOe0ocYfMAc77MytGslAAKaTfkwBoafDRKP93k0FUedqy2VO7QW5VavXajBuQCzHvSJCUlmbr6t9DZWBqHGK8cn2KwATkkMpceMKF1B19KUZddVLQGOe5fFghkLE3sZumhzcoPd8fffG2Rpyew3QpWmCLFw-sFMyN8pfqPZg-RDM9V4_Qh8-d7nL5VupYwGR_8u4j9yzicZEmpSE9YpmrwyPOmXAcjj-PeRcRyJceE4uEi-w", new Uri("http://localhost:5001/callback"), _verifier));

        //    _refreshToken = initialResponse.RefreshToken;

        //    var spotify = new SpotifyClient(initialResponse.AccessToken);
        //    return spotify;
        //}

        //public async Task<SpotifyClient> GetRefresh()
        //{
        //    var newReponse = await new OAuthClient().RequestToken(
        //        new PKCETokenRefreshRequest(_clientId, _refreshToken));

        //    return new SpotifyClient(newReponse.AccessToken);
        //}
    }
}
