using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trading_bot.Market.OrderLimit;
using static Trading_bot.Market.Order;
using Trading_bot.Data;

namespace Trading_bot.Market
{
	internal class OrderIn
	{
		public Exchange market;

		public OrderIn(Exchange market)
		{
			this.market = market;
		}

        public void AddingFeesAndSlippageToOrder (OrderLimit orderLimit)
		{
			// manageing buying order
			if (orderLimit?.Quantity > 0)
			{
				// Managing Order Slippage

				// Managing Order fees of 0.1%
				orderLimit.Price *= (decimal)1.001;

				market.ProcessOrder(orderLimit);
			}
 
		}

	}
}
