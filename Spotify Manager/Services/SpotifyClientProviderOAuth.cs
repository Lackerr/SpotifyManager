﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;
using Spotify_Manager.Models;
using System.IO;
using Spotify_Manager.Secrets;
using System.Text.Json;
using Xamarin.Essentials;

namespace Spotify_Manager.Services
{
    public class  SpotifyClientProviderOAuth : ISpotifyClientProvider
    {
        private static PKCETokenModel _pKCETokenModel = new PKCETokenModel();
        private static string _tokenConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        private static string _tokenConfigFile = Path.Combine(_tokenConfigDir, "auth.json");



        public async Task<SpotifyClient> CreateSpotifyClient()
        {

            string clientId = AppSecret.clientId;
            if (File.Exists(_tokenConfigFile))
            {
                string tokenConfigContent = File.ReadAllText(_tokenConfigFile);
                var deserializedJson = JsonSerializer.Deserialize<PKCETokenModel>(tokenConfigContent);
                if (deserializedJson != null)
                {
                    var newResponses = await TryGetPKCERefreshTokenResponse(clientId, deserializedJson.RefreshToken);
                    if (newResponses != null)
                    {
                        deserializedJson.RefreshToken = newResponses.RefreshToken;
                        string serializedJsons = JsonSerializer.Serialize(deserializedJson);
                        File.WriteAllText(_tokenConfigFile, serializedJsons);
                        return new SpotifyClient(newResponses.AccessToken);
                    }
                }
            }

            var (verifier, challange) = PKCEUtil.GenerateCodes(120);

            Uri serverUri = new Uri("http://localhost:5000/auth");
            var loginRequest = new LoginRequest(serverUri, clientId, LoginRequest.ResponseType.Code)
            {
                CodeChallenge = challange,
                CodeChallengeMethod = "S256"
            };

            try
            {
                await Browser.OpenAsync(loginRequest.ToUri(), BrowserLaunchMode.External);
                //BrowserUtil.Open(loginRequest.ToUri());
            
            var getCallBackTask = await  GetCallbackFromServer(serverUri, clientId);

            //if (getCallBackTask)
            //{
            
                string authorizationCode = getCallBackTask;
                var newResponse = await new OAuthClient().RequestToken(new PKCETokenRequest(
                    clientId, authorizationCode, serverUri, verifier));
                _pKCETokenModel.RefreshToken = newResponse.RefreshToken;
                string serializedJson = JsonSerializer.Serialize(_pKCETokenModel);
                File.WriteAllText(_tokenConfigFile, serializedJson);
                return new SpotifyClient(newResponse.AccessToken);
            }
            catch(Exception x)
            {
                var f = x.Message;
            }
            //}
            //else
            //{
            //    return null;
            //}


            return null;
        }

        private async Task<string> GetCallbackFromServer(Uri redirectUri, string clientId)
        {
            var embededOAuthServer = new EmbedIOAuthServer(redirectUri, 5000);
            await embededOAuthServer.Start();
            string authorizationCode = string.Empty;

            embededOAuthServer.AuthorizationCodeReceived += async (sender, response) =>
            {
                await embededOAuthServer.Stop();
                authorizationCode = response.Code;
            };

            while (authorizationCode == string.Empty)
            {
                 await Task.Delay(1000);
            }
            return authorizationCode;
        }

        private static async Task<PKCETokenResponse> TryGetPKCERefreshTokenResponse(string clientId, string refreshToken)
        {
            try
            {
                return await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(AppSecret.clientId, refreshToken));
            }
            catch (APIException)
            {
                return null;
            }
        }

        public Task<string> GetAccessToken()
        {
            throw new NotImplementedException();
        }
    }
}
