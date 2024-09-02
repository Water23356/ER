using ER.Entity2D;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER
{
    public static class Utils
    {
        /// <summary>
        /// 比较离自身更近的, 如果a比b近则返回true
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool ComparerNearest(Entity a, Entity b, Transform self)
        {
            float pa = ((Vector2)a.transform.position - (Vector2)self.position).magnitude;
            float pb = ((Vector2)b.transform.position - (Vector2)self.position).magnitude;
            return pa < pb;
        }

        public static void InvokeUnscaled(this MonoBehaviour owner, Action method, float delay)
        {
            TimerManager.UnscaledInvoke(method, delay);
        }

        public static void Invoke(this MonoBehaviour owner, Action method, float delay)
        {
            TimerManager.Invoke(method, delay);
        }

        public static void InvokeUnscaled(this GameObject owner, Action method, float delay)
        {
            TimerManager.UnscaledInvoke(method, delay);
        }

        public static void Invoke(this GameObject owner, Action method, float delay)
        {
            TimerManager.Invoke(method, delay);
        }

        /// <summary>
        /// 插值运算, 计算start->end 在t处的插值, maxDelta表示最大变化值限定
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static float Lerp(float start, float end, float t, float maxDelta)
        {
            t = Math.Clamp(t, 0f, 1f);
            float delta = (end - start) * t;
            return Math.Min(maxDelta, delta) + start;
        }

        /// <summary>
        /// 插值运算, 计算start->end 在t处的插值, maxDelta表示最大变化值限定
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 start, Vector2 end, float t, float maxDelta)
        {
            t = Math.Clamp(t, 0f, 1f);
            Vector2 delta = (end - start) * t;
            if (delta.magnitude > maxDelta)
            {
                delta = delta.normalized * maxDelta;
            }
            return delta + start;
        }

        public static Vector2 LerpForTime(Vector2 start, Vector2 end, float lerpSpeed, float maxSpeed, out bool catched, float tolerance = 0.01f)
        {
            var dir = (end - start).normalized;
            var dist = (end - start).magnitude;
            var k = Mathf.Clamp01(Time.deltaTime * lerpSpeed);
            float _dist = Mathf.Lerp(0, dist, k);
            catched = dist <= tolerance;
            return start + dir * Mathf.Min(_dist, maxSpeed * k);
        }

        /// <summary>
        /// 插值运算, 计算start->end 在t处的插值, maxDelta表示最大变化值限定
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static Vector3 Lerp(Vector3 start, Vector3 end, float t, float maxDelta)
        {
            t = Math.Clamp(t, 0f, 1f);
            Vector3 delta = (end - start) * t;
            if (delta.magnitude > maxDelta)
            {
                delta = delta.normalized * maxDelta;
            }
            return delta + start;
        }

        /// <summary>
        /// 将指定子物体设置为显示顶层(排列在最后)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void SetTop(this Transform parent, Transform child)
        {
            int offset = 0;
            for (int i = 0; i < parent.childCount; i++)
            {
                var c = parent.GetChild(i);
                if (c == child)
                {
                    offset += 1;
                    child.SetSiblingIndex(parent.childCount - offset);
                }
                else
                {
                    child.SetSiblingIndex(i - offset);
                }
            }
        }

        /// <summary>
        /// 为此对象创建一个虚拟访问锚点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="anchorName">锚点名称</param>
        public static VirtualAnchor RegisterAnchor(this object obj, string anchorName)
        {
            VirtualAnchor anchor = new VirtualAnchor(anchorName);
            anchor.Owner = obj;
            AM.AddAnchor(anchor);
            return anchor;
        }

        /// <summary>
        /// 判断指定索引是否在合法区间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        public static bool InRange<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        /// <summary>
        /// 获取字典的深拷贝
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<string, TValue> Copy<TValue>(this Dictionary<string, TValue> dic)
        {
            Dictionary<string, TValue> d = new();
            foreach (string key in dic.Keys)
            {
                d[key] = dic[key];
            }
            return d;
        }

        /// <summary>
        /// 尝试获取数组值，如果获取失败（越界），则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">索引</param>
        /// <param name="array"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryValue<T>(this T[] array, int index, T defaultValue)
        {
            if (index < 0 || index >= array.Length)
            {
                return defaultValue;
            }
            return array[index];
        }

        /// <summary>
        /// 尝试以字符串的形式将键值对打印出来
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="pair"></param>
        /// <param name="printDelegate"></param>
        public static void Print<TKey, TValue>(this KeyValuePair<TKey, TValue> pair,
            Action<string> printDelegate = null)
        {
            string txt = $"<{pair.Key?.ToString()}>:{pair.Value?.ToString()}";
            printDelegate?.Invoke(txt);
            Console.WriteLine(txt);
        }

        /// <summary>
        /// 获取此字符串的解析数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Data Parse(this string text)
        {
            return Data.ParseTo(text);
        }

        /// <summary>
        /// 尝试将此字符串解析为整型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParseInt(this string text, out int Value)
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(text);
            }
            catch (FormatException)
            {
                Value = 0;
                return false;
            }
            catch (OverflowException)
            {
                Value = 0;
                return false;
            }
            Value = num;
            return true;
        }

        /// <summary>
        /// 尝试将此字符串解析为整型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParseDouble(this string text, out double Value)
        {
            double num = 0;
            try
            {
                num = Convert.ToDouble(text);
            }
            catch (FormatException)
            {
                Value = 0;
                return false;
            }
            catch (OverflowException)
            {
                Value = 0;
                return false;
            }
            Value = num;
            return true;
        }

        /// <summary>
        /// 尝试将此字符串解析为浮点
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>

        public static bool TryParseFloat(this string text, out float Value)
        {
            if (float.TryParse(text, out Value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试将此字符串解析为布尔值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool TryParseBoolean(this string text, out bool Value)
        {
            if (text.ToUpper() == "TRUE")
            {
                Value = true;
                return true;
            }
            else if (text.ToUpper() == "FALSE")
            {
                Value = false;
                return true;
            }
            Value = false;
            return false;
        }

        public static void Add<T>(this List<T> list, T[] array)
        {
            foreach (T item in array)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// 从外部指定文件中加载图片
        /// </summary>
        /// <param name="height">图片高度</param>
        /// <param name="path">图片的文件路径</param>
        /// <param name="width">图片宽度</param>
        /// <returns></returns>
        public static Texture2D LoadTextureByIO(string path, int width = 2, int height = 2)
        {
            // 从本地文件系统读取图片数据
            byte[] imageData = System.IO.File.ReadAllBytes(path);

            // 创建纹理对象
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(imageData);

            return texture;
        }

        /// <summary>
        /// 浏览文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void ExplorePath(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path.Replace('/', '\\'));
        }

        /// <summary>
        /// 将Texture2d转换为Sprite
        /// </summary>
        /// <param name="tex">参数是texture2d纹理</param>
        /// <returns></returns>
        public static Sprite TextureToSprite(this Texture2D texture)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            return sprite;
        }

        /// <summary>
        /// 将制定事件响应委托封装成可跳过的时间委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="function"></param>
        public static void PassEvent<T>(this PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    break;
                    //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                }
            }
        }

        /// <summary>
        /// 将数组中全部初始化为一个默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="defaultValue"></param>
        public static T[] InitDefault<T>(this T[] array, T defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }
            return array;
        }

        /// <summary>
        /// 将该uid对象注册进管理器
        /// </summary>
        /// <param name="obj"></param>
        public static void Registry(this IUID obj)
        {
            UIDManager.Instance.Registry(obj);
        }

        /// <summary>
        /// 将该uid对象从管理器中注销
        /// </summary>
        /// <param name="obj"></param>
        public static void Unregistry(this IUID obj)
        {
            UIDManager.Instance.Unregistry(obj);
        }

        /// <summary>
        /// 获取组件, 如果组件不存在则添加它
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <param name="set_enable">是否给新添加的组件默认设置为enable</param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component child, bool set_enable = true) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }
            var bcomp = result as Behaviour;
            if (bcomp != null) bcomp.enabled = set_enable;
            return result;
        }

        /// <summary>
        /// 判断值是否在指定范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 判断值是否在指定范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 判断值是否在指定范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 求数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int Sum(this int[] array)
        {
            int sum = 0;
            foreach (int item in array)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>
        /// 求数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float Sum(this float[] array)
        {
            float sum = 0;
            foreach (float item in array)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>
        /// 求数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double Sum(this double[] array)
        {
            double sum = 0;
            foreach (double item in array)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>
        /// 对数组与运算
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool AndAll(this bool[] array)
        {
            foreach (bool item in array)
            {
                if (!item) return false;
            }
            return true;
        }

        /// <summary>
        /// 对数组或运算
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool OrAll(this bool[] array)
        {
            foreach (bool item in array)
            {
                if (item) return true;
            }
            return false;
        }

        /// <summary>
        /// 对数组与运算
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool AndAll(this List<bool> array)
        {
            foreach (bool item in array)
            {
                if (!item) return false;
            }
            return true;
        }

        /// <summary>
        /// 对数组或运算
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool OrAll(this List<bool> array)
        {
            foreach (bool item in array)
            {
                if (item) return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定向量到目标向量所经过的夹角(逆时针为正方向)
        /// </summary>
        public static float ClockAngle(this Vector2 defaultVector, Vector2 aimVector, float offset = 0)
        {
            float angle = Vector2.Angle(defaultVector, aimVector) + offset;
            if (GetRotateDir(defaultVector, aimVector) == RotateDir.Anticlockwise)
                return angle;
            else
                return -angle;
        }

        /// <summary>
        /// 获取旋转方向, 返回逆时针/顺时针/平行
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static RotateDir GetRotateDir(Vector2 origin, Vector2 aim)
        {
            float f = origin.x * aim.y - aim.x * origin.y;
            if (f > 0)
                return RotateDir.Anticlockwise;
            else if (f < 0)
                return RotateDir.Clockwise;
            else
                return RotateDir.Parallel;
        }

        /// <summary>
        /// 获取该向量逆时针旋转angle度
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 dir, float angle)
        {
            return new Vector2(dir.x * MathF.Cos(angle) - dir.y * MathF.Sin(angle), dir.x * MathF.Sin(angle) + dir.y * MathF.Cos(angle));
        }

        /// <summary>
        /// 获取指定向量的垂直向量(逆时针方向)
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Vector2 Vertical(this Vector2 dir)
        {
            Vector2 v = new Vector2(1, -dir.x / dir.y).normalized;
            if (GetRotateDir(dir, v) == RotateDir.Anticlockwise)
                return v;
            else
                return -v;
        }

        /// <summary>
        /// 修改指定向量的值
        /// </summary>
        /// <param name="vector">源向量</param>
        /// <param name="mx">是否修改x</param>
        /// <param name="my">是否修改y</param>
        /// <param name="mz">是否修改z</param>
        /// <param name="modVector">目标向量</param>
        /// <returns></returns>
        public static Vector3 Modify(this Vector3 vector, bool mx, bool my, bool mz, Vector3 modVector)
        {
            Vector3 v = vector;
            if (mx)
                v.x = modVector.x;
            if (my)
                v.y = modVector.y;
            if (mz)
                v.z = modVector.z;
            return v;
        }

        /// <summary>
        /// 判断指定二维向量是否在指定 矩形范围内
        /// </summary>
        /// <returns></returns>
        public static bool InRange(this Vector2 dir, Vector2 min, Vector2 max)
        {
            return dir.x >= min.x && dir.x <= max.x && dir.y >= min.y && dir.y <= max.y;
        }

        /// <summary>
        /// 获取一个颜色仅改变透明度的颜色
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color ModifyAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// 返回贝赛尔曲线插值
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="k">控制点: 控制变化位置, 最后曲线将经过 起点终点和k点</param>
        /// <param name="t">最后返回x=t对应的y值, t 必须在0~1之间</param>
        /// <returns></returns>
        public static float QuadraticBezierInterpolate(Vector2 start, Vector2 end, Vector2 k, float t)
        {
            t = Mathf.Max(t, 0);
            t = Mathf.Min(t, 1);
            return Mathf.Pow(1 - t, 2) * start.y + 2 * (1 - t) * t * k.y + Mathf.Pow(t, 2) * end.y;
        }

        public static TResult Execute<TResult>(Func<TResult> func)
        {
            return func.Invoke();
        }

        public static TResult Execute<TResult, T1>(Func<T1, TResult> func, T1 arg1)
        {
            return func.Invoke(arg1);
        }

        public static string ReplaceWrappedSubstrings(string input, Func<string, string> replaceFunction)
        {
            // 正则表达式匹配 {} 包裹的部分
            return Regex.Replace(input, @"\{(.*?)\}", match =>
            {
                // 获取 {} 中的内容
                string word = match.Groups[1].Value;
                // 使用自定义的替换规则进行替换
                return replaceFunction(word);
            });
        }
    }
}