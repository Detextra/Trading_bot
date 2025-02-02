using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Market;
using Trading_bot_WPF.Strategy;
using Trading_bot_WPF.Strategy.StrategyType;

namespace Trading_bot_WPF.Risk
{
    internal class RiskModule
    {
        public decimal cash;
        public List<Tuple<string, RiskData>> riskDatas;
        public List<OrderLimit> ordersLimit;

        public RiskModule(decimal cash)
        {
            this.cash = cash;
            riskDatas = new List<Tuple<string, RiskData>>();
            ordersLimit = new List<OrderLimit>();
        }

        public void OnPriceReceived(object sender, Price price)
        {
            decimal actualCash = cash;
            foreach (OrderLimit orderLimit in ordersLimit)
            {
                actualCash -= orderLimit.Price * orderLimit.Quantity;
            }
            RiskData riskData = new RiskData();
            riskData.value = actualCash;
            riskDatas.Add(Tuple.Create(price.Date + "" + price.Time, riskData));
        }

        public void ProcessOrderSold(OrderLimit orderLimit)
        {
            // the cash is not substracted when asset is bought for the order => add the buying and solding price to OrderLimit
            cash += orderLimit.Price * orderLimit.Quantity;
            ordersLimit.Remove(orderLimit);
        }

        // convert into OxyPlot DataPoint
        public List<DataPoint> GetRiskData()
        {
            List<DataPoint> dataPoints = riskDatas
            .OrderBy(r => r.Item1)
            .Select(r => new DataPoint(
                DateTime.ParseExact(r.Item1, "yyyyMMddHHmmss", null).ToOADate(),
                (double)r.Item2.value))
            .ToList();
            return dataPoints;
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