namespace Core.Expression
{
    using System.Collections.Generic;

    /// <summary>封装运算符相关操作
    /// </summary>
    internal class OperatorMethod
    {
        /// <summary>操作符集合
        /// </summary>
        private static readonly HashSet<string> OperatorTable = new HashSet<string>
                                                       {
                                                           OperatorConst.GreaterOrEqual,
                                                           OperatorConst.LessOrEqual,
                                                           OperatorConst.Greater,
                                                           OperatorConst.Less,
                                                           OperatorConst.Addition,
                                                           OperatorConst.Subtraction,
                                                           OperatorConst.Division,
                                                           OperatorConst.Multiplication,
                                                           OperatorConst.LeftBracket,
                                                           OperatorConst.RightBracket,
                                                           OperatorConst.And,
                                                           OperatorConst.Or
                                                       };

        /// <summary> 判断指定字符串是否为运算符
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>字符串为运算符返回true</returns>
        internal static bool IsOperator(string stringValue)
        {
            return OperatorTable.Contains(stringValue);
        }
    }
}
