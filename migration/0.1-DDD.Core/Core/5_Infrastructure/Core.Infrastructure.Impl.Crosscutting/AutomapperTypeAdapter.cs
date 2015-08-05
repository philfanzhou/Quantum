namespace Core.Infrastructure.Impl.Crosscutting
{
    using AutoMapper;
    using Core.Infrastructure.Crosscutting;

    /// <summary>
    /// Automapper type adapter implementation
    /// </summary>
    internal class AutomapperTypeAdapter
        :ITypeAdapter
    {
        #region ITypeAdapter Members

        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TTarget>(object source) where TTarget : class
        {
            return Mapper.Map<TTarget>(source);
        }

        #endregion
    }
}
