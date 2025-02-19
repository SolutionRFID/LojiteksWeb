namespace LojiteksApi.Models
{
    public class ResponseResultMessages<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public bool isSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
    }
}
