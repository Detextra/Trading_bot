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
            ohclManager core = new ohclManager(15);
            Exchange exchange = new Exchange();

            StrategyManager strategyManager = new StrategyManager(core, exchange);
            strategyManager.AddStrategy("StrategyA", 3m);

            dataExtractor.PriceReceived += core.OnPriceReceived;
            dataExtractor.PriceReceived += exchange.OnPriceReceived;
            dataExtractor.PriceReceived += strategyManager.OnPriceReceived;

            dataExtractor.ReadPriceAndSendThem("../../../../data/EURUSD/EURUSD_20230101-20240822.txt");


            // Printing results
            //core.PrintOHCLlist(core.OHCLDatas);
            strategyManager.PrintSummaryOfStrategies();
        }
    }
}
