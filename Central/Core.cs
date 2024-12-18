using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading_bot.Data;

namespace Trading_bot.Central
{
    internal class Core
    {
        public Core(int periodInMinutes)
        {
            this.periodInMinutes = periodInMinutes;
            OHCLDatas = new List<OHCLData>();
            currentOHCLData = new OHCLData("", "", "", 0, 0, 0, 0);
        }
        public void OnPriceReceived(object sender, Price price)
        {
            //Console.WriteLine($"Core received price: {price.PriceValue}");
            ProcessPrice(price);
        }


        private int periodMin = 1;
        private int periodInMinutes;
        private OHCLData currentOHCLData;
        public List<OHCLData> OHCLDatas { get; private set; }
        public void ProcessPrice(Price price)
        {
            if (currentOHCLData.Ticker == "")
            {
                currentOHCLData.Ticker = price.Ticker;
                currentOHCLData.Date = price.Date;
                currentOHCLData.Time = price.Time;
                currentOHCLData.OpenPrice = price.PriceValue;
                currentOHCLData.HighPrice = price.PriceValue;
                currentOHCLData.LowPrice = price.PriceValue;
            }

            if (price.PriceValue > currentOHCLData.HighPrice)
            {
                currentOHCLData.HighPrice = price.PriceValue;
            }
            if (price.PriceValue < currentOHCLData.LowPrice)
            {
                currentOHCLData.LowPrice = price.PriceValue;
            }

            if (periodMin == periodInMinutes)
            {
                currentOHCLData.ClosePrice = price.PriceValue;
                OHCLDatas.Add(currentOHCLData);

                currentOHCLData = new OHCLData("", "", "", 0, 0, 0, 0);
                periodMin = 1;
            }
            else
            {
                periodMin++;
            }
        }
        public void PrintOHCLlist(List<OHCLData> datas)
        {
            Console.WriteLine("Ticker\tDate\tTime\tOpenPrice\tHighPrice\tLowPrice\tClosePrice");
            foreach (OHCLData o in datas)
            {
                Console.WriteLine("" + o.Ticker + "\t" + o.Date + "\t" + o.Time + "\t"
                    + o.OpenPrice + "\t" + o.HighPrice + "\t" + o.LowPrice + "\t" + o.ClosePrice);
            }
        }

    }


}
