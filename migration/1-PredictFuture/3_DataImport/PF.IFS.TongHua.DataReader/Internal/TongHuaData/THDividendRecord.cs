using System;
using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    [StructLayout(LayoutKind.Sequential)]
    internal class THDividendRecord : IDividendItem
    {
        #region Field

        [MarshalAs(UnmanagedType.Struct)]
        private readonly THDateTimeStruct date;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly byte[] w1;

        [MarshalAs(UnmanagedType.Struct)]
        private readonly THDateTimeStruct exdividendDate;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private readonly byte[] cash;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private readonly byte[] split;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private readonly byte[] bonus;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private readonly byte[] dispatch;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private readonly byte[] price;

        [MarshalAs(UnmanagedType.Struct)]
        private readonly THDateTimeStruct registerDate;

        [MarshalAs(UnmanagedType.Struct)]
        private readonly THDateTimeStruct listingDate;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 178)]
        private readonly byte[] description;

        #endregion

        #region Property

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return this.date.Value; }
        }

        /// <summary>
        /// 除权日
        /// </summary>
        public DateTime ExdividendDate
        {
            get { return this.exdividendDate.Value; }
        }

        /// <summary>
        /// 分红
        /// </summary>
        public double Cash
        {
            get { return BitConverter.ToDouble(cash, 0); }
        }

        /// <summary>
        /// 总拆股
        /// </summary>
        public double Split
        {
            get { return BitConverter.ToDouble(split, 0); }
        }

        /// <summary>
        /// 转增股
        /// </summary>
        public double Bonus
        {
            get { return BitConverter.ToDouble(bonus, 0); }
        }

        /// <summary>
        /// 配股
        /// </summary>
        public double Dispatch
        {
            get { return BitConverter.ToDouble(dispatch, 0); }
        }

        /// <summary>
        /// 配股价
        /// </summary>
        public double Price
        {
            get { return BitConverter.ToDouble(price, 0); }
        }

        /// <summary>
        /// 登记日
        /// </summary>
        public DateTime RegisterDate
        {
            get { return this.registerDate.Value; }
        }

        /// <summary>
        /// 上市日
        /// </summary>
        public DateTime ListingDate
        {
            get { return this.listingDate.Value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return StringUtil.ReadString(this.description); }
        }

        #endregion
    }
}
