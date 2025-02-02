using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading_bot_WPF.Market
{
    internal abstract class Order
    {
        public string OrderId;
        public string Ticker;
        public decimal Price;
        public int Quantity;

        public Order (string OrderId, string Ticker, decimal Price, int Quantity)
        {
            this.OrderId = OrderId;
            this.Ticker = Ticker;
            this.Price = Price;
            this.Quantity = Quantity;
        }

        internal class OrderDone : Order
        {

            public OrderDone(string OrderId, string Ticker, decimal ExecutedPrice, int ExecutedQuantity) : base (OrderId, Ticker, ExecutedPrice, ExecutedQuantity)
            {
            }
        }
    }

    

    
}
