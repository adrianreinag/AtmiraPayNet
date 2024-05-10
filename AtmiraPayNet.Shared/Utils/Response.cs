namespace AtmiraPayNet.Shared.Utils
{
    public class Response<T>
    {
        public required int StatusCode { get; set; }
        public T? Value { get; set; }
        public string? Message { get; set; }
    }
}
