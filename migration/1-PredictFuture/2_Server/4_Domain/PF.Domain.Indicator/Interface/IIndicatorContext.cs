namespace PF.Domain.Indicator
{
    using System;

    public interface IIndicatorContext
    {
        string StockId { get; }

        DateTime EndDate { get; }
    }
}
