using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using test_wpf1.Configuration;
using test_wpf1.Contracts;
using test_wpf1.Models;

namespace test_wpf1.Helpers
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
