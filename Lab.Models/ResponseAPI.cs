using System;

namespace Lab.Models
{
    public class ResponseAPI<T>
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string HtmlWithValidate { get; set; }
        public T Data { get; set; }
    }
}
