namespace LojiteksApi.Models
{
    public class ResponseResultMessage<T>
    {
        public T Data { get; set; }
        public bool isSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
    }
}
