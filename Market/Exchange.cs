using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Central;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Strategy.Position;

namespace Trading_bot_WPF.Market
{
    internal class Exchange
    {
        public Core ohclDatas;
        public Price Price;

        public PositionManager allPositions;
        public OrderManagement allOrders;

        public decimal slippage;
        public decimal volatilityPctForATR;

        public Exchange(Core ohclDatas, decimal slippage = 0, decimal volatilityPctForATR = 0)
        {
            this.ohclDatas = ohclDatas;
            Price = new Price("", "", "", -1); ;
            allPositions = new PositionManager();
            allOrders = new OrderManagement(this);
            this.slippage = slippage;
            this.volatilityPctForATR = volatilityPctForATR;
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
                return new OrderSpotDone(o.OrderId, o.Ticker, Price.PriceValue, o.Quantity);
                
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

        public decimal ApplySpread(decimal orderPrice, bool isBuyOrder)
        {
            decimal spread = ohclDatas.EstimateSpread(volatilityPctForATR);
            decimal priceWithSpread = isBuyOrder ? orderPrice + (spread / 2) : orderPrice - (spread / 2);
            return priceWithSpread;
        }

        public decimal ApplySlippage (decimal orderPrice)
        {
            decimal slippageAmount = ohclDatas.GetSlippageAmountPctLast24H(slippage);
            orderPrice += slippageAmount;
            return orderPrice;
        }
    }
}
