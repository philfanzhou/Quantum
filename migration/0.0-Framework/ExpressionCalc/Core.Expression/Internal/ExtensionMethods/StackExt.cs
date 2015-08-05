using System.Collections.Generic;
namespace Core.Expression
{
    internal static class StackExt
    {
        /// <summary> 判断堆栈是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        internal static bool IsEmpty<T>(this Stack<T> me)
        {
            return me.Count == 0;
        }
    }
}
