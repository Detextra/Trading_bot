using System;
using System.Collections.Generic;
using OxyPlot;
using Trading_bot_WPF.Central;
using Trading_bot_WPF.Market;
using Trading_bot_WPF.Strategy;
using Trading_bot_WPF.Strategy.StrategyType;
using Trading_bot_WPF.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Trading_bot_WPF
{
    public class TradingManager
    {
        private DataExtractor dataExtractor;
        private Core core;
        private Exchange exchange;
        private StrategyManager strategyManager;

        public event Action<List<DataPoint>, List<DataPoint>, string> GraphDataReady;

        public TradingManager()
        {
            dataExtractor = new DataExtractor();
            int[] ohclPeriods = { 1, 5, 15 };
            core = new Core(ohclPeriods);
            decimal slippage = new decimal(0.10); // in %
            decimal volatilityPctForATR = new decimal(0.02); // in %
            exchange = new Exchange(core, slippage, volatilityPctForATR);
            strategyManager = new StrategyManager(core, exchange);

            strategyManager.AddStrategy("StrategyA", 3m);

            dataExtractor.PriceReceived += core.OnPriceReceived;
            dataExtractor.PriceReceived += exchange.OnPriceReceived;
            dataExtractor.PriceReceived += strategyManager.OnPriceReceived;

            exchange.allOrders.OrderSold += strategyManager.OnOrderSold;
            exchange.OrderReceived += strategyManager.OnOrderReceived;
        }

        public void RunTrading()
        {
            strategyManager.Start();
            strategyManager.StartThreadRiskModule();

            string EURUSD_23y_1min = "../../../../data/EURUSD/EURUSD_23y_1min.txt";
            string EURUSD_17months_1min = "../../../../data/EURUSD/EURUSD_20230101-20240822.txt";
            dataExtractor.ReadPriceAndSendThem(EURUSD_17months_1min);

            // backtesting if finished when ReadPriceAndSendThem
            strategyManager.StopThreadRiskModule();
            strategyManager.Stop();

            foreach (var strategy in strategyManager.strategyList)
            {
                List<DataPoint> riskData = strategy.riskModule.GetRiskData();
                GraphDataReady?.Invoke(riskData, core.Get24HoursOhclListsForPlotting(), strategy.strategyName);
            }
        }

    }
}
