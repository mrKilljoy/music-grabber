using GrabberClient.Internals.Delegates;
using System.Threading.Tasks;

namespace GrabberClient.Contracts
{
    public interface IAuthViewViewModel : IViewModel
    {
        event AuthEventHandler LoggedIn;

        Task Authorize(IServiceCredentials serviceCredentials);
    }
}
