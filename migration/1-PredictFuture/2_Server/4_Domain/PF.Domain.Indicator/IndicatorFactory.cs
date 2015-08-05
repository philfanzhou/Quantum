using System.Collections.Generic;

namespace PF.Domain.Indicator
{
    public static class IndicatorFactory
    {
        private static readonly Dictionary<string, IIndicator> _indicators;

        static IndicatorFactory()
        {
            _indicators = new Dictionary<string, IIndicator>
                {
                    {new AmountIndicator().Name, new AmountIndicator()},
                    {new ClosePriceIndicator().Name, new ClosePriceIndicator()},
                    {new HighPriceIndicator().Name, new HighPriceIndicator()},
                    {new LowPriceIndicator().Name, new LowPriceIndicator()},
                    {new OpenPriceIndicator().Name, new OpenPriceIndicator()},
                };
        }

        public static IIndicator FindIndicatorByName(string name)
        {
            IIndicator indicator;
            _indicators.TryGetValue(name, out indicator);
            return indicator;
        }
    }
}
