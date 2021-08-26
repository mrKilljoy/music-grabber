using System.Collections.Generic;

namespace GrabberClient.Helpers
{
    public sealed class TrackDownloadResult
    {
        public TrackDownloadResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
            OperationData = new Dictionary<string, object>();
        }

        public TrackDownloadResult(bool isSuccess, IDictionary<string, object> operationData = null) : this(isSuccess)
        {
            if (operationData is not null)
                OperationData = operationData;
        }

        public bool IsSuccess { get; }

        public IDictionary<string, object> OperationData { get; }
    }
}
