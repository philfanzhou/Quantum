namespace Core.Infrastructure.Impl.Repository.EntityFramework
{
    using Core.Domain;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;

    public class EntityFrameworkRepositoryContext<TDbContext> : RepositoryContext
        where TDbContext : DbContext
    {
        #region Fields
        private readonly ThreadLocal<TDbContext> _threadLocalDbContext;             
        #endregion

        #region Constructor

        public EntityFrameworkRepositoryContext(TDbContext dbContext)
        {
            _threadLocalDbContext = new ThreadLocal<TDbContext> {Value = dbContext};
        }

        #endregion

        #region Protected Methods

        protected override void DoRegisterNew<TEntity>(TEntity item)
        {
            _threadLocalDbContext.Value.Set<TEntity>().Add(item);
        }

        protected override void DoRegisterModified<TEntity>(TEntity item)
        {
            _threadLocalDbContext.Value.Entry(item).State = EntityState.Modified;
        }

        protected override void DoRegisterDeleted<TEntity>(TEntity item)
        {
            _threadLocalDbContext.Value.Entry(item).State = EntityState.Deleted;
        }

        protected override void DoCommit()
        {
            _threadLocalDbContext.Value.SaveChanges();
        }

        protected override void DoRollback()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            _threadLocalDbContext.Value.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = System.Data.Entity.EntityState.Unchanged);
        }

        protected override bool DoExists<TEntity>(ISpecification<TEntity> specification)
        {
            var count = GetSet<TEntity>().Count(specification.GetExpression());
            return count != 0;
        }

        protected override TEntity DoGet<TEntity>(string id)
        {
            return GetSet<TEntity>().Find(id);
        }

        protected override TEntity DoSingle<TEntity>(ISpecification<TEntity> specification)
        {
            return GetSet<TEntity>().Single(specification.GetExpression());
        }

        protected override IEnumerable<TEntity> DoGetAll<TEntity>()
        {
            return GetSet<TEntity>();
        }

        protected override IEnumerable<TEntity> DoGetAll<TEntity>(ISpecification<TEntity> specification)
        {
            return GetSet<TEntity>().Where(specification.GetExpression());
        }

        #endregion

        #region Private Methods
        public DbSet<TEntity> GetSet<TEntity>()
            where TEntity : class
        {
            return _threadLocalDbContext.Value.Set<TEntity>();
        }
        #endregion

        #region IDisposable Member

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                // Clean up managed resources
                if (_threadLocalDbContext.Value != null)
                {
                    _threadLocalDbContext.Value.Dispose();
                    _threadLocalDbContext.Value = null;
                }

                if (_threadLocalDbContext != null)
                {
                    _threadLocalDbContext.Dispose();
                }
            }

            Disposed = true;
        }

        #endregion
    }
}