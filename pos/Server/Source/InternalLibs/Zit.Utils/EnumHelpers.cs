using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Utils
{
    public static class EnumHelpers
    {
        public static Dictionary<T, string> ToDictionary<T>()
        {
            Dictionary<T, string> dic = new Dictionary<T, string>();
            Type type = typeof(T);
            try
            {
                foreach (var val in Enum.GetValues(type))
                {
                    dic.Add((T)val, Enum.GetName(type, val));
                }
                return dic;
            }
            catch
            {
                return dic;
            }
        }
    }
}
