namespace PF.Domain.Indicator
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IndicatorCategoryAttribute :Attribute
    {
        public IndicatorCategoryAttribute() { }
        public string Name { get; set; }
    }
}
