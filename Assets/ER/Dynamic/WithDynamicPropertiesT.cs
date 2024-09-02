namespace ER.Dynamic
{
    public class WithDynamicProperties<T> : IWithDynamicProperties
    {
        private DynamicProperties<T> m_dynamicProperties;

        public DynamicProperties<T> dynamicProperties
        {
            get
            {
                if (m_dynamicProperties == null)
                {
                    m_dynamicProperties = new DynamicProperties<T>();
                }
                return m_dynamicProperties;
            }
        }

        public virtual T this[string key]
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

        public T GetDynamicProperty(string key)
        {
            return dynamicProperties[key];
        }

        object IWithDynamicProperties.GetDynamicProperty(string key)
        {
            return dynamicProperties[key];
        }

        public void SetDynamicProperty(string key, T value)
        {
            dynamicProperties[key] = value;
        }
        void IWithDynamicProperties.SetDynamicProperty(string key, object value)
        {
            dynamicProperties[key] = (T)value;
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