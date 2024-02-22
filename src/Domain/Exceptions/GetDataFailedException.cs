namespace Domain.Exceptions
{
    public class GetDataFailedException : Exception
    {
        public GetDataFailedException() { }
        public GetDataFailedException(string message) : base(message) { }
        public GetDataFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
