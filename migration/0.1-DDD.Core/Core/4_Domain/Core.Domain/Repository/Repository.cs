namespace Core.Domain
{
    using System;
    using System.Collections.Generic;

    public class Repository<TAggregateRoot>
        : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        #region Fields

        private readonly RepositoryContext _context;

        #endregion

        #region Constructor

        protected Repository(IRepositoryContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException("context");
            }

            this._context = context as RepositoryContext;
            if (null == this._context)
            {
                throw new ArgumentException("context");
            }
        }

        #endregion

        #region IRepository Members

        public virtual void Add(TAggregateRoot entity)
        {
            this._context.UnitOfWork.RegisterNew(entity);
        }

        public virtual void Update(TAggregateRoot entity)
        {
            this._context.UnitOfWork.RegisterModified(entity);
        }

        public virtual void Delete(TAggregateRoot entity)
        {
            this._context.UnitOfWork.RegisterDeleted(entity);
        }

        public virtual bool Exists(ISpecification<TAggregateRoot> specification)
        {
            return this._context.Exists(specification);
        }

        public virtual TAggregateRoot Get(string id)
        {
            return this._context.Get<TAggregateRoot>(id);
        }

        public virtual TAggregateRoot Single(ISpecification<TAggregateRoot> specification)
        {
            return this._context.Single(specification);
        }

        public virtual IEnumerable<TAggregateRoot> GetAll()
        {
            return this._context.GetAll<TAggregateRoot>();
        }

        public virtual IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification)
        {
            return this._context.GetAll(specification);
        }

        #endregion
    }
}
