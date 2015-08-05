using System.Text.RegularExpressions;

namespace Core.Infrastructure.Crosscutting.Util.Extend
{
    public static class StringExtend
    {
        /// <summary>
        /// 把string类最常用的静态方法IsNullOrEmpty扩展成“实例”方法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 字符串处理格式
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 匹配是否满足正则表达式要求
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(s, pattern);
            }
        }

        /// <summary>
        /// 正则表达式：返回符合规则的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string s, string pattern)
        {
            if (s == null) 
            {
                return string.Empty;
            }

            return Regex.Match(s, pattern).Value;
        }

        /// <summary>
        /// 字符串是否是整形
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        /// <summary>
        /// 将字符串转换成整形
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        /// <summary>
        /// 转换成Camel表示法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamel(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return s;
            }

            return s[0].ToString().ToLower() + s.Substring(1);
        }

        /// <summary>
        /// 转换成Pascal表示法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascal(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return s;
            }

            return s[0].ToString().ToUpper() + s.Substring(1);
        }
    }
}
