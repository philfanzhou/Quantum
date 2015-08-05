namespace Core.Infrastructure.Crosscutting
{
    public interface IMd5Encryptor
    {
        string Encrypt(string source);
    }
}