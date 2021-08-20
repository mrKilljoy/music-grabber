using System;

namespace GrabberClient.Internals.Exceptions
{
    public class VkException : Exception
    {
        public VkException(string message, Exception innerException) : base (message, innerException) { }
    }
}
