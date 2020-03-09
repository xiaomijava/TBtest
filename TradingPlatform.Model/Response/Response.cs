

namespace TradingPlatform.Model
{
    public class Response<T>
    {
        public bool result { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}