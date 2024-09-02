using ER.Resource;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ER.ItemStorage
{
    public class ItemStack : IItemStack
    {
        private UID uuid;
        private IItemResource resource;
        private string displayName;//物品堆的显示名称
        private DescriptionInfo[] descriptions;//物品堆的描述信息
        private int amount;//当前堆叠数量
        private bool stackable;//物品堆是否可堆叠
        private int amountMax;//物品堆的堆叠上限
        private Dictionary<string, object> infos;


        public IItemResource Resource
        {
            get => resource;
            set
            {
                resource = value;
                displayName = resource.DisplayName;
                stackable = resource.Stackable;
                amountMax = resource.AmountMax;
                descriptions = resource.Descriptions;
            }
        }
        public int Amount { get => amount; set => amount = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public DescriptionInfo[] Descriptions { get => descriptions; set => descriptions = value; }

        public bool Stackable { get => stackable; set => stackable = value; }

        public int AmountMax { get => amountMax; set => amountMax = value; }

        public UID UUID => uuid;

        public string ClassName => nameof(ItemStack);

        public Dictionary<string, object> Infos { get => infos; set => infos = value; }

        public void Deserialize(ObjectUIDInfo data)
        {
            this.Unregistry();
            uuid = new UID(data.uuid);
            if(data.data.TryGetValue("resource",out object registryName))
            {
                resource = GR.Get<IItemResource>((string)registryName);
            }
            if (data.data.TryGetValue("displayName", out object _displayName))
            {
                displayName = (string)_displayName;
            }
            if (data.data.TryGetValue("descriptions", out object _descriptions))
            {
                descriptions = _descriptions as DescriptionInfo[] ;
            }
            if (data.data.TryGetValue("amount", out object _amount))
            {
                amount = (int)_amount;
            }
            if (data.data.TryGetValue("stackable", out object _stackable))
            {
                stackable = (bool)_stackable;
            }
            if (data.data.TryGetValue("amountMax", out object _amountMax))
            {
                amountMax = (int)_amountMax;
            }
            this.Registry();
        }

        public ObjectUIDInfo Serialize()
        {
            ObjectUIDInfo data = new ObjectUIDInfo(uuid);
            data.data["resource"] = resource.RegistryName;//源物品资源的注册名
            data.data["displayName"] = displayName;
            data.data["descriptions"] = descriptions;
            data.data["amount"] = amount;
            data.data["stackable"] = stackable;
            data.data["amountMax"] = amountMax;
            data.data["infos"] = infos;
            return data;
        }

        public IItemStack Copy()
        {
            DescriptionInfo[] des = new DescriptionInfo[descriptions.Length];
            for(int i=0;i<des.Length;i++)
            {
                des[i] = descriptions[i];
            }
            Dictionary<string, object> _infos = new Dictionary<string, object>();
            if(infos!=null)
            {
                foreach (var info in infos)
                {
                    _infos.Add(info.Key, info.Value);
                }
            }
            return new ItemStack()
            {
                resource = resource,
                displayName = displayName,
                descriptions = des,
                amount = amount,
                stackable = stackable,
                amountMax = amountMax,
                infos = _infos
            };
        }

        public virtual bool IsSameStack(IItemStack stack)
        {
            Debug.Log($"isNull:  this:{infos == null}  aim:{stack.Infos == null}");
            if(stackable==stack.Stackable  
                && resource.RegistryName == stack.Resource.RegistryName 
                && infos.Count == stack.Infos.Count)
            {
                Debug.Log("基础匹配[0]");
                if (descriptions.Length != stack.Descriptions.Length) return false;
                Debug.Log("基础匹配[2]");
                for(int i=0;i<descriptions.Length;i++)
                {
                    if (!stack.Descriptions.Contains(descriptions[i])) return true;
                }
                foreach(var pair in infos)
                {
                    if(stack.Infos.TryGetValue(pair.Key,out object value))
                    {
                        if (pair.Value != value) return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public ItemStack()
        {
            uuid = new UID(ClassName,GetHashCode());
            infos = new Dictionary<string, object>();
            this.Registry();
        }
        public ItemStack(ObjectUIDInfo data)
        {
            Debug.Log($"uuid:{data.uuid}");
            uuid = new UID(data.uuid);
            if (data.data.TryGetValue("resource", out object registryName))
            {
                resource = GR.Get<IItemResource>((string)registryName);
                Debug.Log($"resource origin:{resource != null}");
            }
            if (data.data.TryGetValue("displayName", out object _displayName))
            {
                displayName = (string)_displayName;
            }
            if (data.data.TryGetValue("descriptions", out object _descriptions))
            {
                JArray array = (JArray)_descriptions;
                descriptions = array.ToObject<DescriptionInfo[]>();
            }
            if (data.data.TryGetValue("amount", out object _amount))
            {
                 
                amount = (int)(long)_amount;
                Debug.Log($"amount:{amount}");
            }
            if (data.data.TryGetValue("stackable", out object _stackable))
            {
                stackable = (bool)_stackable;
            }
            if (data.data.TryGetValue("amountMax", out object _amountMax))
            {
                amountMax = (int)(long)_amountMax;
            }
            if (data.data.TryGetValue("infos", out object _infos))
            {
                infos = _infos as Dictionary<string, object>;
            }
            if(infos==null)
            {
                infos = new Dictionary<string, object>();
            }
            this.Registry();
        }
    }
}