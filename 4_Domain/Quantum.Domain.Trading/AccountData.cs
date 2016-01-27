using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Quantum.Domain.Trading
{
    public static partial class Broker
    {
        private static string _accountDataFolder = Path.Combine(Environment.CurrentDirectory, "TradingData");

        /// <summary>
        /// 存储账户信息
        /// </summary>
        /// <param name="account"></param>
        public static void SaveAccountData(IAccount account)
        {
            if(!Directory.Exists(_accountDataFolder))
            {
                Directory.CreateDirectory(_accountDataFolder);
            }

            Account accountData = new Account(account);

            string filePath = Path.Combine(_accountDataFolder, string.Format("{0}.bin", accountData.Id));
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, accountData);
            }
        }

        /// <summary>
        /// 读取账户信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static IAccount LoadAccountData(string accountId)
        {
            string filePath = Path.Combine(_accountDataFolder, string.Format("{0}.bin", accountId));

            if(!File.Exists(filePath))
            {
                return null;
            }

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Account obj = (Account)formatter.Deserialize(stream);
                return obj;
            }
        }
    }
}
