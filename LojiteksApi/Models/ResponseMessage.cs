namespace LojiteksApi.Models
{
    public class ResponseMessage
    {
        public bool isSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public object Data { get; set; } = "";
    }
}
