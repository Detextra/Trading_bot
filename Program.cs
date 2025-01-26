using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Central;
using Trading_bot.Market;
using Trading_bot.Strategy;
using Trading_bot.Strategy.StrategyType;

namespace Trading_bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataExtractor dataExtractor = new DataExtractor();
            int[] ohclPeriods= { 1, 5, 15 };
            Core core = new Core(ohclPeriods);
            Exchange exchange = new Exchange();

            StrategyManager strategyManager = new StrategyManager(core, exchange);
            strategyManager.AddStrategy("StrategyA", 3m);

            dataExtractor.PriceReceived += core.OnPriceReceived;
            dataExtractor.PriceReceived += exchange.OnPriceReceived;
            dataExtractor.PriceReceived += strategyManager.OnPriceReceived;

            exchange.allOrders.OrderSold += strategyManager.OnOrderSold;

            foreach (var strategy in strategyManager.strategyList)
            {
                dataExtractor.PriceReceived += strategy.riskModule.OnPriceReceived;
            }

            dataExtractor.ReadPriceAndSendThem("../../../../data/EURUSD/EURUSD_20230101-20240822.txt");
            //dataExtractor.ReadPriceAndSendThem("../../../../data/EURUSD/EURUSD_test1Min_dumb_values.txt");
            

            // Printing results
            //core.PrintOHCLlist();
            strategyManager.PrintSummaryOfStrategies();
            Console.WriteLine("quantity in Orders: "+exchange.CountQuantitiesFromOrdersLimit()+ 
                " actual market price: "+exchange.Price.PriceValue+ 
                " = " + (exchange.CountQuantitiesFromOrdersLimit()* exchange.Price.PriceValue)+"$");

            foreach (var strategy in strategyManager.strategyList)
            {
                strategy.riskModule.PlotRiskData();
            }
        }
    }
}
