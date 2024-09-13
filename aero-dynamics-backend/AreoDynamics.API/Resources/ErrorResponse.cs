namespace AreoDynamics.API.Resources 
{
    public class ErrorResponse
    {
        public string StatusCode { get; set; }
        public string Detail { get; set; }
        public string StackTrace { get; set; }

        public ErrorResponse(string statusCode, string detail, string stackTrace)
        {
            StatusCode = statusCode;
            Detail = detail;
            StackTrace = stackTrace;
        }

        public ErrorResponse(Grpc.Core.RpcException ex)
        {
            StatusCode = ex.StatusCode.ToString();
            Detail = ex.Status.Detail;
            StackTrace = ex.StackTrace;
        }
    }

}
