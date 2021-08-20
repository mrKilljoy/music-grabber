using System;
using test_wpf1.Auth;

namespace test_wpf1.Internals.Delegates
{
    public class AuthEventArgs : EventArgs
    {
        public AuthEventArgs() : base() => AuthResults = new AuthResponse(default);

        public AuthEventArgs(AuthResponse response) => AuthResults = response;

        public AuthResponse AuthResults { get; }
    }
}
