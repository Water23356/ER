using System.Collections.Generic;

namespace ER.Entity2D
{
    /// <summary>
    /// 击打记录
    /// <code>
    /// 该类用于记录击打信息(不包含伤害信息及其他携带信息)
    /// 主要用途就是制造 击打无敌帧, 每种击打标签都会有一个固定的 无敌时间
    ///
    /// 建议修改:
    /// HitRecorder.CD: 仅读字典, 用来保存项目中各种击打标签对应的无敌时间
    /// </code>
    /// </summary>
    public struct HitRecorder
    {
        /// <summary>
        /// 伤害标签 及 对应攻击cd
        /// </summary>
        public static readonly Dictionary<string, float> CD = new Dictionary<string, float>()
        {
            {"normal" ,0.5f},
        };

        public static readonly string defaultTag = "normal";

        public static float GetHitCD(string tag)
        {
            if (CD.TryGetValue(tag, out float value))
                return value;
            return CD["normal"];
        }

        /****************************************************************************************/

        /// <summary>
        /// 击打标签
        /// </summary>
        public string tag;

        /// <summary>
        /// 击打时刻
        /// </summary>
        public float time;
    }
}