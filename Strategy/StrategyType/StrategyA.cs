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
    internal class StrategyA : Strategy
    {
        public StrategyA(string StrategyName, Core core, Exchange exchange, decimal cash) : base(StrategyName, core, exchange, cash)
        {

        }

        public override void RunStrategy()
        {
            List<OhclData> oHCLDatas = core.OhclDatas[0].OhclDatas;
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
                    
                    int buyingQuantity = 1;
                    // negative trend on 1 candle
                    if (CheckIfEnoughCashToBy(buyingQuantity))
                    {
                        Console.WriteLine("Negative trend; ohcl-2 : " + oHCLDatas[oHCLDatas.Count - 2].ClosePrice +
                        " ; ohcl-1 " + oHCLDatas[oHCLDatas.Count - 1].ClosePrice + " ; money available: " + cash);

                        // stop loss 2%, profit 1%
                        OrderLimit order = new OrderLimit(GenerateOrderId(), exchange.Price.Ticker, exchange.GetPrice(), buyingQuantity, exchange.Price.PriceValue * 0.98m, exchange.Price.PriceValue * 1.01m);
                        SendOrderSignal(order);
                    }
                }
            }
            
        }
    }
}
