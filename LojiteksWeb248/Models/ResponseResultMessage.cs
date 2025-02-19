namespace Skote.Models
{
    public class ResponseResultMessage<T>
    {
        public bool isSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
