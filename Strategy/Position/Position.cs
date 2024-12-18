using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading_bot.Strategy.Position
{
    internal abstract class Position
    {
        public string orderId;
        public string ticker;
        public int quantity;
        public decimal price;

        public Position (string orderId, string ticker, int quantity, decimal price)
        {
            this.ticker = ticker;
            this.quantity = quantity;
            this.orderId = orderId;
            this.price = price;
        }

        public virtual void checkPositionStatus (decimal marketPrice)
        {

        }
    }

    internal class PositionSpot : Position
    {

        public PositionSpot(string orderId, string ticker, int quantity, decimal price) : base(orderId, ticker, quantity, price)
        {

        }
    }

    internal class PositionLimit : Position
    {
        public decimal stopLossPrice;
        public decimal takeProfitPrice;

        public PositionLimit(string orderId, string ticker, int quantity, decimal price, decimal stopLossPrice, decimal takeProfitPrice) : base (orderId, ticker, quantity, price)
        {
            this.stopLossPrice = stopLossPrice;
            this.takeProfitPrice = takeProfitPrice;
        }

        public override void checkPositionStatus (decimal marketPrice)
        {
            if (marketPrice <= stopLossPrice || marketPrice >= takeProfitPrice)
            {
                //signal vente
                Console.WriteLine("vendage");
            }
        }
    }
        
}
