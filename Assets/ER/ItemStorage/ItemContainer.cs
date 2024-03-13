﻿using System.Collections.Generic;
using UnityEngine;
using ER;

namespace ER.ItemStorage
{
    /// <summary>
    /// 物品容器
    /// </summary>
    public class ItemContainer:IUID
    {
        private IItemStack[] stacks;//存储物品堆
        private int stackCount;//存入物品堆的数量
        private UID uuid;
        /// <summary>
        /// 类名
        /// </summary>
        public static string ClassName => nameof(ItemContainer);
        /// <summary>
        /// 容器中物品堆的数量
        /// </summary>
        public int StackCount => stackCount;

        /// <summary>
        /// 容器大小:
        /// 重设大小将会导致内容清空!
        /// </summary>
        public int Size
        {
            get
            {
                return stacks.Length;
            }
            set
            {
                if (value <= 0)
                {
                    stacks = new IItemStack[0];
                }
                else
                {
                    stacks = new IItemStack[value];
                    stacks.InitDefault(null);
                    stackCount = 0;
                }
            }
        }

        public UID UUID => uuid;

        /// <summary>
        /// 获取指定索引位置上的物品堆, 如果不存在或者没有物品则返回null
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dispel">是否移除该物品堆</param>
        /// <returns></returns>
        public IItemStack GetStack(int index,bool dispel = false)
        {
            IItemStack stack = stacks[index];
            if(dispel)
            {
                stacks[index] = null;
            }
            return stack;
        }
        /// <summary>
        /// 移除指定位置上的物品堆
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            stacks[index] = null;
        }

        /// <summary>
        /// 判断指定物品堆是否存在
        /// </summary>
        /// <returns></returns>
        public bool Contains(IItemStack stack)
        {
            for (int i = 0; i < Size; i++)
            {
                if (stacks[i] == stack)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定格子上物品堆是否存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Contains(int index)
        {
            return stacks[index] != null;
        }

        /// <summary>
        /// 获取第一个空格子索引,
        /// 返回-1, 表示没有空格
        /// </summary>
        /// <returns></returns>
        public int GetFirstEmpty()
        {
            for (int i = 0; i < Size; i++)
            {
                if (Contains(i)) continue;
                return i;
            }
            return -1;
        }

        /// <summary>
        /// 判断该容器是否已经装满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return StackCount >= Size;
        }

        /// <summary>
        /// 向容器第一个空栏添加新的物品堆
        /// </summary>
        /// <returns>如果添加失败则返回false</returns>
        public bool Add(IItemStack stack)
        {
            if (stackCount >= Size) return false;
            int index = GetFirstEmpty();
            stacks[index] = stack;
            stackCount++;
            return true;
        }

        /// <summary>
        /// 向容器指定位置添加物新的物品堆
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="index"></param>
        /// <returns>如果添加失败则返回false</returns>
        public bool Add(IItemStack stack, int index)
        {
            if (stackCount >= Size) return false;
            if (Contains(index)) return false;
            stacks[index] = stack;
            stackCount++;
            return true;
        }
        /// <summary>
        /// 替换指定格子上的物品
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="index"></param>
        public void Set(IItemStack stack, int index)
        {
            stacks[index] = stack;
        }

        public ObjectUIDInfo Serialize()
        {
            Dictionary<string,object> dt = new  Dictionary<string,object>();
            dt["stackCount"] = stackCount;
            dt["stacks"] = stacks;
            ObjectUIDInfo data = new ObjectUIDInfo()
            {
                uuid = uuid.ToString(),
                data = dt
            };
            return data;
        }

        public void Deserialize(ObjectUIDInfo data)
        {
            UID uid = new UID(data.uuid);
            if(uid.ClassName!=ClassName)
            {
                Debug.Log("错误类型匹配:\n\t" +
                    $"禁止将 {data} 数据反序列化为 {ClassName}");
                return;
            }
            uuid = uid;
            stacks = (IItemStack[])data.data["stacks"];
            stackCount = (int)data.data["stackCount"];
        }

        public ItemContainer()
        {
            uuid = new UID(ClassName, GetHashCode());
            this.Registry();
        }
    }
}