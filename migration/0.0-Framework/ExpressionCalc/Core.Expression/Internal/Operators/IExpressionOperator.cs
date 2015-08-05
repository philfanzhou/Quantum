namespace Core.Expression
{
    internal interface IExpressionOperator
    {
        int Priority { get; }

        double Operate(params double[] value);
    }
}
