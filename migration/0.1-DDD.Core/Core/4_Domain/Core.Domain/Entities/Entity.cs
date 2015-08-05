namespace Core.Domain
{
    using System;

    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class Entity : IEntity
    {
        #region Constructor

        // EF Needed
        protected Entity() { }

        protected Entity(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            this.Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the persisten object identifier
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        #endregion

        #region Overrides Methods

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return MemberEquals((Entity)other);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Equals(left, null))
            {
                return (Equals(right, null));
            }

            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion

        #region IEquatable Members

        public bool Equals(IEntity other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return MemberEquals(other);
        }

        #endregion

        #region Private Method

        private bool MemberEquals(IEntity other)
        {
            return this.Id.Equals(other.Id);
        }

        #endregion
    }
}
