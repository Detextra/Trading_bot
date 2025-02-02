using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Market;

namespace Trading_bot_WPF.Strategy.Position
{
    internal class PositionManager
    {
        public List<Position> Positions;

        public PositionManager()
        {
            Positions = new List<Position>();
        }

        public void CheckPositionStatus(decimal marketPrice)
        {
            foreach (Position position in Positions)
            {
                position.checkPositionStatus(marketPrice);
            }
        }

        public int GetQuantityOfAsset(string ticker)
        {
            int quantity = 0;
            foreach (Position position in Positions)
            {
                if (position.ticker == ticker)
                {
                    quantity += position.quantity;
                }
            }
            return quantity;
        }

        // testing purpose only

        public int GetQuantityOfAsset()
        {
            int quantity = 0;
            foreach (Position position in Positions)
            {
                quantity+= position.quantity;
            }
            return quantity;
        }

        public bool RemovePosition (OrderSpotDone order)
        {
            foreach(Position position in Positions)
            {
                if (position.orderId == order.OrderId)
                {
                    Positions.Remove(position);
                    return true;
                }
            }
            return false;
            
        }

        public bool AddPosition(PositionSpot position)
        {
            //PositionSpot newPosition = new PositionSpot(order.OrderId, order.Ticker, order.Quantity, order.Price);
            Positions.Add(position);
            return true;
        }

        public bool AddPosition(PositionLimit position)
        {
            //PositionLimit newPosition = new PositionLimit(order.OrderId, order.Ticker, order.Quantity, order.Price, order.stopLossPrice, order.takeProfitPrice);
            Positions.Add(position);
            return true;
        }

    }
}
