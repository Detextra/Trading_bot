using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trading_bot_WPF.Market.OrderLimit;
using static Trading_bot_WPF.Market.Order;
using Trading_bot_WPF.Data;

namespace Trading_bot_WPF.Market
{
	internal class OrderIn
	{
		public Exchange market;

		public OrderIn(Exchange market)
		{
			this.market = market;
		}

        public OrderLimit AddingFeesAndSlippageToOrder (OrderLimit orderLimit)
		{
			// manageing buying order
			if (orderLimit?.Quantity > 0)
			{
				// Managing spread
				orderLimit.Price = market.ApplySpread(orderLimit.Price, true);

                // Managing Order Slippage
				orderLimit.Price = market.ApplySlippage(orderLimit.Price);

                // Managing Order fees of 0.1%
                orderLimit.Price *= (decimal)1.001;

				return market.ProcessOrder(orderLimit);
			}
			return null;
		}

	}
}
