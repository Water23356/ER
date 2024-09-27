using Dev;
using UnityEngine;

namespace Dev2
{
    [RequireComponent(typeof(STG_OA_Handler))]
    public class STG_OA_Steer : MonoBehaviour
    {

        [SerializeField]
        private STG_OA_Map map;

        private STG_OA_Handler orca;

        [SerializeField]
        private Transform aimPos;

        public STG_OA_Handler agent
        {
            get
            {
                if (orca == null)
                    orca = GetComponent<STG_OA_Handler>();
                return orca;
            }
        }

        private void Update()
        {
            if (agent == null) return;
            if (aimPos == null) return;
            FindAimArea();
            var dir = aimPos.position - transform.position;
            if (dir.magnitude < 0.001f)
            {
                agent.PreferredVelocity = Vector2.zero;
            }
            else
            {
                agent.PreferredVelocity = (aimPos.position - transform.position) * agent.MaxSpeed;
            }
        }

        private void FindAimArea()//寻找周围威胁度最小的区块
        {
            var offset = new Vector3Int[]
            {
                new Vector3Int(-1, -1),
                new Vector3Int(0, -1),
                new Vector3Int(1, -1),
                new Vector3Int(-1, 0),
                new Vector3Int(1, 0),
                new Vector3Int(-1, 1),
                new Vector3Int(0, 1),
                new Vector3Int(1, 1),
            };

            var defPos = map.GetGridPoint(transform);
            var aimPos = defPos;
            float aimWeight = map.GetWeight(defPos);

            for (int i = 0; i < offset.Length; i++)
            {
                var pos = defPos + offset[i];
                var weight = map.GetWeight(pos);
                if (weight < aimWeight)
                {
                    aimPos = pos;
                    aimWeight = weight;
                }
            }

            this.aimPos.position = map.GetCellCenter(aimPos);
        }
    }
}