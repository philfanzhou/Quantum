namespace Core.Expression
{
    using System;

    [Serializable]
    internal class Addition : IExpressionOperator
    {
        public int Priority
        {
            get { return 4; }
        }

        public double Operate(params double[] value)
        {
            if (value == null || value.Length < 2)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return value[0] + value[1];
        }

        public override string ToString()
        {
            return OperatorConst.Addition;
        }
    }
}