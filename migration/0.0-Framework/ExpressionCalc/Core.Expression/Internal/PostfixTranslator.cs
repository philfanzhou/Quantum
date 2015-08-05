namespace Core.Expression
{
    using System.Collections.Generic;

    /// <summary> 后缀表达式翻译器
    /// </summary>
    internal class PostfixTranslator
    {
        /// <summary> 将表达式集合翻译为后缀表达式集合
        /// </summary>
        /// <param name="expressionItems"></param>
        /// <returns></returns>
        internal static List<ExpressionItem> Translate(IEnumerable<ExpressionItem> expressionItems)
        {
            List<ExpressionItem> result = new List<ExpressionItem>();
            Stack<ExpressionOperatorItem> operatorStack = new Stack<ExpressionOperatorItem>();

            foreach (ExpressionItem item in expressionItems)
            {
                if (!(item is ExpressionOperatorItem))
                {
                    // 如果不是操作符，直接放入结果
                    result.Add(item);
                }
                else
                {
                    GetValue((ExpressionOperatorItem)item, operatorStack, result);
                }
            }

            //将堆栈中剩下的操作符输出到B中
            while (!operatorStack.IsEmpty())
            {
                result.Add(operatorStack.Pop());
            }

            return result;
        }

        private static void GetValue(ExpressionOperatorItem item, Stack<ExpressionOperatorItem> operatorStack, List<ExpressionItem> result)
        {
            if (item.Value.ToString() == OperatorConst.LeftBracket)
            {
                //如果是'('，将它放入堆栈中
                operatorStack.Push(item);
            }
            else if (item.Value.ToString() == OperatorConst.RightBracket) //如果是')'
            {
                while (!operatorStack.IsEmpty()) //不停地弹出堆栈中的内容，直到遇到'('
                {
                    var tmpItem = operatorStack.Pop();
                    if (tmpItem.Value.ToString() == OperatorConst.LeftBracket)
                    {
                        break;
                    }
                    else
                    {
                        result.Add(tmpItem); //将堆栈中弹出的内容放入B中
                    }
                }
            }
            else //既不是'('，也不是')'，是其它操作符，比如+, -, *, /之类的
            {
                GetValue2(item, operatorStack, result);
            }
        }

        private static void GetValue2(ExpressionOperatorItem item, Stack<ExpressionOperatorItem> operatorStack, List<ExpressionItem> result)
        {
            if (!operatorStack.IsEmpty())
            {
                do
                {
                    ExpressionOperatorItem tmpItem = operatorStack.Pop();
                        if (item.Value.Priority > tmpItem.Value.Priority)
                        //如果栈顶元素的优先级小于读取到的操作符
                    {
                        operatorStack.Push(tmpItem); //将栈顶元素放回堆栈
                        operatorStack.Push(item); //将读取到的操作符放回堆栈
                        break;
                    }
                    else //如果栈顶元素的优先级比较高或者两者相等时
                    {
                        result.Add(tmpItem); //将栈顶元素弹出，放入B中
                        if (operatorStack.IsEmpty())
                        {
                            operatorStack.Push(item); //将读取到的操作符压入堆栈中
                            break;
                        }
                    }
                } while (!operatorStack.IsEmpty());
            }
            else //如果堆栈为空，就把操作符放入堆栈中
            {
                operatorStack.Push(item);
            }
        }
    }
}
