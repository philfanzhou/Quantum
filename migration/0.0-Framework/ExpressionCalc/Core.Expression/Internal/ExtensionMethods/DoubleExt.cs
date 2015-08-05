using System;

namespace Core.Expression
{
    /// <summary> 定义double类型的扩展
    /// </summary>
    internal static class DoubleExt
    {
        /// <summary> 根据指定的精度进行比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value">参与比较的值</param>
        /// <param name="e"> 比较的精度 </param>
        /// <returns></returns>
        internal static bool EqualTo(this double a, double value, double e)
        {
            return Math.Abs(a - value) < e;
        }

        /// <summary> 按照指定精度进行大于比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value">参与比较的值</param>
        /// <param name="e">比较的精度 </param>
        /// <returns></returns>
        internal static bool GreaterThan(this double a, double value, double e)
        {
            if (!a.EqualTo(value, e))
            {
                return a > value;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 按照指定精度进行小于比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value">参与比较的值</param>
        /// <param name="e">比较的精度</param>
        /// <returns></returns>
        internal static bool LessThan(this double a, double value, double e)
        {
            if (!a.EqualTo(value, e))
            {
                return a < value;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 按照指定精度进行大于等于比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value">参与比较的值</param>
        /// <param name="e">比较的精度</param>
        /// <returns></returns>
        internal static bool GreaterThanOrEqualTo(this double a, double value, double e)
        {
            return !a.LessThan(value, e);
        }

        /// <summary> 按照指定精度进行大于等于比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value">参与比较的值</param>
        /// <param name="e">比较的精度</param>
        /// <returns></returns>
        internal static bool LessThanOrEqualTo(this double a, double value, double e)
        {
            return !a.GreaterThan(value, e);
        }

        internal static bool And(this double self, double value, double e)
        {
            if (!self.EqualTo(0, e) && !value.EqualTo(0, e))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool Or(this double self, double value, double e)
        {
            if (!self.EqualTo(0, e) || !value.EqualTo(0, e))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
