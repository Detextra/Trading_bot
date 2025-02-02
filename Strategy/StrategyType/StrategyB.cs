using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Central;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Market;

namespace Trading_bot_WPF.Strategy.StrategyType
{
    internal class StrategyB : Strategy
    {
        public StrategyB(string StrategyName, Core core, Exchange exchange, decimal cash) : base(StrategyName, core, exchange, cash)
        {

        }

        public override void RunStrategy()
        {
            CheckPositionStatus(exchange.GetPrice());
        }
    }
}
