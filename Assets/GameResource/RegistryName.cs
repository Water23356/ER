using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev
{
    public struct RegistryName
    {
        private string head;
        private string module;
        private string path;

        public string Head { get => head; private set => head = value; }
        public string Module { get => module; private set => module = value; }
        public string Path { get => path; private set => path = value; }

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
    }
}