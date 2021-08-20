using System.Threading.Tasks;
using GrabberClient.Models;

namespace GrabberClient.Contracts
{
    /// <summary>
    /// A helper to read login credentials for an external provider.
    /// </summary>
    public interface ICredentialsReader
    {
        Task<ServiceCredentials> GetCredentialsAsync();
    }
}
