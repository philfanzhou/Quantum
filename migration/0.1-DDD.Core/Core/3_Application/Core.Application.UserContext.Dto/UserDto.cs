namespace Core.Application.UserContext.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserMail { get; set; }

        public string EncryptedPassword { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.UserMail))
            {
                return base.ToString();
            }
            else
            {
                return string.Format("{0} {1}", this.Id, this.UserMail);
            }
        }
    }
}