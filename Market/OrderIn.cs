using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trading_bot.Market.OrderLimit;
using static Trading_bot.Market.Order;

namespace Trading_bot.Market
{
	internal class OrderIn
	{
		public Exchange market;

		public OrderIn(Exchange market)
		{
			this.market = market;
		}

		public OrderLimit AddingFeesAndSlippageToOrder (int signal)
		{
			OrderLimit orderLimit = new OrderLimit();
		}

	}
}
