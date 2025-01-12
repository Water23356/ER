using UnityEngine;

namespace ER.GUI
{
    /// <summary>
    /// 简单的伤害值渲染管理器: 可根据项目修改渲染效果
    /// </summary>
    [RequireComponent(typeof(WaterPool))]
    public class DamageNumberManager:MonoSingleton<DamageNumberManager>
    {
        private WaterPool pool;
        [SerializeField]
        private Transform container;

        protected override void Awake()
        {
            if(PasteInstance())
            {
                pool = GetComponent<WaterPool>();
            }
        }

        public void Render(float damage,Vector2 position, int precision=1)
        {
            var number = (DamageNumber)pool.GetObject();
            number.transform.SetParent(container);
            number.SetDamage(damage, precision);
            number.startPos = position;
            number.enabled = true;
        }
    }
}