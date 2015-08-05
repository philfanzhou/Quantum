namespace Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy
{
    public abstract class LimitationStrategyBase<T> where T : class
    {
        private int count;
        private T last;

        public LimitationStrategyBase()
        {
        }

        public T GetLast()
        {
            return this.last;
        }

        public int GetCount()
        {
            return this.count;
        }

        public void ResetCount()
        {
            this.count = 0;
        }

        public virtual void Add(T instance)
        {
            this.last = instance;
            if (instance != null)
            {
                this.count++;
            }
        }

        public abstract bool ExceedLimit();
    }
}