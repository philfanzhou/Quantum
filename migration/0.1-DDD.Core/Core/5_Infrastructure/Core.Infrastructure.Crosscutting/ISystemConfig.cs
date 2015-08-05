namespace Core.Infrastructure.Crosscutting
{
    public interface ISystemConfig
    {
        void AddOrUpdate<T>(T t);

        T ReadConfig<T>();

        void Save();
    }
}
