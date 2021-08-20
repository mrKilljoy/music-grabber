using System;

namespace GrabberClient.Internals.Exceptions
{
    public sealed class VkServiceResolvingException : Exception
    {
        public VkServiceResolvingException(string message) : base(message) { }
    }
}
