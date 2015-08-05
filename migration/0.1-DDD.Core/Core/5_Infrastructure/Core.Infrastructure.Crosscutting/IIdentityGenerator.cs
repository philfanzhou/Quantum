namespace Core.Infrastructure.Crosscutting
{
    using System;

    public interface IIdentityGenerator
    {
        Guid NewGuid();
    }
}