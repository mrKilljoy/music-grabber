using GrabberClient.Contracts;

namespace GrabberClient.Models
{
    public class VkServiceCredentials : IServiceCredentials
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string AppToken { get; set; }
    }
}
