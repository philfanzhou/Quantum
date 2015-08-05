namespace Core.Application.UserContext
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class AuthenticationService
    {
        public bool validate(string identity, string encryptedPassword)
        {
            // get salt
            string salt = identity;

            // password with salt
            string pwdWithSalt = Encrypt(identity + encryptedPassword.ToUpper());

            // get user info and validate
            if (identity == "guest" &&
                pwdWithSalt == "0023921576DA32C18EAFF5D21EC02E0E"/*123456*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string Encrypt(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString().ToUpper();
            }
        }
    }
}
