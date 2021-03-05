using System.Threading.Tasks;
using test_wpf1.Auth;
using test_wpf1.Models;

namespace test_wpf1.Contracts
{
    /// <summary>
    /// A manager to conduct authentication procedure.
    /// </summary>
    public interface IAuthManager
    {
        Task<AuthResponse> AuthenticateAsync(ServiceCredentials credentials); 
    }
}
