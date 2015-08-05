namespace Core.Expression
{
    using System.Collections.Generic;

    /// <summary> 对指定表达式进行编译后的计算器
    /// </summary>
    public interface IEvaluator
    {
        /// <summary> 获取double运算所允许的误差范围
        /// </summary>
        double Accuracy { get; }

        /// <summary> 计算表达式的值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        double Evaluate(IEnumerable<ExpressionParam> parameters = null);
    }
}
