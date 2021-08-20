using System.Collections.Generic;
using System.Threading.Tasks;
using GrabberClient.Models;

namespace GrabberClient.Contracts
{
    /// <summary>
    /// A service to operate musical collection stored at external provider's storage.
    /// </summary>
    public interface IMusicService : IService
    {
        Task<IEnumerable<Track>> GetTracksAsync(string query);
    }
}
