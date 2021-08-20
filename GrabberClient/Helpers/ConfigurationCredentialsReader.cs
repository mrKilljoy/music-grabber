using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Configuration;
using GrabberClient.Models;

namespace GrabberClient.Helpers
{
    public sealed class ConfigurationCredentialsReader : ICredentialsReader
    {
        private readonly CredentialsSection _credentialsSection;

        public ConfigurationCredentialsReader(IOptions<CredentialsSection> credentialsSection) 
            => _credentialsSection = credentialsSection.Value;

        public Task<ServiceCredentials> GetCredentialsAsync()
        {
            var credentials = new ServiceCredentials
            {
                Login = _credentialsSection.Login,
                Password = _credentialsSection.Password,
                AppToken = _credentialsSection.AppToken
            };

            return Task.FromResult(credentials);
        }
    }
}
