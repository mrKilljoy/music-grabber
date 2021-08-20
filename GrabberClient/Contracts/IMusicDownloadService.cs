using System.Threading.Tasks;
using GrabberClient.Helpers;
using GrabberClient.Models;

namespace GrabberClient.Contracts
{
    /// <summary>
    /// A manager to retrieve musical records from an external provider.
    /// </summary>
    public interface IMusicDownloadService
    {
        Task<TrackDownloadResult> DownloadAsync(Track track);
    }
}
