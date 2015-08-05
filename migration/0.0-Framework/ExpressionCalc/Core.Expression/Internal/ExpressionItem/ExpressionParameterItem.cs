namespace Core.Expression
{
    using System;

    [Serializable]
    internal class ExpressionParameterItem : ExpressionItem
    {
        private readonly string _parameterName;

        public ExpressionParameterItem(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            this._parameterName = value;
        }

        public string Value
        {
            get { return this._parameterName; }
        }

        public override string ToString()
        {
            return _parameterName;
        }
    }
}
