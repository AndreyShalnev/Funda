using System.Net;

namespace FundaApi.Client.Data
{
    public class ActionResult<TResult>
    {
        public bool IsSuccess { get; }
        public TResult Result { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public ActionResult(bool isSuccess, TResult result, string message = null, Exception exception = null)
        { 
            IsSuccess = isSuccess;
            Result = result;
            Message = message;
            Exception = exception;
        }

        public static ActionResult<TResult> Success(TResult result) 
        {
            return new ActionResult<TResult>(true, result);
        }


        public static ActionResult<TResult> Failed(string message = null, Exception exception = null)
        {
            return new ActionResult<TResult>(false, default, message, exception);
        }

        public static ActionResult<TResult> FailedWithHttpStatusCode(HttpStatusCode httpStatusCode, string message = null)
        {
            var statusError = $"Request failed with StatusCode={httpStatusCode}";

            if (message != null)
            {
                return ActionResult<TResult>.Failed($"{statusError}, Reason={message}");
            }

            return ActionResult<TResult>.Failed(statusError);
        }
    }
}
