using System;


namespace ER.RPG
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GraphNodeInfoAttribute : Attribute
    {
        public string Description { get; }

        public GraphNodeInfoAttribute(string description = "")
        {
            Description = description;
        }
    }
}