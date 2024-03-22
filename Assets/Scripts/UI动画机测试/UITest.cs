using Assets.ER.UI.Animator.Players;
using ER;
using ER.UI.Animator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI动画机测试
{
    internal class UITest:MonoBehaviour
    {
        public RectTransform img;

        [ContextMenu("初始化")]
        private void Init()
        {
            UIAnimator.Instance.AddPlayer(new BoxPlayer());
            Debug.Log("完成初始化");
        }
        [ContextMenu("播放")]

        private void Play()
        {
            Invoke("SK",2f);
        }

        private void SK()
        {
            Debug.Log("播放");
            UIAnimationCD cd = img.CreateUICD("move", () => {
                Debug.Log("完成播放");
            });
            cd.Type = "box";
            /*
            cd["type"] = "no_start_move";
            cd["dir"] = Vector2.right;
            cd["speed"] = 400f;
            cd["distance"] = 350f;*/

            cd["type"] = "box_open";
            cd["dir_open"] = Dir4.Right;
            cd["speed"] = 6f;

            cd.Register();
            cd.Start();
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
           // Play();
        }
    }
}
