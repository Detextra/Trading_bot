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
    internal class StrategyA : Strategy
    {
        public StrategyA(string StrategyName, ohclManager core, Exchange exchange, decimal cash) : base(StrategyName, core, exchange, cash)
        {

        }

        public override void RunStrategy()
        {
            List<OHCLData> oHCLDatas = core.OHCLDatas;
            CheckPositionStatus(exchange.GetPrice());
            if (oHCLDatas.Count > 2)
            {
                //if (oHCLDatas[oHCLDatas.Count - 1].ClosePrice > oHCLDatas[oHCLDatas.Count - 2].ClosePrice)
                //{
                //    int sellingQuantity = -1;
                //    // positive trend on 1 candle
                //    if (CheckIfEnoughPosition(sellingQuantity))
                //    {
                //        OrderSpot order = new OrderSpot(exchange.Price.Ticker, exchange.Price.PriceValue, sellingQuantity);
                //        SendOrder(order);
                //    }

                //}
                //else 
                if (oHCLDatas[oHCLDatas.Count - 1].ClosePrice < oHCLDatas[oHCLDatas.Count - 2].ClosePrice)
                {
                    //Console.WriteLine("Negative trend; money available: " + cash);
                    //int buyingQuantity = 1;
                    //// negative trend on 1 candle
                    //if (CheckIfEnoughCashToBy(buyingQuantity))
                    //{
                    //    Console.WriteLine("enough cash -> buying");
                    //    // stop loss 2%, profit 5%
                        
                        OrderLimit order = new OrderLimit(GenerateOrderId(), exchange.Price.Ticker, exchange.Price.PriceValue, buyingQuantity, exchange.Price.PriceValue * 0.98m, exchange.Price.PriceValue * 1.05m);
                    //    SendOrder(order);
                    //}
                    SendOrderSignal(1);
                }
            }
            
        }
    }
}
