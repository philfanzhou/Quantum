namespace Core.Domain.UserContext
{
    public interface IUserFactory<out TUserEntity>
        where TUserEntity : User
    {
        TUserEntity CreateUser(string username, string password, params object[] objParam);
    }
}