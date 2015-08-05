namespace Core.Expression
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary> 算数表达式的拆分器
    /// </summary>
    internal class ExpressionSpliter
    {
        internal static IEnumerable<string> Split(string expression)
        {
            List<string> result = new List<string>();

            StringBuilder resultItemBuilder = new StringBuilder();
            List<char> charArray = expression.ToList();
            for (int i = 0; i < charArray.Count; i++)
            {
                string oneCharString = charArray[i].ToString(CultureInfo.InvariantCulture);
                string twoCharString = oneCharString;
                // 读取两个字符，判断是不是特殊双字符的操作符.etc: >=, <=, &&, ||
                if (i < charArray.Count - 1)
                {
                    twoCharString += charArray[i + 1].ToString(CultureInfo.InvariantCulture);
                }

                if (OperatorMethod.IsOperator(twoCharString) &&
                    twoCharString.Length == 2)
                {
                    if (resultItemBuilder.Length > 0)
                    {
                        result.Add(resultItemBuilder.ToString());
                        resultItemBuilder.Clear();
                    }

                    result.Add(twoCharString);
                    i++;
                }
                else if (OperatorMethod.IsOperator(oneCharString))
                {
                    if (resultItemBuilder.Length > 0)
                    {
                        result.Add(resultItemBuilder.ToString());
                        resultItemBuilder.Clear();
                    }

                    result.Add(oneCharString);
                }
                else
                {
                    resultItemBuilder.Append(oneCharString);
                }
            }

            if (resultItemBuilder.Length > 0)
            {
                result.Add(resultItemBuilder.ToString());
                resultItemBuilder.Clear();
            }

            return result;
        }
    }
}
