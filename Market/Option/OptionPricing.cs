using System;

namespace Trading_bot.Market.Option
{
    internal class OptionPricing
    {
        public static double NormalCDF(double x)
        {
            return 0.5 * (1.0 + Math.Sign(x) * Math.Sqrt(1 - Math.Exp(-0.5 * x * x)));
        }

        public static decimal CalculateCallOptionPremium(decimal stockPrice, decimal strikePrice, decimal riskFreeRate,
                                                 decimal timeToExpiration, decimal volatility)
        {
            double d1 = (Math.Log((double)stockPrice / (double)strikePrice) + ((double)riskFreeRate + 0.5 * Math.Pow((double)volatility, 2)) * (double)timeToExpiration) 
                / ((double)volatility * Math.Sqrt((double)timeToExpiration));

            double d2 = d1 - (double)volatility * Math.Sqrt((double)timeToExpiration);

            decimal callPremium = (decimal)((double)stockPrice * NormalCDF(d1) -
                                             (double)strikePrice * Math.Exp((double)(-riskFreeRate) * (double)timeToExpiration) * NormalCDF(d2));
            return callPremium;
        }

        public static decimal CalculatePutOptionPremium(decimal stockPrice, decimal strikePrice, decimal riskFreeRate,
                                                 decimal timeToExpiration, decimal volatility)
        {
            double d1 = (Math.Log((double)stockPrice / (double)strikePrice) +
                         ((double)riskFreeRate + 0.5 * Math.Pow((double)volatility, 2)) * (double)timeToExpiration)
                / ((double)volatility * Math.Sqrt((double)timeToExpiration));

            double d2 = d1 - (double)volatility * Math.Sqrt((double)timeToExpiration);

            decimal putPremium = (decimal)((double)strikePrice * Math.Exp((double)(-riskFreeRate) * (double)timeToExpiration) * NormalCDF(-d2) -
                                           (double)stockPrice * NormalCDF(-d1));
            return putPremium;
        }
    }
}
