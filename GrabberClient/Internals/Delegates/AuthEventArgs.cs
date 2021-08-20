using System;
using GrabberClient.Auth;

namespace GrabberClient.Internals.Delegates
{
    public class AuthEventArgs : EventArgs
    {
        public AuthEventArgs() : base() => AuthResults = new AuthResponse(default);

        public AuthEventArgs(AuthResponse response) => AuthResults = response;

        public AuthResponse AuthResults { get; }
    }
}
