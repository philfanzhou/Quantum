namespace Core.Expression
{
    using System;

    [Serializable]
    internal class GreaterOrEqual : IExpressionOperator
    {
        public int Priority
        {
            get { return 3; }
        }

        public double Operate(params double[] value)
        {
            if (value == null || value.Length < 3)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return value[0].GreaterThanOrEqualTo(value[1], value[2]) ? 1 : 0;
        }

        public override string ToString()
        {
            return OperatorConst.GreaterOrEqual;
        }
    }
}