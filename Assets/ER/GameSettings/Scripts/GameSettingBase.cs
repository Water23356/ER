using ER;
using System;
using System.Collections.Generic;

namespace ER.GameSetting
{
    public abstract class GameSettingBase<T>:Singleton<T> where T:class,new()
    {
        public abstract Dictionary<string, string> GetSettingInfo();

        public abstract void UpdateSettings(Dictionary<string, string> dic);

        public static bool ToBool(string origin,bool defaultValue = false)
        {
            if(origin.ToLower()=="true")return true;
            return defaultValue;
        }
        public static int ToInt(string origin,int defaultValue = 0)
        {
            if (int.TryParse(origin, out int value)) return value;
            return defaultValue;
        }
        public static float ToFloat(string origin, float defaultValue = 0)
        {
            if (float.TryParse(origin, out float value)) return value;
            return defaultValue;
        }
        /// <summary>
        /// 如果键存在则, 则执行处理委托 handle (并将键对应的值传入委托参数)
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="handle"></param>
        public static void HandleDic(Dictionary<string, string> dic, string key,Action<string> handle)
        {
            if(dic.TryGetValue(key,out string value))
            {
                handle(value);
            }
        }
    }
}