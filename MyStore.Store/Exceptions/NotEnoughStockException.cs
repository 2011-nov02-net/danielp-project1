using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public class NotEnoughStockException : ArgumentException
    {
        public NotEnoughStockException(string message) : base(message)
        {
        }
    }
}
