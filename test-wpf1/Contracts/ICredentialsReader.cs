using System.Threading.Tasks;
using test_wpf1.Models;

namespace test_wpf1.Contracts
{
    /// <summary>
    /// A helper to read login credentials for an external provider.
    /// </summary>
    public interface ICredentialsReader
    {
        Task<ServiceCredentials> GetCredentialsAsync();
    }
}
