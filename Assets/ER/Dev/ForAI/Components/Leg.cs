
using ER;
using ER.ForEditor;
using System;
using UnityEngine;

namespace Dev.AI
{
    public class Leg : OutputCell
    {
        [SerializeField]
        [DisplayLabel("状态")]
        private float state;
        [SerializeField]
        [DisplayLabel("前进速度")]
        private float speed = 1;
        [SerializeField]
        [DisplayLabel("旋转速度")]
        private float roatteSpeed = 1;
        [SerializeField]
        [DisplayLabel("前进方向")]
        private Vector2 dir = Vector2.up;
        [SerializeField]
        [DisplayLabel("作用目标")]
        private Transform owner;
        private void Start() 
        {
            if (owner == null)
                owner = this.transform;
        }
        public override void UpdateState(double input)
        {
            state = Mathf.Clamp01((float)input);
        }

        private void FixedUpdate()
        {
            var dir = this.dir.Rotate(transform.eulerAngles.z);//获取正确的前进方向
            owner.transform.position += (Vector3)dir * speed * Time.deltaTime * state;
            owner.transform.eulerAngles += new Vector3(0, 0,1) * roatteSpeed * Time.deltaTime * state;
        }
    }
}