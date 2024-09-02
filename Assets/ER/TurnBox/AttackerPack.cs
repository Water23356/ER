
namespace ER.TurnBox
{
    public class AttackerPack 
    {
        /// <summary>
        /// 封装的伤害源
        /// </summary>
        private object m_value;
        private AttackerPack m_origin;
        /// <summary>
        /// 上层源
        /// </summary>
        public object Value => m_value;
        public AttackerPack Origin { get=> m_origin; set=> m_origin=value; }
        /// <summary>
        /// 是否为根来源
        /// </summary>
        public bool isRoot
        {
            get => Origin == null;
        }
        /// <summary>
        /// 获取根源伤害源
        /// </summary>
        /// <returns></returns>

        public AttackerPack(object attacker)
        {
            m_value = attacker;
        }
        public AttackerPack GetRootAttaker()
        {
            AttackerPack attaker = this;
            while (Origin != null)
            {
                attaker = attaker.Origin;
            }
            return attaker;
        }
        public T GetValue<T>()where T : class
        {
            return m_value as T;
        }
    }
}