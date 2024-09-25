using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ER.ResourceManager
{
    [Serializable]
    public struct RegistryName
    {
        [SerializeField]
        private string head;
        [SerializeField]
        private string module;
        [SerializeField]
        private string path;

        public string Head { get => head; set => head = value; }
        public string Module { get => module; set => module = value; }
        public string Path { get => path; set => path = value; }

        public RegistryName(string originText)
        {
            var parts = originText.Split(":", 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3)
            {
                head = parts[0];
                module = parts[1];
                path = parts[2];
            }
            else if (parts.Length == 2)
            {
                head = parts[0];
                module = "origin";
                path = parts[1];
            }
            else
            {
                head = "unknwon";
                module = "origin";
                path = parts[0];
            }
        }

        public RegistryName(string head, string path)
        {
            this.head = head;
            this.module = "origin";
            this.path = path;
        }

        public RegistryName(string head, string module, string path)
        {
            this.head = head;
            this.module = module;
            this.path = path;
        }

        public override string ToString()
        {
            return string.Join(":", Head, Module, Path);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RegistryName a, RegistryName b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(RegistryName a, RegistryName b)
        {
            return !a.Equals(b);
        }
    }
}