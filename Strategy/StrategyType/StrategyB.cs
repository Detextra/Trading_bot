using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Central;
using Trading_bot.Data;
using Trading_bot.Market;

namespace Trading_bot.Strategy.StrategyType
{
    internal class StrategyB : Strategy
    {
        public StrategyB(string StrategyName, ohclManager core, Exchange exchange, decimal cash) : base(StrategyName, core, exchange, cash)
        {

        }

        public override void RunStrategy()
        {
            CheckPositionStatus(exchange.GetPrice());
        }
    }
}
