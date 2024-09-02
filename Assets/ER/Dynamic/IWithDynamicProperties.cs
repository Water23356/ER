namespace ER.Dynamic
{
    public interface IWithDynamicProperties
    {
        public object GetDynamicProperty(string key);
        public void SetDynamicProperty(string key,object value);

        public bool ContainsDynamicProperty(string key);
    }
}