using System.Threading.Tasks;
using test_wpf1.Helpers;
using test_wpf1.Models;

namespace test_wpf1.Contracts
{
    /// <summary>
    /// A manager to retrieve musical records from an external provider.
    /// </summary>
    public interface IMusicDownloadManager
    {
        Task<TrackDownloadResult> DownloadAsync(Track track, bool overwrite = false);
    }
}
