﻿using OxyPlot;
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
            OhclDatas = new List<OhclList>();
            foreach (int period in periodInMinutes)
            {
                Console.WriteLine("Creating OhclDatas for " +  period + " minutes period");
                OhclDatas.Add(new OhclList(period));
            }

            // add a 24h OhclDatas to plot it in comparison of risk/performance end of trading chart
            OhclDatas.Add(new OhclList(60*24));
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

        public List<DataPoint> Get24HoursOhclListsForPlotting()
        {
            OhclList ohcl24h = GetOhclListsByPeriodInMinutes(60 * 24);
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
