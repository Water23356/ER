
using System.Collections.Generic;

namespace ER.InputSolution
{
    /// <summary>
    /// 输入匹配串
    /// </summary>
    public struct InputMatchingString
    {
        public InputMatchingFrame[] matchings;

        /// <summary>
        /// 判断 origin 是否
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public bool IsMatched(List<InputFrame> origin)
        {
            if (origin.Count < matchings.Length) return false;

            int m = 0;
            int i = origin.Count - 1;
            InputMatchingFrame nowM;
            InputMatchingFrame nextM;
            InputFrame nowIp;

            while (m < matchings.Length)
            {
                if (i < 0) return false;
                nowM = matchings[m];
                nowIp = origin[i];
                if (m == matchings.Length - 1)
                {
                    nextM = nowM;
                }
                else
                {
                    nextM = matchings[m + 1];
                }

                if (nowM.IsMatched(nowIp))//初次匹配, 成功的话继续匹配, 直到无法匹配
                {
                    float offsetTime = nowIp.keepTime;//记录累计时间
                    while (true)
                    {
                        //移动origin至下一个状态
                        i--;
                        if (i < 0) break;
                        nowIp = origin[i];
                        //如果下一个状态仍然匹配当前值
                        if (nowM.IsMatched(nowIp, offsetTime))
                        {
                            offsetTime += nowIp.keepTime;//累计时间

                            //需要处理一种特殊情况, 如果当前状态既满足当前 匹配, 又满足下一个 匹配,应当优先匹配下一个
                            if (nextM.IsMatched(nowIp))
                            {
                                m++;
                                i--;
                                break;
                            }

                            continue;
                        }
                        else//如果不是, 则移至下一位匹配状态(重置该循环)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    return false;
                }
                m++;
            }
            return true;
        }
    }

    /// <summary>
    /// 输入帧状态匹配
    /// </summary>
    public struct InputMatchingFrame
    {
        public HashSet<InputButtons> key;//有效输入
        public float keepTimeMin;//最小持续时间
        public float keepTimeMax;//最大持续时间

        public bool IsMatched(InputFrame input, float offset = 0f)
        {
            if (key.IsSubsetOf(input.keys))
            {
                if (keepTimeMin <= (input.keepTime + offset) && (input.keepTime + offset) <= keepTimeMax)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 输入帧状态: 持续时间为负数则为错误对象
    /// </summary>
    public struct InputFrame
    {
        public HashSet<InputButtons> keys;//有效输入
        public float keepTime;//持续时间

        public static InputFrame Error
        {
            get => new InputFrame() { keepTime = -1 };
        }

        public bool IsError()
        {
            if (keepTime < 0) return true;
            return false;
        }

    }
}