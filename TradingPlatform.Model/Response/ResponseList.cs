

using System;
using System.Collections.Generic;

namespace TradingPlatform.Model
{
    public class ResponseList<T>
    {
        public bool result { get; set; }

        public string message { get; set; }

        public T test { get; set; }
        public List<T> data { get; set; }
    }
}