using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Data;

namespace Trading_bot_WPF.Market
{
    internal class OrderSpot : Order
    {
        public OrderSpot(string OrderId, string Ticker, decimal Price, int Quantity) : base(OrderId, Ticker, Price, Quantity)
        {
        }
    }

    internal class OrderSpotDone : OrderSpot
    {

        public OrderSpotDone(string OrderId, string Ticker, decimal ExecutedPrice, int ExecutedQuantity) : base(OrderId, Ticker, ExecutedPrice, ExecutedQuantity)
        {
        }
    }

    
}
