using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading_bot_WPF.Market
{
    internal class OrderLimit : Order
    {
        public decimal stopLossPrice;
        public decimal takeProfitPrice;

        public OrderLimit (string OrderId, string Ticker, decimal Price, int Quantity, decimal stopLossPrice, decimal takeProfitPrice) : base (OrderId, Ticker, Price, Quantity)
        {
            this.stopLossPrice = stopLossPrice;
            this.takeProfitPrice = takeProfitPrice;
        }
    }

    internal class OrderLimitDone : OrderLimit
    {

        public OrderLimitDone(string OrderId, string Ticker, decimal Price, int Quantity, decimal stopLossPrice, decimal takeProfitPrice) : base(OrderId, Ticker, Price, Quantity, stopLossPrice, takeProfitPrice)
        {
        }
    }
}
