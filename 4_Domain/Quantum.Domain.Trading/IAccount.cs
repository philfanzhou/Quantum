
namespace Quantum.Domain.Trading
{
    public interface IAccount
    {
        bool Buy(string code, decimal price, int quantity);

        int AvailableQuantityToBuy(string code, decimal price);

        bool Sell(string code, decimal price, int quantity);

        int AvailableQuantityToSell(string code);

        void TransferIn(decimal amount);

        void TransferOut(decimal amount);
    }
}
