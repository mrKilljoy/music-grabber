using System.Collections.Generic;

namespace test_wpf1.Helpers
{
    public sealed class TrackDownloadResult
    {
        public TrackDownloadResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
            OperationData = new Dictionary<string, object>();
        }

        public TrackDownloadResult(bool isSuccess, IDictionary<string, object> operationData) : this(isSuccess)
        {
            OperationData = operationData;
        }

        public bool IsSuccess { get; }

        public IDictionary<string, object> OperationData { get; }
    }
}
