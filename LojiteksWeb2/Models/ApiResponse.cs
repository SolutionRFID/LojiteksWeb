﻿namespace LojiteksWeb.Models
{
    public class ApiResponse<T>
    {
        public bool isSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
