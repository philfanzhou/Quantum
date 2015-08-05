namespace Core.Expression
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [KnownType(typeof(Addition))]
    [KnownType(typeof(And))]
    [KnownType(typeof(Division))]
    [KnownType(typeof(Greater))]
    [KnownType(typeof(GreaterOrEqual))]
    [KnownType(typeof(LeftBracket))]
    [KnownType(typeof(Less))]
    [KnownType(typeof(LessOrEqual))]
    [KnownType(typeof(Multiplication))]
    [KnownType(typeof(Or))]
    [KnownType(typeof(RightBracket))]
    [KnownType(typeof(Subtraction))]
    internal class ExpressionOperatorItem : ExpressionItem
    {
        private readonly IExpressionOperator _operator;

        public ExpressionOperatorItem(string operatorType)
        {
            this._operator = OperatorFactory.CreateOperator(operatorType);
        }

        public IExpressionOperator Value
        {
            get { return this._operator; }
        }

        public override string ToString()
        {
            return this._operator.ToString();
        }
    }
}
