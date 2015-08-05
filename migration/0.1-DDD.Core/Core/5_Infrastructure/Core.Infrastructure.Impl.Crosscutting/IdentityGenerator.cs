namespace Core.Infrastructure.Impl.Crosscutting
{
    using Core.Infrastructure.Crosscutting;
    using System;

    internal class IdentityGenerator : IIdentityGenerator
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}