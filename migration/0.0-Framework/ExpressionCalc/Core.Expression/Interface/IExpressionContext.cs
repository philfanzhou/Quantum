namespace Core.Expression
{
    /// <summary> 定义运算算数表达式的上下文
    /// </summary>
    public interface IExpressionContext
    {
        /// <summary> 获取或设置double比较运算所允许的误差范围
        /// </summary>
        double Accuracy { get; set; }

        /// <summary> 计算表达式的值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        double Evaluate(string expression, ExpressionParam[] parameters = null);

        /// <summary> 根据表达式创建对应的求值器
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEvaluator CreateEvaluator(Expression expression);
    }
}
