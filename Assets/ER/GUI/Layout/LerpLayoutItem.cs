using ER.ForEditor;
using UnityEngine;

namespace ER.GUI
{
    public class LerpLayoutItem : MonoBehaviour, ILerpLayoutItem
    {
        [SerializeField]
        private Vector2 layoutPos;

        [Tooltip("启用后: 在抵达归为点后自动失活")]
        [DisplayLabel("自动失活")]
        [SerializeField]
        private bool autoDisable = false;

        [SerializeField]
        [ReadOnly]
        private ILerpLayoutGroup ownerGroup;

        public Vector2 LayoutPos { get => layoutPos; set => layoutPos = value; }
        public ILerpLayoutGroup OwnerGroup { get => ownerGroup; set => ownerGroup = value; }
        public bool AutoDisable { get => autoDisable; set => autoDisable = value; }

        private void Update()
        {
            if (ownerGroup == null) return;
            bool catched = false;
            transform.position = Utils.LerpForTime(transform.position, layoutPos, ownerGroup.LerpSpeed, ownerGroup.MaxLerpSpeed, out catched);
            if (catched)
            {
                enabled = false;
            }
        }

        public float GetDistanceToAim()
        {
            return Vector2.Distance(LayoutPos, transform.position);
        }

        public Vector2 GetDiractionToAim()
        {
            return (LayoutPos - (Vector2)transform.position).normalized;
        }
    }
}