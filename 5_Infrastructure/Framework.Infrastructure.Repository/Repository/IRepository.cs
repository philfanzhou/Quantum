
namespace Framework.Infrastructure.Repository
{
    using System.Collections.Generic;

    /// <summary>
    /// Base interface for implement a "Repository Pattern", for
    /// more information about this pattern see http://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    /// <typeparam name="TEntity">Type of entity for this repository </typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        bool Exists(ISpecification<TEntity> specification);

        TEntity Get(string id);

        TEntity Single(ISpecification<TEntity> specification);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(ISpecification<TEntity> specification);
    }
}
