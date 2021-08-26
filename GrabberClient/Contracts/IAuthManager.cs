using System.Threading.Tasks;
using GrabberClient.Auth;
using GrabberClient.Models;

namespace GrabberClient.Contracts
{
    /// <summary>
    /// A manager to conduct authentication procedure.
    /// </summary>
    public interface IAuthManager
    {
        Task<AuthResponse> AuthenticateAsync(VkServiceCredentials credentials); 
    }
}
