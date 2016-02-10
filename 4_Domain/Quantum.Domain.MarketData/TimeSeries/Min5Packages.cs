using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 以5分钟作为时间片的数据包裹集合
    /// </summary>
    public class Min5Collections<T> : PackageCollections<T>
        where T : ITimeSeries
    {
        protected override void GetTimeZone(DateTime currentTime, out DateTime startTime, out DateTime endTime)
        {
            // 取得分钟的10位数
            int tenDigit = FindNum(currentTime.Minute, 2);
            // 以分钟10位数构造一个临时时间
            DateTime tmpTime = currentTime.Date
                .AddHours(currentTime.Hour)
                .AddMinutes(tenDigit * 10);

            // 当前时间在5分之后，起始时间为5分
            startTime = tmpTime;
            if (currentTime - tmpTime > new TimeSpan(0, 5, 0))
            {
                startTime = startTime.AddMinutes(5);
            }

            // 当前时间在5分之内，起始时间为10分整数
            endTime = startTime.AddMinutes(5);
        }

        /// <summary>
        /// 求num在n位上的数字,取个位,取十位 
        /// </summary>
        /// <param name="num">正整数</param>
        /// <param name="n">所求数字位置(个位 1,十位 2 依此类推)</param>
        /// <returns></returns>
        private int FindNum(int num, int n)
        {
            int power = (int)Math.Pow(10, n);
            return (num - (num / power) * power) * 10 / power;
        }
    }
}
