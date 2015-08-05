namespace Core.Domain
{
    using Core.Infrastructure.Crosscutting;
    using System;
    using System.Collections.Generic;

    public abstract class RepositoryContext
        : IRepositoryContext, IUnitOfWork
    {
        #region Fields
        private readonly Guid _id = Guid.NewGuid();
        #endregion

        #region IRepositoryContext Members

        public Guid Id
        {
            get { return _id; }
        }

        public IUnitOfWork UnitOfWork
        {
            get { return this; }
        }

        public TRepository GetRepository<TRepository>()
        {
            TRepository repository;
            if (ContainerHelper.Instance.IsRegistered<TRepository>())
            {
                repository = ContainerHelper.Instance.Resolve<TRepository>(
                    new ConstructorParameter { Name = "context", Value = this });
            }
            else
            {
                repository = (TRepository)Activator.CreateInstance(typeof(TRepository), this);
            }

            return repository;
        }

        #endregion

        #region IUnitOfWork Members
        void IUnitOfWork.RegisterNew<TEntity>(TEntity item)
        {
            ThrowIfDisposed();

            if (null == item)
            {
                throw new RepositoryException("Cannot add null entity");
            }

            try
            {
                this.DoRegisterNew(item);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        void IUnitOfWork.RegisterModified<TEntity>(TEntity item)
        {
            ThrowIfDisposed();

            if (null == item)
            {
                throw new RepositoryException("Cannot modify null item");
            }

            try
            {
                this.DoRegisterModified(item);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        void IUnitOfWork.RegisterDeleted<TEntity>(TEntity item)
        {
            ThrowIfDisposed();

            if (null == item)
            {
                throw new RepositoryException("Cannot remove null entity");
            }

            try
            {
                this.DoRegisterDeleted(item);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        void IUnitOfWork.Commit()
        {
            ThrowIfDisposed();

            try
            {
                this.DoCommit();
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        void IUnitOfWork.Rollback()
        {
            ThrowIfDisposed();

            try
            {
                this.DoRollback();
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }
        #endregion

        #region Internal Methods
        internal bool Exists<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class
        {
            ThrowIfDisposed();

            if (null == specification)
            {
                var e = new ArgumentNullException("specification");
                throw new RepositoryException(e.Message, e);
            }

            try
            {
                return DoExists(specification);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        internal TEntity Get<TEntity>(string id)
            where TEntity : class
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(id))
            {
                var e = new ArgumentNullException("id");
                throw new RepositoryException(e.Message, e);
            }

            try
            {
                return DoGet<TEntity>(id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        internal TEntity Single<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class
        {
            ThrowIfDisposed();

            if (null == specification)
            {
                var e = new ArgumentNullException("specification");
                throw new RepositoryException(e.Message, e);
            }

            try
            {
                return DoSingle(specification);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        internal IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : class
        {
            ThrowIfDisposed();

            try
            {
                return DoGetAll<TEntity>();
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        internal IEnumerable<TEntity> GetAll<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class
        {
            ThrowIfDisposed();

            if (null == specification)
            {
                var e = new ArgumentNullException("specification");
                throw new RepositoryException(e.Message, e);
            }

            try
            {
                return DoGetAll(specification);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        #endregion

        #region Static Method

        public static IRepositoryContext Create()
        {
            return ContainerHelper.Resolve<IRepositoryContext>();
        }

        public static T Create<T>()
            where T : IRepositoryContext
        {
            return ContainerHelper.Resolve<T>();
        }

        #endregion

        #region Abstract Methods

        protected abstract void DoRegisterNew<TEntity>(TEntity item)
            where TEntity : class;

        protected abstract void DoRegisterModified<TEntity>(TEntity item)
            where TEntity : class;

        protected abstract void DoRegisterDeleted<TEntity>(TEntity item)
            where TEntity : class;

        protected abstract void DoCommit();

        protected abstract void DoRollback();

        protected abstract bool DoExists<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class;

        protected abstract TEntity DoGet<TEntity>(string id)
            where TEntity : class;

        protected abstract TEntity DoSingle<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class;

        protected abstract IEnumerable<TEntity> DoGetAll<TEntity>()
            where TEntity : class;

        protected abstract IEnumerable<TEntity> DoGetAll<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class;
        
        #endregion

        #region IDisposable Member

        protected bool Disposed;

        ~RepositoryContext()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("RepositoryContext", "RepositoryContext has been disposed.");
            }
        }

        protected abstract void Dispose(bool disposing);

        #endregion
    }
}
