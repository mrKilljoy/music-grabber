using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Models;

namespace GrabberClient.Auth
{
    public sealed class DummyAuthManager : IAuthManager
    {
        public Task<AuthResponse> AuthenticateAsync(VkServiceCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.AppToken))
                throw new ArgumentException(nameof(credentials.AppToken));

            return Task.FromResult(new AuthResponse(true, new Dictionary<string, object> { { "dummyToken", Guid.NewGuid() } } ));
        }
    }
}
