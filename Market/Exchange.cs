using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Data;

namespace Trading_bot.Market
{
    internal class Exchange
    {
        public Price Price;

        public Exchange ()
        {
            Price = new Price("", "", "", -1); ;
        }

        public void OnPriceReceived(object sender, Price price)
        {
            //Console.WriteLine($"Exchange received price: {price.PriceValue}");
            Price = price;
        }

        public OrderSpotDone ProcessOrder (OrderSpot o)
        {
            if (o != null)
            {
                if (o.Price < Price.PriceValue * (decimal)1.02 && o.Price > Price.PriceValue * (decimal)0.98)
                {
                    return new OrderSpotDone(o.OrderId, o.Ticker, Price.PriceValue, o.Quantity);
                }
                return new OrderSpotDone(o.OrderId, o.Ticker, Price.PriceValue, 0);
            }
            return new OrderSpotDone(o.OrderId, "error no ticker", Price.PriceValue, 0);
        }

        public OrderLimitDone ProcessOrder(OrderLimit o)
        {
            if (o != null)
            {
                // Example logic: execute if price is within a specific range
                if (o.Price >= o.stopLossPrice && o.Price <= o.takeProfitPrice)
                {
                    return new OrderLimitDone(o.OrderId, o.Ticker, o.Price, o.Quantity, o.stopLossPrice, o.takeProfitPrice);
                }
                return new OrderLimitDone(o.OrderId, o.Ticker, o.Price, 0, o.stopLossPrice, o.takeProfitPrice);
            }
            return new OrderLimitDone(o.OrderId, "error no ticker", o.Price, 0, o.stopLossPrice, o.takeProfitPrice);
        }

        public decimal GetPrice ()
        {
            return Price.PriceValue;
        }
    }
}
