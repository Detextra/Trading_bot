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

		// remplace int argument by Order argument to manage the gap between orderPrice and slippage
		public void AddingFeesAndSlippageToOrder (OrderLimit orderLimit)
		{
			// manageing buying order
			if (orderLimit?.Quantity > 0)
			{
				// Managing Order Slippage

				// Manaing Order fees of 2%
				orderLimit.Price *= (decimal)1.02;

				market.ProcessOrder(orderLimit);
			}
 
		}

	}
}
