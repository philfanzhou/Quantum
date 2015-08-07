namespace Framework.Infrastructure.Repository
{
    using System;

    public interface IEntity : IEquatable<IEntity>
    {
        string Id { get; }
    }
}
