
namespace Quantum.Domain.Trading
{
    public interface IAccount
    {
        bool Buy(string code, double price, int quantity);

        int AvailableQuantityToBuy(string code, double price);

        bool Sell(string code, double price, int quantity);

        int AvailableQuantityToSell(string code);

        bool TransferIn(double amount);

        bool TransferOut(double amount);
    }
}
