namespace Core.Domain
{
    using System;

    public interface IRepositoryContext : IDisposable
    {
        Guid Id { get; }

        IUnitOfWork UnitOfWork { get; }

        TRepository GetRepository<TRepository>();
    }
}
