namespace Core.Application
{
    using Infrastructure.Crosscutting;
    using System.Collections.Generic;

    public static class ProjectionsExtensionMethods
    {
        /// <summary>
        /// Project a type using a DTO
        /// </summary>
        /// <typeparam name="TProjection">The dto projection</typeparam>
        /// <param name="entity">The source entity to project</param>
        /// <returns>The projected type</returns>
        public static TProjection ProjectedAs<TProjection>(this object entity)
            where TProjection : class
        {
            var adapter = ContainerHelper.Resolve<ITypeAdapter>();
            return adapter.Adapt<TProjection>(entity);
        }

        /// <summary>
        /// projected a enumerable collection of items
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <param name="entities">the collection of entity items</param>
        /// <returns>Projected collection</returns>
        public static List<TProjection> ProjectedAsCollection<TProjection>(this IEnumerable<object> entities)
            where TProjection : class
        {
            var adapter = ContainerHelper.Resolve<ITypeAdapter>();
            return adapter.Adapt<List<TProjection>>(entities);
        }
    }
}
