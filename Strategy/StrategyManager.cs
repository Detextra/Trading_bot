using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Central;
using Trading_bot.Data;
using Trading_bot.Market;

namespace Trading_bot.Strategy
{
    internal class StrategyManager
    {
        public ohclManager core;
        public Exchange exchange;
        public List<Strategy> strategyList;
        private StrategyFactory strategyFactory;

        public StrategyManager (ohclManager core, Exchange exchange)
        {
            this.core = core;
            this.exchange = exchange;
            strategyList = new List<Strategy> ();
            this.strategyFactory = new StrategyFactory(core, exchange);
        }

        public void OnPriceReceived(object sender, Price price)
        {
            //Console.WriteLine($"StrategyManager received price: {price.PriceValue}");
            foreach (Strategy strategy in strategyList)
            {
                strategy.RunStrategy();
                
                //Console.WriteLine(" qtt asset : "+strategy.positionManager.GetQuantityOfAsset());
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
