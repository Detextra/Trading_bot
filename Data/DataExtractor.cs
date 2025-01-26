using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Trading_bot.Data;
class DataExtractor
{
    // format is 20010102,230100 -> 2001/01/02 (YYYY/MM/DD) 23h01 00 secondes
    // https://forextester.com/data/datasources
    public static string dataSource = "../../../../data/EURUSD/EURUSD_23y_1min.txt";

    //static void Main()
    //{
    //    string startDate = "20230101"; // Start date in format YYYYMMDD
    //    string endDate = "20240822";   // End date in format YYYYMMDD

    //    //SavePricesBetweenDates(dataSource, "../../../../data/EURUSD/EURUSD_"+startDate+"-"+endDate+".txt", startDate, endDate);

    //    //"../../../../data/EURUSD/EURUSD_test1Min_dumb_values.txt"
    //}

    public event EventHandler<Price> PriceReceived;

    public void ReadPriceAndSendThem (string inputOHCLFile)
    {
        using (StreamReader sr = new StreamReader(inputOHCLFile))
        {
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                Price price = new Price(parts[0], parts[1], parts[2], decimal.Parse(parts[3]));
                SendingPrice(price);
            }
        }
    }

    public void SendingPrice(Price price)
    {
        //Console.WriteLine($"DataExtractor sending price: {price.PriceValue}");
        OnPriceReceived(price);
    }

    protected virtual void OnPriceReceived(Price price)
    {
        PriceReceived?.Invoke(this, price);
    }

    static void SavePricesBetweenDates(string inputFilePath, string outputFilePath, string startDate, string endDate)
    {
        DateTime start = DateTime.ParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture);
        DateTime end = DateTime.ParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture);

        using (StreamReader sr = new StreamReader(inputFilePath))
        using (StreamWriter sw = new StreamWriter(outputFilePath))
        {
            string line;
            bool isFirstLine = true;

            while ((line = sr.ReadLine()) != null)
            {
                // Skip the first line (header)
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                // Split the line into parts
                string[] parts = line.Split(',');

                if (parts.Length >= 2)
                {
                    // Parse the date from the second field
                    DateTime currentDate = DateTime.ParseExact(parts[1], "yyyyMMdd", CultureInfo.InvariantCulture);

                    // Check if the date is within the range
                    if (currentDate >= start && currentDate <= end)
                    {
                        // Write the line to the output file
                        sw.WriteLine(line);
                    }
                }
            }
        }
    }
}