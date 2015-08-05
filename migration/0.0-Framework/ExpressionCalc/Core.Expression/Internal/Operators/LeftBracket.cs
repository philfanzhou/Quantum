namespace Core.Expression
{
    using System;

    [Serializable]
    internal class LeftBracket : IExpressionOperator
    {
        public int Priority
        {
            get { return 0; }
        }

        public double Operate(params double[] value)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return OperatorConst.LeftBracket;
        }
    }
}