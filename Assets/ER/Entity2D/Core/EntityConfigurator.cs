// Ignore Spelling: Configurator

using UnityEngine;

namespace  ER.Entity2D
{
    public abstract class EntityConfigurator:MonoBehaviour
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Install(Entity owner);
    }

}