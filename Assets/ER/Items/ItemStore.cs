﻿// Ignore Spelling: Json
using ER.Save;
using System.Collections.Generic;

namespace ER.Items
{
    /// <summary>
    /// 物品系统（动态）(背包)（可读写）
    /// </summary>
    public class ItemStore : ISavable
    {
        #region 属性

        private int size;

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string storeName;

        /// <summary>
        /// 存档标识符
        /// </summary>
        public string Identifier { get => "ItemStore:" + storeName; }

        /// <summary>
        /// 仓库大小
        /// </summary>
        public int Size
        {
            get => size;
            set
            {
                size = value;
                if (size < 0) size = 0;
                while (items.Count > size)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }
        }

        /// <summary>
        /// 仓库中物品的数量
        /// </summary>
        public int Count { get => items.Count; }

        /// <summary>
        /// 仓库物品
        /// </summary>
        private List<ItemVariable> items = new();

        #endregion 属性

        #region 构造

        public ItemStore()
        { }

        /// <summary>
        /// 创建一个动态仓库
        /// </summary>
        /// <param name="name">仓库名称</param>
        /// <param name="size">仓库大小</param>
        public ItemStore(string name, int size = 64)
        { storeName = name; this.size = size; }

        #endregion 构造

        #region 方法

        /// <summary>
        /// 通过ID查找物品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemVariable[] Find(int id)
        {
            List<ItemVariable> items = new List<ItemVariable>();
            foreach (ItemVariable item in items)
            {
                if (item.ID == id)
                {
                    items.Add(item);
                }
            }
            return items.ToArray();
        }

        public void Print()
        {
            ConsolePanel.Print($"[{storeName}]");
            foreach (var item in items)
            {
                item.Print();
            }
            ConsolePanel.Print($"[{storeName}]");
        }

        /// <summary>
        /// 向仓库添加物品，如果仓库已满，则返回false
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(ItemVariable item)
        {
            if (items.Count < size)
            {
                items.Add(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据索引从此仓库移除物品，如果物品不存在则返回false
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveItem(int index)
        {
            if (index >= 0 || index < items.Count)
            {
                items.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空仓库
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// 根据物品索引获取指定物品
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ItemVariable this[int index]
        {
            get
            {
                return items[index];
            }
        }

        #endregion 方法

        #region 存档接口

        /// <summary>
        /// 存档接口
        /// </summary>
        /// <returns></returns>
        public SaveEntry GetSaveEntry()
        {
            SaveEntry entry = new SaveEntry(Identifier);
            entry.data["storeName"] = storeName;
            entry.data["size"] = size;
            entry.data["items"] = items;
            return entry;
        }

        /// <summary>
        /// 读档接口
        /// </summary>
        /// <param name="saveEntry"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Restore(SaveEntry saveEntry)
        {
            storeName = saveEntry.data["storeName"] as string;
            size = (int)saveEntry.data["size"];
            items = saveEntry.data["items"] as List<ItemVariable>;
        }

        #endregion 存档接口
    }
}