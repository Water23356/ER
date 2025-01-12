using ER;
using ER.ForEditor;
using UnityEngine;

namespace Dev.AI
{
    public class Eye : InputCell
    {
        [SerializeField]
        [DisplayLabel("视角范围")]
        private Vector2 angleRange = new Vector2(-30,30);
        [SerializeField]
        [DisplayLabel("射线数量")]
        private int rayCount = 30;
        [SerializeField]
        [DisplayLabel("射线长度")]
        private float rayLength = 6f;
        [SerializeField]
        [DisplayLabel("指定图层")]
        private LayerMask ignoreLayerMask;

        public override double UpdateState()
        {
            //Debug.Log("更新眼睛状态");
            Debug.DrawLine(transform.position, transform.position + new Vector3(0, 10, 0), Color.red);
            state = 0.0;
            float cellAngle = (angleRange.y - angleRange.x) / (rayCount-1);
            Vector2 ray = Vector2.up.Rotate(transform.eulerAngles.z);
            ray = ray.Rotate(angleRange.x);
            for(int i=0;i<rayCount;i++)
            {
                var hits = Physics2D.RaycastAll(transform.position, ray, rayLength, ignoreLayerMask);
                if (hits.Length>0)
                {
                    foreach(var hit in hits)
                    {
                        state += 0.1;
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position+ (Vector3)ray*rayLength,Color.red);
                }
                ray = ray.Rotate(cellAngle);
            }
            return state;
        }
    }
}