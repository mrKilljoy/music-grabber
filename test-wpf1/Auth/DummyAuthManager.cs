using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_wpf1.Contracts;
using test_wpf1.Models;

namespace test_wpf1.Auth
{
    public sealed class DummyAuthManager : IAuthManager
    {
        public Task<AuthResponse> AuthenticateAsync(ServiceCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.AppToken))
                throw new ArgumentException(nameof(credentials.AppToken));

            return Task.FromResult(new AuthResponse(true, new Dictionary<string, object> { { "dummyToken", Guid.NewGuid() } } ));
        }
    }
}
