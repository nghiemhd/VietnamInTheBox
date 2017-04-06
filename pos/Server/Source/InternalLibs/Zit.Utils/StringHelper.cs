namespace Zit.Utils
{
    public static class StringHelper
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsSameAs(this string str1, string str2)
        {
            return string.Compare(str1, str2, true) == 0;
        }

        public static byte[] ToByte(this string str)
        {
            if (str == null) return null;
            return System.Convert.FromBase64String(str);
        }
    }
}