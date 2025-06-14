using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Trading_bot_WPF.Central;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Market;
using Trading_bot_WPF.Risk;

namespace Trading_bot_WPF.Strategy
{
    internal class StrategyManager
    {
        public Core core;
        public Exchange exchange;
        public List<Strategy> strategyList;
        private StrategyFactory strategyFactory;

        private BlockingCollection<object> eventQueue;
        private CancellationTokenSource cts;

        public StrategyManager (Core core, Exchange exchange)
        {
            this.core = core;
            this.exchange = exchange;
            strategyList = new List<Strategy> ();
            this.strategyFactory = new StrategyFactory(core, exchange);
            eventQueue = new BlockingCollection<object>();
            cts = new CancellationTokenSource();
        }

        public void Start()
        {
            Task.Run(() => ProcessLoop(cts.Token), cts.Token);
        }

        public void Stop()
        {
            Console.WriteLine("StrategyManager stopping...");
            cts.Cancel();
            eventQueue.CompleteAdding();
            Console.WriteLine("StrategyManager stopped.");
        }

        private void ProcessLoop(CancellationToken token)
        {
            try
            {
                foreach (var item in eventQueue.GetConsumingEnumerable(token))
                {
                    if (item is Price price)
                    {
                        HandlePrice(price);
                    }
                    else if (item is OrderLimit orderLimit)
                    {
                        ProcessOrder(orderLimit);
                    }
                }
            }
            catch (OperationCanceledException) { }
        }

        public void EnqueuePrice(Price price)
        {
            eventQueue.Add(price);
        }

        public void EnqueueOrder(OrderLimit orderLimit)
        {
            eventQueue.Add(orderLimit);
        }

        private void HandlePrice(Price price)
        {
            //Console.WriteLine($"StrategyManager received price: {price.PriceValue}");
            foreach (Strategy strategy in strategyList)
            {
                strategy.RunStrategy();
                strategy.riskModule.OnPriceReceived(this, price);

                //Console.WriteLine(" cash : " + strategy.cash);
                //Console.WriteLine(" qtt asset : " + strategy.positionManager.GetQuantityOfAsset());
            }
        }

        public void OnPriceReceived(object sender, Price price)
        {
            EnqueuePrice(price);
        }

        public void ProcessOrder(OrderLimit orderLimit)
        {
            lock (orderLimit)
            {
                IEnumerable<Strategy> targetStrategies = orderLimit.sold
                    ? strategyList.Where(s => orderLimit.OrderId.Contains(s.strategyName))
                    : strategyList;

                foreach (var strategy in targetStrategies)
                {
                    if (orderLimit.sold)
                    {
                        strategy.cash += orderLimit.Quantity * orderLimit.Price;
                    }
                    else
                    {
                        strategy.cash -= orderLimit.Quantity * orderLimit.Price;
                    }

                    
                    strategy.riskModule.EnqueueOrder(orderLimit);
                }
            }
        }

        public void OnOrderSold (object sender, OrderLimit order)
        {
            EnqueueOrder(order);
        }

        public void OnOrderReceived(object sender, OrderLimit order)
        {
            EnqueueOrder(order);
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

        public void StartThreadRiskModule()
        {
            foreach (var strategy in strategyList)
            {
                Console.WriteLine(strategy.strategyName + " starting RiskModule");
                strategy.riskModule.Start();
            }
        }

        public void StopThreadRiskModule()
        {
            foreach (var strategy in strategyList)
            {
                Console.WriteLine(strategy.strategyName+ " stopping RiskModule");
                strategy.riskModule.Stop();
            }
        }
    }
}
