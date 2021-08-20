using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Internals;
using GrabberClient.Internals.Exceptions;
using GrabberClient.Models;
using VkNet;
using VkNet.Exception;
using VkNet.Model;

namespace GrabberClient.Auth
{
    public sealed class VkAuthManager : IAuthManager
    {
        #region Fields

        private const string FootprintFileName = ".vkfp";

        private readonly VkApi apiInstance;

        #endregion

        #region .ctr

        public VkAuthManager(VkApi api)
        {
            this.apiInstance = api;
        }

        #endregion

        #region Methods

        public async Task<AuthResponse> AuthenticateAsync(ServiceCredentials credentials)
        {
            string storedFootprint = RetrieveAuthFootprint();

            try
            {
                var authParams = new ApiAuthParams();
                if (string.IsNullOrEmpty(storedFootprint))
                {
                    authParams.Login = credentials.Login;
                    authParams.Password = credentials.Password;
                }
                else
                {
                    authParams.AccessToken = storedFootprint;
                }

                await this.apiInstance.AuthorizeAsync(authParams).ConfigureAwait(false);
            }
            catch (CaptchaNeededException cee)
            {
                throw new VkException(AppConstants.Messages.CaptchaRequiredMessage, cee);
            }
            catch (Exception)
            {
                throw;
            }

            var response = new AuthResponse(this.apiInstance.IsAuthorized, this.apiInstance.IsAuthorized ?
                new Dictionary<string, object> { ["api"] = this.apiInstance } :
                null);

            //  todo: check token lifetime?
            if (this.apiInstance.IsAuthorized)
                SaveAuthFootprint(this.apiInstance.Token);

            return response;
        }

        private string RetrieveAuthFootprint()
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), FootprintFileName);

            if (!File.Exists(fileName))
                return null;

            using (var streamReader = new StreamReader(fileName))
            {
                return streamReader.ReadToEnd();
            }
        }

        private void SaveAuthFootprint(string token)
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), FootprintFileName);

            FileStream fs;
            if (!File.Exists(fileName))
            {
                fs = File.Create(Path.Combine(Directory.GetCurrentDirectory(), FootprintFileName));
            }
            else
            {
                fs = File.Open(
                    Path.Combine(Directory.GetCurrentDirectory(), FootprintFileName), 
                    FileMode.Create);
            }

            var bytes = Encoding.UTF8.GetBytes(token);
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush(true);

            fs.Dispose();
        }

        #endregion
    }
}
