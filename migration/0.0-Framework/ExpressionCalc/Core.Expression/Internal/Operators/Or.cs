namespace Core.Expression
{
    using System;

    [Serializable]
    internal class Or : IExpressionOperator
    {
        public int Priority
        {
            get { return 1; }
        }

        public double Operate(params double[] value)
        {
            if (value == null || value.Length < 3)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return value[0].Or(value[1], value[2]) ? 1 : 0;
        }

        public override string ToString()
        {
            return OperatorConst.Or;
        }
    }
}