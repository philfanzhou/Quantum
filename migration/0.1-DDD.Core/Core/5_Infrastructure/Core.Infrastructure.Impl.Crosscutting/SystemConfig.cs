using Core.Infrastructure.Crosscutting;
namespace Core.Infrastructure.Impl.Crosscutting
{
    internal class SystemConfig : ISystemConfig
    {
        public void AddOrUpdate<T>(T t)
        {
            throw new System.NotImplementedException();
        }

        public T ReadConfig<T>()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}