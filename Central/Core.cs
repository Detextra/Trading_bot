using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Trading_bot_WPF.Data;
using Trading_bot_WPF.Risk;

namespace Trading_bot_WPF.Central
{
    internal class Core
    {

        public List<OhclList> OhclDatas { get; private set; }

        public Core(int[] periodInMinutes)
        {
            List<int> periodList = new List<int>(periodInMinutes);
            
            // adding 1 minutes Ohcl for slippage & spread
            if (!periodList.Contains(1))
            {
                periodList.Add(1);
            }

            // add a 24h OhclDatas to plot it in comparison of risk/performance end of trading chart
            if (!periodList.Contains(1440))
            {
                periodList.Add(1440);
            }

            OhclDatas = new List<OhclList>();

            foreach (int period in periodList)
            {
                Console.WriteLine("Creating OhclDatas for " +  period + " minutes period");
                OhclDatas.Add(new OhclList(period));
            }
        }

        public void OnPriceReceived(object sender, Price price)
        {
            //Console.WriteLine($"Core received price: {price.PriceValue}");
            ProcessPrice(price);
        }
        
        public void ProcessPrice(Price price)
        {
            foreach (var item in OhclDatas)
            {
                 OhclData ohclData = item.currentOhcl;
                // this is a new OhclData
                if (string.IsNullOrEmpty(ohclData.Date) && string.IsNullOrEmpty(ohclData.Time))
                {
                   ohclData.Ticker = price.Ticker;
                   ohclData.Date = price.Date;
                   ohclData.Time = price.Time;
                   ohclData.OpenPrice = price.PriceValue;
                   ohclData.HighPrice = price.PriceValue;
                   ohclData.ClosePrice = price.PriceValue;
                   ohclData.LowPrice = price.PriceValue;
                   ohclData.CurrentPrice = price.PriceValue;
                    continue;
                }

                // Store DateTime objects instead of separate Date and Time strings in your OhclData and PriceData objects.

                // Use Span<char> for Parsing ex : int year = int.Parse(date.Slice(0, 4));

                DateTime OhclDate = DateTime.ParseExact(ohclData.Date, "yyyyMMdd", null);
                TimeSpan OhclTime = TimeSpan.ParseExact(ohclData.Time, "hhmmss", null);
                OhclDate = OhclDate.Add(OhclTime);

                DateTime PriceDate = DateTime.ParseExact(price.Date, "yyyyMMdd", null);
                TimeSpan PriceTime = TimeSpan.ParseExact(price.Time, "hhmmss", null);
                PriceDate = PriceDate.Add(PriceTime);

                // Instead of recalculating the difference between priceDateTime and ohclDateTime for every iteration, use a precomputed threshold time.
                if ( (PriceDate - OhclDate).TotalMinutes >= item.OhclTimeFrame)
                {
                    item.Add(price);
                }
                else
                {
                   ohclData.CurrentPrice = price.PriceValue;

                    if (ohclData.HighPrice < price.PriceValue)
                        ohclData.HighPrice = price.PriceValue;

                    if (ohclData.LowPrice > price.PriceValue)
                        ohclData.LowPrice = price.PriceValue;
                }
                //last Ohcl data is not stored because there is no "next" Ohcl Data. When a new Ohcl Data is created the previous one is stored
            }
        }

        public void PrintOHCLlist()
        {
            foreach (OhclList list in OhclDatas) {
                list.PrintOhclList();
            }            
        }

        public OhclList GetOhclListsByPeriodInMinutes (int periodInMinutes)
        {
            foreach (OhclList list in OhclDatas)
            {
                if (list.OhclTimeFrame == periodInMinutes)
                {
                    return list;
                }
            }
            return new OhclList();
        }

        public OhclList GetOhclListsWithShortestPeriodInMinutes()
        {
            return OhclDatas.MinBy(list => list.OhclTimeFrame) ?? new OhclList();
        }

        public decimal GetSlippageAmountPctLast24H(decimal slippage)
        {
            OhclList ohclList = GetOhclListsByPeriodInMinutes(1);

            if (ohclList.OhclDatas == null || ohclList.OhclDatas.Count == 0)
                return 0m;
            OhclData last = ohclList.OhclDatas.Last();
            DateTime cutoffTime = DateTime.ParseExact(last.Date+last.Time, "yyyyMMddHHmmss", null).AddHours(-24);

            // todo : bug never find ohcl in the period
            var filteredData = ohclList.OhclDatas
                .Where(ohcl => DateTime.TryParseExact(ohcl.Date+ohcl.Time, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, 
                out DateTime timestamp) && timestamp >= cutoffTime)
                .ToList();

            if (filteredData.Count == 0)
                return 0m;

            decimal lowestPrice = filteredData.Min(ohcl => ohcl.LowPrice);
            decimal highestPrice = filteredData.Max(ohcl => ohcl.HighPrice);

            decimal volatility = highestPrice-lowestPrice;
            return volatility;
        }

        // using Average True Range (ATR)
        public decimal EstimateSpread(decimal volatility, int periodForAverage = 24 * 60)
        {
            List<OhclData> ohclList = GetOhclListsByPeriodInMinutes(1).OhclDatas;
            if(ohclList == null || ohclList.Count == 0)
                return 0m;

            if (ohclList.Count < periodForAverage) return (0.01m+volatility)/2; // 0.01 is quite low for testing so adding volatility to bump up the spread

            decimal sum = 0; 
            for (int i = ohclList.Count - periodForAverage; i < ohclList.Count; i++)
            {
                sum += ohclList[i].HighPrice - ohclList[i].LowPrice;
            }

            decimal atr = sum / periodForAverage;
            if (atr == 0)
            {
                return (0.01m + volatility) / 2;
            }
            return atr * volatility;
        }

        public List<DataPoint> Get24HoursOhclListsForPlotting()
        {
            OhclList ohcl24h = GetOhclListsByPeriodInMinutes(1440);
            List<DataPoint> dataPoints = ohcl24h.OhclDatas
            .OrderBy(o => o.Date + o.Time)
            .Select(o => new DataPoint(
                DateTime.ParseExact(o.Date + o.Time, "yyyyMMddHHmmss", null).ToOADate(),
                (double)o.ClosePrice))
            .ToList();
            return dataPoints;
        }


    }


}
