namespace Core.Application.UserContext.Dto
{
    public class RoleDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return base.ToString();
            }

            return this.Name;
        }
    }
}