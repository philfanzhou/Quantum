using Quantum.Infrastructure.Trading.Repository;

namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 交易费用
    /// </summary>
    internal class TradeCost
    {
        public static decimal GetCommission(double price, int quantity)
        {
            decimal amount = (decimal)price * quantity;
            decimal fee = amount * Broker.CommissionRate;

            if(fee < 5)
            {
                return 5;
            }
            else
            {
                return fee;
            }
        }

        public static decimal GetStampDuty(TradeType type, string code, double price, int quantity)
        {
            if(type != TradeType.Sell)
            {
                return 0;
            }

            if(!IsStock(code))
            {
                return 0;
            }

            return (decimal)price * quantity * Broker.StampDutyRate;
        }

        public static decimal GetTransferFees(string code, double price, int quantity)
        {
            if (!IsStock(code))
            {
                return 0;
            }

            if(IsShanghaiStock(code))
            {
                return (decimal)price * quantity * Broker.TransferFeesRate;
            }
            else
            {
                return 0;
            }
        }

        private static bool IsStock(string code)
        {
            return IsShanghaiStock(code) || IsShenzhenStock(code);
        }

        private static bool IsShenzhenStock(string code)
        {
            if (code.Length != 6)
            {
                return false;
            }

            if (code.StartsWith("000") ||
                code.StartsWith("001") ||
                code.StartsWith("002") ||
                code.StartsWith("300"))
            {
                return true;
            }
            return false;
        }

        private static bool IsShanghaiStock(string code)
        {
            if (code.Length != 6)
            {
                return false;
            }

            if (code.StartsWith("600") ||
                code.StartsWith("601") ||
                code.StartsWith("603"))
            {
                return true;
            }

            return false;
        }
    }
}
