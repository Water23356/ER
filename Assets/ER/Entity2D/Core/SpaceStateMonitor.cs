using UnityEngine;

namespace  ER.Entity2D
{
    public class SpaceStateMonitor : MonoAttribute
    {
        private DynamicEntity _owner;

        public override Entity Owner { get => _owner; set => _owner = value as DynamicEntity; }

        private int landCount = 0;

        private int leftCount = 0;

        private int rightCount = 0;

        [SerializeField]
        [Tooltip("检测区域-底部")]
        private GameObject regionObjectButtom;

        [SerializeField]
        [Tooltip("检测区域-左侧")]
        private GameObject regionObjectLeft;

        [SerializeField]
        [Tooltip("检测区域-右侧")]
        private GameObject regionObjectRight;

        private IRegion regionB;

        private IRegion regionL;

        private IRegion regionR;


        public override void Initialize()
        {
            Debug.Log("空间检测器初始化");
            if (regionObjectButtom != null)
            {
                regionB = regionObjectButtom.GetComponent<IRegion>();
                regionB.EnterEvent += (r, c) =>
                {
                    landCount++;
                    if (_owner.VAttribute.SpaceState != SpaceState.Water)
                    {
                        _owner.VAttribute.SpaceState = SpaceState.Land;
                    }
                };
                regionB.ExitEvent += (r, c) =>
                {
                    landCount--;
                    if (landCount == 0)
                        if (_owner.VAttribute.SpaceState != SpaceState.Water)
                        {
                            _owner.VAttribute.SpaceState = SpaceState.Air;
                        }

                };
            }

            if (regionObjectLeft != null)
            {
                regionL = regionObjectLeft.GetComponent<IRegion>();
                regionL.EnterEvent += (r, c) =>
                {
                    leftCount++;
                    _owner.VAttribute.LeftSpace = true;
                };
                regionL.ExitEvent += (r, c) =>
                {
                    leftCount--;
                    if (leftCount == 0)
                        _owner.VAttribute.LeftSpace = false;

                };
            }

            if (regionObjectRight != null)
            {
                regionR = regionObjectRight.GetComponent<IRegion>();
                regionR.EnterEvent += (r, c) =>
                {
                    rightCount++;
                    _owner.VAttribute.RightSpace = true;
                };
                regionR.ExitEvent += (r, c) =>
                {
                    rightCount--;
                    if (rightCount == 0)
                        _owner.VAttribute.RightSpace = false;

                };
            }

        }

    }
}