namespace Core.Expression
{
    using System;

    [Serializable]
    internal class Division : IExpressionOperator
    {
        public int Priority
        {
            get { return 5; }
        }

        public double Operate(params double[] value)
        {
            if (value == null || value.Length < 2)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return value[0] / value[1];
        }

        public override string ToString()
        {
            return OperatorConst.Division;
        }
    }
}