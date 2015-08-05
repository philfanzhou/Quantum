namespace Core.Domain.UserContext
{
    using Core.Domain;

    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public Role GetByName(string roleName)
        {
            var spec = Specification<Role>.Eval(role =>
                role.Name.Equals(roleName));
            return this.Single(spec);
        }

        public bool Exists(string roleName)
        {
            var spec = Specification<Role>.Eval(role =>
                role.Name.Equals(roleName));
            return Exists(spec);
        }
    }
}