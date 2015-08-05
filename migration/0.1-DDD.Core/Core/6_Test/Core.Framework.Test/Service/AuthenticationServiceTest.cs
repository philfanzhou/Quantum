using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Application.UserContext.Test.Service
{
    [TestClass]
    public class AuthenticationServiceTest
    {
        [TestMethod]
        public void TestAuthentication()
        {
            AuthenticationService service = new AuthenticationService();
            bool result = service.validate("guest", Encrypt("123456"));
            Assert.IsTrue(result);

            result = service.validate("guest", Encrypt("34545"));
            Assert.IsFalse(result);
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
