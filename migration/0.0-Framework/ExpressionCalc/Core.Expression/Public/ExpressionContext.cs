namespace Core.Expression
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ExpressionContext : IExpressionContext
    {
        public ExpressionContext()
        {
            this.Accuracy = 0.0001;
        }

        public double Accuracy { get; set; }
        
        public double Evaluate(string expression, ExpressionParam[] parameters = null)
        {
            var exp = new Expression(expression);
            IEvaluator eval = CreateEvaluator(exp);
            return eval.Evaluate(parameters);
        }

        public IEvaluator CreateEvaluator(Expression expression)
        {
            // 转换为后缀表达式集合
            List<ExpressionItem> postfixExpressions = PostfixTranslator.Translate(expression.Items);

            Evaluator eval = new Evaluator(postfixExpressions, this.Accuracy);

            return eval;
        }
    }
}
