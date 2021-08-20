using System.Collections.Generic;

namespace GrabberClient.Auth
{
    public sealed class AuthResponse
    {
        public AuthResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
            ResponseData = new Dictionary<string, object>();
        }

        public AuthResponse(bool isSuccess, IDictionary<string, object> responseData) : this(isSuccess)
        {
            ResponseData = responseData;
        }

        public bool IsSuccess { get; }

        public IDictionary<string, object> ResponseData { get; }
    }
}
