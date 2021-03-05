using System;
using System.Threading.Tasks;
using test_wpf1.Contracts;

namespace test_wpf1.Services
{
    public sealed class DummyServiceManager : IServiceManager
    {
        public async Task<IService> GetServiceAsync(string name)
        {
            if (name == "music")
                return new DummyMusicService();
            else
                throw new ArgumentException(nameof(name));
        }
    }
}
