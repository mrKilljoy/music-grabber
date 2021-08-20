using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using test_wpf1.Contracts;
using test_wpf1.Internals.Exceptions;

namespace test_wpf1.Services
{
    public sealed class VkServiceManager : IServiceManager
    {
        private readonly IServiceProvider serviceProvider;

        public VkServiceManager(IServiceProvider serviceProvider) 
            => this.serviceProvider = serviceProvider;

        public Task<IService> GetServiceAsync(string name)
        {
            switch (name)
            {
                case "music":
                    return Task.FromResult((IService)this.serviceProvider.GetService<IMusicService>());

                default:
                    throw new VkServiceResolvingException("Unknown service name");
            }
        }
    }
}
