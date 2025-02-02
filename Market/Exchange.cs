using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Strategy.Position;

namespace Trading_bot_WPF.Market
{
    internal class Exchange
    {
        public Price Price;

        public PositionManager allPositions;
        public OrderManagement allOrders;

        public Exchange ()
        {
            Price = new Price("", "", "", -1); ;
            allPositions = new PositionManager();
            allOrders = new OrderManagement();
        }

        public void OnPriceReceived(object sender, Price price)
        {
            //Console.WriteLine($"Exchange received price: {price.PriceValue}");
            Price = price;
            allOrders.CheckingOrderStatus(price.PriceValue);
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

        public void ProcessOrder(OrderLimit o)
        {
            if (o != null)
            {
                //if (o.Price >= o.stopLossPrice && o.Price <= o.takeProfitPrice)
                //{
                    allOrders.orders.Add(o);
                    Console.WriteLine("Order Limit id: "+o.OrderId+" processed for " + o.Quantity + " asset at " + o.Price);
                //}
                //Console.WriteLine("Sl or TJ already hitten");
            }
        }

        // for debug purpose
        public int CountQuantitiesFromOrdersLimit()
        {   
            int count = 0;
            foreach (OrderLimit o in allOrders.orders)
            {
                count += o.Quantity;
            }
            return count;
        }

        public decimal GetPrice ()
        {
            return Price.PriceValue;
        }
    }
}
