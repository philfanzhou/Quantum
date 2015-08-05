using System.Text;

namespace Core.Infrastructure.Crosscutting.Util.String
{
    public class StringUtil
    {
        public static string ReadString(byte[] data)
        {
            try
            {
                int index = 0;
                while (index < data.Length)
                {
                    if (data[index] == 0)
                    {
                        break;
                    }

                    index++;
                }

                return Encoding.GetEncoding("GB2312").GetString(data, 0, index);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
