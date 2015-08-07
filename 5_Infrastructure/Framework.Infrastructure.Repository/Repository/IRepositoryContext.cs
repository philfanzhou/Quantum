namespace Framework.Infrastructure.Repository
{
    using System;

    public interface IRepositoryContext : IDisposable
    {
        Guid Id { get; }

        IUnitOfWork UnitOfWork { get; }

        TRepository GetRepository<TRepository>();
    }
}
