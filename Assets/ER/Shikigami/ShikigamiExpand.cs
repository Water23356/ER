using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Shikigami
{
    public static class ShikigamiExpand
    {
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
        public static bool ToBool(this string str)
        {
            if(str.ToLower() == "true")
            {
                return true;
            }
            return false;
        }
        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }
    }
}
