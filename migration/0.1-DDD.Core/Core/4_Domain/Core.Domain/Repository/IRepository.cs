namespace Core.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Base interface for implement a "Repository Pattern", for
    /// more information about this pattern see http://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    /// <typeparam name="TAggregateRoot">Type of aggregateRoot for this repository </typeparam>
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        void Add(TAggregateRoot entity);

        void Update(TAggregateRoot entity);

        void Delete(TAggregateRoot entity);

        bool Exists(ISpecification<TAggregateRoot> specification);

        TAggregateRoot Get(string id);

        TAggregateRoot Single(ISpecification<TAggregateRoot> specification);
        
        IEnumerable<TAggregateRoot> GetAll();

        IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification);
    }
}
