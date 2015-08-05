namespace Core.Infrastructure.Crosscutting
{
    public interface ISerializer
    {
        string JsonSerializer<T>(T t);

        T JsonDeserialize<T>(string jsonString);
    }
}
