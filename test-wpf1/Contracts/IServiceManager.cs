using System.Threading.Tasks;

namespace test_wpf1.Contracts
{
    /// <summary>
    /// An intermediate manager to operate resources of an external provider.
    /// </summary>
    public interface IServiceManager
    {
        Task<IService> GetServiceAsync(string name);
    }
}
