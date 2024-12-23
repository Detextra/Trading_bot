using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trading_bot.Market.OrderLimit;
using static Trading_bot.Market.Order;

namespace Trading_bot.Market
{
    internal class OrderManagement
    {
        public Exchange market;
        public OrderManagement (Exchange market)
        {
            this.market = market;
            }

        // checking if SL or TK need to be triggered
        public void CheckingOrderStatus(OrderLimit orderLimit)
        {
            

        }

    }
}
