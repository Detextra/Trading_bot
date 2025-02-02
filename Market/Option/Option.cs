namespace Trading_bot_WPF.Market.Option
{
    internal abstract class Option
    {
        public string Ticker { get; set; }
        public decimal StrikePrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Premium { get; set; }
        public int Quantity { get; set; }

        public Option(string ticker, decimal strikePrice, DateTime expirationDate, decimal premium, int quantity)
        {
            Ticker = ticker;
            StrikePrice = strikePrice;
            ExpirationDate = expirationDate;
            Premium = premium;
            Quantity = quantity;
        }

        public bool IsExpired(DateTime currentDate)
        {
            return currentDate >= ExpirationDate;
        }

        public abstract decimal GetOptionValue(decimal currentPrice);
    }

    internal class CallOption : Option
    {
        public CallOption(string ticker, decimal strikePrice, DateTime expirationDate, decimal premium, int quantity)
            : base(ticker, strikePrice, expirationDate, premium, quantity) { }

        // a call option value at expiration is the difference between the stock price and strike price (if the price is above the strike price)
        public override decimal GetOptionValue(decimal currentPrice)
        {
            if (currentPrice > StrikePrice)
            {
                return (currentPrice - StrikePrice) * Quantity - Premium;
            }
            return -Premium;
        }
    }

    internal class PutOption : Option
    {
        public PutOption(string ticker, decimal strikePrice, DateTime expirationDate, decimal premium, int quantity)
            : base(ticker, strikePrice, expirationDate, premium, quantity) { }

        // a put option value at expiration is the difference between the strike price and the stock price (if the price is below the strike price)
        public override decimal GetOptionValue(decimal currentPrice)
        {
            if (currentPrice < StrikePrice)
            {
                return (StrikePrice - currentPrice) * Quantity - Premium;
            }
            return -Premium;
        }
    }
}
