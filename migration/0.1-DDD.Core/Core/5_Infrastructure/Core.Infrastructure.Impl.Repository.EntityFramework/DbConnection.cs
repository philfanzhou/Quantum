namespace Core.Infrastructure.Impl.Repository.EntityFramework
{
    using System.Text;

    public class DbConnection
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public bool TrustedConnection { get; set; }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append(string.Format("server={0};", this.Server));
            
            if (!string.IsNullOrWhiteSpace(this.Port))
            {
                strBuilder.Append(string.Format("port={0};", this.Port));
            }

            strBuilder.Append(string.Format("database={0};", this.Database));
            
            if (TrustedConnection)
            {
                strBuilder.Append("Trusted_Connection=true");
            }
            else
            {
                strBuilder.Append(string.Format("uid={0};password={1}", this.Uid, this.Password));
            }

            return strBuilder.ToString();
        }
    }
}
