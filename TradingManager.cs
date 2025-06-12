using System;
using System.Collections.Generic;
using OxyPlot;
using Trading_bot_WPF.Central;
using Trading_bot_WPF.Market;
using Trading_bot_WPF.Strategy;
using Trading_bot_WPF.Strategy.StrategyType;
using Trading_bot_WPF.Data;

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
            decimal slippage = new decimal(0.02); // in %
            decimal volatilityPctForATR = new decimal(0.01); // in %
            exchange = new Exchange(core, slippage, volatilityPctForATR);
            strategyManager = new StrategyManager(core, exchange);

            strategyManager.AddStrategy("StrategyA", 3m);

            dataExtractor.PriceReceived += core.OnPriceReceived;
            dataExtractor.PriceReceived += exchange.OnPriceReceived;
            dataExtractor.PriceReceived += strategyManager.OnPriceReceived;

            exchange.allOrders.OrderSold += strategyManager.OnOrderSold;
        }

        public void RunTrading()
        {
            dataExtractor.ReadPriceAndSendThem("../../../../data/EURUSD/EURUSD_20230101-20240822.txt");

            foreach (var strategy in strategyManager.strategyList)
            {
                List<DataPoint> riskData = strategy.riskModule.GetRiskData();
                GraphDataReady?.Invoke(riskData, core.Get24HoursOhclListsForPlotting(), strategy.strategyName);
            }
        }

    }
}
