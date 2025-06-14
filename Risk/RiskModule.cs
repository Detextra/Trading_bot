using OxyPlot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Market;
using Trading_bot_WPF.Strategy;
using Trading_bot_WPF.Strategy.Position;
using Trading_bot_WPF.Strategy.StrategyType;

namespace Trading_bot_WPF.Risk
{
    internal class RiskModule
    {
        public decimal cash;
        public List<Tuple<string, RiskData>> riskDatas;
        public List<OrderLimit> ordersLimit;

        private BlockingCollection<object> eventQueue;
        private CancellationTokenSource cts;

        public RiskModule(decimal cash)
        {
            this.cash = cash;
            riskDatas = new List<Tuple<string, RiskData>>();
            ordersLimit = new List<OrderLimit>();
            eventQueue = new BlockingCollection<object>();
            cts = new CancellationTokenSource();
        }
        public void Start()
        {
            Task.Run(() => ProcessLoop(cts.Token), cts.Token);
        }

        public void Stop()
        {
            Console.WriteLine("RiskModule stopping...");
            cts.Cancel();
            eventQueue.CompleteAdding();
            Console.WriteLine("RiskModule stopped.");
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
            var riskData = new RiskData
            {
                value = cash + GetQuantityOfAsset() * price.PriceValue
            };
            string timestamp = price.Date + "" + price.Time;
            lock (riskDatas)
            {
                riskDatas.Add(Tuple.Create(timestamp, riskData));
            }
        }

        public int GetQuantityOfAsset()
        {
            lock (ordersLimit)
            {
                return ordersLimit.Sum(order => order.Quantity);
            }
        }

        public void OnPriceReceived(object sender, Price price)
        {
            EnqueuePrice(price);
        }

        public void ProcessOrder(OrderLimit orderLimit)
        {
            lock (ordersLimit)
            {
                if (orderLimit.sold)
                {
                    cash += orderLimit.Price * orderLimit.Quantity;
                    ordersLimit.Remove(orderLimit);
                }
                else
                {
                    ordersLimit.Add(orderLimit);
                }
                
            }
        }

        // convert into OxyPlot DataPoint
        public List<DataPoint> GetRiskData()
        {
            lock (riskDatas)
            {
                return riskDatas
                    .OrderBy(r => r.Item1)
                    .Select(r => new DataPoint(
                        DateTime.ParseExact(r.Item1, "yyyyMMddHHmmss", null).ToOADate(),
                        (double)r.Item2.value))
                    .ToList();
            }
        }
    }

    internal class RiskData
    {
        public decimal value;

        public RiskData() { }

        public RiskData(decimal value)
        {
            this.value = value;
        }
    }
}