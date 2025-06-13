using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Central;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Market;

namespace Trading_bot_WPF.Strategy
{
    internal class StrategyManager
    {
        public Core core;
        public Exchange exchange;
        public List<Strategy> strategyList;
        private StrategyFactory strategyFactory;

        public StrategyManager (Core core, Exchange exchange)
        {
            this.core = core;
            this.exchange = exchange;
            strategyList = new List<Strategy> ();
            this.strategyFactory = new StrategyFactory(core, exchange);
        }

        public void OnPriceReceived(object sender, Price price)
        {
            Console.WriteLine($"StrategyManager received price: {price.PriceValue}");
            foreach (Strategy strategy in strategyList)
            {
                strategy.RunStrategy();
                strategy.riskModule.OnPriceReceived(this, price);
                Console.WriteLine(" cash : "+strategy.cash);
                Console.WriteLine(" qtt asset : "+strategy.positionManager.GetQuantityOfAsset());
            }
        }

        public void OnOrderSold (object sender, OrderLimit order)
        {
            foreach (Strategy strategy in strategyList)
            {
                if (order.OrderId.Contains(strategy.strategyName))
                {
                    //Console.WriteLine("strat cash avant: " + strategy.cash);
                    strategy.cash += order.Quantity * order.Price;
                    //Console.WriteLine("strat cash apres: " + strategy.cash + " ajout de: " + exchange.Price.PriceValue);
                    strategy.riskModule.ProcessOrderSold(order);
                }
            }
        }

        public void OnOrderReceived(object sender, OrderLimit order)
        {
            foreach (Strategy strategy in strategyList)
            {
                if (order.OrderId.Contains(strategy.strategyName))
                {
                    strategy.riskModule.ordersLimit.Add(order);
                }
            }
        }

        public void AddStrategy (string strategyType, decimal startingCash)
        {
            Strategy strategy = strategyFactory.CreateStrategy(strategyType, startingCash);
            strategyList.Add(strategy);
        }

        public void PrintSummaryOfStrategies()
        {
            foreach (Strategy strategy in strategyList)
            {
                strategy.PrintSummaryOfStrategy();
            }
        }
    }
}
