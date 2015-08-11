using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 交易费用
    /// </summary>
    internal class TradeCost
    {
        /// <summary>
        /// 佣金费率
        /// </summary>
        private decimal commissionRate = 0.0003m;

        /// <summary>
        /// 印花税率
        /// </summary>
        private decimal stampDutyRate = 0.001m;

        /// <summary>
        /// 交易费率
        /// </summary>
        private decimal transferFeesRate = 0.00002m;

        public decimal GetCommission(decimal price, int quantity)
        {
            decimal amount = price * quantity;
            decimal fee = amount * commissionRate;

            if(fee < 5)
            {
                return 5;
            }
            else
            {
                return fee;
            }
        }

        public decimal GetStampDuty(string code, decimal price, int quantity)
        {
            if(!IsStock(code))
            {
                return 0;
            }

            return price * quantity * stampDutyRate;
        }

        public decimal GetTransferFees(string code, decimal price, int quantity)
        {
            if (!IsStock(code))
            {
                return 0;
            }

            if(IsShanghaiStock(code))
            {
                return price * quantity * transferFeesRate;
            }
            else
            {
                return 0;
            }
        }

        private bool IsStock(string code)
        {
            return IsShanghaiStock(code) || IsShenzhenStock(code);
        }

        private bool IsShanghaiStock(string code)
        {
            if (code.Length != 6)
            {
                return false;
            }

            if (!code.StartsWith("000") &&
                !code.StartsWith("001") &&
                !code.StartsWith("002") &&
                !code.StartsWith("300"))
            {
                return false;
            }
            return true;
        }

        private bool IsShenzhenStock(string code)
        {
            if (code.Length != 6)
            {
                return false;
            }

            if (!code.StartsWith("600") &&
                !code.StartsWith("601") &&
                !code.StartsWith("603"))
            {
                return false;
            }

            return true;
        }
    }
}
