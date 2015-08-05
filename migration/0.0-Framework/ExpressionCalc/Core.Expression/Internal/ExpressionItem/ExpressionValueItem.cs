namespace Core.Expression
{
    using System;
    using System.Globalization;

    [Serializable]
    internal class ExpressionValueItem : ExpressionItem
    {
        private readonly double _value;

        public ExpressionValueItem(double value)
        {
            this._value = value;
        }

        public double Value
        {
            get { return this._value; }
        }

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
