namespace Core.Domain
{
    using System;

    public interface IEntity : IEquatable<IEntity>
    {
        string Id { get; }
    }
}