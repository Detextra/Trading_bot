using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Central;
using Trading_bot.Data;
using Trading_bot.Market;
using Trading_bot.Strategy.Position;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Trading_bot.Strategy
{
    internal abstract class Strategy
    {
        public string strategyName;
        public PositionManager positionManager;
        public Core core;
        public Exchange exchange;
        public decimal cash;
        public int orderId;
        private readonly object orderIdLock = new object();

        public Strategy(string strategyName, Core core, Exchange exchange, decimal cash)
        {
            this.strategyName = strategyName;
            positionManager = new PositionManager();
            this.exchange = exchange;
            this.cash = cash;
            this.core = core;
            orderId = 0;
        }

        public virtual void RunStrategy()
        {
            CheckPositionStatus(exchange.GetPrice());
        }

        public void CheckPositionStatus(decimal price)
        {
            positionManager.CheckPositionStatus(price);
        }

        public bool CheckIfEnoughCashToBy(int quantity)
        {
            if (exchange.Price.PriceValue * quantity <= cash)
            {
                return true;
            }
            else return false;
        }

        public string GenerateOrderId()
        {
            lock (orderIdLock)
            {
                orderId += 1;
                return $"{strategyName}{orderId}";
            }
        }

        public bool CheckIfEnoughPosition(int quantity)
        {
            quantity = Math.Abs(quantity);
            return positionManager.GetQuantityOfAsset() >= quantity;
        }

        public OrderSpotDone SendOrder (OrderSpot order)
        {
            OrderSpotDone orderDone = exchange.ProcessOrder(order);
            if (orderDone.Quantity < 0)
            {
                //selling
                if (positionManager.RemovePosition(orderDone))
                {
                    cash += Math.Abs(orderDone.Quantity) * orderDone.Price;
                }
            }
            else if (orderDone.Quantity > 0)
            {
                // buying
                if (positionManager.AddPosition(orderDone))
                {
                    cash -= orderDone.Quantity * orderDone.Price;
                }
                
                
            } 
            return orderDone;
        }

        public OrderLimitDone SendOrder(OrderLimit order)
        {
            
            OrderLimitDone orderDone = exchange.ProcessOrder(order);
            // In a limit order, the order is only ended by the stopLoss or the takeProfit, so the "sell" is automatic
            //if (orderDone.Quantity < 0)
            //{
            //    //selling
            //    cash += Math.Abs(orderDone.Quantity) * orderDone.Price;
            //    positionManager.RemovePosition(orderDone.Ticker, orderDone.Quantity);
            //}
            if (orderDone.Quantity > 0)
            {
                // buying
                if (positionManager.AddPosition(orderDone))
                {
                    cash -= orderDone.Quantity * orderDone.Price;
                }

            }
            return orderDone;
        }

        public void PrintSummaryOfStrategy ()
        {
            decimal assetPositionsPrice = positionManager.GetQuantityOfAsset() * exchange.Price.PriceValue;
            Console.WriteLine(strategyName+" cash: " + cash+ " ; asset: "+positionManager.GetQuantityOfAsset()+" * "+exchange.Price.PriceValue+" = "+ assetPositionsPrice);
        }
    }
}
