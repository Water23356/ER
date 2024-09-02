namespace ER.Dynamic
{
    public class WithDynamicProperties : IWithDynamicProperties
    {
        private DynamicProperties m_dynamicProperties;

        public DynamicProperties dynamicProperties
        {
            get
            {
                if (m_dynamicProperties == null)
                {
                    m_dynamicProperties = new DynamicProperties();
                }
                return m_dynamicProperties;
            }
        }

        public virtual object this[string key]
        {
            get
            {
                return dynamicProperties[key];
            }
            set
            {
                dynamicProperties[key] = value;
            }
        }

        public object GetDynamicProperty(string key,object defaultValue)
        {
            object value = dynamicProperties[key];
            if (value == null)
                return defaultValue;
            return value;
        }

        public object GetDynamicProperty(string key)
        {
            return dynamicProperties[key];
        }

        public void SetDynamicProperty(string key, object value)
        {
            dynamicProperties[key] = value;
        }

        public bool ContainsDynamicProperty(string key)
        {
            return dynamicProperties.ContainsKey(key);
        }

        public void ClearDynamicProperty()
        {
            dynamicProperties.Clear();
        }
    }
}