
using  ER.Entity2D.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace  ER.Entity2D
{
    [CreateAssetMenu(fileName = "ActionParams", menuName = "AnimationEvents/ActionParams", order = 1)]
    public sealed class ActionParams : ScriptableObject
    {
        [Tooltip("目标动作名称")]
        public ActionName actionName;

        [Header("参数组")]
        public TextParam[] textParams;

        public NumberParam[] numberParams;
        public BooleanParam[] boolParams;

        public BaseActionParams BaseParams { get;private set; }


        /// <summary>
        /// 获取对应属性值, 仅限于 float, string, bool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetParam<T>(string key) where T : struct
        {
            System.Type type = typeof(T);
            if (type == typeof(float))
            {
                for (int i = 0; i < numberParams.Length; i++)
                {
                    if (numberParams[i].key == key) return (T)(object)numberParams[i].value;
                }
            }
            else if (type == typeof(string))
            {
                for (int i = 0; i < numberParams.Length; i++)
                {
                    if (textParams[i].key == key) return (T)(object)numberParams[i].value;
                }
            }
            else if (type == typeof(bool))
            {
                for (int i = 0; i < numberParams.Length; i++)
                {
                    if (boolParams[i].key == key) return (T)(object)numberParams[i].value;
                }
            }
            else
            {
                throw new ArgumentException($"传入错误参数类型请求: {type.Name}");
            }
            throw new NotFondParamException($"未找到指定参数: {key}, 类型: {type.Name}");
        }

        /// <summary>
        /// 更新 BaseParams
        /// </summary>
        /// <returns></returns>
        public BaseActionParams ToBaseParams()
        {
            var tp = new Dictionary<string, string>();
            var fp = new Dictionary<string, float>();
            var bp = new Dictionary<string, bool>();
            if (textParams != null && textParams.Length > 0)
            {
                foreach (var v in textParams)
                {
                    tp[v.key] = v.value;
                }
            }
            if (numberParams != null && numberParams.Length > 0)
            {
                foreach (var v in numberParams)
                {
                    fp[v.key] = v.value;
                }
            }
            if (boolParams != null && boolParams.Length > 0)
            {
                foreach (var v in boolParams)
                {
                    bp[v.key] = v.value;
                }
            }
            BaseParams = new BaseActionParams()
            {
                actionName = actionName,
                textParams = tp,
                boolParams = bp,
                floatParams = fp,
            };

            return BaseParams;
        }

        private void OnEnable()
        {
            ToBaseParams();
        }
    }

    public struct BaseActionParams
    {
        public ActionName actionName;
        public Dictionary<string,string> textParams;
        public Dictionary<string,float> floatParams;
        public Dictionary<string, bool> boolParams;
        /// <summary>
        /// 获取值, 如果获取成功, 附带返回 true, 否则返回 (0,false)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (float,bool) GetFloat(string key)
        {
            if(floatParams.TryGetValue(key,out float v))
            {
                return (v, true);
            }
            return (0f, false);
        }
        /// <summary>
        /// 获取值, 如果获取成功, 附带返回 true, 否则返回 (0,false)
        /// 由float四舍五入得来
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (int,bool) GetInt(string key)
        {
            if (floatParams.TryGetValue(key, out float v))
            {
                return (Convert.ToInt32(v), true);
            }
            return (0, false);
        }

        /// <summary>
        /// 获取值, 如果获取成功, 附带返回 true, 否则返回 (string.Empty,false)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (string, bool) GetString(string key)
        {
            if (textParams.TryGetValue(key, out string v))
            {
                return (v, true);
            }
            return (string.Empty, false);
        }

        /// <summary>
        /// 获取值, 如果获取成功, 附带返回 true, 否则返回 (false,false)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (bool, bool) GetBool(string key)
        {
            if (boolParams.TryGetValue(key, out bool v))
            {
                return (v, true);
            }
            return (false, false);
        }
    }

    public class NotFondParamException : Exception
    {
        public NotFondParamException(string log) : base(log)
        {
        }
    }

    [Serializable]
    public struct TextParam
    {
        public string key;
        public string value;
    }

    [Serializable]
    public struct NumberParam
    {
        public string key;
        public float value;
    }

    [Serializable]
    public struct BooleanParam
    {
        public string key;
        public bool value;
    }
}