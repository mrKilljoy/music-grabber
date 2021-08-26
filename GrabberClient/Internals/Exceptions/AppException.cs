using System;

namespace GrabberClient.Internals.Exceptions
{
    /// <summary>
    /// A base class for all app exceptions.
    /// </summary>
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, Exception innerException) : base(message, innerException) { }
    }
}
