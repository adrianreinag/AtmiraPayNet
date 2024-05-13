namespace AtmiraPayNet.Shared.DTO
{
    public class ResponseDTO<T>
    {
        required public int StatusCode { get; set; }
        public T? Value { get; set; }
        required public string Message { get; set; }
    }
}
