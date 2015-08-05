namespace Core.Domain.UserContext
{
    using Core.Domain;

    public class UserRepository<TUserEntity>
        : Repository<TUserEntity>
        where TUserEntity : User
    {
        public UserRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public bool UserMailExists(string userMail)
        {
            var spec = Specification<TUserEntity>.Eval(user => 
                user.UserMail.Equals(userMail));
            return this.Exists(spec);
        }


        public bool CheckPassword(string userMail, string password)
        {
            var spec = Specification<TUserEntity>.Eval(user => user.UserMail.Equals(userMail));
            var target = this.Single(spec);
            if (target != null)
            {
                return target.CheckPassword(password);
            }
            return false;
        }


        public TUserEntity GetByMail(string userMail)
        {
            var spec = Specification<TUserEntity>.Eval(user => 
                user.UserMail.Equals(userMail));
            return this.Single(spec);
        }
    }
}