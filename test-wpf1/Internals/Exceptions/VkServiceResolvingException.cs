using System;

namespace test_wpf1.Internals.Exceptions
{
    public sealed class VkServiceResolvingException : Exception
    {
        public VkServiceResolvingException(string message) : base(message) { }
    }
}
