using System;

namespace Lab.Models
{
    public class ResponseAPI<T> where T: new()
    {
        public int? Status { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; } = "Phản hồi không xác định";
        public string HtmlWithValidate { get; set; } = string.Empty;
        public T? Data { get; set; } = new T();
    }
}
