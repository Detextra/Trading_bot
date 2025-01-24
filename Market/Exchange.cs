using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Data;
using Trading_bot.Strategy.Position;

namespace Trading_bot.Market
{
    internal class Exchange
    {
        public Price Price;

        public PositionManager allPositions;

        public Exchange ()
        {
            Price = new Price("", "", "", -1); ;
            allPositions = new PositionManager();
        }

        public void OnPriceReceived(object sender, Price price)
        {
            //Console.WriteLine($"Exchange received price: {price.PriceValue}");
            Price = price;
            allPositions.CheckPositionStatus(GetPrice());
        }

        // as OrderLimit utilize Position and no OrderLimitDone, use PositionSpot instead
        public OrderSpotDone ProcessOrder (OrderSpot o)
        {

            if (o != null)
            {
                if (o.Price < Price.PriceValue * (decimal)1.02 && o.Price > Price.PriceValue * (decimal)0.98) // spread management -> better in a proper fonction
                {
                    return new OrderSpotDone(o.OrderId, o.Ticker, Price.PriceValue, o.Quantity);
                }
                return new OrderSpotDone(o.OrderId, o.Ticker, Price.PriceValue, 0);
            }
            return new OrderSpotDone(o.OrderId, "error no ticker", Price.PriceValue, 0);
        }

        public PositionLimit ProcessOrder(OrderLimit o)
        {
            if (o != null)
            {
                if (o.Price >= o.stopLossPrice && o.Price <= o.takeProfitPrice)
                {
                    //Console.WriteLine("Order Limit processed for " + o.Quantity + " asset at " + o.Price);
                    return new PositionLimit(o.OrderId, o.Ticker, o.Quantity, this.GetPrice(), o.stopLossPrice, o.takeProfitPrice);
                    //return new OrderLimitDone(o.OrderId, o.Ticker, o.Price, o.Quantity, o.stopLossPrice, o.takeProfitPrice);
                }
                //return new OrderLimitDone(o.OrderId, o.Ticker, o.Price, 0, o.stopLossPrice, o.takeProfitPrice);
            }
            return null;
        }

        public decimal GetPrice ()
        {
            return Price.PriceValue;
        }
    }
}
