namespace Core.Domain.UserContext
{
    using System.Text.RegularExpressions;

    internal static class StringExtension
    {
         internal static bool SingleByteLengthInRange(this string me, int minLength, int maxLength)
         {
             if (string.IsNullOrEmpty(me))
             {
                 return false;
             }

             if (me.Length > maxLength)
             {
                 return false;
             }

             int singleByteLength = Regex.Replace(me, @"[^x00-xff]", "aa").Length;
             if (singleByteLength < minLength || singleByteLength > maxLength)
             {
                 return false;
             }

             return true;
         }
    }
}