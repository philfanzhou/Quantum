namespace PF.Domain.FilterConditions.Entities
{
    public interface IConditionItem
    {
        T GetValue<T>();

        T SetValue<T>();
    }
}
