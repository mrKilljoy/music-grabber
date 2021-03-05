using System.Collections.Generic;
using System.Threading.Tasks;
using test_wpf1.Models;

namespace test_wpf1.Contracts
{
    /// <summary>
    /// A service to operate musical collection stored at external provider's storage.
    /// </summary>
    public interface IMusicService : IService
    {
        Task<IEnumerable<Track>> GetTracksAsync(string query);
    }
}
