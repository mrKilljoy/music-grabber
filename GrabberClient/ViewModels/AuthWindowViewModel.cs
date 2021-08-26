using GrabberClient.Contracts;
using GrabberClient.Internals.Delegates;
using GrabberClient.Internals.Exceptions;
using GrabberClient.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GrabberClient.ViewModels
{
    public sealed class AuthWindowViewModel : IAuthViewViewModel
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event AuthEventHandler LoggedIn;

        #endregion

        #region Fields

        //  todo: remove the reader?
        private readonly ICredentialsReader credentialsReader;
        private readonly IAuthManager authManager;

        #endregion

        #region .ctr

        public AuthWindowViewModel(ICredentialsReader credentialsReader, IAuthManager authManager)
        {
            this.credentialsReader = credentialsReader;
            this.authManager = authManager;
        }

        public async Task Authorize(IServiceCredentials serviceCredentials)
        {
            //  todo: handle the exception
            var credentials = serviceCredentials as VkServiceCredentials;
            if (credentials is null)
                throw new AppException("no credentials set");

            //  todo: replace with credentials from the arguments
            var response = await this.authManager
                .AuthenticateAsync(await this.credentialsReader.GetCredentialsAsync().ConfigureAwait(false))
                //.AuthenticateAsync(credentials)
                .ConfigureAwait(false);

            this.LoggedIn?.Invoke(this, new AuthEventArgs(response));
        }

        #endregion
    }
}
