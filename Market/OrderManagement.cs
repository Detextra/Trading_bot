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
    internal class OrderManagement
    {
        public List<OrderLimit> orders;
        public OrderManagement ()
        {
            orders = new List<OrderLimit>();
        }

        public event EventHandler<OrderLimit> OrderSold;

        protected virtual void OnOrderSold(OrderLimit order)
        {
            OrderSold?.Invoke(this, order);
            orders.Remove(order);
            Console.WriteLine("Order sold:" + order.OrderId);
        }

        public void SendingOrderSold(OrderLimit order)
        {
            //Console.WriteLine($"DataExtractor sending price: {price.PriceValue}");
            OnOrderSold(order);
        }


        // checking if SL or TK need to be triggered
        public void CheckingOrderStatus(decimal priceValue)
        {
            List<OrderLimit> ordersCopy = new List<OrderLimit>(orders);
            foreach (OrderLimit order in ordersCopy)
            {
                if (priceValue >= order.takeProfitPrice)
                {
                    Console.WriteLine("TK reached: " + order.takeProfitPrice + " at:" + priceValue);
                    order.Price = priceValue;
                    SendingOrderSold(order);
                }
                else if (priceValue <= order.stopLossPrice)
                {
                    Console.WriteLine("SL reached: " + order.stopLossPrice + " at:" + priceValue);
                    order.Price = priceValue;
                    SendingOrderSold(order);
                }
            }
        }

    }
}
