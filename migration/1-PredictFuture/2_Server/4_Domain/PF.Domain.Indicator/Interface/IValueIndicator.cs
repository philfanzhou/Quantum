namespace PF.Domain.Indicator
{
    using System;

    public interface IValueIndicator : IIndicator
    {
        string Unit { get; }
    }
}
