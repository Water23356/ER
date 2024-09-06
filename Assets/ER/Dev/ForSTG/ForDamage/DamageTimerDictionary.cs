using System.Collections.Generic;

namespace ER.STG
{
    /// <summary>
    /// 伤害计时器字典(根据项目需要修改)
    ///
    /// </summary>
    public class DamageTimerDictionary
    {
        /*
         * 1 tick = 0.02s (fixedDeltaTime)
         * 10 tick = 0.2s
         * 25 tick = 0.5s
         * 50 tick = 1s
         * */

        private static Dictionary<string, int> tickDic = new Dictionary<string, int>()
        {
            {"normal",20}
        };

        private static int defaultTick = 20;

        public static int TickNeed(string key)
        {
            if (tickDic.TryGetValue(key, out int value)) return value;
            return defaultTick;
        }
    }
}