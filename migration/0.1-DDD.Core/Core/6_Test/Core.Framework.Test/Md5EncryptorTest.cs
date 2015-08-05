using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Core.Infrastructure.Crosscutting.Test
{
    [TestClass]
    public class Md5EncryptorTest
    {
        private IMd5Encryptor _encryptor;

        [TestInitialize]
        public void Init()
        {
            _encryptor = ContainerHelper.Resolve<IMd5Encryptor>();
        }

        [TestMethod]
        public void TestMd5Encrypt()
        {
            string source = "1";
            string hash = _encryptor.Encrypt(source);
            Assert.AreEqual("C4CA4238A0B923820DCC509A6F75849B", hash);

            source = "helloworld";
            hash = _encryptor.Encrypt(source);
            Assert.AreEqual("FC5E038D38A57032085441E7FE7010B0", hash);

            source = "你好";
            hash = _encryptor.Encrypt(source);
            Assert.AreEqual("7ECA689F0D3389D9DEA66AE112E5CFD7", hash);

            source = "33487343";
            hash = _encryptor.Encrypt(source);
            Assert.AreEqual("6FCBCD2575DA2F0A54DB4110D118E523", hash);

            source = "！＠＃！＃！＠";
            hash = _encryptor.Encrypt(source);
            Assert.AreEqual("F8B19B7FBF3F8A1432D8B0435B58DD76", hash);

            source = "    ";
            hash = _encryptor.Encrypt(source);
            Assert.AreEqual("0CF31B2C283CE3431794586DF7B0996D", hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMd5EncryptWithNull()
        {
            _encryptor.Encrypt(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMd5EncryptWithEmptyString()
        {
            _encryptor.Encrypt(string.Empty);
        }
    }
}
