namespace Core.Expression
{
    using System;

    [Serializable]
    public class ExpressionParam : IEquatable<ExpressionParam>
    {
        public ExpressionParam(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name
        {
            get;
            private set;
        }

        public double Value
        {
            get;
            private set;
        }

        #region Override

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((ExpressionParam)obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.Name, this.Value);
        }
        
        #endregion

        #region IEquatable Members

        public bool Equals(ExpressionParam other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return 
                this.Name.Equals(other.Name) 
                && this.Value.Equals(other.Value);
        }
        
        #endregion
    }
}
