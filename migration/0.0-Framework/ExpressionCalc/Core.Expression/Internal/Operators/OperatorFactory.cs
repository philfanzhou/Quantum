namespace Core.Expression
{
    using System;

    internal static class OperatorFactory
    {
        public static IExpressionOperator CreateOperator(string operatorType)
        {
            switch (operatorType)
            {
                case OperatorConst.Addition:
                    return new Addition();
                case OperatorConst.Subtraction:
                    return new Subtraction();
                case OperatorConst.Multiplication:
                    return new Multiplication();
                case OperatorConst.Division:
                    return new Division();
                case OperatorConst.LeftBracket:
                    return new LeftBracket();
                case OperatorConst.RightBracket:
                    return new RightBracket();
                case OperatorConst.Greater:
                    return new Greater();
                case OperatorConst.GreaterOrEqual:
                    return new GreaterOrEqual();
                case OperatorConst.Less:
                    return new Less();
                case OperatorConst.LessOrEqual:
                    return new LessOrEqual();
                case OperatorConst.And:
                    return new And();
                case OperatorConst.Or:
                    return new Or();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
