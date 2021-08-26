using System;

namespace GrabberClient.Internals.Exceptions
{
    public class VkException : AppException
    {
        public VkException(string message) : base(message) { }

        public VkException(string message, Exception innerException) : base (message, innerException) { }
    }
}
