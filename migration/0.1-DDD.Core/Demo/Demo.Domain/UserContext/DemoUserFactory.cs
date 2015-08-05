namespace Demo.Domain.UserContext
{
    using Core.Infrastructure.Crosscutting;
    using Extension.Domain.UserContext;

    public class DemoUserFactory : IUserFactory<DemoUser>
    {
        public DemoUser CreateUser(string userName, string passWord, params object[] objParam)
        {
            IIdentityGenerator idGenerator = FactoryHandler<IIdentityGenerator>.CreateOperator();
            DemoUser user = new DemoUser(idGenerator.NewGuid())
            {
                Username = userName,
                Password = passWord
            };

            if (null == objParam)
            {
                return user;
            }

            if (null != objParam[0])
            {
                user.Email = objParam[0].ToString();
            }

            return user;
        }
    }
}