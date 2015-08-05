namespace PF.Domain.FilterConditions.Entities
{
    using PF.Domain.Indicator;
    using System;

    public class ConditionIndicatorItem<T> : IConditionItem
        where T : IIndicator
    {
        public T GetValue<T>()
        {
            throw new NotImplementedException();
        }

        public T SetValue<T>()
        {
            throw new NotImplementedException();
        }
    }
}
