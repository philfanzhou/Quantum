namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public interface ILimitationStrategy<T>
    {
        T GetLast();

        int GetCount();

        void ResetCount();

        void Add(T instance);

        bool ExceedLimit();
    }
}
