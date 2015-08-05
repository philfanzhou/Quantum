namespace Core.Domain.UserContext
{
    using Core.Domain;
    using System.Collections.Generic;

    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<UserRole> GetUserRoleByRoleId(string roleId)
        {
            var spec = Specification<UserRole>.Eval(r => r.RoleId.Equals(roleId));
            return GetAll(spec);
        }

        public IEnumerable<UserRole> GetUserRoleByUserId(string userId)
        {
            var spec = Specification<UserRole>.Eval(t => t.UserId.Equals(userId));
            return GetAll(spec);
        }


        public UserRole GetUserRole(string userId, string roleId)
        {
            var spec = Specification<UserRole>.Eval(t =>
                    t.UserId.Equals(userId) &&
                    t.RoleId.Equals(roleId));
            return Single(spec);
        }


        public bool Exists(string userId, string roleId)
        {
            var spec = Specification<UserRole>.Eval(t =>
                    t.UserId.Equals(userId) &&
                    t.RoleId.Equals(roleId));
            return Exists(spec);
        }
    }
}