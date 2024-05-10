namespace AtmiraPayNet.Shared.DTO
{
    public class ResponseDTO<T>
    {
        public required int StatusCode { get; set; }
        public T? Value { get; set; }
        public string? Message { get; set; }
    }
}
