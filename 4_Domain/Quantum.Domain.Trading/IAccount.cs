
namespace Quantum.Domain.Trading
{
    public interface IAccount
    {
        void Buy(string code, double price, int quantity);

        int AvailableQuantityToBuy(double price);

        void Sell(string code, double price, int quantity);

        int AvailableQuantityToSell(string code);

        void TransferIn(decimal amount);

        void TransferOut(decimal amount);
    }
}
