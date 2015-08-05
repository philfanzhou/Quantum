using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.DistributedService;

namespace PF.TestWithoutDB
{
    [TestClass]
    public class AuthServiceTest
    {
        [TestMethod]
        public void RegUserTest()
        {
            AuthenticationService service = new AuthenticationService();
            Assert.IsTrue(service.register("guest@bala.com", Encrypt("passw0rd")));
        }


        private static string Encrypt(string source)
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
